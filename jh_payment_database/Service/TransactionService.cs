using jh_payment_database.DatabaseContext;
using jh_payment_database.Entity;
using jh_payment_database.Model;

namespace jh_payment_database.Service
{
    public class TransactionService
    {
        private readonly JHDataAccessContext _context;
        private readonly ILogger<TransactionService> _logger;
        public TransactionService(JHDataAccessContext context, ILogger<TransactionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseModel> CreditFund(PaymentRequest paymentRequest)
        {
            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _context.Users.FindAsync(paymentRequest.SenderUserId);
                if (user == null)
                    return ResponseModel.BadRequest("User not found");

                var receiver = await _context.UserAccounts.FindAsync(paymentRequest.SenderUserId);

                if (receiver == null)
                {
                    receiver = UserAccount.GetUserAccount(user, paymentRequest);
                    _context.UserAccounts.Add(receiver);
                }
                else
                {
                    receiver.Balance += paymentRequest.Amount;
                    _context.UserAccounts.Update(receiver);
                }

                // Add transaction
                var transaction = Transaction.GetTransaction(paymentRequest, PaymentStatus.Credited);
                _context.Transactions.Add(transaction);

                // Add transactioninformation
                _context.TransactionInformations.Add(TransactionInformation.GetTransactionInformation(paymentRequest));

                await _context.SaveChangesAsync();

                await tx.CommitAsync();

                return await Task.FromResult(ResponseModel.Ok(transaction, "Success"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                tx.Rollback();
                throw;
            }
        }

        public async Task<ResponseModel> DebitFund(DebitPaymentRequest paymentRequest)
        {
            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                // var sender = await _context.UserAccounts.FindAsync(transaction.FromUserId);
                var receiver = await _context.UserAccounts.FindAsync(paymentRequest.SenderUserId);

                if (receiver == null)
                    return ResponseModel.BadRequest("User not found");

                receiver.Balance -= paymentRequest.Amount;
                _context.UserAccounts.Update(receiver);


                // Add transaction
                var transaction = Transaction.GetTransaction(paymentRequest, PaymentStatus.Debited);
                transaction.ProductId = paymentRequest.ProductId;
                _context.Transactions.Add(transaction);

                await _context.SaveChangesAsync();

                await tx.CommitAsync();

                return await Task.FromResult(ResponseModel.Ok(transaction, "Success"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                tx.Rollback();
                throw;
            }
        }

        public async Task<ResponseModel> CheckBalance(long userId)
        {
            try
            {
                // var sender = await _context.UserAccounts.FindAsync(transaction.FromUserId);
                var userAccount = await _context.UserAccounts.FindAsync(userId);

                if (userAccount == null)
                    return ResponseModel.BadRequest("Account not opened");

                return await Task.FromResult(ResponseModel.Ok(userAccount, "Success"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<ResponseModel> GetTransactionDetails(long userId, PageRequestModel pageRequestModel)
        {
            var transactions = _context.Transactions
                .Where(x => x.FromUserId.Equals(userId))
                .OrderByDescending(x => x.CreatedAt)
                .Skip(pageRequestModel.PageSize * (pageRequestModel.PageNumber - 1))
                .Take(pageRequestModel.PageSize)
                .ToList<Transaction>();

            if (transactions == null)
                return ResponseModel.Ok(new List<Transaction> { }, "No record found");

            return await Task.FromResult(ResponseModel.Ok(transactions, "Success"));
        }

        public async Task<ResponseModel> TransferAsync(PaymentRequest paymentRequest)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            var sender = await _context.UserAccounts.FindAsync(paymentRequest.SenderUserId);
            var receiver = await _context.UserAccounts.FindAsync(paymentRequest.ReceiverUserId);

            if (sender == null || receiver == null)
                return ResponseModel.BadRequest("User not found");

            if (sender.Balance < paymentRequest.Amount)
                return ResponseModel.BadRequest("Insufficient balance");

            sender.Balance -= paymentRequest.Amount;
            receiver.Balance += paymentRequest.Amount;

            var payment = Payment.GetPayment(paymentRequest);
            _context.Payments.Add(payment);

            var txDebit = Transaction.GetTransaction(paymentRequest, PaymentStatus.Debited);
            var txCredit = Transaction.GetTransaction(paymentRequest, PaymentStatus.Credited);

            _context.Transactions.AddRange(txDebit, txCredit);

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return ResponseModel.Ok("Transfered successfully");
        }

        public async Task<ResponseModel> TransferCardAsync(CardPaymentRequest paymentRequest)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            var sender = await _context.UserAccounts.FindAsync(paymentRequest.SenderUserId);
            var receiver = await _context.UserAccounts.FindAsync(paymentRequest.ReceiverUserId);

            if (sender == null || receiver == null)
                return ResponseModel.BadRequest("User not found");

            if (sender.Balance < paymentRequest.Amount)
                return ResponseModel.BadRequest("Insufficient balance");


            if (paymentRequest.Amount <= 0)
            {
                return ResponseModel.BadRequest("Amount must be greater than zero");
            }
            if (paymentRequest.CardDetails.CardNumber == paymentRequest.ReceiverCardNumber)
            {
                return ResponseModel.BadRequest("Sender and receiver can't be same");
            }

            sender.Balance -= paymentRequest.Amount;

            var payment = Payment.GetCardPayment(paymentRequest);
            _context.Payments.Add(payment);

            var txDebit = Transaction.GetTransaction(paymentRequest, PaymentStatus.Debited);

            _context.Transactions.AddRange(txDebit);

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return ResponseModel.Ok("Transfered successfully");
        }

        public async Task<ResponseModel> ReFund(long userId, string transactionId)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            var userAccount = await _context.UserAccounts.FindAsync(userId);

            Guid.TryParse(transactionId, out Guid transactionGuid);
            var transactionDetail = await _context.Transactions.FindAsync(transactionGuid);

            if (userAccount == null)
                return ResponseModel.BadRequest("User not found");

            if (transactionDetail == null)
                return ResponseModel.BadRequest("Invalid transaction id used");

            userAccount.Balance += transactionDetail.Amount;
            _context.UserAccounts.Update(userAccount);

            transactionDetail.TrasactionStatus = PaymentStatus.Refund;
            _context.Transactions.Update(transactionDetail);

            var txCredit = Transaction.GetTransaction(new PaymentRequest
            {
                SenderUserId = userId,
                ReceiverUserId = 0,
                Amount = transactionDetail.Amount,
                PaymentMethod = PaymentMethodType.System
            }, PaymentStatus.Refund);

            _context.Transactions.AddRange(txCredit);

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return ResponseModel.Ok("Transfered successfully");
        }

        public async Task<ResponseModel> PartialRefund(long userId, string transactionId)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            var userAccount = await _context.UserAccounts.FindAsync(userId);

            Guid.TryParse(transactionId, out Guid transactionGuid);
            var transactionDetail = await _context.Transactions.FindAsync(transactionGuid);

            if (userAccount == null)
                return ResponseModel.BadRequest("User not found");

            if (transactionDetail == null)
                return ResponseModel.BadRequest("Invalid transaction id used");

            var partialRefund = transactionDetail.Amount / 2;
            userAccount.Balance += partialRefund;
            _context.UserAccounts.Update(userAccount);

            transactionDetail.TrasactionStatus = PaymentStatus.PartialRefund;
            _context.Transactions.Update(transactionDetail);

            var txCredit = Transaction.GetTransaction(new PaymentRequest
            {
                SenderUserId = userId,
                ReceiverUserId = 0,
                Amount = partialRefund,
                PaymentMethod = PaymentMethodType.System
            }, PaymentStatus.PartialRefund);

            _context.Transactions.AddRange(txCredit);

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return ResponseModel.Ok("Transfered successfully");
        }
    }
}

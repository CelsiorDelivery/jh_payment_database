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

        public async Task<ResponseModel> CreditFund(Transaction transaction)
        {
            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _context.Users.FindAsync(transaction.FromUserId);
                if (user == null)
                    return ResponseModel.BadRequest("User not found");

                var receiver = await _context.UserAccounts.FindAsync(transaction.FromUserId);

                if (receiver == null)
                {
                    receiver = GetAccount(user, transaction);
                    _context.UserAccounts.Add(receiver);
                }
                else
                {
                    receiver.Balance += transaction.Amount;
                    _context.UserAccounts.Update(receiver);
                }


                transaction.TransactionId = DateTime.Now.Ticks;
                transaction.PaymentId = DateTime.Now.Ticks;
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

        public UserAccount GetAccount(User user, Transaction transaction)
        {
            return new UserAccount
            {
                Balance = transaction.Amount,
                Email = user.Email,
                FullName = string.Concat(user.FirstName, "", user.LastName),
                MobileNumber = user.Mobile,
                UserId = user.UserId
            };
        }

        public async Task<ResponseModel> DebitFund(Transaction transaction)
        {
            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                // var sender = await _context.UserAccounts.FindAsync(transaction.FromUserId);
                var receiver = await _context.UserAccounts.FindAsync(transaction.ToUserId);

                if (receiver == null)
                    return ResponseModel.BadRequest("User not found");

                receiver.Balance -= transaction.Amount;
                _context.UserAccounts.Update(receiver);

                transaction.TransactionId = DateTime.Now.Ticks;
                transaction.PaymentId = DateTime.Now.Ticks;
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

        public async Task<ResponseModel> TransferAsync(long senderId, long receiverId, decimal amount)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            var sender = await _context.UserAccounts.FindAsync(senderId);
            var receiver = await _context.UserAccounts.FindAsync(receiverId);

            if (sender == null || receiver == null)
                return ResponseModel.BadRequest("User not found");

            if (sender.Balance < amount)
                return ResponseModel.BadRequest("Insufficient balance");

            sender.Balance -= amount;
            receiver.Balance += amount;

            var payment = new Payment { SenderUserId = senderId, ReceiverUserId = receiverId, Amount = amount, Method = PaymentMethodType.Wallet, Status = PaymentStatus.Success };
            _context.Payments.Add(payment);

            var txDebit = new Transaction { PaymentId = payment.PaymentId, FromUserId = senderId, ToUserId = receiverId, Amount = amount, Type = TransactionType.Debit };
            var txCredit = new Transaction { PaymentId = payment.PaymentId, FromUserId = senderId, ToUserId = receiverId, Amount = amount, Type = TransactionType.Credit };

            _context.Transactions.AddRange(txDebit, txCredit);

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return ResponseModel.Ok("Transfered successfully");
        }
    }
}

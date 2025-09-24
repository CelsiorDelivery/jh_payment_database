using jh_payment_database.Model;
using System.ComponentModel.DataAnnotations;

namespace jh_payment_database.Entity
{
    public class TransactionInformation
    {
        /// <summary>
        /// Represents the card number.
        /// </summary>
        [Key]
        public Guid TransactionId { get; set; }


        /// <summary>
        /// Represents the sender user id.
        /// </summary>
        public string UserId { get; set; }


        /// <summary>
        /// Represents the card number.
        /// </summary>
        public DateTime AddedOn { get; set; }


        #region CREDIT OR DEBIT CARD DETAIL

        /// <summary>
        /// Represents the card number.
        /// </summary>
        public string CardNumber { get; set; } = string.Empty;

        /// <summary>
        /// Represents the name of the card holder.
        /// </summary>
        public string CardHolderName { get; set; } = string.Empty;

        /// <summary>
        /// Represents the expiry month of the card in MM format and expiry year in YYYY format.
        /// </summary>
        public string ExpiryMonth { get; set; } = string.Empty; // MM

        /// <summary>
        /// Represents the expiry year of the card in YYYY format.
        /// </summary>
        public string ExpiryYear { get; set; } = string.Empty;  // YYYY

        /// <summary>
        /// Represents the CVV (Card Verification Value) of the card.
        /// </summary>
        public string CVV { get; set; } = string.Empty;

        #endregion

        #region UPI DETAIL

        /// <summary>
        /// Represents the Virtual Payment Address (VPA) used in UPI transactions.
        /// </summary>
        public string Vpa { get; set; } = string.Empty; // e.g. user@upi

        #endregion

        #region NET BANKING DETAIL

        /// <summary>
        /// Represents the name of the bank.
        /// </summary>
        public string BankName { get; set; } = string.Empty;

        /// <summary>
        /// Represents the account holder's name.
        /// </summary>
        public string AccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// Represents the IFSC code of the bank branch.
        /// </summary>
        public string IFSCCode { get; set; } = string.Empty;

        #endregion

        public static TransactionInformation GetTransactionInformation(PaymentRequest paymentRequest)
        {
            var transactionInfo = new TransactionInformation
            {
                UserId = paymentRequest.SenderUserId,
                AddedOn = DateTime.Now,
            };

            switch (paymentRequest.PaymentMethod)
            {
                case PaymentMethodType.NetBanking:
                    if (paymentRequest.NetBankingDetails == null)
                    {
                        throw new Exception("Netbanking detail is found null or empty");
                    }

                    transactionInfo.BankName = paymentRequest.NetBankingDetails.BankName;
                    transactionInfo.AccountNumber = paymentRequest.NetBankingDetails.AccountNumber;
                    transactionInfo.IFSCCode = paymentRequest.NetBankingDetails.IFSCCode;
                    break;
                case PaymentMethodType.Card:
                    if (paymentRequest.CardDetails == null)
                    {
                        throw new Exception("Card detail is found null or empty");
                    }

                    transactionInfo.CardNumber = paymentRequest.CardDetails.CardNumber;
                    transactionInfo.CardHolderName = paymentRequest.CardDetails.CardHolderName;
                    transactionInfo.ExpiryMonth = paymentRequest.CardDetails.ExpiryMonth;
                    transactionInfo.ExpiryYear = paymentRequest.CardDetails.ExpiryYear;
                    transactionInfo.CVV = paymentRequest.CardDetails.CVV;
                    break;
                case PaymentMethodType.UPI:
                    if (paymentRequest.UpiDetails == null)
                    {
                        throw new Exception("UPI detail is found null or empty");
                    }

                    transactionInfo.Vpa = paymentRequest.UpiDetails.Vpa;
                    break;
                default:
                    throw new Exception("Invalid transaction method opted.");
            }

            return transactionInfo;
        }
    }
}

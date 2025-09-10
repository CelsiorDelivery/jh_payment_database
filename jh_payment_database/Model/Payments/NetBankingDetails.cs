namespace jh_payment_database.Model.Payments
{
    /// <summary>
    /// This class represents the details required for processing a net banking payment.
    /// </summary>
    public class NetBankingDetails
    {
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
    }
}

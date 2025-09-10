namespace jh_payment_database.Model.Payments
{
    /// <summary>
    /// This class represents the details of a digital wallet used for payments.
    /// </summary>
    public class WalletDetails
    {
        /// <summary>
        /// Represents the wallet provider name.
        /// </summary>
        public string WalletProvider { get; set; } = string.Empty; // e.g. Paytm, PhonePe
        
        /// <summary>
        /// Represents the unique identifier for the wallet.
        /// </summary>
        public string WalletId { get; set; } = string.Empty;
    }
}

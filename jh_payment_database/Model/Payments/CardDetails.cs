namespace jh_payment_database.Model.Payments
{
    /// <summary>
    /// This class represents the details of a payment card.
    /// </summary>
    public class CardDetails
    {
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
    }
}

namespace jh_payment_database.Model.Payments
{
    /// <summary>
    /// This class represents UPI (Unified Payments Interface) payment details.
    /// </summary>
    public class UpiDetails
    {
        /// <summary>
        /// Represents the Virtual Payment Address (VPA) used in UPI transactions.
        /// </summary>
        public string Vpa { get; set; } = string.Empty; // e.g. user@upi
    }
}

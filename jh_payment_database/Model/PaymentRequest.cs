using jh_payment_database.Model.Payments;

namespace jh_payment_database.Model
{
    /// <summary>
    /// This class represents a payment request made by a user.
    /// </summary>
    public class PaymentRequest
    {
        /// <summary>
        /// Represents the Email of the user sending the payment.
        /// </summary>
        public string SenderUserId { get; set; }

        /// <summary>
        /// Represents the Email of the user receiving the payment.
        /// </summary>
        public string ReceiverUserId { get; set; }

        /// <summary>
        /// Represents the amount to be paid.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Represents the payment method chosen by the user.
        /// </summary>
        public PaymentMethodType PaymentMethod { get; set; }

        /// <summary>
        /// Represents the details of the card if the payment method is Card.
        /// </summary>
        public CardDetails? CardDetails { get; set; }

        /// <summary>
        /// Represents the details of the UPI if the payment method is UPI.
        /// </summary>
        public UpiDetails? UpiDetails { get; set; }

        /// <summary>
        /// Represents the details of the net banking if the payment method is Net Banking.
        /// </summary>
        public NetBankingDetails? NetBankingDetails { get; set; }

        /// <summary>
        /// Represents the details of the wallet if the payment method is Wallet.
        /// </summary>
        public WalletDetails? WalletDetails { get; set; }
    }
}

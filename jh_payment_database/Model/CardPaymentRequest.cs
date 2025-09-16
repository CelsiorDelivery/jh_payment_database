namespace jh_payment_database.Model
{
    /// <summary>
    /// This class represents the details of a Card To Card Payment.
    /// </summary>
    public class CardPaymentRequest : PaymentRequest
    {
        public string ReceiverCardNumber { get; set; }

    }
}

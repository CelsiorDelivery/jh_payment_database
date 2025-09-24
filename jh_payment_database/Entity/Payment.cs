using jh_payment_database.Model;

namespace jh_payment_database.Entity
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public string SenderUserId { get; set; }
        public string ReceiverUserId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethodType Method { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public static Payment GetPayment(PaymentRequest paymentRequest, PaymentStatus paymentStatus = PaymentStatus.Success)
        {
            return new Payment
            {
                SenderUserId = paymentRequest.SenderUserId,
                ReceiverUserId = paymentRequest.ReceiverUserId,
                Amount = paymentRequest.Amount,
                Method = paymentRequest.PaymentMethod,
                Status = paymentStatus,
                CreatedAt = DateTime.UtcNow,
                PaymentId = Guid.NewGuid()
            };
        }

        public static Payment GetCardPayment(CardPaymentRequest paymentRequest, PaymentStatus paymentStatus = PaymentStatus.Success)
        {
            return new Payment
            {
                SenderUserId = paymentRequest.SenderUserId,
                ReceiverUserId = paymentRequest.ReceiverUserId,
                Amount = paymentRequest.Amount,
                Method = paymentRequest.PaymentMethod,
                Status = paymentStatus,
                CreatedAt = DateTime.UtcNow,
                PaymentId = Guid.NewGuid()
            };
        }
    }
}

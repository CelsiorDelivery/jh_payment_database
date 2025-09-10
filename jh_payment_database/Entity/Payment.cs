using jh_payment_database.Model;

namespace jh_payment_database.Entity
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public long SenderUserId { get; set; }
        public long ReceiverUserId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethodType Method { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public static Payment GetPayment(PaymentRequest paymentRequest)
        {
            return new Payment
            {
                SenderUserId = paymentRequest.SenderUserId,
                ReceiverUserId = paymentRequest.ReceiverUserId,
                Amount = paymentRequest.Amount,
                Method = PaymentMethodType.Wallet,
                Status = PaymentStatus.Success,
                CreatedAt = DateTime.UtcNow,
                PaymentId = Guid.NewGuid()
            };
        }
    }
}

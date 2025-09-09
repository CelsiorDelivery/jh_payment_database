using jh_payment_database.Model;

namespace jh_payment_database.Entity
{
    public class Payment
    {
        public long PaymentId { get; set; }
        public long SenderUserId { get; set; }
        public long ReceiverUserId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethodType Method { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

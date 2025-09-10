using jh_payment_database.Model;

namespace jh_payment_database.Entity
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public Guid PaymentId { get; set; }
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus TrasactionStatus { get; set; } = PaymentStatus.Success;
        public PaymentMethodType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public static Transaction GetTransaction(PaymentRequest paymentRequest, PaymentStatus paymentStatus)
        {
            return new Transaction
            {
                Amount = paymentRequest.Amount,
                CreatedAt = DateTime.Now,
                FromUserId = paymentRequest.SenderUserId,
                PaymentId = Guid.NewGuid(),
                ToUserId = paymentRequest.ReceiverUserId,
                TransactionId = Guid.NewGuid(),
                Type = paymentRequest.PaymentMethod,
                TrasactionStatus = paymentStatus
            };
        }
    }
}
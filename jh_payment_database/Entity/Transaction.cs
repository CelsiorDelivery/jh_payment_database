namespace jh_payment_database.Entity
{
    public class Transaction
    {
        public long TransactionId { get; set; }
        public long PaymentId { get; set; }
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

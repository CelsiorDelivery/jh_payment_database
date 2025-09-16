namespace jh_payment_database.Model
{
    public enum PaymentStatus
    {
        Pending = 1,
        Success = 2,
        Failed = 3,
        Cancelled = 4,
        Credited = 5,
        Debited = 6,
        Refund = 7,
        PartialRefund = 8
    }
}

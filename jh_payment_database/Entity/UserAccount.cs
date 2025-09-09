using System.ComponentModel.DataAnnotations;

namespace jh_payment_database.Entity
{
    public class UserAccount
    {
        public long UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }

        // Concurrency token to avoid lost updates
        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}

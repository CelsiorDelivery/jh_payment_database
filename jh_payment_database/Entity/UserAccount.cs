using jh_payment_database.Model;
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

        public static UserAccount GetUserAccount(User user, PaymentRequest paymentRequest)
        {
            return new UserAccount
            {
                Balance = paymentRequest.Amount,
                Email = user.Email,
                FullName = string.Concat(user.FirstName, "", user.LastName),
                MobileNumber = user.Mobile,
                UserId = user.UserId
            };
        }
    }
}

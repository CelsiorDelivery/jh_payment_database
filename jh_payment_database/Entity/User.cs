namespace jh_payment_database.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class User
    {
        /// <summary>
        /// 
        /// </summary>
        public long UserId { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Mobile { set; get; }
        public string AccountNumber { set; get; }
        public string BankName { set; get; }
        public string IFCCode { set; get; }
        public string Branch { set; get; }
        public string UPIID { set; get; }
        public string CVV { set; get; }
        public bool IsActive { set; get; }
        public Roles Role { set; get; } = Roles.User;
        public DateTime DateOfExpiry { set; get; }
    }

    public enum Roles
    {
        User,
        Admin,
        Murchant
    }
}

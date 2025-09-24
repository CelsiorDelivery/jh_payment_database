using jh_payment_database.Entity;
using jh_payment_database.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace jh_payment_database.Service
{
    public class OperationService
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public OperationService(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public async Task<ResponseModel> CreateSample()
        {
            List<User> users = new List<User>();
            _configuration.GetSection("SampleUsers").Bind(users);
            if (users.Count == 0)
            {
                throw new Exception("Sample user record not found");
            }

            // var users = JsonSerializer.Deserialize<List<User>>(sampleUsers!);
            Random random = new Random();

            int i = 1000;
            foreach (var user in users!)
            {
                var ac = Generate14DigitNumber(random);
                await _userService.AddUser(new User
                {
                    AccountNumber = ac,
                    BankName = user.BankName,
                    Branch = user.City,
                    BankCode = user.BankCode,
                    City = user.City,
                    CVV = ac.Substring(1, 3),
                    DateOfExpiry = DateTime.Now.AddDays(double.Parse(ac.Substring(1, 3))),
                    Email = $"{user.FirstName}{ac.Substring(10, 2)}@gmail.com",
                    FirstName = user.FirstName,
                    IFCCode = user.BankCode,
                    IsActive = true,
                    Password = user.FirstName,
                    LastName = user.LastName,
                    Mobile = user.Mobile,
                    UPIID = $"{user.FirstName}{ac.Substring(10, 2)}@ybl",
                    UserId = Guid.NewGuid().ToString(),
                    Age = 25,
                    Address = user.City
                });

                i++;
            }

            return await Task.FromResult(ResponseModel.Ok("Created"));
        }

        private string Generate14DigitNumber(Random random)
        {
            // Ensure leading digit is not zero
            long firstPart = random.Next(1, 10); // 1–9
            long remaining = (long)(random.NextDouble() * 1_000_000_000_0000L); // up to 13 digits
            string num = firstPart.ToString() + remaining.ToString("D13"); // pad remaining with leading zeros
            return num;
        }
    }
}

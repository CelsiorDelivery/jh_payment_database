using jh_payment_database.DatabaseContext;
using jh_payment_database.Entity;
using jh_payment_database.Model;
using Microsoft.AspNetCore.Mvc;

namespace jh_payment_database.Service
{
    public class UserService
    {
        private readonly JHDataAccessContext _context;
        private readonly ILogger<UserService> _logger;
        public UserService(JHDataAccessContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseModel> AddUser(User user)
        {
            string message = string.Empty;
            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                // var sender = await _context.UserAccounts.FindAsync(transaction.FromUserId);
                var presentUser = await _context.Users.FindAsync(user.UserId);

                if (presentUser == null)
                {
                    user.IsActive = true;
                    _context.Users.Add(user);

                    var userAccount = new UserAccount
                    {
                        Balance = user.Balance,
                        Email = user.Email,
                        FullName = string.Concat(user.FirstName, "", user.LastName),
                        MobileNumber = user.Mobile,
                        UserId = user.UserId
                    };

                    _context.UserAccounts.Add(userAccount);

                    message = "User Added";
                }
                else
                {
                    var updateUser = new User
                    {
                        BankName = presentUser.BankName,
                        Branch = presentUser.Branch,
                        CVV = presentUser.CVV,
                        DateOfExpiry = presentUser.DateOfExpiry,
                        Email = presentUser.Email,
                        FirstName = presentUser.FirstName,
                        LastName = presentUser.LastName,
                        IFCCode = presentUser.IFCCode,
                        Mobile = presentUser.Mobile,
                        UPIID = presentUser.UPIID,
                    };

                    _context.Users.Update(updateUser);
                    message = "User Updated";
                }

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                return await Task.FromResult(ResponseModel.Ok(message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                tx.Rollback();
                throw;
            }
        }

        public async Task<ResponseModel> DeactivateUser(long userId)
        {
            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var presentUser = await _context.Users.FindAsync(userId);

                if (presentUser != null)
                {
                    presentUser.IsActive = false;
                    _context.Users.Update(presentUser);
                }

                await _context.SaveChangesAsync();

                await tx.CommitAsync();

                return await Task.FromResult(ResponseModel.Ok("Deactivated"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                tx.Rollback();
                throw;
            }
        }

        public async Task<ResponseModel> GetUser(string email)
        {
            try
            {
                User presentUser = null;
                if (long.TryParse(email, out var id))
                {
                    presentUser = _context.Users.Where(x => x.UserId.Equals(id)).FirstOrDefault();
                }
                else
                {
                    presentUser = _context.Users.Where(x => x.Email.Equals(email)).FirstOrDefault();
                }

                if (presentUser == null)
                {
                    throw new Exception("User not found");
                }
                var userAccount = await _context.UserAccounts.FindAsync(presentUser.UserId);

                if (userAccount != null) 
                    presentUser.Balance = userAccount.Balance;

                return await Task.FromResult(ResponseModel.Ok(presentUser, "Success"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<ResponseModel> GetAllUser()
        {
            try
            {
                var presentUser = _context.Users.Where(x => x.UserId > 0).ToList<User>();

                if (presentUser == null)
                {
                    throw new Exception("User not found");
                }

                return await Task.FromResult(ResponseModel.Ok(presentUser, "Success"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<ResponseModel> GetUserByPageAsync(int pageSize, int pageNumber, string searchString, string sortBy)
        {
            try
            {
                var presentUser = _context.Users
                    .OrderBy(x => x.UserId)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .ToList<User>();

                if (presentUser == null)
                {
                    throw new Exception("User not found");
                }

                return await Task.FromResult(ResponseModel.Ok(presentUser, "Success"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<ResponseModel> UpdateUser(User user)
        {
            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var presentUser = await _context.Users.FindAsync(user.UserId);
                if (presentUser != null && presentUser.AccountNumber == user.AccountNumber)
                {
                    presentUser.BankName = user.BankName;
                    presentUser.BankCode = user.BankCode;
                    presentUser.Branch = user.Branch;
                    presentUser.CVV = user.CVV;
                    presentUser.DateOfExpiry = user.DateOfExpiry;
                    presentUser.Email = user.Email;
                    presentUser.FirstName = user.FirstName;
                    presentUser.LastName = user.LastName;
                    presentUser.IFCCode = user.IFCCode;
                    presentUser.Mobile = user.Mobile;
                    presentUser.UPIID = user.UPIID;
                    presentUser.City = user.City;

                    _context.Users.Update(presentUser);

                    await _context.SaveChangesAsync();
                    await tx.CommitAsync();
                    return await Task.FromResult(ResponseModel.Ok("Updated"));
                }
                else
                {
                    return ResponseModel.BadRequest("User not found with the UserId and AccountNumber.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                tx.Rollback();
                throw;
            }
        }
    }
}

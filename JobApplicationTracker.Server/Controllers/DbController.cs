using JobApplicationTracker.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Server.Controllers
{
    public interface IDbController
    {
        Task<IUser?> GetUserByEmailAsync(string email);
        Task<HashSet<IUser>> GetAllUsersAsync();

        Task<bool> AddUserAsync(string email, string userName);
        Task<bool> RemoveUserAsync(string email);
        Task<bool> SaveChangesToUserAsync(IUser user);
    }

    public class DbController : Controller, IDbController
    {
        private AppDbContext _dbContext;
        private ILogger<DbController> _logger;

        public DbController(AppDbContext appDbContext, ILogger<DbController> logger)
        {
            _dbContext = appDbContext;
            _logger = logger;
        }

        public async Task<bool> AddUserAsync(string email, string userName)
        {
            var user = new User()
            {
                UserName = userName,
                NormalizedUserName = userName.ToUpperInvariant(),
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
            };

            try
            {
                var addResult = await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error adding user, a user with the email:{0} may already exist", email);
                return false;
            }
        }

        public async Task<HashSet<IUser>> GetAllUsersAsync()
        {
            return new HashSet<IUser>(await _dbContext.Users.ToListAsync());
        }

        async Task<IUser?> IDbController.GetUserByEmailAsync(string email) => await GetUserByEmailAsync(email);

        internal async Task<User?> GetUserByEmailAsync(string email)
        {
            var normalizedEmail = email.ToUpper();
            var user = await _dbContext.Users
                .Include(u => u.JobApplications)
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);

            return user;
        }



        public async Task<bool> RemoveUserAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);

            if (user is null)
                return false;

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SaveChangesToUserAsync(IUser user)
        {
            var userFromDb = await GetUserByEmailAsync(user.Email ?? "");
            if (userFromDb is null)
                throw new InvalidOperationException($"Cannot update user:{user.Email} when that user does not exist in the database.");

            if (ReferenceEquals(user, userFromDb))
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }

            throw new InvalidOperationException("Update user was given an instance of a user that is not the same instance acquired for modification");

        }
    }
}

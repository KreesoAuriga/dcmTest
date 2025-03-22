using JobApplicationTracker.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Server.Controllers
{
    public interface IDbController
    {
        Task<IUser?> GetUserByNameAsync(string username, bool caseSensitive);
        Task<IUser?> GetUserByEmailAsync(string email);
        Task<HashSet<IUser>> GetAllUsersAsync();

        Task<bool> AddUserAsync(string email, string userName);
        Task<bool> RemoveUserAsync(string email);
    }

    public class DbController : Controller, IDbController
    {
        private AppDbContext _dbContext;
        private ILogger _logger;

        public DbController(AppDbContext appDbContext, ILogger logger)
        {
            _dbContext = appDbContext;
            _logger = logger;
        }

        public async Task<bool> AddUserAsync(string email, string userName)
        {
            var user = new User(userName, email);

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
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);

            return user;
        }


        public async Task<IUser?> GetUserByNameAsync(string username, bool caseSensitive)
        {
            if (caseSensitive)
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);

                return user;
            }
            else
            {
                var normalizedUserName = username.ToUpper();
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName);

                return user;
            }
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
    }
}

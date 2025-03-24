using JobApplicationTracker.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Server.Controllers
{
    /// <summary>
    /// Interface for basic interaction with the databse.
    /// </summary>
    public interface IDbController
    {
        /// <summary>
        /// Get user by email.
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <returns>user interface</returns>
        Task<IUser?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Gets all users currently in the database.
        /// </summary>
        /// <returns>HashSet of all users</returns>
        Task<HashSet<IUser>> GetAllUsersAsync();

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<bool> AddUserAsync(string email, string userName);

        /// <summary>
        /// Permanently deletes the specified user.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<bool> RemoveUserAsync(string email);

        /// <summary>
        /// Saves changes made to the specified user object.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task<HashSet<IUser>> GetAllUsersAsync()
        {
            return new HashSet<IUser>(await _dbContext.Users.ToListAsync());
        }

        async Task<IUser?> IDbController.GetUserByEmailAsync(string email) => await GetUserByEmailAsync(email);

        /// <inheritdoc />
        internal async Task<User?> GetUserByEmailAsync(string email)
        {
            var normalizedEmail = email.ToUpper();
            var user = await _dbContext.Users
                .Include(u => u.JobApplications)
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);

            return user;
        }



        /// <inheritdoc />
        public async Task<bool> RemoveUserAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);

            if (user is null)
                return false;

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc />
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

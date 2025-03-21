using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Server.Data
{
    public interface IUser
    {
        string? Email { get; }
        string? UserName { get; }
        ICollection<IJobApplication> JobApplications { get; }
    }

    public class User : IdentityUser, IUser
    {
        internal JobApplication[] _jobApplications;

        public ICollection<IJobApplication> JobApplications => _jobApplications;
    }

    /// <summary>
    /// Represents the current status of a <see cref="IJobApplication"/>.
    /// </summary>
    public enum JobApplicationStatus
    {
        /// <summary>
        /// The application has been created in the database, but not yet progressed from creation of the entry.
        /// </summary>
        Created,

        /// <summary>
        /// The application has been submitted.
        /// </summary>
        Applied,

        /// <summary>
        /// The application was rejected by the company.
        /// </summary>
        RejectedByCompany,

        /// <summary>
        /// The applicant has rejected an offer from the company or has abandoned the application.
        /// </summary>
        RejectedByApplicant,

        /// <summary>
        /// An interview has been scheduled (initial or subsequent).
        /// </summary>
        InterviewScheduled,

        /// <summary>
        /// Awaiting a response after an interview has been completed.
        /// </summary>
        InterviewedAndAwaitingResponse,

        /// <summary>
        /// An offer has been made by the company
        /// </summary>
        OfferHasBeenMade,


    }

    public interface IJobApplication
    {
        string CompanyName { get; }
        string Position { get; }
        JobApplicationStatus Status { get; }
    }

    internal class JobApplication : IJobApplication
    {
        public string CompanyName             { get; set; }
        public string Position                { get; set; }
        public JobApplicationStatus Status    { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } // Example model
    }
}

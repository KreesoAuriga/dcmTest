using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Server.Data
{
    public interface IUser
    {
        string? Email { get; }
        string? UserName { get; }
        IReadOnlyCollection<IJobApplication> JobApplications { get; }

        void AddJobApplication(IJobApplication jobApplication);
    }

    [Index(nameof(NormalizedEmail), IsUnique = true)]
    public class User : IdentityUser, IUser
    {
        internal HashSet<JobApplication> _jobApplications = new HashSet<JobApplication>();

        public IReadOnlyCollection<IJobApplication> JobApplications => _jobApplications;

        public User() 
        {
        }
/*
        public void Initialize(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }*/

        public void AddJobApplication(IJobApplication jobApplication)
        {
            if (jobApplication is not JobApplication asJobApplicationClass)
                throw new ArgumentException($"{nameof(jobApplication)} is not an instance of {nameof(jobApplication)}");

            _jobApplications.Add(asJobApplicationClass);
        }
    }
}

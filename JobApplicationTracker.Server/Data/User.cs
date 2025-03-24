using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JobApplicationTracker.Server.Data
{


    /// <summary>
    /// Interface for a user.
    /// </summary>
    public interface IUser
    {
        string? Email { get; }
        string? UserName { get; }
        IReadOnlyCollection<IJobApplication> JobApplications { get; }

        void AddJobApplication(IJobApplication jobApplication);
        void RemoveJobApplication(IJobApplication jobApplication);
    }

    [Index(nameof(NormalizedEmail), IsUnique = true)]
    public class User : IdentityUser, IUser
    {
        public ICollection<JobApplication> JobApplications { get; set; } = new HashSet<JobApplication>();

        IReadOnlyCollection<IJobApplication> IUser.JobApplications => (HashSet<JobApplication>)JobApplications;

        public User() 
        {
        }

        public void AddJobApplication(IJobApplication jobApplication)
        {
            if (jobApplication is not JobApplication asJobApplicationClass)
                throw new ArgumentException($"{nameof(jobApplication)} is not an instance of {nameof(jobApplication)}");

            JobApplications.Add(asJobApplicationClass);
        }

        public void RemoveJobApplication(IJobApplication jobApplication)
        {
            if (jobApplication is not JobApplication asJobApplicationClass)
                throw new ArgumentException($"{nameof(jobApplication)} is not an instance of {nameof(jobApplication)}");

            JobApplications.Remove(asJobApplicationClass);
        }
    }
}

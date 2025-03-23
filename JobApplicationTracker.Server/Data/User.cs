﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
        public ICollection<JobApplication> JobApplications { get; set; } = new HashSet<JobApplication>();

        IReadOnlyCollection<IJobApplication> IUser.JobApplications => (HashSet<JobApplication>)JobApplications;

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

            JobApplications.Add(asJobApplicationClass);
        }
    }
}

using JobApplicationTracker.Server.DTOs;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Server.Data
{
    public interface IJobApplication
    {
        public ulong Id             { get; }
        string CompanyName          { get; }
        string Position             { get; }
        JobApplicationStatus Status { get; }
        DateTime DateApplied        { get; }
    }

    [Index(nameof(Id), IsUnique = true)]
    public class JobApplication : IJobApplication
    {
        public ulong                Id              { get; set; }
        public string               CompanyName     { get; set; }
        public string               Position        { get; set; }
        public JobApplicationStatus Status          { get; set; }
        public DateTime             DateApplied     { get; set; }
        
    }
}

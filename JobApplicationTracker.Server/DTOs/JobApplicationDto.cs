using JobApplicationTracker.Server.Data;
using Microsoft.AspNetCore.Builder;

namespace JobApplicationTracker.Server.DTOs
{

    public class NewJobApplicationDto(string companyName, string position, DateTime dateApplied)
    {
        public string CompanyName           { get; set; } = companyName;
        public string Position              { get; set; } = position;
        public DateTime DateApplied         { get; set; } = dateApplied;

        public NewJobApplicationDto(IJobApplication jobApplication) : this(
            jobApplication.CompanyName, 
            jobApplication.Position,
            jobApplication.DateApplied)
        {
        }

        public NewJobApplicationDto() : this("", "", DateTime.Now)
        {
        }
    }

    public class JobApplicationDto(ulong id, string companyName, string position, JobApplicationStatus status, DateTime dateApplied)
        : NewJobApplicationDto(companyName, position, dateApplied)
    {
        public ulong Id { get; set; } = id;
        public JobApplicationStatus Status { get; set; } = status;

        public JobApplicationDto(IJobApplication jobApplication) : this(
            jobApplication.Id,
            jobApplication.CompanyName,
            jobApplication.Position,
            jobApplication.Status,
            jobApplication.DateApplied)
        {
        }

    }
}

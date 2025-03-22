using JobApplicationTracker.Server.Data;

namespace JobApplicationTracker.Server.DTOs
{
    public class JobApplicationDto(string companyName, string position, JobApplicationStatus status, DateTime dateApplied)
    {
        public string CompanyName { get; set; } = companyName;
        public string Position { get; set; } = position;
        public JobApplicationStatus Status { get; set; } = status;
        public DateTime DateApplied { get; set; } = dateApplied;

        public JobApplicationDto(IJobApplication jobApplication) : this(jobApplication.CompanyName, 
            jobApplication.Position,
            jobApplication.Status,
            jobApplication.DateApplied)
        {
        }
    }
}

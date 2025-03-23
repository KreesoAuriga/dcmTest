namespace JobApplicationTracker.Server.DTOs
{
    public class JobApplicationsListDto(string userEmail, JobApplicationDto[] jobApplications)
    {
        public string UserEmail { get; } = userEmail;
        public JobApplicationDto[] JobApplications { get; } = jobApplications;

    }
}

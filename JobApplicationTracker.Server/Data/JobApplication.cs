namespace JobApplicationTracker.Server.Data
{
    public interface IJobApplication
    {
        string CompanyName { get; }
        string Position { get; }
        JobApplicationStatus Status { get; }
        DateTime DateApplied { get; }
    }

    internal class JobApplication : IJobApplication
    {
        public string CompanyName             { get; set; }
        public string Position                { get; set; }
        public JobApplicationStatus Status    { get; set; }
        public DateTime DateApplied           { get; set; }
    }
}

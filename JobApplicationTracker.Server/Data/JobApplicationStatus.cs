namespace JobApplicationTracker.Server.Data
{
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
}

using JobApplicationTracker.Server.Data;
using JobApplicationTracker.Server.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Server.Controllers
{

    [Route("api/{userEmail}/[controller]")]
    [ApiController]
    public class JobApplicationsController : Controller
    {


        private IDbController _dbController;
        private ILogger<JobApplicationsController> _logger;

        public JobApplicationsController(ILogger<JobApplicationsController> logger, IDbController dbController)
        {
            _logger = logger;
            _dbController = dbController;
        }

        /// <summary>
        /// Gets all job applications for the specified user.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAllApplications(string userEmail)
        {
            var user = await _dbController.GetUserByEmailAsync(userEmail);
            if (user is null)
                return NotFound();

            var jobApplications = user.JobApplications.Select(a => new JobApplicationDto(a)).ToArray();

            //var result = new JobApplicationsListDto(userEmail, jobApplications);
            return Ok(jobApplications);
        }

        /// <summary>
        /// Gets the application with the specified job id for the specified user.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetApplication(string userEmail, ulong id)
        {
            var user = await _dbController.GetUserByEmailAsync(userEmail);
            if (user is null)
                return BadRequest("The specified user was not found");

            var application = user.JobApplications.FirstOrDefault(a => a.Id == id);
            if (application is null)
                return NotFound();

            var result = new JobApplicationDto(application);
            return Ok(result);
        }

        /// <summary>
        /// Adds a new job application for the specified user.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="applicationDto"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPut]
        [Route("Add")]
        public async Task<ActionResult> AddApplication(string userEmail, NewJobApplicationDto applicationDto)
        {
            var user = await _dbController.GetUserByEmailAsync(userEmail);
            if (user is null)
                return BadRequest("The specified user was not found");

            var newApplication = new JobApplication
            {
                CompanyName = applicationDto.CompanyName,
                Position    = applicationDto.Position   ,
                DateApplied = applicationDto.DateApplied,
            };

            user.AddJobApplication(newApplication);

            var saveSuccess = await _dbController.SaveChangesToUserAsync(user);
            if (!saveSuccess)
                throw new InvalidOperationException($"Unexpected failure saving changes to user:{user.Email}");

            return Ok(newApplication.Id);
        }

        /// <summary>
        /// Updates the provided job application.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="applicationDto"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">The changes failed to save.</exception>
        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult> UpdateApplication(string userEmail, JobApplicationDto applicationDto)
        {
            var user = await _dbController.GetUserByEmailAsync(userEmail);
            if (user is null)
                return BadRequest("The specified user was not found");
            
            var application = user.JobApplications.FirstOrDefault(j => j.Id == applicationDto.Id);
            if (application is null)
                return NotFound();

            application.CompanyName = applicationDto.CompanyName;
            application.Position = applicationDto.Position;
            application.Status = applicationDto.Status;
            application.DateApplied = applicationDto.DateApplied;
            
            var saveSuccess = await _dbController.SaveChangesToUserAsync(user);
            if (!saveSuccess)
                throw new InvalidOperationException($"Unexpected failure saving changes to user:{user.Email}");

            return Ok();
        }

        /// <summary>
        /// Delete's the application for the specified id.
        /// </summary>
        /// <returns>Ok on success. NotFound if the userEmail is not found. </returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> DeleteApplication(string userEmail, ulong id)
        {
            var user = await _dbController.GetUserByEmailAsync(userEmail);
            if (user is null)
                return BadRequest("The specified user was not found");

            var application = user.JobApplications.FirstOrDefault(j => j.Id == id);
            if (application is null)
                return NotFound();

            user.RemoveJobApplication(application);

            var saveSuccess = await _dbController.SaveChangesToUserAsync(user);
            if (!saveSuccess)
                throw new InvalidOperationException($"Unexpected failure saving changes to user:{user.Email}");

            return Ok();
        }

    }

}

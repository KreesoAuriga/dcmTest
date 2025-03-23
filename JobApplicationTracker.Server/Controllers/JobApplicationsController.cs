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

        [HttpGet]
        public async Task<ActionResult> GetAllApplications(string userEmail)
        {
            var user = await _dbController.GetUserByEmailAsync(userEmail);
            if (user is null)
                return NotFound();

            var jobApplications = user.JobApplications.Select(a => new JobApplicationDto(a)).ToArray();

            var result = new JobApplicationsListDto(userEmail, jobApplications);
            return Ok(result);
        }

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

        [HttpPost]
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

            return Ok();
        }


        /*
                public IActionResult Index()
                {
                    return View();
                }*/
    }

}

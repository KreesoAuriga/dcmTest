using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Server.Controllers
{
    public class UserController :Controller
    {

        private ILogger _logger;

        public UserController(ILogger logger)
        {
            _logger = logger; 
        }
    }
}

using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using JobApplicationTracker.Server.Controllers;
using JobApplicationTracker.Server.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;
using ILogger = Castle.Core.Logging.ILogger;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ServerTests
{
    public class DbControllerTests : IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private AppDbContext _dbContext;
        private Mock<ILogger<DbController>> _logger;
        private DbController _dbController;
        private TestWebApplicationFactory<Program> _factory;

        public DbControllerTests(TestWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _logger = new Mock<ILogger<DbController>>();
            _factory = factory;


        }

        [Fact]
        public async void AddUser()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var dbContext = scopedServices.GetRequiredService<AppDbContext>(); // Get the AppDbContext from DI
                var dbController = new DbController(dbContext, _logger.Object); // Inject AppDbContext into DbController

                string email = "foobar@foo.com";
                string normalizedEmail = email.ToUpperInvariant();
                string userName = "foobar";

                var user = new User()
                {
                    Email = email,
                    UserName = userName,
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(user),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _client.PutAsync($"/api/User", content);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var addUserSuccess = await dbController.AddUserAsync("foobar@foobar.com", "foobar");
                Xunit.Assert.True(addUserSuccess);

                var searchResult = dbContext.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);

                Xunit.Assert.NotNull(searchResult);
            }
        }

        [Fact]
        public async void GetUser()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var dbContext = scopedServices.GetRequiredService<AppDbContext>(); // Get the AppDbContext from DI
                var dbController = new DbController(dbContext, _logger.Object); // Inject AppDbContext into DbController

                string email = "foobar@foo.com";
                string normalizedEmail = email.ToUpperInvariant();
                string userName = "foobar";

                var user = new User()
                {
                    Email = email,
                    UserName = userName,
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(user),
                    Encoding.UTF8,
                    "application/json"
                );

                var putResponse = await _client.PutAsync($"/api/User", content);
                Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

                var addUserSuccess = await dbController.AddUserAsync("foobar@foobar.com", "foobar");
                Xunit.Assert.True(addUserSuccess);


                var getResponse = await _client.GetAsync($"/api/User/{Uri.EscapeDataString(email)}");
                Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            }
        }
    }
}
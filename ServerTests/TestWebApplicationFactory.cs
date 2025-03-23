using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using JobApplicationTracker.Server;
using JobApplicationTracker.Server.Data;
using System.Linq;
using JobApplicationTracker.Server.Data;
using Microsoft.AspNetCore.Hosting;

namespace ServerTests
{
    public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

            builder.ConfigureServices(services =>
            {
                // Remove existing DbContext registration
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add a new in-memory database
                services.AddDbContext<AppDbContext>(options => { options.UseInMemoryDatabase("jobAppTracker"); });

                // Ensure database is created 
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();
                    //SeedDatabase(db);
                }
            });
        }
/*
        private static void SeedDatabase(MyDbContext db)
        {
            db.Users.AddRange(
                new User { Id = 1, Name = "Test User 1" },
                new User { Id = 2, Name = "Test User 2" }
            );
            db.SaveChanges();
        }*/
    }
}
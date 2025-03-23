using JobApplicationTracker.Server.Data;

namespace JobApplicationTracker.Server.DTOs
{
    public class NewUserDto(string? email, string? userName)
    {
        public string?              Email            { get; set; } = email;
        public string?              UserName         { get; set; } = userName;

        public NewUserDto(IUser user) : this(user.Email, user.UserName)
        {

        }

        public NewUserDto() : this(null, null)
        {
        }
    }

    public class UserDto(string? email, string? userName, JobApplicationDto[]? jobApplications) : NewUserDto
    {
        public string? Email { get; set; } = email;
        public string? UserName { get; set; } = userName;
        public JobApplicationDto[]? JobApplications { get; set; } = jobApplications;

        public UserDto(IUser user) : this(user.Email, user.UserName,
            user.JobApplications.Select(j => new JobApplicationDto(j)).ToArray())
        {

        }

        public UserDto() : this(null, null, null)
        {
        }
    }

    public class UsersDto(UserDto[] users)
    {
        public UserDto[] Users { get; set; } = users;


    }
}

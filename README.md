Programming test for a software developer position.


<h1>Scenario</h1>
You need to build a Job Application Tracker where users can add, update, and view job applications they have submitted. The system should keep track of jobs applied for, status updates, and the date applied.

Requirements:
ASP.net core web API
EFCore with in-memory db or SQLite
Repository pattern and dependency injection
Bonus: use SwaggerUI
Frontend: React or Angular allows for viewing all applicaitons, adding a new application, editing an existing application.
Application consists of Company Name, Position, Status, Date Applied
use of async/await


<h1>Notes on solution:</h1>
Authored in Visual Studio 2022 in Windows 10, as a cross platform ASP.NET core project, using EntityFramework for database interaction, and React for the front-end.

The initial database can be created before first run via these commands from the JobApplication.Server project folder.
dotnet ef migrations add InitialCreate
dotnet ef database update

Can be built and run from visual studio or via 'dotnet run' from the JobApplicationTracker.Server project folder.
Navigate to https://localhost:7056

notes:
- In the interest of time, this implementation does not yet provide any affordance for sign-in credentials and authorization.
- The asp.net server project defines interfaces for the user and jobApplication. It also defines an interface for 'dbController' which provides functionality for adding and getting instances of users by interface.
  - DbController is dependency injected, to allow for exact database implementation to be agnostic. A final implementation of this project would have the implementation of IDbController defined in a separate project.
- In the absence of a dedicated page for user management, which would normally be locked behind some form of sign-in credentials, the React front-end page implements the functionality for adding and deleting a user.
- Functionality to get a list of all users exists, and can be tested using the Swagger UI. Launching the project for debugging inside Visual Studio will also launch a browser window with the Swagger UI.
- A test project has been created, but in the interest of time only implements tests for AddUser and GetUser, but provides the template for implementing a more complete set of automated tests.

- Uses SQLite as the databse. The test project uses an in-memory database for testing purposes.

- In the interest of time, full xml summaries of classes exist for only some of the key components. Final implementation would make lack of method/class summaries a warning as error on build.

- The code App.tsx should be refactored for better organization and maintainability, rather than it's current state of being all in a single file.

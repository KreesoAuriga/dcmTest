Programming test for a software developer position.

Scenario
You need to build a Job Application Tracker where users can add, update, and view job applications they have submitted. The system should keep track of jobs applied for, status updates, and the date applied.

Authored in Visual Studio 2022 as a cross platform ASP.NET core project, using React for the front-end.
Can be build and run from visual studio or via 'dotnet run' from the JobApplicationTracker.Server project folder.
Navigate to https://localhost:7056

notes:
- In there interest of time, this implementation does not yet provide any affordance for sign-in credentials and authorization.
- The asp.net server project defines interfaces for the user and jobApplication. It also defines an interface for 'dbController' which provides functionality for adding and getting instances of users by interface.
  - DbController is dependency injected, to allow for exact database implementation to be agnostic. A final implementation of this project would have the implementation of IDbController defined in a separate project.
- In the absence of a dedicated page for user management, which would normally be locked behind some form of sign-in credentials, the React front-end page implements the functionality for adding and deleting a user.
- Functionality to get a list of all users exists, and can be tested using the Swagger UI. Launching the project for debugging inside Visual Studio will also launch a browser window with the Swagger UI.
- A test project has been created, but in the interest of time only implements tests for AddUser and GetUser, but provides the template for implementing a more complete set of automated tests.

- Uses SQLite as the databse. The test project uses an in-memory database for testing purposes.

- In the interest of time, full xml summaries of classes exist for only some of the key components. Final implementation would make lack of method/class summaries a warning as error on build.

- The code App.tsx should be refactored for better organization and maintainability, rather than it's current state of being all in a single file.

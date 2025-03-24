Programming test for a software developer position.

Scenario
You need to build a Job Application Tracker where users can add, update, and view job applications they have submitted. The system should keep track of jobs applied for, status updates, and the date applied.

Authored in Visual Studio 2022.
Can be build and run from visual studio or via 'dotnet run' from the JobApplicationTracker.Server project folder.
Navigate to https://localhost:7056

notes:
- In there interest of time, this implementation does not yet provide any affordance for sign-in credentials and authorization.
  - The asp.net server project defines interfaces for the user and jobApplication. It also defines an interface for 'dbController' which provides functionality for adding and getting instances of users by interface.
- In the absence of a dedicated page for user management, which would normally be locked behind some form of sign-in credentials, the React front-end page implements the functionality for adding and deleting a user.
- Functionality to get a list of all users exists, and can be tested using the Swagger UI. Launching the project for debugging inside Visual Studio will also launch a browser window with the Swagger UI.
- A test project has been created, but in the interest of time only implements tests for AddUser and GetUser, but provides the template for implementing a more complete set of automated tests.

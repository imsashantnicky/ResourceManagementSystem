
## Project Design

- **Project structure:** I have organized the project into separate folders to keep the code clean, simple and easy to understand.
- **Allocation management:** The allocation module stores the employee ID, project ID, start date, end date and allocation percentage.
- **Employment type:** I have used an enum for different employment types because they do not require separate business logic at the moment.
- **Repository pattern:** I have used repository interfaces so that JSON storage can be replaced with a database like SQL Server in the future.
- **Matching strategy:** I have used the `IMatchingStrategy` interface to keep employee skill-matching logic separate and make it easier to update later.
- **Program setup:** `Program.cs` is the main entry point. It registers dependencies and starts the application.
- **Console menu and seed data:** These are handled separately to keep the main program organized.
- **Error handling:** The application uses `OperationResult` for expected allocation errors and argument exceptions for invalid domain data.

## SOLID Principles

- **Single Responsibility Principle (SRP):** Each class has a specific responsibility, such as managing employees, handling allocations, saving data or displaying the console menu.
- **Open/Closed Principle (OCP):** New matching strategies or storage implementations can be added without making unnecessary changes to existing code.
- **Liskov Substitution Principle (LSP):** Different repository implementations can be used wherever their corresponding interfaces are required.
- **Interface Segregation Principle (ISP):** Separate repository interfaces are used for employees, projects and allocations.
- **Dependency Inversion Principle (DIP):** Application services depend on repository interfaces instead of directly depending on JSON storage classes.

## Testing

I have included unit tests for important scenarios, including date overlap validation and employee skill matching.

To build and test the project, run:

```bash
dotnet build
dotnet test
```

The project builds successfully, and all three included unit tests passed in my local environment. Additional manual testing can be done to check allocation validation, inactive employee handling and JSON data persistence.

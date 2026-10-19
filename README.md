# Resource Management System

## About the Project

I have developed this project as a console-based Resource Management System using C# and .NET 10. It helps manage employees, projects and employee allocations.

The project is organized into separate folders to keep the code clean, structured and easy to maintain. JSON files are used to store application data.

## Features

- View the list of employees.
- View the list of projects.
- Allocate employees to projects.
- Match employees with projects based on their skills.
- Deactivate employees.
- Validate allocation dates and prevent overlapping allocations.
- Store application data in JSON files.
- Seed the application with initial sample data.

## Project Structure

- `Domain/` – Contains domain models, enums and business rules.
- `Application/` – Contains application services and repository interfaces.
- `Infrastructure/` – Handles JSON data storage and repository implementations.
- `Presentation/` – Contains the console menu and user interaction.
- `Tests/` – Contains unit tests.
- `Program.cs` – Entry point of the application.
- `DESIGN.md` – Explains the project design and SOLID principles.

## Requirements

- .NET 10 SDK
- A terminal or IDE such as Visual Studio Code or Visual Studio

## How to Run

Open the terminal in the project root directory and run:

```bash
dotnet restore
dotnet build
dotnet run
```

To run the unit tests, execute:

```bash
dotnet test Tests/ResourceManagementSystem.Tests.csproj
```

## Sample Data

On the first run, the application seeds the following sample data:

- 15 employees
- 5 projects
- 10 allocations

Employee IDs, project IDs, allocation IDs and their related foreign keys use the `int` data type.

JSON data is stored in the application's output `data` directory.

## Technologies Used

- C#
- .NET 10
- JSON file persistence
- xUnit for unit testing
- SOLID principles

## Testing

The project builds successfully, and all three included unit tests passed. The tests cover date overlap validation and employee skill matching.


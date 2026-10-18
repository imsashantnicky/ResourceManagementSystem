# RMS Console Application

This is a **single console application project**. There is no `src` folder. The modular folders are directly inside the project root.

## Run
Install the .NET 10 SDK, then run from the project root:

```bash
dotnet restore
dotnet build
dotnet run
dotnet test Tests/ResourceManagementSystem.Tests.csproj
```

All entity identifiers (`Employee.Id`, `Project.Id`, `Allocation.Id`, and foreign keys) use `int`. Repository interfaces are grouped under `Application/Repository`. The first run seeds 15 employees, 5 projects and 10 allocations. JSON data is stored in the application's output `data` directory.

Menu: list employees, list projects, allocate an employee, match employees to project skills, deactivate an employee, exit.
# ResourceManagementSystem

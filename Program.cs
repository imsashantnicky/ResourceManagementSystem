using Microsoft.Extensions.DependencyInjection;
using ResourceManagementSystem.Application.Abstractions;
using ResourceManagementSystem.Application.Repository;
using ResourceManagementSystem.Application.Services;
using ResourceManagementSystem.Infrastructure.Notifications;
using ResourceManagementSystem.Infrastructure.Persistence;
using ResourceManagementSystem.Presentation;

var services = new ServiceCollection();

var data = Path.Combine(AppContext.BaseDirectory, "data");

services.AddSingleton<IEmployeeRepository>(
    _ => new JsonEmployeeRepository(
        Path.Combine(data, "employees.json")));

services.AddSingleton<IProjectRepository>(
    _ => new JsonProjectRepository(
        Path.Combine(data, "projects.json")));

services.AddSingleton<IAllocationRepository>(
    _ => new JsonAllocationRepository(
        Path.Combine(data, "allocations.json")));

services.AddSingleton<IMatchingStrategy, SkillMatchingStrategy>();

services.AddSingleton<INotificationService, ConsoleNotificationService>();

services.AddSingleton<RmsService>();

services.AddSingleton<SeedData>();

services.AddSingleton<ConsoleMenu>();

using var provider = services.BuildServiceProvider();

await provider
    .GetRequiredService<SeedData>()
    .EnsureSeededAsync();

await provider
    .GetRequiredService<ConsoleMenu>()
    .RunAsync();
using ResourceManagementSystem.Application.Abstractions;
using ResourceManagementSystem.Domain.Entities;

namespace ResourceManagementSystem.Infrastructure.Notifications;

public sealed class ConsoleNotificationService : INotificationService
{
    public void Allocated(Employee e, Project p, Allocation a)
    {
        Console.WriteLine(
            $"[Notification] {e.Name} allocated to {p.Name} ({a.Percentage}%).");
    }

    public void Released(Employee e, Project p, Allocation a)
    {
        Console.WriteLine(
            $"[Notification] {e.Name} released from {p.Name}.");
    }
}
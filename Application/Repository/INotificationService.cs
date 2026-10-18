using ResourceManagementSystem.Domain.Entities;

namespace ResourceManagementSystem.Application.Abstractions;

public interface INotificationService
{
    void Allocated(Employee employee, Project project, Allocation allocation);

    void Released(Employee employee, Project project, Allocation allocation);
}
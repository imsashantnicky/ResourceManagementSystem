namespace ResourceManagementSystem.Domain.Entities;

public sealed record Allocation(
    int Id,
    int EmployeeId,
    int ProjectId,
    DateOnly StartDate,
    DateOnly EndDate,
    decimal Percentage)
{
    public bool Overlaps(DateOnly start, DateOnly end)
        => StartDate <= end && start <= EndDate;
}
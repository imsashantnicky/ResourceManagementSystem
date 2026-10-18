using ResourceManagementSystem.Application.Abstractions;
using ResourceManagementSystem.Application.Repository;
using ResourceManagementSystem.Domain.Entities;

namespace ResourceManagementSystem.Application.Services;

public sealed record OperationResult(bool Success, string Message)
{
    public static OperationResult Ok(string m) => new(true, m);

    public static OperationResult Fail(string m) => new(false, m);
}

public sealed class RmsService(
    IEmployeeRepository employees,
    IProjectRepository projects,
    IAllocationRepository allocations,
    IMatchingStrategy matcher,
    INotificationService notifications)
{
    public Task<IReadOnlyList<Employee>> GetEmployeesAsync(
        CancellationToken ct = default)
        => employees.GetAllAsync(ct);

    public Task<IReadOnlyList<Project>> GetProjectsAsync(
        CancellationToken ct = default)
        => projects.GetAllAsync(ct);

    public async Task<OperationResult> DeactivateEmployeeAsync(
        int id,
        CancellationToken ct = default)
    {
        var e = await employees.GetByIdAsync(id, ct);

        if (e is null)
        {
            return OperationResult.Fail("Employee not found.");
        }

        e.SetActive(false);

        await employees.UpsertAsync(e, ct);

        return OperationResult.Ok("Employee deactivated.");
    }

    public async Task<OperationResult> AllocateAsync(
        int employeeId,
        int projectId,
        DateOnly start,
        DateOnly end,
        decimal percentage,
        CancellationToken ct = default)
    {
        var e = await employees.GetByIdAsync(employeeId, ct);
        var p = await projects.GetByIdAsync(projectId, ct);

        if (e is null)
        {
            return OperationResult.Fail("Employee not found.");
        }

        if (p is null)
        {
            return OperationResult.Fail("Project not found.");
        }

        if (!e.IsActive)
        {
            return OperationResult.Fail(
                "Inactive employees cannot be allocated.");
        }

        if (end < start || percentage <= 0 || percentage > 100)
        {
            return OperationResult.Fail("Invalid dates or percentage.");
        }

        if (start < p.StartDate || end > p.EndDate)
        {
            return OperationResult.Fail(
                "Allocation must be within project dates.");
        }

        var old = (await allocations.GetAllAsync(ct))
            .Where(a =>
                a.EmployeeId == employeeId &&
                a.Overlaps(start, end))
            .ToList();

        var days = new SortedSet<DateOnly> { start };

        foreach (var a in old)
        {
            days.Add(a.StartDate < start ? start : a.StartDate);

            if (a.EndDate < end)
            {
                days.Add(a.EndDate.AddDays(1));
            }
        }

        foreach (var day in days.Where(d => d >= start && d <= end))
        {
            var existingPercentage = old
                .Where(a => a.StartDate <= day && day <= a.EndDate)
                .Sum(a => a.Percentage);

            if (existingPercentage + percentage > 100)
            {
                return OperationResult.Fail(
                    "Allocation would exceed 100% during an overlapping period.");
            }
        }

        var allocation = new Allocation(
            (await allocations.GetAllAsync(ct))
                .Select(a => a.Id)
                .DefaultIfEmpty(0)
                .Max() + 1,
            employeeId,
            projectId,
            start,
            end,
            percentage);

        await allocations.UpsertAsync(allocation, ct);

        notifications.Allocated(e, p, allocation);

        return OperationResult.Ok("Employee allocated.");
    }

    public async Task<IReadOnlyList<EmployeeMatch>> MatchEmployeesAsync(
        int projectId,
        DateOnly start,
        DateOnly end,
        CancellationToken ct = default)
    {
        var p = await projects.GetByIdAsync(projectId, ct);

        if (p is null ||
            end < start ||
            start < p.StartDate ||
            end > p.EndDate)
        {
            return [];
        }

        var es = await employees.GetAllAsync(ct);
        var als = await allocations.GetAllAsync(ct);

        return es
            .Where(e => e.IsActive)
            .Select(e => new
            {
                Employee = e,
                Peak = Peak(
                    als.Where(a =>
                        a.EmployeeId == e.Id &&
                        a.Overlaps(start, end)),
                    start,
                    end)
            })
            .Where(x => x.Peak < 100)
            .Select(x => new EmployeeMatch(
                x.Employee.Id,
                x.Employee.Name,
                matcher.Score(x.Employee, p)))
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Name)
            .ToList();
    }

    private static decimal Peak(
        IEnumerable<Allocation> source,
        DateOnly start,
        DateOnly end)
    {
        var a = source.ToList();

        var days = new SortedSet<DateOnly> { start };

        foreach (var x in a)
        {
            days.Add(x.StartDate < start ? start : x.StartDate);

            if (x.EndDate < end)
            {
                days.Add(x.EndDate.AddDays(1));
            }
        }

        return days
            .Select(d => a
                .Where(x => x.StartDate <= d && d <= x.EndDate)
                .Sum(x => x.Percentage))
            .DefaultIfEmpty(0)
            .Max();
    }
}
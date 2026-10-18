using ResourceManagementSystem.Domain.Enums;
using ResourceManagementSystem.Domain.ValueObjects;

namespace ResourceManagementSystem.Domain.Entities;

public sealed class Project
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public string Client { get; private set; }

    public DateOnly StartDate { get; private set; }

    public DateOnly EndDate { get; private set; }

    public ProjectStatus Status { get; private set; }

    public List<SkillRequirement> RequiredSkills { get; private set; }

    public Project(
        int id,
        string name,
        string client,
        DateOnly startDate,
        DateOnly endDate,
        ProjectStatus status,
        List<SkillRequirement>? requiredSkills = null)
    {
        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(client))
        {
            throw new ArgumentException(
                "Project name and client are required.");
        }

        if (endDate < startDate)
        {
            throw new ArgumentException(
                "End date must not precede start date.");
        }

        Id = id;
        Name = name.Trim();
        Client = client.Trim();
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        RequiredSkills = requiredSkills ?? [];
    }
}
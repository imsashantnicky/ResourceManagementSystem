using ResourceManagementSystem.Domain.Entities;

namespace ResourceManagementSystem.Application.Services;

public sealed class SkillMatchingStrategy : IMatchingStrategy
{
    public int Score(Employee employee, Project project)
    {
        if (project.RequiredSkills.Count == 0)
        {
            return 100;
        }

        var matched = project.RequiredSkills.Count(r =>
            employee.Skills.Any(s =>
                string.Equals(
                    s.Name,
                    r.Name,
                    StringComparison.OrdinalIgnoreCase
                ) &&
                s.Proficiency >= r.MinimumProficiency
            )
        );

        return (int)Math.Round(
            100.0 * matched / project.RequiredSkills.Count
        );
    }
}
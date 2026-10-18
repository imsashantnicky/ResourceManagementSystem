using ResourceManagementSystem.Application.Services;
using ResourceManagementSystem.Domain.Entities;
using ResourceManagementSystem.Domain.Enums;
using ResourceManagementSystem.Domain.ValueObjects;
using Xunit;

namespace ResourceManagementSystem.Tests;

public sealed class MatchingTests
{
    [Fact]
    public void ScoreIsPercentOfRequirementsMatched()
    {
        var e = new Employee(
            1,
            "Test",
            "test@example.com",
            "Engineer",
            "Engineering",
            EmploymentType.FullTime,
            100,
            skills:
            [
                new Skill("C#", ProficiencyLevel.Expert),
                new Skill("SQL", ProficiencyLevel.Beginner)
            ]);

        var p = new Project(
            1,
            "Project",
            "Client",
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 12, 31),
            ProjectStatus.Active,
            [
                new SkillRequirement("C#", ProficiencyLevel.Advanced),
                new SkillRequirement("SQL", ProficiencyLevel.Advanced)
            ]);

        Assert.Equal(50, new SkillMatchingStrategy().Score(e, p));
    }
}
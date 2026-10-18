using ResourceManagementSystem.Application.Abstractions;
using ResourceManagementSystem.Application.Repository;
using ResourceManagementSystem.Domain.Entities;
using ResourceManagementSystem.Domain.Enums;
using ResourceManagementSystem.Domain.ValueObjects;

namespace ResourceManagementSystem.Presentation;

public sealed class SeedData(
    IEmployeeRepository employees,
    IProjectRepository projects,
    IAllocationRepository allocations)
{
    public async Task EnsureSeededAsync()
    {
        if ((await employees.GetAllAsync()).Count > 0)
        {
            return;
        }

        string[] names =
        [
            "Aarav", "Diya", "Ishaan", "Meera", "Kabir",
            "Anaya", "Vivaan", "Sara", "Arjun", "Ira",
            "Reyansh", "Myra", "Advait", "Kiara", "Rohan"
        ];

        string[] skills =
        [
            "C#", "SQL", "Azure", "React", "Testing", "DevOps"
        ];

        var es = new List<Employee>();

        for (int i = 0; i < names.Length; i++)
        {
            var e = new Employee(
                i + 1,
                names[i],
                $"employee{i + 1}@technova.example",
                i % 3 == 0 ? "Senior Engineer" : "Software Engineer",
                new[] { "Engineering", "QA", "Cloud" }[i % 3],
                i % 5 == 0
                    ? EmploymentType.Contract
                    : EmploymentType.FullTime,
                500 + i * 25,
                skills:
                [
                    new Skill(
                        skills[i % 6],
                        (ProficiencyLevel)(2 + i % 4)),

                    new Skill(
                        skills[(i + 1) % 6],
                        ProficiencyLevel.Intermediate)
                ]);

            es.Add(e);

            await employees.UpsertAsync(e);
        }

        var today = DateOnly.FromDateTime(DateTime.Today);

        string[] pnames =
        [
            "Client Portal",
            "Cloud Migration",
            "Mobile Banking",
            "QA Automation",
            "Data Platform"
        ];

        string[] clients =
        [
            "Northwind",
            "Contoso",
            "Fabrikam",
            "Adventure Works",
            "Tailspin"
        ];

        var ps = new List<Project>();

        for (int i = 0; i < pnames.Length; i++)
        {
            var p = new Project(
                i + 1,
                pnames[i],
                clients[i],
                today.AddDays(-30),
                today.AddDays(180),
                ProjectStatus.Active,
                [
                    new SkillRequirement(
                        skills[i],
                        ProficiencyLevel.Basic)
                ]);

            ps.Add(p);

            await projects.UpsertAsync(p);
        }

        for (int i = 0; i < 10; i++)
        {
            await allocations.UpsertAsync(
                new Allocation(
                    i + 1,
                    es[i].Id,
                    ps[i % 5].Id,
                    today.AddDays(-5),
                    today.AddDays(60),
                    i % 3 == 0 ? 50 : 75));
        }
    }
}
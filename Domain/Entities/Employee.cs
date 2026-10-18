using ResourceManagementSystem.Domain.Enums;
using ResourceManagementSystem.Domain.ValueObjects;

namespace ResourceManagementSystem.Domain.Entities;

public sealed class Employee
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public string Email { get; private set; }

    public string Designation { get; private set; }

    public string Department { get; private set; }

    public EmploymentType EmploymentType { get; private set; }

    public decimal CostRate { get; private set; }

    public bool IsActive { get; private set; }

    public List<Skill> Skills { get; private set; }

    public Employee(
        int id,
        string name,
        string email,
        string designation,
        string department,
        EmploymentType employmentType,
        decimal costRate,
        bool isActive = true,
        List<Skill>? skills = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Name is required.",
                nameof(name));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException(
                "Email is required.",
                nameof(email));
        }

        if (costRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(costRate));
        }

        Id = id;
        Name = name.Trim();
        Email = email.Trim();
        Designation = designation;
        Department = department;
        EmploymentType = employmentType;
        CostRate = costRate;
        IsActive = isActive;
        Skills = skills ?? [];
    }

    public void UpdateProfile(
        string name,
        string email,
        string designation,
        string department,
        EmploymentType employmentType,
        decimal costRate)
    {
        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException(
                "Name and email are required.");
        }

        if (costRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(costRate));
        }

        Name = name.Trim();
        Email = email.Trim();
        Designation = designation;
        Department = department;
        EmploymentType = employmentType;
        CostRate = costRate;
    }

    public void SetActive(bool active)
        => IsActive = active;

    public void AddOrUpdateSkill(Skill skill)
    {
        var i = Skills.FindIndex(s =>
            string.Equals(
                s.Name,
                skill.Name,
                StringComparison.OrdinalIgnoreCase));

        if (i >= 0)
        {
            Skills[i] = skill;
        }
        else
        {
            Skills.Add(skill);
        }
    }
}
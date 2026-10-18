using ResourceManagementSystem.Domain.Enums;

namespace ResourceManagementSystem.Domain.ValueObjects;

public sealed record Skill(
    string Name,
    ProficiencyLevel Proficiency
);

public sealed record SkillRequirement(
    string Name,
    ProficiencyLevel MinimumProficiency
);
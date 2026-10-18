using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManagementSystem.Application.Services;

public sealed record EmployeeMatch(
    int EmployeeId,
    string Name,
    int Score
);
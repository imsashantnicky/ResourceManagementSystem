using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ResourceManagementSystem.Domain.Entities;

namespace ResourceManagementSystem.Application.Services;

public interface IMatchingStrategy
{
    int Score(Employee employee, Project project);
}
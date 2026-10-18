using ResourceManagementSystem.Application.Services;

namespace ResourceManagementSystem.Presentation;

public sealed class ConsoleMenu(RmsService rms)
{
    public async Task RunAsync()
    {
        Console.WriteLine("=== TechNova Resource Management System ===");

        while (true)
        {
            Console.WriteLine(
                "\n1. List employees\n" +
                "2. List projects\n" +
                "3. Allocate employee\n" +
                "4. Match employees\n" +
                "5. Deactivate employee\n" +
                "0. Exit");

            Console.Write("Choose: ");

            switch (Console.ReadLine())
            {
                case "1":
                    foreach (var e in await rms.GetEmployeesAsync())
                    {
                        Console.WriteLine(
                            $"{e.Id} | {e.Name} | {e.Department} | " +
                            $"{e.EmploymentType} | Active={e.IsActive}");
                    }
                    break;

                case "2":
                    foreach (var p in await rms.GetProjectsAsync())
                    {
                        Console.WriteLine(
                            $"{p.Id} | {p.Name} | {p.Client} | " +
                            $"{p.StartDate:yyyy-MM-dd} - " +
                            $"{p.EndDate:yyyy-MM-dd} | {p.Status}");
                    }
                    break;

                case "3":
                    await AllocateAsync();
                    break;

                case "4":
                    await MatchAsync();
                    break;

                case "5":
                    await DeactivateAsync();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private async Task AllocateAsync()
    {
        var es = await rms.GetEmployeesAsync();
        var ps = await rms.GetProjectsAsync();

        for (int i = 0; i < es.Count; i++)
        {
            Console.WriteLine($"E{i + 1}. {es[i].Name}");
        }

        Console.Write("Employee #: ");

        if (!int.TryParse(Console.ReadLine(), out int ei) ||
            ei < 1 ||
            ei > es.Count)
        {
            Console.WriteLine("Invalid employee.");
            return;
        }

        for (int i = 0; i < ps.Count; i++)
        {
            Console.WriteLine($"P{i + 1}. {ps[i].Name}");
        }

        Console.Write("Project #: ");

        if (!int.TryParse(Console.ReadLine(), out int pi) ||
            pi < 1 ||
            pi > ps.Count)
        {
            Console.WriteLine("Invalid project.");
            return;
        }

        Console.Write("Start date yyyy-MM-dd: ");

        if (!DateOnly.TryParse(Console.ReadLine(), out var start))
        {
            Console.WriteLine("Invalid date.");
            return;
        }

        Console.Write("End date yyyy-MM-dd: ");

        if (!DateOnly.TryParse(Console.ReadLine(), out var end))
        {
            Console.WriteLine("Invalid date.");
            return;
        }

        Console.Write("Percentage: ");

        if (!decimal.TryParse(Console.ReadLine(), out var percent))
        {
            Console.WriteLine("Invalid percentage.");
            return;
        }

        var result = await rms.AllocateAsync(
            es[ei - 1].Id,
            ps[pi - 1].Id,
            start,
            end,
            percent);

        Console.WriteLine(result.Message);
    }

    private async Task MatchAsync()
    {
        var ps = await rms.GetProjectsAsync();

        for (int i = 0; i < ps.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {ps[i].Name}");
        }

        Console.Write("Project #: ");

        if (!int.TryParse(Console.ReadLine(), out int n) ||
            n < 1 ||
            n > ps.Count)
        {
            Console.WriteLine("Invalid project.");
            return;
        }

        var p = ps[n - 1];

        foreach (var m in await rms.MatchEmployeesAsync(
            p.Id,
            p.StartDate,
            p.EndDate))
        {
            Console.WriteLine(
                $"{m.Name} | skill match {m.Score}%");
        }
    }

    private async Task DeactivateAsync()
    {
        var es = await rms.GetEmployeesAsync();

        for (int i = 0; i < es.Count; i++)
        {
            Console.WriteLine(
                $"{i + 1}. {es[i].Name} Active={es[i].IsActive}");
        }

        Console.Write("Employee #: ");

        if (!int.TryParse(Console.ReadLine(), out int n) ||
            n < 1 ||
            n > es.Count)
        {
            Console.WriteLine("Invalid employee.");
            return;
        }

        var result = await rms.DeactivateEmployeeAsync(es[n - 1].Id);

        Console.WriteLine(result.Message);
    }
}
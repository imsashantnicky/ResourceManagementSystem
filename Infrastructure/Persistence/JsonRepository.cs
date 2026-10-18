using System.Text.Json;
using ResourceManagementSystem.Application.Repository;
using ResourceManagementSystem.Domain.Entities;

namespace ResourceManagementSystem.Infrastructure.Persistence;

public class JsonRepository<T>(string path) : IRepository<T>
    where T : class
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true
    };

    public async Task<IReadOnlyList<T>> GetAllAsync(
        CancellationToken ct = default)
    {
        if (!File.Exists(path))
        {
            return [];
        }

        await using var s = File.OpenRead(path);

        return await JsonSerializer.DeserializeAsync<List<T>>(
            s,
            Options,
            ct) ?? [];
    }

    public async Task<T?> GetByIdAsync(
        int id,
        CancellationToken ct = default)
    {
        var all = await GetAllAsync(ct);
        var prop = typeof(T).GetProperty("Id");

        return all.FirstOrDefault(x =>
            prop?.GetValue(x) is int value && value == id);
    }

    public async Task UpsertAsync(
        T item,
        CancellationToken ct = default)
    {
        var prop = typeof(T).GetProperty("Id")
            ?? throw new InvalidOperationException(
                "Model must expose Id.");

        var id = (int)(
            prop.GetValue(item)
            ?? throw new InvalidOperationException(
                "Id cannot be null."));

        var all = (await GetAllAsync(ct)).ToList();

        var i = all.FindIndex(x =>
            prop.GetValue(x) is int existing && existing == id);

        if (i >= 0)
        {
            all[i] = item;
        }
        else
        {
            all.Add(item);
        }

        var dir = Path.GetDirectoryName(path);

        if (!string.IsNullOrEmpty(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var tmp = path + ".tmp";

        await using (var s = File.Create(tmp))
        {
            await JsonSerializer.SerializeAsync(
                s,
                all,
                Options,
                ct);
        }

        File.Move(tmp, path, true);
    }
}

public sealed class JsonEmployeeRepository(string path)
    : JsonRepository<Employee>(path), IEmployeeRepository;

public sealed class JsonProjectRepository(string path)
    : JsonRepository<Project>(path), IProjectRepository;

public sealed class JsonAllocationRepository(string path)
    : JsonRepository<Allocation>(path), IAllocationRepository;
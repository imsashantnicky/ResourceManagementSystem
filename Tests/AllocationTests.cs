using ResourceManagementSystem.Domain.Entities;
using Xunit;

namespace ResourceManagementSystem.Tests;

public sealed class AllocationTests
{
    [Fact]
    public void OverlapIncludesSharedEndDate()
    {
        var a = new Allocation(
            1,
            2,
            3,
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 1, 10),
            50);

        Assert.True(
            a.Overlaps(
                new DateOnly(2026, 1, 10),
                new DateOnly(2026, 1, 20)));
    }

    [Fact]
    public void NonOverlappingRangesReturnFalse()
    {
        var a = new Allocation(
            1,
            2,
            3,
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 1, 10),
            50);

        Assert.False(
            a.Overlaps(
                new DateOnly(2026, 1, 11),
                new DateOnly(2026, 1, 20)));
    }
}
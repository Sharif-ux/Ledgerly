using Ledgerly.Domain.Common;
using Ledgerly.Domain.Periods;

namespace Ledgerly.Domain.Tests.Periods;

public class AccountingPeriodTests
{
    [Fact]
    public void Create_makes_open_period_with_month_range()
    {
        var period = AccountingPeriod.Create(2026, 11);

        Assert.True(period.IsOpen);
        Assert.Equal(new DateOnly(2026, 11, 1), period.StartDate);
        Assert.Equal(new DateOnly(2026, 11, 30), period.EndDate);
        Assert.Equal("2026-11", period.Name);
    }

    [Theory]
    [InlineData(2024, 2, 29)]   // tahun kabisat
    [InlineData(2025, 2, 28)]
    [InlineData(2026, 12, 31)]  // pergantian tahun
    public void Create_computes_last_day_of_month(int year, int month, int lastDay)
    {
        var period = AccountingPeriod.Create(year, month);

        Assert.Equal(new DateOnly(year, month, lastDay), period.EndDate);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void Create_rejects_invalid_month(int month)
    {
        Assert.Throws<DomainException>(() => AccountingPeriod.Create(2026, month));
    }

    [Theory]
    [InlineData(1999)]
    [InlineData(2101)]
    public void Create_rejects_invalid_year(int year)
    {
        Assert.Throws<DomainException>(() => AccountingPeriod.Create(year, 1));
    }

    [Fact]
    public void Contains_includes_boundary_dates_only()
    {
        var period = AccountingPeriod.Create(2026, 11);

        Assert.True(period.Contains(new DateOnly(2026, 11, 1)));
        Assert.True(period.Contains(new DateOnly(2026, 11, 30)));
        Assert.False(period.Contains(new DateOnly(2026, 10, 31)));
        Assert.False(period.Contains(new DateOnly(2026, 12, 1)));
    }

    [Fact]
    public void Close_then_Reopen_changes_status()
    {
        var period = AccountingPeriod.Create(2026, 11);

        period.Close();
        Assert.False(period.IsOpen);

        period.Reopen();
        Assert.True(period.IsOpen);
    }

    [Fact]
    public void Close_rejects_already_closed_period()
    {
        var period = AccountingPeriod.Create(2026, 11);
        period.Close();

        Assert.Throws<DomainException>(() => period.Close());
    }

    [Fact]
    public void Reopen_rejects_open_period()
    {
        var period = AccountingPeriod.Create(2026, 11);

        Assert.Throws<DomainException>(() => period.Reopen());
    }
}
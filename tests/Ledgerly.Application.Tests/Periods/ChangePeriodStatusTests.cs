using Ledgerly.Application.Periods.ChangePeriodStatus;
using Ledgerly.Application.Periods.ListPeriods;
using Ledgerly.Application.Tests.Fakes;
using Ledgerly.Domain.Common;
using Ledgerly.Domain.Periods;

namespace Ledgerly.Application.Tests.Periods;

public class ChangePeriodStatusTests
{
    private readonly FakeAccountingPeriodRepository _repo = new();
    private readonly FakeUnitOfWork _uow = new();

    private AccountingPeriod Seed(int year, int month)
    {
        var period = AccountingPeriod.Create(year, month);
        _repo.Items.Add(period);
        return period;
    }

    [Fact]
    public async Task Close_closes_open_period_and_saves()
    {
        var period = Seed(2026, 11);

        await new ClosePeriodHandler(_repo, _uow).HandleAsync(period.Id);

        Assert.Equal(PeriodStatus.Closed, period.Status);
        Assert.Equal(1, _uow.SaveCount);
    }

    [Fact]
    public async Task Close_rejects_missing_period()
    {
        await Assert.ThrowsAsync<DomainException>(() =>
            new ClosePeriodHandler(_repo, _uow).HandleAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task Close_rejects_already_closed_period_without_saving()
    {
        var period = Seed(2026, 11);
        period.Close();

        await Assert.ThrowsAsync<DomainException>(() =>
            new ClosePeriodHandler(_repo, _uow).HandleAsync(period.Id));

        Assert.Equal(0, _uow.SaveCount);
    }

    [Fact]
    public async Task Reopen_reopens_closed_period_and_saves()
    {
        var period = Seed(2026, 11);
        period.Close();

        await new ReopenPeriodHandler(_repo, _uow).HandleAsync(period.Id);

        Assert.Equal(PeriodStatus.Open, period.Status);
        Assert.Equal(1, _uow.SaveCount);
    }

    [Fact]
    public async Task Reopen_rejects_open_period()
    {
        var period = Seed(2026, 11);

        await Assert.ThrowsAsync<DomainException>(() =>
            new ReopenPeriodHandler(_repo, _uow).HandleAsync(period.Id));
    }

    [Fact]
    public async Task List_returns_periods_in_chronological_order()
    {
        Seed(2026, 11);
        Seed(2026, 10);
        Seed(2025, 12);

        var result = await new ListPeriodsHandler(_repo).HandleAsync();

        Assert.Equal(["2025-12", "2026-10", "2026-11"], result.Select(p => p.Name));
    }
}
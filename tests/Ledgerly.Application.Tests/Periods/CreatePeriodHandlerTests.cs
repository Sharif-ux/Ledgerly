using Ledgerly.Application.Periods.CreatePeriod;
using Ledgerly.Application.Tests.Fakes;
using Ledgerly.Domain.Common;

namespace Ledgerly.Application.Tests.Periods;

public class CreatePeriodHandlerTests
{
    private readonly FakeAccountingPeriodRepository _repo = new();
    private readonly FakeUnitOfWork _uow = new();
    private readonly CreatePeriodHandler _handler;

    public CreatePeriodHandlerTests() => _handler = new CreatePeriodHandler(_repo, _uow);

    [Fact]
    public async Task Creates_period_and_saves()
    {
        var id = await _handler.HandleAsync(new CreatePeriodCommand(2026, 11));

        Assert.Equal(id, _repo.Items.Single().Id);
        Assert.Equal(1, _uow.SaveCount);
    }

    [Fact]
    public async Task Rejects_duplicate_month()
    {
        await _handler.HandleAsync(new CreatePeriodCommand(2026, 11));

        await Assert.ThrowsAsync<DomainException>(() =>
            _handler.HandleAsync(new CreatePeriodCommand(2026, 11)));

        Assert.Single(_repo.Items);
    }

    [Fact]
    public async Task Allows_same_month_in_different_year()
    {
        await _handler.HandleAsync(new CreatePeriodCommand(2025, 11));
        await _handler.HandleAsync(new CreatePeriodCommand(2026, 11));

        Assert.Equal(2, _repo.Items.Count);
    }

    [Fact]
    public async Task Rejects_invalid_month_without_saving()
    {
        await Assert.ThrowsAsync<DomainException>(() =>
            _handler.HandleAsync(new CreatePeriodCommand(2026, 13)));

        Assert.Empty(_repo.Items);
        Assert.Equal(0, _uow.SaveCount);
    }
}
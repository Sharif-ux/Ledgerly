using Ledgerly.Application.Abstractions;
using Ledgerly.Domain.Periods;

namespace Ledgerly.Application.Tests.Fakes;

internal class FakeAccountingPeriodRepository : IAccountingPeriodRepository
{
    public List<AccountingPeriod> Items { get; } = [];

    public Task<bool> ExistsAsync(int year, int month, CancellationToken ct = default) =>
        Task.FromResult(Items.Any(p => p.Year == year && p.Month == month));

    public Task<AccountingPeriod?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(Items.FirstOrDefault(p => p.Id == id));

    public Task<IReadOnlyList<AccountingPeriod>> ListAsync(CancellationToken ct = default) =>
        Task.FromResult<IReadOnlyList<AccountingPeriod>>(
            Items.OrderBy(p => p.Year).ThenBy(p => p.Month).ToList());

    public Task AddAsync(AccountingPeriod period, CancellationToken ct = default)
    {
        Items.Add(period);
        return Task.CompletedTask;
    }
}
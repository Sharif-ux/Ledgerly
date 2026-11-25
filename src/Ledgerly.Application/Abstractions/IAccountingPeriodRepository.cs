using Ledgerly.Domain.Periods;

namespace Ledgerly.Application.Abstractions;

public interface IAccountingPeriodRepository
{
    Task<bool> ExistsAsync(int year, int month, CancellationToken ct = default);
    Task<AccountingPeriod?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<AccountingPeriod>> ListAsync(CancellationToken ct = default);
    Task AddAsync(AccountingPeriod period, CancellationToken ct = default);
}
using Ledgerly.Application.Abstractions;
using Ledgerly.Domain.Periods;
using Microsoft.EntityFrameworkCore;

namespace Ledgerly.Infrastructure.Persistence.Repositories;

public class AccountingPeriodRepository(AppDbContext db) : IAccountingPeriodRepository
{
    public Task<bool> ExistsAsync(int year, int month, CancellationToken ct = default) =>
        db.AccountingPeriods.AnyAsync(p => p.Year == year && p.Month == month, ct);

    public Task<AccountingPeriod?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.AccountingPeriods.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IReadOnlyList<AccountingPeriod>> ListAsync(CancellationToken ct = default) =>
        await db.AccountingPeriods.AsNoTracking()
            .OrderBy(p => p.Year).ThenBy(p => p.Month)
            .ToListAsync(ct);

    public async Task AddAsync(AccountingPeriod period, CancellationToken ct = default) =>
        await db.AccountingPeriods.AddAsync(period, ct);
}
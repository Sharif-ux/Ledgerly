using Ledgerly.Application.Abstractions;
using Ledgerly.Domain.Accounts;
using Microsoft.EntityFrameworkCore;

namespace Ledgerly.Infrastructure.Persistence.Repositories;

public class AccountRepository(AppDbContext db) : IAccountRepository
{
    public Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default) =>
        db.Accounts.AnyAsync(a => a.Code == code, ct);

    public Task<Account?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.Accounts.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task AddAsync(Account account, CancellationToken ct = default) =>
        await db.Accounts.AddAsync(account, ct);
}
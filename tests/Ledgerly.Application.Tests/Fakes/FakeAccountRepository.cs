using Ledgerly.Application.Abstractions;
using Ledgerly.Domain.Accounts;

namespace Ledgerly.Application.Tests.Fakes;

internal class FakeAccountRepository : IAccountRepository
{
    public List<Account> Items { get; } = [];

    public Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default) =>
        Task.FromResult(Items.Any(a => a.Code == code));

    public Task<Account?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(Items.FirstOrDefault(a => a.Id == id));

    public Task<IReadOnlyList<Account>> ListAsync(CancellationToken ct = default) =>
        Task.FromResult<IReadOnlyList<Account>>(Items.OrderBy(a => a.Code).ToList());

    public Task AddAsync(Account account, CancellationToken ct = default)
    {
        Items.Add(account);
        return Task.CompletedTask;
    }
}

internal class FakeUnitOfWork : IUnitOfWork
{
    public int SaveCount { get; private set; }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        SaveCount++;
        return Task.FromResult(0);
    }
}
using Ledgerly.Domain.Accounts;

namespace Ledgerly.Application.Abstractions;

public interface IAccountRepository
{
    Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default);
    Task<Account?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Account account, CancellationToken ct = default);
}
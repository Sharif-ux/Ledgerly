using Ledgerly.Application.Abstractions;

namespace Ledgerly.Application.Accounts.ListAccounts;

public class ListAccountsHandler(IAccountRepository accounts)
{
    public async Task<IReadOnlyList<AccountDto>> HandleAsync(CancellationToken ct = default)
    {
        var items = await accounts.ListAsync(ct);

        return items
            .Select(a => new AccountDto(
                a.Id, a.Code, a.Name, a.Type, a.NormalBalance,
                a.IsPostable, a.IsActive, a.ParentId))
            .ToList();
    }
}
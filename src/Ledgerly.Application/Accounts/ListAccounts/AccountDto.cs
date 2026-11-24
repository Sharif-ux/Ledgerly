using Ledgerly.Domain.Accounts;

namespace Ledgerly.Application.Accounts.ListAccounts;

public record AccountDto(
    Guid Id,
    string Code,
    string Name,
    AccountType Type,
    NormalBalance NormalBalance,
    bool IsPostable,
    bool IsActive,
    Guid? ParentId);
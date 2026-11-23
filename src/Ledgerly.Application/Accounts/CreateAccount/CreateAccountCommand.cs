using Ledgerly.Domain.Accounts;

namespace Ledgerly.Application.Accounts.CreateAccount;

public record CreateAccountCommand(
    string Code,
    string Name,
    AccountType Type,
    bool IsPostable,
    Guid? ParentId = null);
using Ledgerly.Domain.Accounts;

namespace Ledgerly.WebApi.Contracts;

public record CreateAccountRequest(
    string Code,
    string Name,
    AccountType Type,
    bool IsPostable,
    Guid? ParentId = null);
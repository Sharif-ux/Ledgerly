using Ledgerly.Application.Abstractions;
using Ledgerly.Domain.Accounts;
using Ledgerly.Domain.Common;

namespace Ledgerly.Application.Accounts.CreateAccount;

public class CreateAccountHandler(IAccountRepository accounts, IUnitOfWork unitOfWork)
{
    public async Task<Guid> HandleAsync(CreateAccountCommand command, CancellationToken ct = default)
    {
        var code = command.Code?.Trim() ?? string.Empty;

        if (await accounts.ExistsByCodeAsync(code, ct))
            throw new DomainException($"Kode akun '{code}' sudah dipakai.");

        if (command.ParentId is { } parentId)
        {
            var parent = await accounts.GetByIdAsync(parentId, ct)
                ?? throw new DomainException("Akun induk tidak ditemukan.");

            if (parent.IsPostable)
                throw new DomainException("Akun induk harus berupa akun header (tidak postable).");
        }

        var account = Account.Create(
            code, command.Name, command.Type, command.IsPostable, command.ParentId);

        await accounts.AddAsync(account, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return account.Id;
    }
}
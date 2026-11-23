using Ledgerly.Domain.Common;

namespace Ledgerly.Domain.Accounts;

public class Account
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public AccountType Type { get; private set; }
    public Guid? ParentId { get; private set; }
    public bool IsPostable { get; private set; }   // false = akun header/induk
    public bool IsActive { get; private set; }

    public NormalBalance NormalBalance =>
        Type is AccountType.Asset or AccountType.Expense
            ? NormalBalance.Debit
            : NormalBalance.Credit;

    private Account() { } // untuk EF Core nanti

    public static Account Create(
        string code, string name, AccountType type,
        bool isPostable, Guid? parentId = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("Kode akun wajib diisi.");
        if (code.Trim().Length > 20)
            throw new DomainException("Kode akun maksimal 20 karakter.");
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nama akun wajib diisi.");
        if (!Enum.IsDefined(type))
            throw new DomainException("Tipe akun tidak valid.");

        return new Account
        {
            Id = Guid.NewGuid(),
            Code = code.Trim(),
            Name = name.Trim(),
            Type = type,
            IsPostable = isPostable,
            ParentId = parentId,
            IsActive = true
        };
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nama akun wajib diisi.");
        Name = name.Trim();
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
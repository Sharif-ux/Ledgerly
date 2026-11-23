using Ledgerly.Domain.Accounts;
using Ledgerly.Domain.Common;
using System.Security.Principal;

namespace Ledgerly.Domain.Tests.Accounts;

public class AccountTests
{
    [Theory]
    [InlineData(AccountType.Asset, NormalBalance.Debit)]
    [InlineData(AccountType.Expense, NormalBalance.Debit)]
    [InlineData(AccountType.Liability, NormalBalance.Credit)]
    [InlineData(AccountType.Equity, NormalBalance.Credit)]
    [InlineData(AccountType.Revenue, NormalBalance.Credit)]
    public void NormalBalance_follows_account_type(AccountType type, NormalBalance expected)
    {
        var account = Account.Create("1000", "Test", type, isPostable: true);

        Assert.Equal(expected, account.NormalBalance);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_rejects_empty_code(string code)
    {
        Assert.Throws<DomainException>(() =>
            Account.Create(code, "Kas", AccountType.Asset, isPostable: true));
    }

    [Fact]
    public void Create_rejects_empty_name()
    {
        Assert.Throws<DomainException>(() =>
            Account.Create("1100", " ", AccountType.Asset, isPostable: true));
    }

    [Fact]
    public void Create_sets_account_active_and_trims_input()
    {
        var account = Account.Create(" 1100 ", " Kas ", AccountType.Asset, isPostable: true);

        Assert.True(account.IsActive);
        Assert.Equal("1100", account.Code);
        Assert.Equal("Kas", account.Name);
    }
}
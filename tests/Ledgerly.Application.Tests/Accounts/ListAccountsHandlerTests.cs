using Ledgerly.Application.Accounts.ListAccounts;
using Ledgerly.Application.Tests.Fakes;
using Ledgerly.Domain.Accounts;

namespace Ledgerly.Application.Tests.Accounts;

public class ListAccountsHandlerTests
{
    private readonly FakeAccountRepository _repo = new();
    private readonly ListAccountsHandler _handler;

    public ListAccountsHandlerTests() => _handler = new ListAccountsHandler(_repo);

    [Fact]
    public async Task Returns_empty_list_when_no_accounts()
    {
        var result = await _handler.HandleAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task Returns_accounts_ordered_by_code()
    {
        _repo.Items.Add(Account.Create("1100", "Kas", AccountType.Asset, true));
        _repo.Items.Add(Account.Create("1000", "Aset", AccountType.Asset, false));

        var result = await _handler.HandleAsync();

        Assert.Equal(["1000", "1100"], result.Select(a => a.Code));
    }

    [Fact]
    public async Task Maps_normal_balance_and_parent()
    {
        var parent = Account.Create("2000", "Liabilitas", AccountType.Liability, false);
        var child = Account.Create("2100", "Utang Usaha", AccountType.Liability, true, parent.Id);
        _repo.Items.AddRange([parent, child]);

        var result = await _handler.HandleAsync();

        var dto = result.Single(a => a.Code == "2100");
        Assert.Equal(NormalBalance.Credit, dto.NormalBalance);
        Assert.Equal(parent.Id, dto.ParentId);
    }
}
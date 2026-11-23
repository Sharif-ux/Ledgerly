using Ledgerly.Application.Accounts.CreateAccount;
using Ledgerly.Application.Tests.Fakes;
using Ledgerly.Domain.Accounts;
using Ledgerly.Domain.Common;

namespace Ledgerly.Application.Tests.Accounts;

public class CreateAccountHandlerTests
{
    private readonly FakeAccountRepository _repo = new();
    private readonly FakeUnitOfWork _uow = new();
    private readonly CreateAccountHandler _handler;

    public CreateAccountHandlerTests() => _handler = new CreateAccountHandler(_repo, _uow);

    [Fact]
    public async Task Creates_account_and_saves()
    {
        var id = await _handler.HandleAsync(
            new CreateAccountCommand("1100", "Kas", AccountType.Asset, true));

        Assert.Single(_repo.Items);
        Assert.Equal(id, _repo.Items[0].Id);
        Assert.Equal(1, _uow.SaveCount);
    }

    [Fact]
    public async Task Rejects_duplicate_code()
    {
        await _handler.HandleAsync(new CreateAccountCommand("1100", "Kas", AccountType.Asset, true));

        await Assert.ThrowsAsync<DomainException>(() =>
            _handler.HandleAsync(new CreateAccountCommand("1100", "Bank", AccountType.Asset, true)));

        Assert.Single(_repo.Items);
    }

    [Fact]
    public async Task Rejects_missing_parent()
    {
        await Assert.ThrowsAsync<DomainException>(() =>
            _handler.HandleAsync(
                new CreateAccountCommand("1100", "Kas", AccountType.Asset, true, Guid.NewGuid())));
    }

    [Fact]
    public async Task Rejects_postable_parent()
    {
        var parentId = await _handler.HandleAsync(
            new CreateAccountCommand("1000", "Kas Utama", AccountType.Asset, IsPostable: true));

        await Assert.ThrowsAsync<DomainException>(() =>
            _handler.HandleAsync(
                new CreateAccountCommand("1100", "Kas Kecil", AccountType.Asset, true, parentId)));
    }

    [Fact]
    public async Task Accepts_header_account_as_parent()
    {
        var parentId = await _handler.HandleAsync(
            new CreateAccountCommand("1000", "Aset", AccountType.Asset, IsPostable: false));

        var childId = await _handler.HandleAsync(
            new CreateAccountCommand("1100", "Kas", AccountType.Asset, true, parentId));

        Assert.Equal(parentId, _repo.Items.Single(a => a.Id == childId).ParentId);
    }
}
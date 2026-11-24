using Ledgerly.Application.Accounts.CreateAccount;
using Ledgerly.Application.Accounts.ListAccounts;
using Ledgerly.WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Ledgerly.WebApi.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController(
    CreateAccountHandler createAccount,
    ListAccountsHandler listAccounts) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct) =>
        Ok(await listAccounts.HandleAsync(ct));

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAccountRequest request, CancellationToken ct)
    {
        var id = await createAccount.HandleAsync(
            new CreateAccountCommand(
                request.Code, request.Name, request.Type,
                request.IsPostable, request.ParentId),
            ct);

        return Created($"/api/accounts/{id}", new { id });
    }
}
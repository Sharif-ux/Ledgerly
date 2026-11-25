using Ledgerly.Application.Periods.ChangePeriodStatus;
using Ledgerly.Application.Periods.CreatePeriod;
using Ledgerly.Application.Periods.ListPeriods;
using Ledgerly.WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Ledgerly.WebApi.Controllers;

[ApiController]
[Route("api/periods")]
public class PeriodsController(
    CreatePeriodHandler createPeriod,
    ClosePeriodHandler closePeriod,
    ReopenPeriodHandler reopenPeriod,
    ListPeriodsHandler listPeriods) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct) =>
        Ok(await listPeriods.HandleAsync(ct));

    [HttpPost]
    public async Task<IActionResult> Create(CreatePeriodRequest request, CancellationToken ct)
    {
        var id = await createPeriod.HandleAsync(
            new CreatePeriodCommand(request.Year, request.Month), ct);

        return Created($"/api/periods/{id}", new { id });
    }

    [HttpPost("{id:guid}/close")]
    public async Task<IActionResult> Close(Guid id, CancellationToken ct)
    {
        await closePeriod.HandleAsync(id, ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/reopen")]
    public async Task<IActionResult> Reopen(Guid id, CancellationToken ct)
    {
        await reopenPeriod.HandleAsync(id, ct);
        return NoContent();
    }
}
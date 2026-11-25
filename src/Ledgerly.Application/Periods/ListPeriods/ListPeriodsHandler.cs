using Ledgerly.Application.Abstractions;

namespace Ledgerly.Application.Periods.ListPeriods;

public class ListPeriodsHandler(IAccountingPeriodRepository periods)
{
    public async Task<IReadOnlyList<PeriodDto>> HandleAsync(CancellationToken ct = default)
    {
        var items = await periods.ListAsync(ct);

        return items
            .Select(p => new PeriodDto(
                p.Id, p.Year, p.Month, p.Name, p.StartDate, p.EndDate, p.Status))
            .ToList();
    }
}
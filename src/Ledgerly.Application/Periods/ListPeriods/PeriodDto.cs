using Ledgerly.Domain.Periods;

namespace Ledgerly.Application.Periods.ListPeriods;

public record PeriodDto(
    Guid Id,
    int Year,
    int Month,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    PeriodStatus Status);
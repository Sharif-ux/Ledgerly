using Ledgerly.Application.Abstractions;
using Ledgerly.Domain.Common;

namespace Ledgerly.Application.Periods.ChangePeriodStatus;

public class ReopenPeriodHandler(IAccountingPeriodRepository periods, IUnitOfWork unitOfWork)
{
    public async Task HandleAsync(Guid periodId, CancellationToken ct = default)
    {
        var period = await periods.GetByIdAsync(periodId, ct)
            ?? throw new DomainException("Periode tidak ditemukan.");

        period.Reopen();

        await unitOfWork.SaveChangesAsync(ct);
    }
}
using Ledgerly.Application.Abstractions;
using Ledgerly.Domain.Common;

namespace Ledgerly.Application.Periods.ChangePeriodStatus;

public class ClosePeriodHandler(IAccountingPeriodRepository periods, IUnitOfWork unitOfWork)
{
    public async Task HandleAsync(Guid periodId, CancellationToken ct = default)
    {
        var period = await periods.GetByIdAsync(periodId, ct)
            ?? throw new NotFoundException("Periode tidak ditemukan.");

        period.Close();

        await unitOfWork.SaveChangesAsync(ct);
    }
}
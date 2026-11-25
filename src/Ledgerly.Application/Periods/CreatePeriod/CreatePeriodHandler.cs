using Ledgerly.Application.Abstractions;
using Ledgerly.Domain.Common;
using Ledgerly.Domain.Periods;

namespace Ledgerly.Application.Periods.CreatePeriod;

public class CreatePeriodHandler(IAccountingPeriodRepository periods, IUnitOfWork unitOfWork)
{
    public async Task<Guid> HandleAsync(CreatePeriodCommand command, CancellationToken ct = default)
    {
        // Domain memvalidasi bulan dan tahun lebih dulu
        var period = AccountingPeriod.Create(command.Year, command.Month);

        if (await periods.ExistsAsync(command.Year, command.Month, ct))
            throw new DomainException($"Periode {period.Name} sudah ada.");

        await periods.AddAsync(period, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return period.Id;
    }
}
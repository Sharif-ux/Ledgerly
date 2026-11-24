using Ledgerly.Domain.Common;

namespace Ledgerly.Domain.Periods;

public class AccountingPeriod
{
    public Guid Id { get; private set; }
    public int Year { get; private set; }
    public int Month { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public PeriodStatus Status { get; private set; }

    public string Name => $"{Year}-{Month:00}";
    public bool IsOpen => Status == PeriodStatus.Open;

    private AccountingPeriod() { } // untuk EF Core nanti

    public static AccountingPeriod Create(int year, int month)
    {
        if (year is < 2000 or > 2100)
            throw new DomainException("Tahun periode harus antara 2000 dan 2100.");
        if (month is < 1 or > 12)
            throw new DomainException("Bulan periode harus antara 1 dan 12.");

        var start = new DateOnly(year, month, 1);

        return new AccountingPeriod
        {
            Id = Guid.NewGuid(),
            Year = year,
            Month = month,
            StartDate = start,
            EndDate = start.AddMonths(1).AddDays(-1),
            Status = PeriodStatus.Open
        };
    }

    public bool Contains(DateOnly date) => date >= StartDate && date <= EndDate;

    public void Close()
    {
        if (Status == PeriodStatus.Closed)
            throw new DomainException($"Periode {Name} sudah ditutup.");
        Status = PeriodStatus.Closed;
    }

    public void Reopen()
    {
        if (Status == PeriodStatus.Open)
            throw new DomainException($"Periode {Name} masih terbuka.");
        Status = PeriodStatus.Open;
    }
}
using Ledgerly.Application.Abstractions;
using Ledgerly.Domain.Accounts;
using Ledgerly.Domain.Periods;
using Microsoft.EntityFrameworkCore;

namespace Ledgerly.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<AccountingPeriod> AccountingPeriods => Set<AccountingPeriod>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
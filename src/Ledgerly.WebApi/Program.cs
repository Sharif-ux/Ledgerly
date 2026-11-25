using System.Text.Json.Serialization;
using Ledgerly.Application.Accounts.CreateAccount;
using Ledgerly.Application.Accounts.ListAccounts;
using Ledgerly.Application.Periods.ChangePeriodStatus;
using Ledgerly.Application.Periods.CreatePeriod;
using Ledgerly.Application.Periods.ListPeriods;
using Ledgerly.Infrastructure;
using Ledgerly.WebApi.Middleware;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("Default")!);

builder.Services.AddScoped<CreateAccountHandler>();
builder.Services.AddScoped<ListAccountsHandler>();
builder.Services.AddScoped<CreatePeriodHandler>();
builder.Services.AddScoped<ClosePeriodHandler>();
builder.Services.AddScoped<ReopenPeriodHandler>();
builder.Services.AddScoped<ListPeriodsHandler>();

builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<DomainExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
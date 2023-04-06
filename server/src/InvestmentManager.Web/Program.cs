using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Services;

var builder = WebApplication.CreateBuilder(args);

// Verify which method to add services to the container
builder.Services.AddSingleton<IStockPositionService, StockPositionService>(); 
builder.Services.AddSingleton<ITransactionService, TransactionService>();
// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

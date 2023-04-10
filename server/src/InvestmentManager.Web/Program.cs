using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Services;
using InvestmentManager.ApplicationCore.Mapper;
using InvestmentManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(MappingProfile));
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Verify which method to add services to the container
builder.Services.AddTransient<IStockPositionService, StockPositionService>(); 
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddTransient<IFinnhubService, FinnhubService>();
builder.Services.AddTransient<IStockPositionRepository, StockPositionRepository>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
builder.Services.AddTransient<IFinnhubRepository, FinnhubRepository>();
// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHsts();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

using InvestmentManager;
using InvestmentManager.Infrastructure.AppDbContext;
using InvestmentManager.Web.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders().AddConsole().AddDebug();
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UsePathBase(new PathString("/api"));
app.UseHsts();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

if (builder.Environment.IsEnvironment("Development"))
{
	using (var scope = app.Services.CreateScope())
	{
		var services = scope.ServiceProvider;
		try
		{
			var db = services.GetRequiredService<ApplicationDbContext>();
			db.Database.EnsureCreated();
		}
		catch (Exception ex)
		{
			var logger = services.GetRequiredService<ILogger<Program>>();
			logger.LogError(ex, "An error occurred while migrating the database.");
		}   
	}
}

app.Run();

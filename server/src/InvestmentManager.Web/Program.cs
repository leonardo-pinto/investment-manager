using InvestmentManager;
using InvestmentManager.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders().AddConsole().AddDebug();
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

if(builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandlingMiddleware();
}


app.UsePathBase(new PathString("/api"));
app.UseHsts();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

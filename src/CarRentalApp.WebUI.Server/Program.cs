using CarRentalApp.Application;
using CarRentalApp.Infrastructure;
using CarRentalApp.Infrastructure.Data;
using CarRentalApp.WebUI.Server.Auth;
using CarRentalApp.WebUI.Server.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddBlazorBootstrap();
builder.Services.AddMudServices();

var app = builder.Build();

var useMockDatabase = builder.Configuration.GetValue<bool>("UseMockDatabase");
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    if (useMockDatabase)
    {
        context.Database.EnsureCreated();
        app.Logger.LogInformation("Using In-Memory database with mock data");
    }
    else
    {
        DbInitializer.Initialize(context);
        app.Logger.LogInformation("Using Azure SQL Database");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

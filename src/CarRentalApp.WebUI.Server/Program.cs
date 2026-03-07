using CarRentalApp.Application;
using CarRentalApp.Infrastructure;
using CarRentalApp.Infrastructure.Data;
using CarRentalApp.WebUI.Server.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Razor components with interactivity
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add cascading authentication state for Blazor
builder.Services.AddCascadingAuthenticationState();

// Add application and infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Blazor Bootstrap for UI components
builder.Services.AddBlazorBootstrap();

var app = builder.Build();

// Initialize database with seed data
var useMockDatabase = builder.Configuration.GetValue<bool>("UseMockDatabase");
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    
    if (useMockDatabase)
    {
        // For in-memory database
        context.Database.EnsureCreated();
        app.Logger.LogInformation("Using In-Memory database with mock data");
    }
    else
    {
        // For Azure SQL Database
        DbInitializer.Initialize(context);
        app.Logger.LogInformation("Using Azure SQL Database");
    }
}

// Configure HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map Razor components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

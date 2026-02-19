using CarRentalApp.WebUI.Server.Components;
using CarRentalApp.Application;
using CarRentalApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add Razor components with interactivity
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add cascading authentication state for Blazor
builder.Services.AddCascadingAuthenticationState();

// Add application and infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

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

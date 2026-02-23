using CarRentalApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register application services
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IRentalService, RentalService>();
            
            return services;
        }
    }
}

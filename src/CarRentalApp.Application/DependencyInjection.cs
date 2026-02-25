using CarRentalApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register application services as Transient (each service call gets new instance)
            services.AddTransient<ICarService, CarService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IRentalService, RentalService>();
            
            return services;
        }
    }
}

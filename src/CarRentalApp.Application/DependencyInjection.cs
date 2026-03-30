using CarRentalApp.Application.Services;
using CarRentalApp.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<ICarService, CarService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IRentalService, RentalService>();

            services.AddScoped<IReportDamageService, ReportDamageService>();
            services.AddScoped<IAdminBreakdownReportsService, AdminBreakdownReportsService>();
            services.AddScoped<IMyRentalsService, MyRentalsService>();
            services.AddScoped<IAdminClientsService, AdminClientsService>();
            services.AddScoped<IAdminFleetService, AdminFleetService>();
            services.AddScoped<IAdminEmployeeService, AdminEmployeeService>();

            return services;
        }
    }
}

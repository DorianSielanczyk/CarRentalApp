using CarRentalApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.Migrate();
        }

        public static void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Roles
            var userRole = new IdentityRole
            {
                Id = "role-user-id",
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = "role-user-stamp"
            };

            var workerRole = new IdentityRole
            {
                Id = "role-worker-id",
                Name = "Worker",
                NormalizedName = "WORKER",
                ConcurrencyStamp = "role-worker-stamp"
            };

            var adminRole = new IdentityRole
            {
                Id = "role-admin-id",
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "role-admin-stamp"
            };

            modelBuilder.Entity<IdentityRole>().HasData(userRole, workerRole, adminRole);

            // Seed Identity Users
            var hasher = new PasswordHasher<IdentityUser>();

            var userAccount = new IdentityUser
            {
                Id = "user-customer-id",
                UserName = "customer@carrental.com",
                NormalizedUserName = "CUSTOMER@CARRENTAL.COM",
                Email = "customer@carrental.com",
                NormalizedEmail = "CUSTOMER@CARRENTAL.COM",
                EmailConfirmed = true,
                SecurityStamp = "user-customer-stamp",
                ConcurrencyStamp = "user-customer-concurrency"
            };
            userAccount.PasswordHash = hasher.HashPassword(userAccount, "Customer123!");

            var workerAccount = new IdentityUser
            {
                Id = "user-worker-id",
                UserName = "worker@carrental.com",
                NormalizedUserName = "WORKER@CARRENTAL.COM",
                Email = "worker@carrental.com",
                NormalizedEmail = "WORKER@CARRENTAL.COM",
                EmailConfirmed = true,
                SecurityStamp = "user-worker-stamp",
                ConcurrencyStamp = "user-worker-concurrency"
            };
            workerAccount.PasswordHash = hasher.HashPassword(workerAccount, "Worker123!");

            var adminAccount = new IdentityUser
            {
                Id = "user-admin-id",
                UserName = "admin@carrental.com",
                NormalizedUserName = "ADMIN@CARRENTAL.COM",
                Email = "admin@carrental.com",
                NormalizedEmail = "ADMIN@CARRENTAL.COM",
                EmailConfirmed = true,
                SecurityStamp = "user-admin-stamp",
                ConcurrencyStamp = "user-admin-concurrency"
            };
            adminAccount.PasswordHash = hasher.HashPassword(adminAccount, "Admin123!");

            modelBuilder.Entity<IdentityUser>().HasData(userAccount, workerAccount, adminAccount);

            // Assign Roles to Users
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = userRole.Id, UserId = userAccount.Id },
                new IdentityUserRole<string> { RoleId = workerRole.Id, UserId = workerAccount.Id },
                new IdentityUserRole<string> { RoleId = adminRole.Id, UserId = adminAccount.Id }
            );

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Economy", Description = "Affordable and fuel-efficient cars." },
                new Category { Id = 2, Name = "SUV", Description = "Spacious and comfortable SUVs." },
                new Category { Id = 3, Name = "Luxury", Description = "High-end and premium vehicles." },
                new Category { Id = 4, Name = "Sports", Description = "High-performance sports cars." },
                new Category { Id = 5, Name = "Van", Description = "Large capacity vans for groups." }
            );

            // Seed Cars
            modelBuilder.Entity<Car>().HasData(
                new Car { Id = 1, Brand = "Toyota", Model = "Corolla", RegistrationNumber = "ABC123", YearOfProduction = 2022, PricePerDay = 50.00m, Mileage = 15000, IsAvailable = true, CategoryId = 1 },
                new Car { Id = 2, Brand = "Honda", Model = "Civic", RegistrationNumber = "DEF456", YearOfProduction = 2023, PricePerDay = 55.00m, Mileage = 8000, IsAvailable = true, CategoryId = 1 },
                new Car { Id = 3, Brand = "Ford", Model = "Explorer", RegistrationNumber = "GHI789", YearOfProduction = 2021, PricePerDay = 85.00m, Mileage = 25000, IsAvailable = true, CategoryId = 2 },
                new Car { Id = 4, Brand = "Jeep", Model = "Grand Cherokee", RegistrationNumber = "JKL012", YearOfProduction = 2022, PricePerDay = 90.00m, Mileage = 18000, IsAvailable = true, CategoryId = 2 },
                new Car { Id = 5, Brand = "BMW", Model = "5 Series", RegistrationNumber = "MNO345", YearOfProduction = 2023, PricePerDay = 150.00m, Mileage = 5000, IsAvailable = true, CategoryId = 3 },
                new Car { Id = 6, Brand = "Mercedes-Benz", Model = "E-Class", RegistrationNumber = "PQR678", YearOfProduction = 2023, PricePerDay = 160.00m, Mileage = 3000, IsAvailable = false, CategoryId = 3 },
                new Car { Id = 7, Brand = "Porsche", Model = "911", RegistrationNumber = "STU901", YearOfProduction = 2024, PricePerDay = 250.00m, Mileage = 1000, IsAvailable = true, CategoryId = 4 },
                new Car { Id = 8, Brand = "Mercedes-Benz", Model = "Sprinter", RegistrationNumber = "VWX234", YearOfProduction = 2021, PricePerDay = 100.00m, Mileage = 40000, IsAvailable = true, CategoryId = 5 }
            );

            // Seed Clients
            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, FirstName = "John", LastName = "Doe", DriverLicense = "DL123456", UserId = "user-customer-id" },
                new Client { Id = 2, FirstName = "Worker", LastName = "Worker", DriverLicense = "DL345678", UserId = "user-worker-id" },
                new Client { Id = 3, FirstName = "Admin", LastName = "Admin", DriverLicense = "DL901234", UserId = "user-admin-id" }
            );

            // Seed Rentals
            modelBuilder.Entity<Rental>().HasData(
                new Rental { Id = 1, RentalDate = new DateTime(2026, 2, 10), ReturnDate = new DateTime(2026, 2, 15), TotalCost = 800.00m, Status = "Active", IsPaid = true, CarId = 6, ClientId = 1 }
            );

            // Seed CarPhotos
            modelBuilder.Entity<CarPhoto>().HasData(
                new CarPhoto { Id = 1, PhotoUrl = "/images/cars/toyota-corolla-1.jpg", IsMain = true, CarId = 1 },
                new CarPhoto { Id = 2, PhotoUrl = "/images/cars/toyota-corolla-2.jpg", IsMain = false, CarId = 1 },
                new CarPhoto { Id = 3, PhotoUrl = "/images/cars/honda-civic-1.jpg", IsMain = true, CarId = 2 },
                new CarPhoto { Id = 4, PhotoUrl = "/images/cars/ford-explorer-1.jpg", IsMain = true, CarId = 3 },
                new CarPhoto { Id = 5, PhotoUrl = "/images/cars/jeep-grandcherokee-1.jpg", IsMain = true, CarId = 4 },
                new CarPhoto { Id = 6, PhotoUrl = "/images/cars/bmw-5series-1.jpg", IsMain = true, CarId = 5 },
                new CarPhoto { Id = 7, PhotoUrl = "/images/cars/mercedesbenz-eclass-1.jpg", IsMain = true, CarId = 6 },
                new CarPhoto { Id = 8, PhotoUrl = "/images/cars/porsche-911-1.jpg", IsMain = true, CarId = 7 },
                new CarPhoto { Id = 9, PhotoUrl = "/images/cars/mercedesbenz-sprinter-1.jpg", IsMain = true, CarId = 8 }
            );
        }
    }
}
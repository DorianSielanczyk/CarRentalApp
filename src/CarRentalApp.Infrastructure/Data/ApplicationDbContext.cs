using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Entities.Breakdowns;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<CarPhoto> CarPhotos { get; set; }
        public DbSet<RentalPhoto> RentalPhotos { get; set; }
        public DbSet<BreakdownReport> BreakdownReports { get; set; }
        public DbSet<BreakdownReportPhoto> BreakdownReportPhotos { get; set; }
        public DbSet<BreakdownReportNote> BreakdownReportNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureRelationships(modelBuilder);
            ConfigurePrecision(modelBuilder);

            DbInitializer.SeedData(modelBuilder);
        }

        private static void ConfigurePrecision(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .Property(c => c.PricePerDay)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Rental>()
                .Property(r => r.TotalCost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<BreakdownReport>()
                .Property(b => b.Latitude)
                .HasPrecision(9, 6);

            modelBuilder.Entity<BreakdownReport>()
                .Property(b => b.Longitude)
                .HasPrecision(9, 6);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Client>(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Category)
                .WithMany(cat => cat.Cars)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Client)
                .WithMany(cl => cl.Rentals)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CarPhoto>()
                .HasOne(cp => cp.Car)
                .WithMany(c => c.CarPhotos)
                .HasForeignKey(cp => cp.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RentalPhoto>()
                .HasOne(rp => rp.Rental)
                .WithMany(r => r.RentalPhotos)
                .HasForeignKey(rp => rp.RentalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BreakdownReport>()
                .HasOne(br => br.Rental)
                .WithMany(r => r.BreakdownReports)
                .HasForeignKey(br => br.RentalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BreakdownReportPhoto>()
                .HasOne(p => p.BreakdownReport)
                .WithMany(r => r.Photos)
                .HasForeignKey(p => p.BreakdownReportId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BreakdownReportNote>()
                .HasOne(n => n.BreakdownReport)
                .WithMany(r => r.Notes)
                .HasForeignKey(n => n.BreakdownReportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

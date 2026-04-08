using CarRentalApp.Domain.Interfaces;
using CarRentalApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace CarRentalApp.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public ICarRepository Cars { get; }
        public ICategoryRepository Categories { get; }
        public IClientRepository Clients { get; }
        public IRentalRepository Rentals { get; }
        public ICarPhotoRepository CarPhotos { get; }
        public IBreakdownReportRepository BreakdownReports { get; }
        public IBreakdownReportNoteRepository BreakdownReportNotes { get; }
        public IRentalPhotoRepository RentalPhotos { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            ICarRepository carRepository,
            ICategoryRepository categoryRepository,
            IClientRepository clientRepository,
            IRentalRepository rentalRepository,
            ICarPhotoRepository carPhotoRepository,
            IRentalPhotoRepository rentalPhotoRepository,
            IBreakdownReportRepository breakdownReportRepository,
            IBreakdownReportNoteRepository breakdownReportNoteRepository)
        {
            _context = context;
            Cars = carRepository;
            Categories = categoryRepository;
            Clients = clientRepository;
            Rentals = rentalRepository;
            CarPhotos = carPhotoRepository;
            RentalPhotos = rentalPhotoRepository;
            BreakdownReports = breakdownReportRepository;
            BreakdownReportNotes = breakdownReportNoteRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
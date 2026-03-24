using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Domain.Entities.Breakdowns
{
    public class BreakdownReport
    {
        public int Id { get; set; }

        public int RentalId { get; set; }
        public Rental? Rental { get; set; }

        public BreakdownType BreakdownType { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public string? LocationText { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public BreakdownStatus Status { get; set; } = BreakdownStatus.New;

        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }

        public ICollection<BreakdownReportPhoto> Photos { get; set; } = new List<BreakdownReportPhoto>();
        public ICollection<BreakdownReportNote> Notes { get; set; } = new List<BreakdownReportNote>();
    }
}
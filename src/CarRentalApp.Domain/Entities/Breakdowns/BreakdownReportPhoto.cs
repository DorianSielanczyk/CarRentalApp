namespace CarRentalApp.Domain.Entities.Breakdowns
{
    public class BreakdownReportPhoto
    {
        public int Id { get; set; }

        public int BreakdownReportId { get; set; }
        public BreakdownReport? BreakdownReport { get; set; }

        public string PhotoUrl { get; set; } = string.Empty;
        public DateTime UploadedAtUtc { get; set; }
    }
}
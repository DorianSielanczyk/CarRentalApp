namespace CarRentalApp.Domain.Entities.Breakdowns
{
    public class BreakdownReportNote
    {
        public int Id { get; set; }

        public int BreakdownReportId { get; set; }
        public BreakdownReport? BreakdownReport { get; set; }

        public string Note { get; set; } = string.Empty;
        public string AddedBy { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; }
    }
}
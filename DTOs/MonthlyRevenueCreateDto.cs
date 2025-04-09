using System.ComponentModel.DataAnnotations;

namespace WebApplicationforTest.DTOs
{
    public class MonthlyRevenueCreateDto
    {
        [Required]
        public string CompanyId { get; set; } = string.Empty;

        [Required]
        public string ReportYearMonth { get; set; } = string.Empty;

        public string? CompanyName { get; set; }
        public string? IndustryCategory { get; set; }
        public string? CurrentMonthRevenue { get; set; }
        public string? PreviousMonthRevenue { get; set; }
        public string? LastYearMonthRevenue { get; set; }
        public string? MoMChangePercent { get; set; }
        public string? YoYChangePercent { get; set; }
        public string? AccumulatedRevenue { get; set; }
        public string? LastYearAccumulatedRevenue { get; set; }
        public string? AccumulatedChangePercent { get; set; }
        public string? Note { get; set; }
    }
}
namespace WebApplicationforTest.DTOs
{
    public class MonthlyRevenuePagedDto
    {

        public string? ReportYearMonth { get; set; }
        public string? CompanyId { get; set; }
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

    public class MonthlyRevenuePagedResultDto
    {
        public List<MonthlyRevenuePagedDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
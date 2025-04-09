using CsvHelper.Configuration.Attributes;



namespace WebApplicationforTest.DTOs
{
    public class MonthlyRevenueDto
    {
        [Name("出表日期")]
        public string? ReportDate { get; set; }

        [Name("資料年月")]
        public string? ReportYearMonth { get; set; }

        [Name("公司代號")]
        public string? CompanyId { get; set; }

        [Name("公司名稱")]
        public string? CompanyName { get; set; }

        [Name("產業別")]
        public string? IndustryCategory { get; set; }

        [Name("營業收入-當月營收")]
        public string? CurrentMonthRevenue { get; set; }

        [Name("營業收入-上月營收")]
        public string? PreviousMonthRevenue { get; set; }

        [Name("營業收入-去年當月營收")]
        public string? LastYearMonthRevenue { get; set; }

        [Name("營業收入-上月比較增減(%)")]
        public string? MoMChangePercent { get; set; }

        [Name("營業收入-去年同月增減(%)")]
        public string? YoYChangePercent { get; set; }

        [Name("累計營業收入-當月累計營收")]
        public string? AccumulatedRevenue { get; set; }

        [Name("累計營業收入-去年累計營收")]
        public string? LastYearAccumulatedRevenue { get; set; }

        [Name("累計營業收入-前期比較增減(%)")]
        public string? AccumulatedChangePercent { get; set; }

        [Name("備註")]
        public string? Note { get; set; }
    }
}
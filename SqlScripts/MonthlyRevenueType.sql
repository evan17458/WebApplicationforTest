
CREATE TYPE dbo.MonthlyRevenueType AS TABLE
(
    CompanyId NVARCHAR(50),
    CompanyName NVARCHAR(200),
    ReportYearMonth NVARCHAR(6),
    IndustryCategory NVARCHAR(50),
    CurrentMonthRevenue NVARCHAR(50),
    PreviousMonthRevenue NVARCHAR(50),
    LastYearMonthRevenue NVARCHAR(50),
    MoMChangePercent NVARCHAR(50),
    YoYChangePercent NVARCHAR(50),
    AccumulatedRevenue NVARCHAR(50),
    LastYearAccumulatedRevenue NVARCHAR(50),
    AccumulatedChangePercent NVARCHAR(50),
    Note NVARCHAR(500)
);

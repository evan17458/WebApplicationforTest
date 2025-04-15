-- =============================================
-- �إ� Stored Procedure�Gsp_BulkInsertMonthlyRevenue
-- �ϥ� TVP �i��妸�פJ
-- =============================================
CREATE PROCEDURE dbo.sp_BulkInsertMonthlyRevenue
    @Revenues dbo.MonthlyRevenueType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO MonthlyRevenue (
        CompanyId,
        CompanyName,
        ReportYearMonth,
        IndustryCategory,
        CurrentMonthRevenue,
        PreviousMonthRevenue,
        LastYearMonthRevenue,
        MoMChangePercent,
        YoYChangePercent,
        AccumulatedRevenue,
        LastYearAccumulatedRevenue,
        AccumulatedChangePercent,
        Note
    )
    SELECT 
        CompanyId,
        CompanyName,
        ReportYearMonth,
        IndustryCategory,
        CurrentMonthRevenue,
        PreviousMonthRevenue,
        LastYearMonthRevenue,
        MoMChangePercent,
        YoYChangePercent,
        AccumulatedRevenue,
        LastYearAccumulatedRevenue,
        AccumulatedChangePercent,
        Note
    FROM @Revenues;
END;

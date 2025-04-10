USE [CompanyRevenueDb]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetPagedMonthlyRevenue]    Script Date: 2025/4/10 下午 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_GetPagedMonthlyRevenue]
    @Page INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM (
        SELECT 
            ROW_NUMBER() OVER (ORDER BY CompanyId, ReportYearMonth DESC) AS RowNum,
            ReportYearMonth,
            CompanyId,
            CompanyName,
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
        FROM MonthlyRevenue
    ) AS Result
    WHERE RowNum BETWEEN (@Page - 1) * @PageSize + 1 AND @Page * @PageSize;

    SELECT COUNT(*) AS TotalCount FROM MonthlyRevenue;
END
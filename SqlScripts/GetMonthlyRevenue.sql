USE [CompanyRevenueDb]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetMonthlyRevenueByCompanyId]    Script Date: 2025/4/10 下午 12:23:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_GetMonthlyRevenueByCompanyId]
    @CompanyId NVARCHAR(10)
AS
BEGIN
    SELECT * FROM MonthlyRevenue
    WHERE CompanyId = @CompanyId
    ORDER BY ReportYearMonth DESC;
END;

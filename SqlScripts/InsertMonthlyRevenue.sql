USE [CompanyRevenueDb]
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertMonthlyRevenue]    Script Date: 2025/4/10 下午 12:21:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER   PROCEDURE [dbo].[sp_InsertMonthlyRevenue]
    @CompanyId NVARCHAR(10),
    @CompanyName NVARCHAR(100),
    @ReportYearMonth NVARCHAR(6),
    @IndustryCategory NVARCHAR(100),
    @CurrentMonthRevenue NVARCHAR(50),
    @PreviousMonthRevenue NVARCHAR(50),
    @LastYearMonthRevenue NVARCHAR(50),
    @MoMChangePercent NVARCHAR(50),
    @YoYChangePercent NVARCHAR(50),
    @AccumulatedRevenue NVARCHAR(50),
    @LastYearAccumulatedRevenue NVARCHAR(50),
    @AccumulatedChangePercent NVARCHAR(50),
    @Note NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    -- 如果資料尚未存在（根據主鍵 CompanyId + ReportYearMonth）
    IF NOT EXISTS (
        SELECT 1 
        FROM dbo.MonthlyRevenue
        WHERE CompanyId = @CompanyId AND ReportYearMonth = @ReportYearMonth
    )
    BEGIN
        INSERT INTO dbo.MonthlyRevenue (
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
        VALUES (
            @CompanyId,
            @CompanyName,
            @ReportYearMonth,
            @IndustryCategory,
            @CurrentMonthRevenue,
            @PreviousMonthRevenue,
            @LastYearMonthRevenue,
            @MoMChangePercent,
            @YoYChangePercent,
            @AccumulatedRevenue,
            @LastYearAccumulatedRevenue,
            @AccumulatedChangePercent,
            @Note
        );

        RETURN 1; -- 插入成功
    END
    ELSE
    BEGIN
        RETURN 0; -- 資料已存在，不插入
    END
END;

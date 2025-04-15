# 上市公司每月營業收入彙總 API 專案

此專案負責匯入及查詢 [政府資料開放平台](https://data.gov.tw/dataset/2456) 的「上市公司每月營業收入彙總表」資料。  
可依照公司代號查詢指定公司每月營收紀錄，並將資料儲存至 MSSQL 資料庫中。

---

## ✅ 功能特色

- ✅ 讀取 CSV 並匯入至資料庫
- ✅ 使用 Dapper 搭配 Stored Procedure 操作資料庫
- ✅ 查詢功能實作 CQRS 架構（MediatR）
- ✅ 使用 AutoMapper 做 DTO 映射
- ✅ 加入 GlobalExceptionFilter 做統一錯誤處理
- ✅ 預防 SQL Injection（預存程序處理）

---

## 📂 資料夾說明

| 資料夾 / 檔案         | 說明                             |
|---------------------|----------------------------------|
| `SqlScripts/`       | 放置預存程序 SQL 檔案（Insert / Select） |
| `Repositories/`     | 資料存取邏輯（Dapper）                  |
| `Queries/`          | 查詢邏輯 (CQRS Handler / Query)      |
| `Models/`           | 資料模型（對應資料表）                 |
| `DTOs/`             | 資料傳輸物件                        |
| `Filters/`          | 全域例外處理                        |
| `Profiles/`         | AutoMapper 設定檔案                   |
| `t187ap05_L.csv`    | 原始匯入資料 CSV 檔案                |

---

## 🚀 專案啟動流程

1. 使用 SSMS 匯入資料表與執行預存程序：

   ```sql
   -- 請至 SqlScripts/ 執行下列 SQL 檔案
   SqlScripts/InsertMonthlyRevenue.sql           -- 建立單筆新增 SP
   SqlScripts/GetMonthlyRevenue.sql              -- 查詢所有資料 SP
   SqlScripts/GetPagedMonthlyRevenue.sql         -- 分頁查詢 SP
   SqlScripts/MonthlyRevenueType.sql             --  建立 TVP（匯入資料使用）
   SqlScripts/BulkInsertMonthlyRevenue.sql       --  使用 TVP 批次匯入的 SP

   ```md
### 📘 關於 TVP（Table-Valued Parameter）

TVP 是 SQL Server 中的使用者定義資料表型別，可用來將大量資料（如 DataTable）一次傳入 Stored Procedure 進行批次處理。  
本專案中，`MonthlyRevenueType` 即為 TVP，搭配 `sp_BulkInsertMonthlyRevenue` 使用，大幅提升匯入效能。

可程式性 → 類型 → 使用者定義的表格類型（User-Defined Table Types）
你會看到 MonthlyRevenueType 出現在這裡 ✅

ompanyRevenueDb
└── 可程式性
    └── 類型
        └── 使用者定義的表格類型
            └── dbo.MonthlyRevenueType ←  就在這！
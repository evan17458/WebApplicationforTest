using System.Data;
using System.Data.SqlClient;
using WebApplicationforTest.DTOs;
using WebApplicationforTest.Enum;
using WebApplicationforTest.Models;
using Dapper;

namespace WebApplicationforTest.Repositories
{
    public class RevenueRepository : IRevenueRepository
    {
        private readonly string _connectionString;

        public RevenueRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? @"Server=(localdb)\ProjectModels;Database=CompanyRevenueDb;Trusted_Connection=True;";
        }

        public async Task<List<MonthlyRevenueDto>> GetByCompanyIdAsync(string companyId)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new { CompanyId = companyId };

            var result = await connection.QueryAsync<MonthlyRevenueDto>(
                "sp_GetMonthlyRevenueByCompanyId",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }


        public async Task<(List<MonthlyRevenue>, int)> GetPagedAsync(int page, int pageSize)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new
            {
                Page = page,
                PageSize = pageSize
            };

            await connection.OpenAsync();

            using var multi = await connection.QueryMultipleAsync(
                "sp_GetPagedMonthlyRevenue",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            // 第一個結果集：分頁資料
            var list = (await multi.ReadAsync<MonthlyRevenue>()).ToList();

            // 第二個結果集：總筆數（SELECT COUNT(*) AS TotalCount）
            int totalCount = await multi.ReadFirstAsync<int>();
            return (list, totalCount);
        }

        public async Task<InsertResult> CreateAsync(MonthlyRevenue revenue)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var parameters = new DynamicParameters();

                parameters.Add("@CompanyId", revenue.CompanyId ?? "");
                parameters.Add("@CompanyName", revenue.CompanyName ?? "");
                parameters.Add("@ReportYearMonth", revenue.ReportYearMonth ?? "");
                parameters.Add("@IndustryCategory", revenue.IndustryCategory ?? "");
                parameters.Add("@CurrentMonthRevenue", revenue.CurrentMonthRevenue ?? "");
                parameters.Add("@PreviousMonthRevenue", revenue.PreviousMonthRevenue ?? "");
                parameters.Add("@LastYearMonthRevenue", revenue.LastYearMonthRevenue ?? "");
                parameters.Add("@MoMChangePercent", revenue.MoMChangePercent ?? "");
                parameters.Add("@YoYChangePercent", revenue.YoYChangePercent ?? "");
                parameters.Add("@AccumulatedRevenue", revenue.AccumulatedRevenue ?? "");
                parameters.Add("@LastYearAccumulatedRevenue", revenue.LastYearAccumulatedRevenue ?? "");
                parameters.Add("@AccumulatedChangePercent", revenue.AccumulatedChangePercent ?? "");
                parameters.Add("@Note", revenue.Note ?? "");

                //  加入 Return Value 參數
                parameters.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await connection.ExecuteAsync(
                    "sp_InsertMonthlyRevenue",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                var result = parameters.Get<int>("@ReturnVal");

                return result switch
                {
                    1 => InsertResult.Success,
                    0 => InsertResult.AlreadyExists,
                    _ => InsertResult.Error
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(" 錯誤：" + ex.Message);
                return InsertResult.Error;
            }
        }

    }
}

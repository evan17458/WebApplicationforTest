using System.Data;
using System.Data.SqlClient;
using WebApplicationforTest.DTOs;
using WebApplicationforTest.Enum;
using WebApplicationforTest.Models;

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
            var list = new List<MonthlyRevenueDto>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetMonthlyRevenueByCompanyId", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CompanyId", companyId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new MonthlyRevenueDto
                {
                    CompanyId = reader["CompanyId"].ToString(),
                    CompanyName = reader["CompanyName"].ToString(),
                    ReportYearMonth = reader["ReportYearMonth"].ToString(),
                    IndustryCategory = reader["IndustryCategory"].ToString(),
                    CurrentMonthRevenue = reader["CurrentMonthRevenue"].ToString(),
                    PreviousMonthRevenue = reader["PreviousMonthRevenue"].ToString(),
                    LastYearMonthRevenue = reader["LastYearMonthRevenue"].ToString(),
                    MoMChangePercent = reader["MoMChangePercent"].ToString(),
                    YoYChangePercent = reader["YoYChangePercent"].ToString(),
                    AccumulatedRevenue = reader["AccumulatedRevenue"].ToString(),
                    LastYearAccumulatedRevenue = reader["LastYearAccumulatedRevenue"].ToString(),
                    AccumulatedChangePercent = reader["AccumulatedChangePercent"].ToString(),
                    Note = reader["Note"].ToString()
                });
            }

            return list;
        }

        public async Task<(List<MonthlyRevenue>, int)> GetPagedAsync(int page, int pageSize)
        {
            var list = new List<MonthlyRevenue>();
            int totalCount = 0;

            // 安全轉型小工具方法
            string? SafeToString(object? value)
            {
                if (value is null || value == DBNull.Value)
                    return null;

                return value.ToString();
            }

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetPagedMonthlyRevenue", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            // 第一個結果集：分頁資料
            while (await reader.ReadAsync())
            {
                list.Add(new MonthlyRevenue
                {
                    CompanyId = SafeToString(reader["CompanyId"]),
                    CompanyName = SafeToString(reader["CompanyName"]),
                    ReportYearMonth = SafeToString(reader["ReportYearMonth"]),
                    IndustryCategory = SafeToString(reader["IndustryCategory"]),
                    CurrentMonthRevenue = SafeToString(reader["CurrentMonthRevenue"]),
                    PreviousMonthRevenue = SafeToString(reader["PreviousMonthRevenue"]),
                    LastYearMonthRevenue = SafeToString(reader["LastYearMonthRevenue"]),
                    MoMChangePercent = SafeToString(reader["MoMChangePercent"]),
                    YoYChangePercent = SafeToString(reader["YoYChangePercent"]),
                    AccumulatedRevenue = SafeToString(reader["AccumulatedRevenue"]),
                    LastYearAccumulatedRevenue = SafeToString(reader["LastYearAccumulatedRevenue"]),
                    AccumulatedChangePercent = SafeToString(reader["AccumulatedChangePercent"]),
                    Note = SafeToString(reader["Note"]),

                });
            }

            // 第二個結果集：總筆數
            if (await reader.NextResultAsync() && await reader.ReadAsync())
            {
                totalCount = reader.GetInt32(0);
            }

            return (list, totalCount);
        }












        public async Task<InsertResult> CreateAsync(MonthlyRevenue revenue)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var cmd = new SqlCommand("sp_InsertMonthlyRevenue", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@CompanyId", revenue.CompanyId ?? "");
                cmd.Parameters.AddWithValue("@CompanyName", revenue.CompanyName ?? "");
                cmd.Parameters.AddWithValue("@ReportYearMonth", revenue.ReportYearMonth ?? "");
                cmd.Parameters.AddWithValue("@IndustryCategory", revenue.IndustryCategory ?? "");
                cmd.Parameters.AddWithValue("@CurrentMonthRevenue", revenue.CurrentMonthRevenue ?? "");
                cmd.Parameters.AddWithValue("@PreviousMonthRevenue", revenue.PreviousMonthRevenue ?? "");
                cmd.Parameters.AddWithValue("@LastYearMonthRevenue", revenue.LastYearMonthRevenue ?? "");
                cmd.Parameters.AddWithValue("@MoMChangePercent", revenue.MoMChangePercent ?? "");
                cmd.Parameters.AddWithValue("@YoYChangePercent", revenue.YoYChangePercent ?? "");
                cmd.Parameters.AddWithValue("@AccumulatedRevenue", revenue.AccumulatedRevenue ?? "");
                cmd.Parameters.AddWithValue("@LastYearAccumulatedRevenue", revenue.LastYearAccumulatedRevenue ?? "");
                cmd.Parameters.AddWithValue("@AccumulatedChangePercent", revenue.AccumulatedChangePercent ?? "");
                cmd.Parameters.AddWithValue("@Note", revenue.Note ?? "");


                var returnParam = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                await cmd.ExecuteNonQueryAsync();

                var result = (int)returnParam.Value;

                return result switch
                {
                    1 => InsertResult.Success,
                    0 => InsertResult.AlreadyExists,
                    _ => InsertResult.Error
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ 錯誤：" + ex.Message);
                return InsertResult.Error;
            }
        }


    }
}

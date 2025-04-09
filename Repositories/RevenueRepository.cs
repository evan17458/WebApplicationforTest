using System.Data;
using System.Data.SqlClient;
using WebApplicationforTest.DTOs;
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

        public async Task<bool> CreateAsync(MonthlyRevenue revenue)
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
            cmd.Parameters.AddWithValue("@CurrentMonthRevenue", revenue.CurrentMonthRevenue);
            cmd.Parameters.AddWithValue("@PreviousMonthRevenue", revenue.PreviousMonthRevenue);
            cmd.Parameters.AddWithValue("@LastYearMonthRevenue", revenue.LastYearMonthRevenue);
            cmd.Parameters.AddWithValue("@MoMChangePercent", revenue.MoMChangePercent);
            cmd.Parameters.AddWithValue("@YoYChangePercent", revenue.YoYChangePercent);
            cmd.Parameters.AddWithValue("@AccumulatedRevenue", revenue.AccumulatedRevenue);
            cmd.Parameters.AddWithValue("@LastYearAccumulatedRevenue", revenue.LastYearAccumulatedRevenue);
            cmd.Parameters.AddWithValue("@AccumulatedChangePercent", revenue.AccumulatedChangePercent);
            cmd.Parameters.AddWithValue("@Note", revenue.Note ?? "");

            var affected = await cmd.ExecuteNonQueryAsync();
            return affected > 0;
        }
    }
}

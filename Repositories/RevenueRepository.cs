using System.Data;
using System.Data.SqlClient;
using WebApplicationforTest.DTOs;


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
    }
}

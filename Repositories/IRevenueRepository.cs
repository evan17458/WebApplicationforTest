using WebApplicationforTest.DTOs;
using WebApplicationforTest.Enum;
using WebApplicationforTest.Models;

namespace WebApplicationforTest.Repositories
{
    public interface IRevenueRepository
    {
        Task<List<MonthlyRevenueDto>> GetByCompanyIdAsync(string companyId);
        Task<InsertResult> CreateAsync(MonthlyRevenue entity);

        Task<(List<MonthlyRevenue>, int)> GetPagedAsync(int page, int pageSize);
    }
}
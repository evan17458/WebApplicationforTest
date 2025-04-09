using WebApplicationforTest.DTOs;
using WebApplicationforTest.Models;

namespace WebApplicationforTest.Repositories
{
    public interface IRevenueRepository
    {
        Task<List<MonthlyRevenueDto>> GetByCompanyIdAsync(string companyId);
        Task<bool> CreateAsync(MonthlyRevenue entity);
    }
}
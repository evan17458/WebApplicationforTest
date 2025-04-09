using WebApplicationforTest.DTOs;

namespace WebApplicationforTest.Repositories
{
    public interface IRevenueRepository
    {
        Task<List<MonthlyRevenueDto>> GetByCompanyIdAsync(string companyId);
    }
}

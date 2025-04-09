using MediatR;
using WebApplicationforTest.DTOs;

namespace WebApplicationforTest.Queries.GetRevenueByCompanyId
{
    public record GetRevenueByCompanyIdQuery(string CompanyId) : IRequest<List<MonthlyRevenueDto>>;
}

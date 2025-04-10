using MediatR;
using WebApplicationforTest.DTOs;

namespace WebApplicationforTest.Queries.GetPagedMonthlyRevenue
{
    public class GetPagedMonthlyRevenueQuery : IRequest<MonthlyRevenuePagedResultDto>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

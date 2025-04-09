using MediatR;
using WebApplicationforTest.DTOs;
using WebApplicationforTest.Repositories;

namespace WebApplicationforTest.Queries.GetRevenueByCompanyId
{
    public class GetRevenueByCompanyIdHandler : IRequestHandler<GetRevenueByCompanyIdQuery, List<MonthlyRevenueDto>>
    {
        private readonly IRevenueRepository _repository;

        public GetRevenueByCompanyIdHandler(IRevenueRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MonthlyRevenueDto>> Handle(GetRevenueByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByCompanyIdAsync(request.CompanyId);
        }
    }
}

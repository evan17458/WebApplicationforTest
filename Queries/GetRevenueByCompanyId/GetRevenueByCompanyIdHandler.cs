using AutoMapper;
using MediatR;
using WebApplicationforTest.DTOs;
using WebApplicationforTest.Repositories;

namespace WebApplicationforTest.Queries.GetRevenueByCompanyId
{
    public class GetRevenueByCompanyIdHandler : IRequestHandler<GetRevenueByCompanyIdQuery, List<MonthlyRevenueDto>>
    {
        private readonly IRevenueRepository _repository;
        private readonly IMapper _mapper;

        public GetRevenueByCompanyIdHandler(IRevenueRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<MonthlyRevenueDto>> Handle(GetRevenueByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetByCompanyIdAsync(request.CompanyId);
            var dtos = _mapper.Map<List<MonthlyRevenueDto>>(entities);
            return dtos;
        }
    }
}

using AutoMapper;
using MediatR;
using WebApplicationforTest.Repositories;
using WebApplicationforTest.DTOs;

namespace WebApplicationforTest.Queries.GetPagedMonthlyRevenue
{
    public class GetPagedMonthlyRevenueQueryHandler : IRequestHandler<GetPagedMonthlyRevenueQuery, MonthlyRevenuePagedResultDto>
    {
        private readonly IRevenueRepository _repository;
        private readonly IMapper _mapper;

        public GetPagedMonthlyRevenueQueryHandler(IRevenueRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<MonthlyRevenuePagedResultDto> Handle(GetPagedMonthlyRevenueQuery request, CancellationToken cancellationToken)
        {
            var (data, totalCount) = await _repository.GetPagedAsync(request.Page, request.PageSize);
            var dtoList = _mapper.Map<List<MonthlyRevenuePagedDto>>(data);

            return new MonthlyRevenuePagedResultDto
            {
                Items = dtoList,
                TotalCount = totalCount
            };
        }
    }
}
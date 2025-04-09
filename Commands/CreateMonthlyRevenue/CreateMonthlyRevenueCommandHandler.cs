using AutoMapper;
using MediatR;
using WebApplicationforTest.Models;
using WebApplicationforTest.Repositories;

namespace WebApplicationforTest.Commands
{
    public class CreateMonthlyRevenueCommandHandler : IRequestHandler<CreateMonthlyRevenueCommand, bool>
    {
        private readonly IRevenueRepository _repository;
        private readonly IMapper _mapper;

        public CreateMonthlyRevenueCommandHandler(IRevenueRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateMonthlyRevenueCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<MonthlyRevenue>(request.Dto);
            return await _repository.CreateAsync(entity);
        }
    }
}
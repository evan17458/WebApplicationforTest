using MediatR;
using WebApplicationforTest.DTOs;

namespace WebApplicationforTest.Commands
{
    public class CreateMonthlyRevenueCommand : IRequest<bool>
    {
        public MonthlyRevenueCreateDto Dto { get; set; }

        public CreateMonthlyRevenueCommand(MonthlyRevenueCreateDto dto)
        {
            Dto = dto;
        }
    }
}

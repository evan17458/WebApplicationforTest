using MediatR;
using WebApplicationforTest.DTOs;
using WebApplicationforTest.Enum;

namespace WebApplicationforTest.Commands
{
    public class CreateMonthlyRevenueCommand : IRequest<InsertResult>
    {
        public MonthlyRevenueCreateDto Dto { get; }

        public CreateMonthlyRevenueCommand(MonthlyRevenueCreateDto dto)
        {
            Dto = dto;
        }
    }
}
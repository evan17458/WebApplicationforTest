using AutoMapper;
using WebApplicationforTest.Models;
using WebApplicationforTest.DTOs;

namespace WebApplicationforTest.Profiles
{
    public class MappingProfile : Profile
    {
       
        public MappingProfile()
        {
            CreateMap<MonthlyRevenueCreateDto, MonthlyRevenue>();
            CreateMap<MonthlyRevenue, MonthlyRevenueDto>();
        }
    }
}
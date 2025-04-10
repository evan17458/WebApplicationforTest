using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApplicationforTest.Commands;
using WebApplicationforTest.DTOs;
using WebApplicationforTest.Queries.GetRevenueByCompanyId;
using WebApplicationforTest.Enum;
using WebApplicationforTest.Queries.GetPagedMonthlyRevenue;
namespace WebApplicationforTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RevenueController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RevenueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{companyId}")]
        public async Task<IActionResult> Get(string companyId)
        {
            var result = await _mediator.Send(new GetRevenueByCompanyIdQuery(companyId));
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedMonthlyRevenueQuery { Page = page, PageSize = pageSize };
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MonthlyRevenueCreateDto dto)
        {
            // 執行指令，回傳 InsertResult
            var result = await _mediator.Send(new CreateMonthlyRevenueCommand(dto));

            // 使用 switch 表達式處理結果
            return result switch
            {
                InsertResult.Success => Ok(" 資料新增成功"),
                InsertResult.AlreadyExists => Conflict("資料已存在"),
                InsertResult.Error => StatusCode(500, " 系統錯誤"),
                _ => StatusCode(500, " 未知錯誤")
            };
        }
    }
}

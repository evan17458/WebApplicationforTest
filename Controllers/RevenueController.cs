using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApplicationforTest.Queries.GetRevenueByCompanyId;

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
    }
}

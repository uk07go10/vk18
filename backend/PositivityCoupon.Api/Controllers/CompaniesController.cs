using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PositivityCoupon.Api.Dtos;
using PositivityCoupon.Api.Services;

namespace PositivityCoupon.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly AdminViewModelService _service;

        public CompaniesController(AdminViewModelService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<CompanySummaryDto>>> Get([FromQuery] string? view = "pool")
        {
            var response = await _service.GetCompaniesAsync(view);
            return Ok(response);
        }

        [HttpGet("{companyId:int}")]
        public async Task<ActionResult<CompanyDetailDto>> GetById(int companyId)
        {
            var response = await _service.GetCompanyDetailAsync(companyId);
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}

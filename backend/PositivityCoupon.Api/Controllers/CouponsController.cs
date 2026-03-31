using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PositivityCoupon.Api.Dtos;
using PositivityCoupon.Api.Services;

namespace PositivityCoupon.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponsController : ControllerBase
    {
        private readonly AdminViewModelService _service;

        public CouponsController(AdminViewModelService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<CouponListResponseDto>> Get([FromQuery] string? search, [FromQuery] string? type, [FromQuery] string? status)
        {
            var response = await _service.GetCouponsAsync(search, type, status);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CouponDetailDto>> GetById(int id)
        {
            var detail = await _service.GetCouponDetailAsync(id);
            if (detail == null)
            {
                return NotFound();
            }

            return Ok(detail);
        }

        [HttpPost]
        public async Task<ActionResult<CouponDetailDto>> Create([FromBody] CreateCouponRequestDto request)
        {
            var detail = await _service.CreateCouponAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = detail.Id }, detail);
        }
    }
}


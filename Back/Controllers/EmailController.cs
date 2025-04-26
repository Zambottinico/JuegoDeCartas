using Juego_Sin_Nombre.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Juego_Sin_Nombre.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        

        [HttpPost("sendCupones/{cupon}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> EnviarCuponATodos(string cupon)
        {
            if (string.IsNullOrWhiteSpace(cupon))
                return BadRequest("El código de cupón es obligatorio.");

            await _emailService.SendGiftCouponToAllUsersAsync(cupon);

            return Ok("Se enviaron los cupones a todos los usuarios.");
        }
    }

}

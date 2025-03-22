using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Juego_Sin_Nombre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MercadoPagoController : ControllerBase
    {
        private readonly MercadoPagoService _mercadoPagoService;
        private readonly UserService _userService;

        public MercadoPagoController(MercadoPagoService mercadoPagoService, UserService userService)
        {
            _mercadoPagoService = mercadoPagoService;
            _userService = userService;
        }

        [HttpPost("crear-preferencia/{diamondOfertId}")]
        public async Task<IActionResult> CrearPreferencia(int diamondOfertId, [FromBody] UserCredentials userCredentials)
        {
            if (!await _userService.ValidateCredentialsAsync(userCredentials))
            {
                return BadRequest(new { message = "Las credenciales del usuario son requeridas." });
            }

            // Llamar al servicio para crear la preferencia con el ID de la oferta y las credenciales
            var preference = await _mercadoPagoService.CrearPreferenciaAsync(diamondOfertId, userCredentials);

            if (preference == null)
            {
                return StatusCode(500, new { message = "No se pudo generar la preferencia de pago." });
            }

            return Ok(new { preferenceId = preference.Id, initPoint = preference.InitPoint });
        }

    }
}

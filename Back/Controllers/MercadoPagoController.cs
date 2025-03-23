using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Juego_Sin_Nombre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MercadoPagoController : ControllerBase
    {
        private readonly MercadoPagoService _mercadoPagoService;
        private readonly UserService _userService;
        private readonly InvoiceService _invoiceService;

        public MercadoPagoController(MercadoPagoService mercadoPagoService, UserService userService, InvoiceService invoiceService)
        {
            _mercadoPagoService = mercadoPagoService;
            _userService = userService;
            _invoiceService = invoiceService;
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

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] JsonElement payload)
        {
            try
            {
                Console.WriteLine($"🔔 Notificación recibida: {payload}");

                // Extraer datos importantes
                string type = payload.GetProperty("type").GetString();
                string action = payload.GetProperty("action").GetString();
                string id = payload.GetProperty("data").GetProperty("id").GetString();

                Console.WriteLine($"📢 Tipo: {type}, Acción: {action}, ID: {id}");

                if (type == "payment" && action == "payment.created")
                {
                    // Obtener detalles del pago
                    var payment = await _invoiceService.ObtenerPagoPorIdAsync(id);

                    
                    
                        await _invoiceService.MarcarComoPagado(payment.PreferenceId);
                        Console.WriteLine("✅ Pago confirmado y actualizado en la base de datos.");
                    
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en Webhook: {ex.Message}");
                return BadRequest();
            }
        }

    }
}

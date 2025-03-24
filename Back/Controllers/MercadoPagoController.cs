using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Services;
using MercadoPago.Client.MerchantOrder;
using MercadoPago.Client.Payment;
using Microsoft.AspNetCore.Authorization;
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
        private readonly MerchantOrderClient _merchantOrderClient;
        private readonly PaymentClient _paymentClient;
        private readonly TiendaService _tiendaService;
        public MercadoPagoController(MercadoPagoService mercadoPagoService, UserService userService, InvoiceService invoiceService, MerchantOrderClient merchantOrderClient, PaymentClient paymentClient, TiendaService tiendaService)
        {
            _mercadoPagoService = mercadoPagoService;
            _userService = userService;
            _invoiceService = invoiceService;
            _merchantOrderClient = merchantOrderClient;
            _paymentClient = paymentClient;
            _tiendaService = tiendaService;
        }

        [HttpPost("crear-preferencia/{diamondOfertId}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> CrearPreferencia(int diamondOfertId, [FromBody] UserCredentials userCredentials)
        {
            await _userService.ValidateCredentialsAsync(userCredentials);
           

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
            Console.WriteLine($"🔔 Webhook recibido: {payload}");

            try
            {
                if (payload.TryGetProperty("topic", out var topic))
                {
                    string topicValue = topic.GetString();

                    if (topicValue == "merchant_order")
                    {
                        // Extraer el ID desde "resource"
                        string resource = payload.GetProperty("resource").GetString();
                        string orderId = resource.Split('/').Last();

                        var merchantOrder = await _merchantOrderClient.GetAsync(long.Parse(orderId));
                        Console.WriteLine($"📦 Merchant Order ID: {merchantOrder.Id}");
                    }
                    else if (topicValue == "payment")
                    {
                        // Extraer el ID del pago desde "resource"
                        string paymentId = payload.GetProperty("resource").GetString();
                        var payment = await _paymentClient.GetAsync(long.Parse(paymentId));
                        await _tiendaService.CompleteDiamondCompleteDiamondPurchaseAsync(payment.ExternalReference, payment.Status);

                        Console.WriteLine($"💰 Pago recibido - ID: {payment.Id}, Estado: {payment.Status}");
                    }
                }
                else if (payload.TryGetProperty("type", out var type) && type.GetString() == "payment")
                {
                    // Otra estructura de webhook, extraer ID desde "data.id"
                    string paymentId = payload.GetProperty("data").GetProperty("id").GetString();
                    var payment = await _paymentClient.GetAsync(long.Parse(paymentId));

                    await _tiendaService.CompleteDiamondCompleteDiamondPurchaseAsync(payment.ExternalReference, payment.Status);
                    Console.WriteLine($"💰 Pago recibido - ID: {payment.Id}, Estado: {payment.Status}");
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

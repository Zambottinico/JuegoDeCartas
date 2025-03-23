using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.EntityFrameworkCore;

namespace Juego_Sin_Nombre.Services
{
    public class MercadoPagoService
        
    {
        private readonly Data.ApplicationContext _context;
        private readonly InvoiceService _invoiceService;
        private readonly IConfiguration _configuration;

        public MercadoPagoService(Data.ApplicationContext context, IConfiguration configuration,InvoiceService invoiceService)
        {

            _invoiceService = invoiceService;
            _context = context;
            _configuration = configuration;
            MercadoPagoConfig.AccessToken = _configuration["MercadoPagoSettings:AccessToken"];
        }

        public async Task<Preference> CrearPreferenciaAsync()
        {
            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
            {
                new PreferenceItemRequest
                {
                    Title = "Producto de prueba",
                    Quantity = 1,
                    CurrencyId = "ARS",
                    UnitPrice = 1000
                }
            },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://tu-sitio.com/success",
                    Failure = "https://tu-sitio.com/failure",
                    Pending = "https://tu-sitio.com/pending"
                },
                AutoReturn = "approved"
            };

            var client = new PreferenceClient();
            return await client.CreateAsync(request);
        }

        internal async Task<Preference> CrearPreferenciaAsync(int diamondOfertId, UserCredentials userCredentials)
        {
            DiamondOfert diamondOfert = await _context.DiamondOfert.FirstOrDefaultAsync(d => d.Id == diamondOfertId);
            string webhookUrl = _configuration["MercadoPagoSettings:WebhookUrl"];
            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Title = diamondOfert.Nombre,
                        Quantity = 1,
                        CurrencyId = "ARS",
                        UnitPrice = diamondOfert.PrecioEnPesos
                    }
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://tu-sitio.com/success",
                    Failure = "https://tu-sitio.com/failure",
                    Pending = "https://tu-sitio.com/pending"
                },
                AutoReturn = "approved",
                NotificationUrl = webhookUrl
            };

            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);
            await _invoiceService.CreateInvoiceAsync(preference.Id,InvoiceStatus.Pending,diamondOfertId);
            return preference;
        }
    }
    }

﻿using Juego_Sin_Nombre.Dtos;
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
        private readonly UserService _userService;

        public MercadoPagoService(Data.ApplicationContext context, IConfiguration configuration,InvoiceService invoiceService, UserService userService)
        {

            _invoiceService = invoiceService;
            _context = context;
            _configuration = configuration;
            MercadoPagoConfig.AccessToken = _configuration["MercadoPagoSettings:AccessToken"];
            MercadoPagoConfig.IntegratorId = "dev_24c65fb163bf11ea96500242ac130004";
            _userService = userService;
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

        

        internal async Task<CreatePreferenceResponse> CrearPreferenciaAsync(int diamondOfertId, UserCredentials userCredentials)
        {
            Usuario user = await _userService.ValidateCredentialsAsync(userCredentials);

            DiamondOfert diamondOfert = await _context.DiamondOfert.FirstOrDefaultAsync(d => d.Id == diamondOfertId);
            Invoice invoice =  await _invoiceService.CreateInvoiceAsync(InvoiceStatus.Pending, diamondOfertId,user);
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
                NotificationUrl = webhookUrl,
                ExternalReference = invoice.Id.ToString()
            };

            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);
            CreatePreferenceResponse response = new CreatePreferenceResponse();
            response.Preference = preference;
            response.InvoiceId = invoice.Id;
            return response;
        }
    }
    }

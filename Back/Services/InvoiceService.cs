using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using Microsoft.EntityFrameworkCore;

namespace Juego_Sin_Nombre.Services
{
    public class InvoiceService
    {
        private readonly Data.ApplicationContext _context;

        public InvoiceService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Invoice> CreateInvoiceAsync(InvoiceStatus status, int diamondOfertId)
        {
            try
            {
                DiamondOfert diamondOfert = await _context.DiamondOfert.FirstOrDefaultAsync(d => d.Id == diamondOfertId);
                if (diamondOfert == null)
                {
                    throw new Exception("Oferta de diamantes no encontrada.");
                }

                Invoice invoice = new Invoice(status, diamondOfertId, diamondOfert);
                await _context.Invoices.AddAsync(invoice);
                await _context.SaveChangesAsync();

                return invoice; // Devuelve la invoice ya con ID
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar cambios: {ex.InnerException?.Message}");
                throw;
            }
        }





    }
}

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

        public async Task CreateInvoiceAsync(string preferenceId,InvoiceStatus status,int diamondOfertId)
        {
            try { 
            DiamondOfert diamondOfert = await _context.DiamondOfert.FirstOrDefaultAsync(d => d.Id == diamondOfertId);
            Invoice invoice = new Invoice(preferenceId,status,diamondOfertId,diamondOfert);
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                Console.WriteLine($"Error al guardar cambios: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<Invoice> ObtenerPagoPorIdAsync(string preferenceId)
        {
            return await _context.Invoices.FirstOrDefaultAsync(i => i.PreferenceId == preferenceId);
        }

        public async Task MarcarComoPagado(string preferenceId)
        {
            var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.PreferenceId == preferenceId);

            if (invoice != null)
            {
                invoice.Status = InvoiceStatus.Paid; // Enum
                invoice.PaidAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}

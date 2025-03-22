using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using Juego_Sin_Nombre.Services.interfacez;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Juego_Sin_Nombre.Services
{
    public class DiamondOfertService : IDiamondOfertService
    {
        private readonly ApplicationContext _context;

        public DiamondOfertService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<DiamondOfert> CreateDiamondOfertAsync(DiamondOfertRequest request)
        {
            // Aquí puedes validar el usuario y la clave antes de guardar
            Usuario usuario = await _context.Usuarios.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
            if (request.Clave != usuario.Clave)
            {
                throw new InvalidOperationException("La contraseña es incorrecta");
            }

            var diamondOfert = new DiamondOfert
            {
                PrecioEnPesos = request.PrecioEnPesos,
                MontoDeDiamantes = request.MontoDeDiamantes,
                Nombre = request.Nombre
            };

            _context.DiamondOfert.Add(diamondOfert);
            await _context.SaveChangesAsync();
            return diamondOfert;
        }
        public async Task<DiamondOfert> UpdateDiamondOfertAsync(int id, DiamondOfertRequest request)
        {
            Usuario usuario = await _context.Usuarios.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
            if (request.Clave != usuario.Clave)
            {
                throw new InvalidOperationException("La contraseña es incorrecta");
            }
            var diamondOfert = await _context.DiamondOfert.FindAsync(id);
            if (diamondOfert == null)
            {
                return null; // Indica que no se encontró la oferta
            }

            // Actualizar los datos
            diamondOfert.PrecioEnPesos = request.PrecioEnPesos;
            diamondOfert.MontoDeDiamantes = request.MontoDeDiamantes;
            diamondOfert.Nombre = request.Nombre;

            await _context.SaveChangesAsync();
            return diamondOfert;
        }

        public async Task<List<DiamondOfert>> GetAllDiamondOfertsAsync()
        {
            return await _context.DiamondOfert
        .Where(o => !o.IsDeleted) // Filtrar eliminados
        .ToListAsync();
        }

        public async Task<bool> SoftDeleteDiamondOfertAsync(UserCredentials request,int id)
        {
            Usuario usuario = await _context.Usuarios.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
            if (request.Clave != usuario.Clave)
            {
                throw new InvalidOperationException("La contraseña es incorrecta");
            }
            var diamondOfert = await _context.DiamondOfert.FindAsync(id);
            if (diamondOfert == null || diamondOfert.IsDeleted)
            {
                return false; // No se encontró o ya está eliminado
            }

            // Marcar como eliminado
            diamondOfert.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }


}

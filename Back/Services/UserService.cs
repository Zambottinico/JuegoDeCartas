using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Juego_Sin_Nombre.Services
{
    public class UserService

    {
        private readonly Data.ApplicationContext _context;

        public UserService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Usuario> ValidateCredentialsAsync(UserCredentials credentials)
        {
            Usuario usuario = await _context.Usuarios.Where(u => u.Id == credentials.UserId).FirstOrDefaultAsync();
            if (usuario ==null)
            {
                throw new InvalidOperationException("El Usuario con id "+credentials.UserId+" no existe");
            }
            if (credentials.Clave != usuario.Clave)
            {
                throw new InvalidOperationException("La contraseña es incorrecta");
            }
            return usuario;
        }
    }
}

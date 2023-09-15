using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Juego_Sin_Nombre.Bussines.UserBussines.Commands
{
    public class LoginUser
    {
        public class LoginUserCommand : IRequest<LoginResponse>
        {
            public string? Username { get; set; }

            public string? Password { get; set; }
            
        }
        public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginResponse>
        {
            private readonly Data.ApplicationContext _context;

            public LoginUserHandler(Data.ApplicationContext context)
            {
                _context = context;
            }

            public async Task<LoginResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
            {
                Usuario user = await _context.Usuarios.Where(u=>u.Username==request.Username && u.Password==request.Password)
                    .FirstOrDefaultAsync();
                LoginResponse loginResponse = new LoginResponse();
                if (user != null)
                {
                    string clave = ClaveGenerator();
                    user.Clave = clave;
                    await _context.SaveChangesAsync();
                    
                    loginResponse.Clave = clave;
                    loginResponse.Username = user.Username;
                    loginResponse.id = user.Id;
                    loginResponse.Ok = true;
                    return loginResponse;
                }
                loginResponse.Ok = false;
                return loginResponse;
            }

            public string ClaveGenerator()
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+"; // Caracteres permitidos en la contraseña
                Random random = new Random();
                StringBuilder passwordBuilder = new StringBuilder();

                for (int i = 0; i < 20; i++)
                {
                    passwordBuilder.Append(chars[random.Next(chars.Length)]);
                }

                return passwordBuilder.ToString();
            }
        }
    }
}

using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using Juego_Sin_Nombre.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Juego_Sin_Nombre.Bussines.UserBussines.Commands
{
    public class LoginUser
    {
        public class LoginUserCommand : IRequest<LoginResponse>
        {
            public string? Email { get; set; }

            public string? Password { get; set; }
            
        }
        public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginResponse>
        {
            private readonly Data.ApplicationContext _context;
            private readonly JwtService _jwtService;

            public LoginUserHandler(Data.ApplicationContext context, JwtService jwtService)
            {
                _context = context;
                _jwtService = jwtService;
            }

            public async Task<LoginResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    Usuario user = await _context.Usuarios
                        .Where(u => u.Email == request.Email && u.Password == request.Password)
                        .FirstOrDefaultAsync();

                    LoginResponse loginResponse = new LoginResponse();

                    if (user != null)
                    {
                        string clave = ClaveGenerator();
                        user.Clave = clave;
                        await _context.SaveChangesAsync();
                        var token = _jwtService.GenerateToken(user.Email,user.Rol);
                        loginResponse.Token = token;
                        loginResponse.Clave = clave;
                        loginResponse.Username = user.Email;
                        loginResponse.id = user.Id;

                        if (user.Rol != null)
                        {
                            loginResponse.Rol = user.Rol;
                        }

                        loginResponse.Ok = true;
                        return loginResponse;
                    }

                    loginResponse.Ok = false;
                    return loginResponse;
                }
                catch (Exception ex)
                {
                    // Manejo de la excepción
                    // Aquí puedes loguear el error, devolver un mensaje apropiado, etc.
                    return new LoginResponse
                    {
                        Ok = false,
                        
                    };
                }

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

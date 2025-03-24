using Google.Apis.Auth;
using Juego_Sin_Nombre.Bussines.UserBussines.Commands;
using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Juego_Sin_Nombre.Bussines.UserBussines.Commands.CreateUser;
using static Juego_Sin_Nombre.Bussines.UserBussines.Commands.LoginUser;

namespace Juego_Sin_Nombre.Services
{
    public class UserService

    {
        private readonly Data.ApplicationContext _context;
        private readonly IMediator _mediator;

        public UserService(ApplicationContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
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

        internal async Task<LoginResponse> LoginGoogleAsync(GoogleJsonWebSignature.Payload payload)
        {
            Usuario usuario = await _context.Usuarios.Where(u => u.Email == payload.Email).FirstOrDefaultAsync();
            //No existe usuario con ese Email
            if (usuario==null)
            {
                CreateUserCommand createUserCommand = new CreateUserCommand();
                createUserCommand.Email = payload.Email;
                createUserCommand.Username = payload.Name;
                createUserCommand.Password = payload.JwtId;
                UserResponseDto userResponse = await _mediator.Send(createUserCommand);
                LoginUserCommand loginUser = new LoginUserCommand();
                loginUser.Email = userResponse.Username;
                loginUser.Password = payload.JwtId;
                return await _mediator.Send(loginUser);
            }
            else{
                //existe el usuario
                LoginUserCommand loginUser = new LoginUserCommand();
                loginUser.Email = usuario.Email;
                loginUser.Password = usuario.Password;
                return await _mediator.Send(loginUser);
            }
            throw new NotImplementedException();
        }
    }
}

using Juego_Sin_Nombre.Bussines.CharacterBussines.Queries;
using Juego_Sin_Nombre.Bussines.UserBussines.Validations;
using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Juego_Sin_Nombre.Bussines.UserBussines.Commands
{
    public class CreateUser
    {

        public class CreateUserCommand : IRequest<UserResponseDto>
        {
            public string Username { get; set; }
            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserResponseDto>
        {
            private readonly Data.ApplicationContext _context;
            private readonly CreateUserValidator _validator;

            public CreateUserHandler(Data.ApplicationContext context,CreateUserValidator validation)
            {
                _context = context;
                _validator=validation;
            }

            //private readonly SaveSocioCommandValidation _validator;
            public async Task<UserResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                
                await _validator.ValidateAsync(request);
                try
                {
                    //Validar que no exista un usuario con el mismo username
                    if (_context.Usuarios.Any(c=> c.Email==request.Email))
                    {
                        // El usuario ya existe, manejar el error
                        throw new InvalidOperationException("El Email ya está en uso.");
                    }
                    Usuario user = new Usuario();
                    List<Character> characters = await _context.Characters
                         .Where(c => c.Id >= 1 && c.Id <= 5)  // Filtrar por IDs 1 a 5
                         .Take(5)  
                         .ToListAsync();
                    user.Characters =characters;
                    user.Email = request.Email;
                    user.Password = request.Password;
                    user.Username = request.Username;
                    user.Rol = "User";
                    //se asignan en 0 el oro y los diamantes
                    user.Gold = 0;
                    user.Diamonds = 0;
                    //Se asignan vidas en 5
                    user.MaxLives = 5;
                    user.Lives = 5;

                    await _context.Usuarios.AddAsync(user);
                    await _context.SaveChangesAsync();
                    UserResponseDto userResponse = new UserResponseDto();
                    userResponse.Email = user.Email;
                    userResponse.Username = user.Username;
                    if (userResponse != null)
                    {

                        return userResponse;
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(userResponse));
                    }

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
    }
}

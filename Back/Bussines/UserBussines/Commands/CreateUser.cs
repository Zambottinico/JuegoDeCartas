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
                    Usuario user = new Usuario();
                    List<Character> characters = await _context.Characters
                         .Where(c => c.Id >= 1 && c.Id <= 5)  // Filtrar por IDs 1 a 5
                         .Take(5)  
                         .ToListAsync();
                    user.Characters =characters;
                    user.Username = request.Username;
                    user.Password = request.Password;
                    await _context.Usuarios.AddAsync(user);
                    await _context.SaveChangesAsync();
                    UserResponseDto userResponse = new UserResponseDto();
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

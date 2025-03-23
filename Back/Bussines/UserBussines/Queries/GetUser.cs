using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Juego_Sin_Nombre.Bussines.UserBussines.Queries
{
    public class GetUser
    {
        public class GetUserCommand : IRequest<UserResponseDto>
        {
            public int Id { get; set; } 
        }
        public class GetUserHandler : IRequestHandler<GetUserCommand, UserResponseDto>
        {
            private readonly Data.ApplicationContext _context;

            public GetUserHandler(Data.ApplicationContext context)
            {
                _context = context;
            }

            public async Task<UserResponseDto> Handle(GetUserCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    Usuario user = await _context.Usuarios.FirstOrDefaultAsync(u=>u.Id==request.Id);
                    if (user!=null)
                    {
                        UserResponseDto dto = new UserResponseDto();
                        dto.Username = user.Username;
                        dto.MaxDays = user.Maxdays;
                        dto.Gold = (int)user.Gold;
                        dto.Diamonds = (int)user.Diamonds;
                        dto.MaxLives = (int)user.MaxLives;
                        dto.Lives = (int)user.Lives;
                        dto.LastLifeRecharge = user.LastLifeRecharge;
                        return dto;
                    }
                    throw new Exception();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
    }
}

using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Juego_Sin_Nombre.Bussines.UserBussines.Queries.GetUser;

namespace Juego_Sin_Nombre.Bussines.UserBussines.Queries
{
    public class GetUsers
    {


        public class GetUsersCommand : IRequest<List<UserResponseDto>>
        {
            public int Id { get; set; }
        }
        public class GetUsersHandler : IRequestHandler<GetUsersCommand, List<UserResponseDto>>
        {
            private readonly Data.ApplicationContext _context;

            public GetUsersHandler(Data.ApplicationContext context)
            {
                _context = context;
            }
            public async Task<List<UserResponseDto>> Handle(GetUsersCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    List<Usuario> users = await _context.Usuarios.Where(u => u.Maxdays != 0 && u.Maxdays!=null).OrderByDescending(u=>u.Maxdays).Take(100).ToListAsync();
                    List<UserResponseDto> usersResponse = new List<UserResponseDto>();
                    if (users != null)
                    {
                        foreach (Usuario u in users)
                        {
                            UserResponseDto dto = new UserResponseDto();
                            dto.Username = u.Username;
                            if (u.Maxdays!=null)
                            {
                                dto.MaxDays = (int)u.Maxdays;
                            }
                            usersResponse.Add(dto);
                        }
                        return usersResponse;
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

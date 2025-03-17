using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Juego_Sin_Nombre.Bussines.CharacterBussines.Queries
{
    public class GetCharacterByPlayerId
    {
        public class GetCharacterByPlayerIdCommand : IRequest<List<CharacterResponse>>
        {
            public int idPlayer { get; set; }
        }
        public class GetCharacterByPlayerHandler : IRequestHandler<GetCharacterByPlayerIdCommand, List<CharacterResponse>>
        {

            private readonly Data.ApplicationContext _context;

            public GetCharacterByPlayerHandler(ApplicationContext context)
            {
                _context = context;
            }

            public async Task<List<CharacterResponse>> Handle(GetCharacterByPlayerIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    List<CharacterResponse> characterResponses = new List<CharacterResponse>();
                    Usuario player = await _context.Usuarios.FirstOrDefaultAsync(u=>u.Id==request.idPlayer);
                    List<Character> characters = await _context.Characters.Where(c=>c.Players.Contains(player)).ToListAsync();
                    if (characters != null)
                    {
                        foreach (Character c in characters)
                        {
                            CharacterResponse characterResponse = new()
                            {
                                Id = c.Id,
                                Name = c.Name,
                                Lore = c.Lore
                            };
                            characterResponses.Add(characterResponse);
                        }
                        return characterResponses;
                    }
                    throw new NullReferenceException("characters was null");
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}

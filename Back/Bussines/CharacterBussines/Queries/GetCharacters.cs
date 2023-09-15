using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Juego_Sin_Nombre.Bussines.CardBussines.Queries.GetCards;

namespace Juego_Sin_Nombre.Bussines.CharacterBussines.Queries
{
    public class GetCharacters
    {
        public class GetCharactersCommand : IRequest<List<CharacterResponse>>
        {

        }


        public class GetCharactersHandler : IRequestHandler<GetCharactersCommand, List<CharacterResponse>>
        {
            private readonly Data.ApplicationContext _context;

            public GetCharactersHandler(Data.ApplicationContext context)
            {
                _context = context;
            }

            public async Task<List<CharacterResponse>> Handle(GetCharactersCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    List<CharacterResponse> characterResponses = new List<CharacterResponse>();
                    List<Character> characters = await _context.Characters.ToListAsync();
                    if (characters != null)
                    {
                        foreach (Character c in characters)
                        {
                            CharacterResponse characterResponse = new()
                            {
                                Id=c.Id,
                                Name=c.Name
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

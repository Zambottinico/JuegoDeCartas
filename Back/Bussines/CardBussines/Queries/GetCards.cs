
using Juego_Sin_Nombre.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Juego_Sin_Nombre.Bussines.CardBussines.Queries
{
    public class GetCards
    {
        public class GetCardsCommand : IRequest<List<Card>>
        {

        }
        public class GetCardsHandler : IRequestHandler<GetCardsCommand, List<Card>>
        {
            private readonly Data.ApplicationContext _context;

            public GetCardsHandler(Data.ApplicationContext context)
            {
                _context = context;
            }

            public async Task<List<Card>> Handle(GetCardsCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    List<Card> cards =await _context.Cards.ToListAsync();
                    if (cards != null)
                    {
                        return cards;
                    }
                    throw new NullReferenceException("cards fue null");
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}

using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Juego_Sin_Nombre.Bussines.CardBussines
{
    public class PostCard
    {
        public class PostCardCommand:IRequest<CardResponse>
        {
            public int PlayerId { get; set; }
            public string Clave { get; set; }
            public int Typeid { get; set; }

            public string Description { get; set; }
            public int CharacterId { get; set; }

            public PostDecisionDto decision1 { get; set; }
            public PostDecisionDto decision2 { get; set; }
        }

        public class PostCardHandler : IRequestHandler<PostCardCommand, CardResponse>
        {
            private readonly Data.ApplicationContext _context;

            public PostCardHandler(Data.ApplicationContext context)
            {
                _context = context;
            }

            public async Task<CardResponse> Handle(PostCardCommand request, CancellationToken cancellationToken)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        //validacion
                        Usuario usuario = await _context.Usuarios.Where(u => u.Id == request.PlayerId).FirstOrDefaultAsync();
                        if (request.Clave != usuario.Clave)
                        {
                            throw new InvalidOperationException("La contraseña es incorrecta");
                        }
                        if (usuario.Rol!="Admin")
                        {
                            throw new Exception("El Usuario con id: "+request.PlayerId+" no tiene permisos de Administrador");
                        }
                        // GUARDAR DECISIONES
                        Decision decision1 = new Decision
                        {
                            Description = request.decision1.Description,
                            Population = request.decision1.Population,
                            Army = request.decision1.Army,
                            Economy = request.decision1.Economy,
                            Magic = request.decision1.Magic,
                            UnlockableCharacter = request.decision1.UnlockableCharacter
                        };

                        Decision decision2 = new Decision
                        {
                            Description = request.decision2.Description,
                            Population = request.decision2.Population,
                            Army = request.decision2.Army,
                            Economy = request.decision2.Economy,
                            Magic = request.decision2.Magic,
                            UnlockableCharacter = request.decision2.UnlockableCharacter
                        };

                        await _context.Decisions.AddRangeAsync(decision1, decision2);
                        await _context.SaveChangesAsync();

                        // RECUPERAR ID DECISIONES


                        Card card = new Card
                        {
                            Description = request.Description,
                            Typeid = request.Typeid,
                            Decision1 = decision1.Id,
                            Decision2 = decision2.Id,
                            CharacterId = request.CharacterId,
                            IsPlayable=true
                        };

                        await _context.Cards.AddAsync(card);
                        await _context.SaveChangesAsync();

                        CardResponse cardResponse = new CardResponse
                        {
                            Description = request.Description,
                            Decision1 = request.decision1,
                            Decision2 = request.decision2,
                            Typeid = request.Typeid
                        };

                        Card carOld = await _context.Cards
                            .Include(c => c.Character)
                            .Where(c => c.Id == card.Id)
                            .FirstOrDefaultAsync();

                        cardResponse.Character = carOld.Character.Name;

                        await transaction.CommitAsync(); // Confirmar la transacción.

                        return cardResponse;
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync(); // Revertir la transacción en caso de error.
                        throw;
                    }
                }
            }

        }
    }
}

using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using Juego_Sin_Nombre.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Juego_Sin_Nombre.Bussines.GameBussines.Command
{
    public class PlayGame
    {
        public class PlayGameCommand : IRequest<GameResponse>
        {
            public int Userid { get; set; }
            public string clave { get; set; }
            public int Decision { get; set; }
        }
        public class PlayGameHandler : IRequestHandler<PlayGameCommand, GameResponse>
        {
            private readonly Data.ApplicationContext _context;
            private GameService gameService;

            public PlayGameHandler(Data.ApplicationContext context, GameService gameService)
            {
                _context = context;
                this.gameService = gameService;
            }


            public async Task<GameResponse> Handle(PlayGameCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    //validar usuario 
                    Usuario usuario = await _context.Usuarios.Where(u => u.Id == request.Userid).FirstOrDefaultAsync();
                    if (request.clave != usuario.Clave)
                    {
                        throw new InvalidOperationException("La contraseña es incorrecta");
                    }
                    if (usuario.Lives < 1)
                    {
                        throw new InvalidOperationException("No tiene vidas suficientes");
                    }

                    Game game = await _context.Games.FirstOrDefaultAsync(g => g.Userid == request.Userid && g.Gamestate!= "FINISHED");
                    if (game != null)
                    {
                        GameResponse gameResponse = await gameService.Play(game, request.Decision);
                        return gameResponse;
                    }
                    else
                    {
                        Game game1 = await gameService.CreateGame(request.Userid);
                        await _context.AddAsync(game1);
                        await _context.SaveChangesAsync();
                        return await gameService.MapperGameToGameResponseAsync(game1,false);
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

using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using Juego_Sin_Nombre.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Juego_Sin_Nombre.Bussines.GameBussines.Command
{
    public class PostGame
    {
        public class PostGameCommand : IRequest<GameResponse>
        {
            public int Userid { get; set; }
            public string clave { get; set; }
        }

        public class PostGameHandler : IRequestHandler<PostGameCommand, GameResponse>
        {
            private readonly Data.ApplicationContext _context;
            private GameService gameService; 

            public PostGameHandler(Data.ApplicationContext context,GameService gameService)
            {
                _context = context;
                this.gameService = gameService;
            }

            public async Task<GameResponse> Handle(PostGameCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    Usuario usuario = await _context.Usuarios.Where(u => u.Id == request.Userid).FirstOrDefaultAsync();
                    if (request.clave != usuario.Clave)
                    {
                        throw new InvalidOperationException("La contraseña es incorrecta");
                    }

                    Game gameSaved = await _context.Games.Where(g=>g.Gamestate!= "FINISHED").FirstOrDefaultAsync(g=>g.Userid==request.Userid);          

                    if (gameSaved!=null && gameSaved.Gamestate!= "FINISHED")
                    { 
                        return gameService.MapperGameToGameResponse(gameSaved,false);
                    }
                    else
                    {
                        
                        Game game = await gameService.CreateGame(request.Userid);
                        await _context.AddAsync(game);
                        await _context.SaveChangesAsync();

                        return gameService.MapperGameToGameResponse(game, false);
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

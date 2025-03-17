using Juego_Sin_Nombre.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Juego_Sin_Nombre.Bussines.GameBussines.Command.PlayGame;
using static Juego_Sin_Nombre.Bussines.GameBussines.Command.PostGame;

namespace Juego_Sin_Nombre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GameController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Post")]
        public async Task<GameResponse> PostGame([FromBody] PostGameCommand postGameCommand)
        {
            return await _mediator.Send(postGameCommand);
        }
        [HttpPut]
        [Route("Play")]
        public async Task<GameResponse> PostGame([FromBody] PlayGameCommand playGameCommand)
        {
             return await _mediator.Send(playGameCommand); 
        }

    }
}

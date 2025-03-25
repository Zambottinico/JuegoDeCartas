using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using Juego_Sin_Nombre.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        private readonly GameService _gameService;

        public GameController(IMediator mediator,GameService gameService)
        {
            _mediator = mediator;
            _gameService = gameService;
        }

        [HttpPost]
        [Route("Post")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<GameResponse> PostGame([FromBody] PostGameCommand postGameCommand)
        {
            return await _mediator.Send(postGameCommand);
        }
        [HttpPut]
        [Route("Play")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<GameResponse> PostGame([FromBody] PlayGameCommand playGameCommand)
        {
             return await _mediator.Send(playGameCommand); 
        }


        [HttpPut]
        [Route("config")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateOrCreateGameConfig([FromBody] GameConfig gameConfig)
        {
            if (gameConfig == null)
            {
                return BadRequest("Invalid GameConfig data.");
            }

            var updatedConfig = await _gameService.UpdateOrCreateGameConfigAsync(gameConfig);

            return Ok(updatedConfig);
        }

        [HttpGet]
        [Route("config")]
        public async Task<IActionResult> GetGameConfig()
        {
            try
            {
                var config = await _gameService.GetGameConfigAsync();
                return Ok(config);
            }
            catch (InvalidOperationException ex)
            {
                
                return NotFound(ex.Message);
            }
        }
    }
}

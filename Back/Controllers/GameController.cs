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
        private readonly UserService _userService;

        public GameController(IMediator mediator,GameService gameService, UserService userService)
        {
            _mediator = mediator;
            _gameService = gameService;
            _userService = userService;
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

        [HttpPost("cupon/crear")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CrearCupon([FromBody] CrearCuponRequest request)
        {
            if (request == null)
            {
                return BadRequest("Los datos del cupón no son válidos.");
            }

            try
            {
                var cuponCreado = await _gameService.CrearCupon(request);
                return Ok(cuponCreado); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el cupón: {ex.Message}");
            }
        }
        [HttpPut("cupon/actualizar")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> updateCupon([FromBody] UpdateCuponRequest request)
        {
            if (request == null)
            {
                return BadRequest("Los datos del cupón no son válidos.");
            }

            try
            {
                 await _gameService.updateCupon(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el cupón: {ex.Message}");
            }
        }
        [HttpGet("cupon/getAll")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllCupons()
        {
            var cupons = await _gameService.GetAllCupons();
            return Ok(cupons);
        }

        [HttpPost("canjear/{codigo}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> CanjearCupon(string codigo, [FromBody] UserCredentials credentials)
        {
            try
            {
                Usuario user = await _userService.ValidateCredentialsAsync(credentials);
                CanjearCuponResponse result = await _gameService.CanjearCupon(codigo, user);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al canjear el cupón: {ex.Message}");
            }
        }

    }
}

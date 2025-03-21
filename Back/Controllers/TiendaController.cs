using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Juego_Sin_Nombre.Controllers { 

   

    public class TiendaController : Controller


    {
        private readonly GameService _gameService;
        private readonly TiendaService _tiendaService;
        public TiendaController(GameService gameService, TiendaService tiendaService) {
        _gameService= gameService;
            _tiendaService = tiendaService;
        }

        [HttpPost("api/lives/recharge")]
        public async Task<IActionResult> RechargeLives([FromBody] RechargeLivesRequest request)
        {
            try
            {
                await _gameService.RechargeAsync(request);
                return Ok(new { message = "Lives recharged successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", details = ex.Message });
            }
        }

        [HttpPost("api/tienda/createCardOfert")]
        public async Task<IActionResult> createCardOfert([FromBody] CreateCardOfertRequest request)
        {
            try
            {
                await _tiendaService.createCardOfert(request);
                return Ok(new { message = "Se creo correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", details = ex.Message });
            }
        }
        [HttpPut("api/tienda/updateCardOfert")]
        public async Task<IActionResult> updateCardOfert([FromBody] UpdateCardOfertRequest request)
        {
            try
            {
                await _tiendaService.UpdateCardOfert(request);
                return Ok(new { message = "se actualizo correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", details = ex.Message });
            }
        }
        [HttpDelete("api/tienda/deleteCardOfert/{id}")]
        public async Task<IActionResult> DeleteCardOfert(int id, [FromBody] DeleteRequest request)
        {
            try
            {
                bool result = await _tiendaService.DeleteCardOfertAsync(id, request);
                if (result)
                {
                    return Ok(new { message = "Oferta eliminada correctamente" });
                }
                return BadRequest(new { message = "No se pudo eliminar la oferta" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpGet("api/cardoferts/all")]
        public async Task<IActionResult> GetAllCardOferts()
        {
            try
            {
                var cardOferts = await _tiendaService.GetAllCardOfertsAsync();
                return Ok(cardOferts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", details = ex.Message });
            }
        }

        [HttpGet("api/cardoferts")]
        public async Task<IActionResult> GetCardOfertsByUser([FromQuery] int userId)
        {
            try
            {
                var cardOferts = await _tiendaService.GetCardOfertsAsync(userId);
                return Ok(cardOferts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", details = ex.Message });
            }
        }

        [HttpPost("api/cardoferts/UnlockCharacter")]
        public async Task<IActionResult> UnlockCharacter([FromBody] UnlockCharacterRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Clave))
            {
                return BadRequest("Datos inválidos");
            }

            var result = await _tiendaService.UnlockCharacterAsync(request.UserId, request.Clave, request.IdCardOfert);

            

            return Ok(result);
        }
    }


}

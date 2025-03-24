using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using Juego_Sin_Nombre.Services.interfacez;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Juego_Sin_Nombre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiamondOfertController : ControllerBase
    {
        private readonly IDiamondOfertService _diamondOfertService;

        public DiamondOfertController(IDiamondOfertService diamondOfertService)
        {
            _diamondOfertService = diamondOfertService;
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateDiamondOfert([FromBody] DiamondOfertRequest request)
        {
            if (request == null)
            {
                return BadRequest("La solicitud no puede estar vacía.");
            }

            // Opcional: Validar userId y clave (puedes hacerlo contra la base de datos)
            if (string.IsNullOrEmpty(request.UserId.ToString()) || string.IsNullOrEmpty(request.Clave))
            {
                return Unauthorized("Usuario o clave inválidos.");
            }

            var createdOfert = await _diamondOfertService.CreateDiamondOfertAsync(request);
            return CreatedAtAction(nameof(CreateDiamondOfert), new { id = createdOfert.Id }, createdOfert);
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateDiamondOfert(int id, [FromBody] DiamondOfertRequest request)
        {
            if (request == null)
            {
                return BadRequest("La solicitud no puede estar vacía.");
            }

            if (string.IsNullOrEmpty(request.UserId.ToString()) || string.IsNullOrEmpty(request.Clave))
            {
                return Unauthorized("Usuario o clave inválidos.");
            }

            var updatedOfert = await _diamondOfertService.UpdateDiamondOfertAsync(id, request);

            if (updatedOfert == null)
            {
                return NotFound($"No se encontró una oferta con el ID {id}.");
            }

            return Ok(updatedOfert);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDiamondOferts()
        {
            var ofertas = await _diamondOfertService.GetAllDiamondOfertsAsync();
            return Ok(ofertas);
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteDiamondOfert(int id, [FromBody] UserCredentials request)
        {
            var deleted = await _diamondOfertService.SoftDeleteDiamondOfertAsync(request,id);
            if (!deleted)
            {
                return NotFound($"No se encontró la oferta con ID {id} o ya está eliminada.");
            }

            return NoContent();
        }
    }
}

using Juego_Sin_Nombre.Dtos;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Juego_Sin_Nombre.Bussines.CharacterBussines.Queries.GetCharacterByPlayerId;
using static Juego_Sin_Nombre.Bussines.CharacterBussines.Queries.GetCharacters;
using static Juego_Sin_Nombre.Bussines.UserBussines.Queries.GetUser;

namespace Juego_Sin_Nombre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CharacterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetCharacters")]
        public async Task<List<CharacterResponse>> GetCharacters()
        {
            return await _mediator.Send(new GetCharactersCommand());
        }

        [HttpGet]
        [Route("GetCharactersByUserId/{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<List<CharacterResponse>> GetCharacterByUserId(int id)
        {
            return await _mediator.Send(new GetCharacterByPlayerIdCommand { idPlayer = id });
        }
    }
}

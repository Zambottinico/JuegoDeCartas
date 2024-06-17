using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Juego_Sin_Nombre.Bussines.CardBussines.PostCard;
using static Juego_Sin_Nombre.Bussines.CardBussines.Queries.GetCards;

namespace Juego_Sin_Nombre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Post")]
        public async Task<CardResponse> CreateCard([FromBody] PostCardCommand postCardCommand)
        {
            return await _mediator.Send(postCardCommand);
        }

        [HttpGet]
        [Route("GetCards")]
        public async Task<List<Card>> GetCard()
        {
            return await _mediator.Send(new GetCardsCommand());
        }

        [HttpGet]
        [Route("Ping")]
        public string PingPong()
        {
            return "pong";
        }
    }
}

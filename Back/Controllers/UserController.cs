using Juego_Sin_Nombre.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Juego_Sin_Nombre.Bussines.UserBussines.Commands.CreateUser;
using static Juego_Sin_Nombre.Bussines.UserBussines.Commands.LoginUser;
using static Juego_Sin_Nombre.Bussines.UserBussines.Queries.GetUser;
using static Juego_Sin_Nombre.Bussines.UserBussines.Queries.GetUsers;

namespace Juego_Sin_Nombre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Post")]
        public async Task<UserResponseDto> CreateUser([FromBody] CreateUserCommand createUserCommand)
        {
            return await _mediator.Send(createUserCommand);
        }

        [HttpGet]
        [Route("GetUserById/{id}")]
        public async Task<UserResponseDto> GetUserById(int id)
        {
            return await _mediator.Send(new GetUserCommand { Id= id });
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<List<UserResponseDto>> GetUsers()
        {
            return await _mediator.Send(new GetUsersCommand());
        }

        [HttpPost]
        [Route("Login")]
        public async Task<LoginResponse> login([FromBody] LoginUserCommand loginUser )
        {
            return await _mediator.Send(loginUser);
        }
    }
}

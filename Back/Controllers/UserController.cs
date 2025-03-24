using Google.Apis.Auth;
using Juego_Sin_Nombre.Dtos;
using Juego_Sin_Nombre.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        private readonly UserService _userService;

        public UserController(IMediator mediator, UserService userService)
        {
            _mediator = mediator;
            _userService = userService;
        }

        [HttpPost]
        [Route("Post")]
        public async Task<UserResponseDto> CreateUser([FromBody] CreateUserCommand createUserCommand)
        {
            return await _mediator.Send(createUserCommand);
        }

        [HttpGet]
        [Route("GetUserById/{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<UserResponseDto> GetUserById(int id)
        {
            return await _mediator.Send(new GetUserCommand { Id= id });
        }

        [HttpGet]
        [Route("GetUsers")]
        [Authorize(Policy = "UserOrAdmin")]
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


        // Endpoint para manejar la autenticación con Google
        [HttpPost("google")]
        public async Task<LoginResponse> GoogleLogin([FromBody] GoogleTokenRequest tokenRequest)
        {
            var idToken = tokenRequest.Token;

            try
            {
                // Verifica y valida el token de Google
                var payload = await VerifyGoogleToken(idToken);
               return await _userService.LoginGoogleAsync(payload);
                // Si el token es válido, puedes autenticar al usuario o crear un nuevo usuario en tu base de datos
                // Aquí puedes personalizar la lógica para autenticar al usuario en tu sistema

                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Método para verificar el token de Google
        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                return payload; 
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar el token de Google: " + ex.Message);
            }
        }
    }

    // Modelo para recibir el token de Google desde el frontend
    public class GoogleTokenRequest
    {
        public string Token { get; set; }
    }
}


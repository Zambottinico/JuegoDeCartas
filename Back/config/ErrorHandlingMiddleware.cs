namespace Juego_Sin_Nombre.config
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pasar la solicitud al siguiente middleware o controlador
                await _next(context);
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción no manejada
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                // Serializar el error a JSON para devolverlo al cliente
                var result = JsonSerializer.Serialize(new
                {
                    message = "Ocurrió un error en el servidor",
                    details = ex.Message,
                    // Si quieres agregar detalles adicionales como el stack trace, descomenta la siguiente línea
                    // stackTrace = ex.StackTrace
                });

                await context.Response.WriteAsync(result);
            }
        }
    }

}

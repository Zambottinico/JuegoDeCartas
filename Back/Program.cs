using FluentValidation.AspNetCore;
using Juego_Sin_Nombre.config;
using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using static Juego_Sin_Nombre.Bussines.GameBussines.Command.PlayGame;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddAuthentication(JwtBearerDefaults.Authentic)

// Obtener la configuración de la cadena de conexión
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

// Agregar configuración del DbContext
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("ConnectionString")),
    ServiceLifetime.Scoped);

// Registra las clases con sus dependencias
builder.Services.AddScoped<PlayGameHandler>();
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<CardService>();


builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});
builder.Services.AddFluentValidation(config =>
{
    config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});

//Scaffold-DbContext "Name=ConnectionString" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir Models -ContextDir Data -Context ApplicationContext -Force

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

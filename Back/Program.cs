using FluentValidation.AspNetCore;
using Juego_Sin_Nombre.config;
using Juego_Sin_Nombre.Data;
using Juego_Sin_Nombre.Services;
using Juego_Sin_Nombre.Services.interfacez;
using MercadoPago.Client.MerchantOrder;
using MercadoPago.Client.Payment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using static Juego_Sin_Nombre.Bussines.GameBussines.Command.PlayGame;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });

    // ?? Configurar autenticación JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token en formato: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

//builder.Services.AddAuthentication(JwtBearerDefaults.Authentic)

// Obtener la configuración de la cadena de conexión
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

// Agregar configuración del DbContext
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("ConnectionString")),
    ServiceLifetime.Scoped);

// Registra las clases con sus dependencias
builder.Services.AddScoped<PlayGameHandler>();
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<TiendaService>();
builder.Services.AddScoped<CardService>();
builder.Services.AddScoped<MercadoPagoService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<InvoiceService>();
builder.Services.AddScoped<MerchantOrderClient>();
builder.Services.AddScoped<PaymentClient>();
builder.Services.AddScoped<IDiamondOfertService, DiamondOfertService>();
builder.Services.AddSingleton<JwtService>();


builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);



builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});
builder.Services.AddFluentValidation(config =>
{
    config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});

//Scaffold-DbContext "Name=ConnectionString" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir Models -ContextDir Data -Context ApplicationContext -Force

// Configurar autenticación JWT
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOrAdmin", policy => policy.RequireRole("User","Admin"));
});




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
app.UseAuthentication();  
app.UseAuthorization();
app.MapControllers();

app.Run();

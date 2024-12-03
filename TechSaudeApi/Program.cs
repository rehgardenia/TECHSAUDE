using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using TechSaude.Server.Data;
using TechSaude.Server.Models;
using TechSaude.Server.Repository.AlergiasRepository;
using TechSaude.Server.Repository.AuthRepository;
using TechSaude.Server.Repository.ConsultaRespository;
using TechSaude.Server.Repository.DocumentosRepository;
using TechSaude.Server.Repository.DoencasRepository;
using TechSaude.Server.Repository.MedicoRepository;
using TechSaude.Server.Repository.PacienteRepository;
using TechSaude.Server.Repository.VacinaRepository;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

// Configuração dos Repositories
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();
builder.Services.AddScoped<IVacinaRepository, VacinasRepository>();
builder.Services.AddScoped<IAlergiasRepository, AlergiasRepository>();
builder.Services.AddScoped<IDoencasRepository, DoencasRepository>();
builder.Services.AddScoped<IDocumentoRepository, DocumentoRepository>();

// Configuração do Identity
builder.Services.AddIdentity<Usuario, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Configuração do JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtSecretKey = jwtSettings["SecretKey"];
var jwtExpiryMinutes = int.TryParse(jwtSettings["ExpiryMinutes"], out var expiry) ? expiry : 60; // Default value

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecretKey ?? string.Empty))
    };
});

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Permitir apenas o frontend
              .AllowAnyMethod() // Permitir qualquer método HTTP (GET, POST, etc.)
              .AllowAnyHeader(); // Permitir qualquer cabeçalho
    });
});

// Configuração dos Controllers e JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

// Configuração do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no campo abaixo.\nExemplo: 'Bearer {token}'"
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

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TechSaude API V1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

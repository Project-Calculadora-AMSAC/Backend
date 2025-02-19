using System.Text;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.CommandServices;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.QueryServices;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Infrastructure.Persistence.EFC.Repositories;
using ProjectCalculadoraAMSAC.Infrastructure.Persistence.Repositories;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Interfaces.ASP.Configuration;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Configuration;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Repositories;
using ProjectCalculadoraAMSAC.User.Application.Internal.CommandServices;
using ProjectCalculadoraAMSAC.User.Application.Internal.OutboundServices;
using ProjectCalculadoraAMSAC.User.Application.Internal.QueryServices;
using ProjectCalculadoraAMSAC.User.Domain.Repositories;
using ProjectCalculadoraAMSAC.User.Domain.Services;
using ProjectCalculadoraAMSAC.User.Infraestructure.Hashing.BCrypt.Services;
using ProjectCalculadoraAMSAC.User.Infraestructure.Persistence.EFC.Repositories;
using ProjectCalculadoraAMSAC.User.Infraestructure.Tokens.JWT.Configurations;
using ProjectCalculadoraAMSAC.User.Infraestructure.Tokens.JWT.Services;
using ProjectCalculadoraAMSAC.User.Interfaces.ACL;
using ProjectCalculadoraAMSAC.User.Interfaces.ACL.Services;

var builder = WebApplication.CreateBuilder(args);


// Configurar autenticación JWT
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "Bearer";
        options.DefaultChallengeScheme = "Bearer";
    })
    .AddJwtBearer("Bearer", options =>
    {
        var secret = builder.Configuration["TokenSettings:Secret"];
        if (string.IsNullOrEmpty(secret))
        {
            throw new ArgumentNullException(nameof(secret), "JWT Secret is not configured in appsettings.json.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };
    });

// Configurar DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
        .EnableDetailedErrors(builder.Environment.IsDevelopment());
});

builder.Services.AddControllers(options =>
{
        options.Conventions.Add(new KebabCaseRouteNamingConvention());
});

// Configurar opciones de enrutamiento
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.WithOrigins("http://localhost:8081") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CalculadoraAMSAC API",
        Version = "v1",
        Description = "AMSAC Platform API",
        TermsOfService = new Uri("http://localhost:5000/swagger/index.html"),
        Contact = new OpenApiContact
        {
            Name = "Bruno&Erick",
            Email = "erickpalomino0723@gmail.com"
        },
        License = new OpenApiLicense
        {
            Name = "Apache 2.0",
            Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT token with Bearer prefix",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IAuthUserRepository, AuthUserRepository>();
builder.Services.AddScoped<IAuthUserCommandService, AuthUserCommandService>();
builder.Services.AddScoped<IAuthUserQueryService, AuthUserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();
builder.Services.AddScoped<IEstimacionRepository, EstimacionRepository>();
builder.Services.AddScoped<IEstimacionCommandService, EstimacionCommandService>();
builder.Services.AddScoped<IEstimacionQueryService, EstimacionQueryService>();
builder.Services.AddScoped<IProyectoRepository, ProyectoRepository>();
builder.Services.AddScoped<IProyectoCommandService, ProyectoCommandService>();
builder.Services.AddScoped<IProyectoQueryService, ProyectoQueryService>();
builder.Services.AddScoped<ITipoPamRepository, TipoPamRepository>();
builder.Services.AddScoped<ITipoPamCommandService, TipoPamCommandService>();
builder.Services.AddScoped<ITipoPamQueryService, TipoPamQueryService>();
builder.Services.AddScoped<IAtributoPamRepository, AtributoPamRepository>();
builder.Services.AddScoped<IAtributoPamCommandService, AtributoPamCommandService>();
builder.Services.AddScoped<IAtributoPamQueryService, AtributoPamQueryService>();
builder.Services.AddScoped<IUnidadDeMedidaRepository, UnidadDeMedidaRepository>();
builder.Services.AddScoped<IUnidadDeMedidaCommandService, UnidadDeMedidaCommandService>();
builder.Services.AddScoped<IUnidadDeMedidaQueryService, UnidadDeMedidaQueryService>();
builder.Services.AddScoped<IVariablesPamRepository, VariablesPamRepository>();
builder.Services.AddScoped<IVariablesPamCommandService, VariablesPamCommandService>();
builder.Services.AddScoped<IVariablesPamQueryService, VariablesPamQueryService>();
builder.Services.AddScoped<IValorAtributoEstimacionQueryService, ValorAtributoEstimacionQueryService>();
builder.Services.AddScoped<IValorAtributoEstimacionCommandService, ValorAtributoEstimacionCommandService>();
builder.Services.AddScoped<IValorAtributoEstimacionRepository, ValorAtributoEstimacionRepository>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICostoEstimadoRepository, CostoEstimadoRepository>();

var app = builder.Build();

// Log Server Addresses
var serverAddresses = app.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>();

foreach (var address in serverAddresses.Addresses)
{
    Console.WriteLine($"Listening on {address}");
}

// Ensure Database is Created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Configure Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

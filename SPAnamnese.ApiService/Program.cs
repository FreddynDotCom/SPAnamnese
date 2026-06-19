using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SPAnamnese.ApiService.Data;
using SPAnamnese.ApiService.Interfaces;
using SPAnamnese.ApiService.Mapper;
using SPAnamnese.ApiService.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

string defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(defaultConnection, ServerVersion.AutoDetect(defaultConnection)));

//AUTH SETTINGS-------------------------------------------------------------------
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key não configurada em appsettings.json.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "JwtRoleDemo.Api";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "JwtRoleDemo.Client";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAudience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception is SecurityTokenExpiredException)
                {
                    context.Response.Headers.Append("Token-Expired", "true");
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
//--------------------------------------------------------------------------------
// CORS SETTINGS------------------------------------------------------------------
var origensPermitidas = builder.Configuration.GetSection("CorsOrigensPermitidas").Get<string[]>()
    ?? new[] { "https://localhost:5205" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
    {
        policy.WithOrigins(origensPermitidas)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
//--------------------------------------------------------------------------------

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new();
        document.Components.SecuritySchemes ??= new Dictionary<string, Microsoft.OpenApi.Models.OpenApiSecurityScheme>();

        document.Components.SecuritySchemes.Add("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Description = "Informe **somente** o token JWT (sem o prefixo 'Bearer '). " +
                       "O Swagger adiciona o prefixo automaticamente.",
            Name = "Authorization",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });

        document.SecurityRequirements ??= new List<Microsoft.OpenApi.Models.OpenApiSecurityRequirement>();
        document.SecurityRequirements.Add(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        return Task.CompletedTask;
    });
});

builder.Services.AddAutoMapper(typeof(CoreMapper));

//Injeção de Dependencia
builder.Services.AddScoped<IPacientesSistema, PacientesSistemaService>();
builder.Services.AddScoped<IAnamnese, AnamneseService>();
builder.Services.AddScoped<IUsuario, UsuarioService>();

//AUTH INJECTION
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(op => op.SwaggerEndpoint("/openapi/v1.json", "SPAnamnese"));
}

app.MapDefaultEndpoints();

app.MapControllers();
app.Run();
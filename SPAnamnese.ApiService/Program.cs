using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SPAnamnese.ApiService.Data;
using SPAnamnese.ApiService.Interfaces;
using SPAnamnese.ApiService.Mapper;
using SPAnamnese.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

string defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(defaultConnection, ServerVersion.AutoDetect(defaultConnection)));

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(CoreMapper));

//Injeção de Dependencia
builder.Services.AddScoped<IPacientesSistema, PacientesSistemaService>();
builder.Services.AddScoped<IAnamnese, AnamneseService>();

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
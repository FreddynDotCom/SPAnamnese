using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SPAnamnese.Web;
using SPAnamnese.Web.Auth;
using SPAnamnese.Web.Components;
using SPAnamnese.Web.Services;
using SPAnamnese.Web.ServiceWeb;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

//Configuração das APIs
string urlService = "https+http://apiservice/api/";

// ---------------------------------------------------------------------
// LocalStorage (armazenamento dos tokens no navegador)
// ---------------------------------------------------------------------
builder.Services.AddBlazoredLocalStorage();

// ---------------------------------------------------------------------
// Autenticação / autorização do Blazor
// ---------------------------------------------------------------------
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());

builder.Services.AddScoped<AuthService>();
builder.Services.AddTransient<AuthTokenHandler>();

// ---------------------------------------------------------------------
// HttpClient "API.Anonimo": sem o handler de token.
// Usado pelo AuthService para login/registro (evita recursão no 401).
// ---------------------------------------------------------------------
builder.Services.AddHttpClient("API.Anonimo", client =>
{
    client.BaseAddress = new Uri(urlService);
});

// ---------------------------------------------------------------------
// Clients autenticados: agora com o AuthTokenHandler injetando o Bearer token
// ---------------------------------------------------------------------
builder.Services.AddHttpClient<PacientesSistemaServiceWeb>(client =>
{
    client.BaseAddress = new Uri(urlService);
}).AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<AnamneseServiceWeb>(client =>
{
    client.BaseAddress = new Uri(urlService);
}).AddHttpMessageHandler<AuthTokenHandler>();
//END

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
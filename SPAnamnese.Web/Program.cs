using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SPAnamnese.Web;
using SPAnamnese.Web.Auth;
using SPAnamnese.Web.Components;
using SPAnamnese.Web.Handler;
using SPAnamnese.Web.Services;
using SPAnamnese.Web.ServiceWeb;

var builder = WebApplication.CreateBuilder(args);

//builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

//Autenticação
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<TokenStore>();
builder.Services.AddScoped<AuthorizationMessageHandlerScoped>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

//Configuração das APIs
string urlService = "https+http://apiservice/api/";

builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri(urlService);
});

builder.Services.AddHttpClient<PacientesSistemaServiceWeb>(client =>
{
    client.BaseAddress = new Uri(urlService);
}).AddHttpMessageHandler<AuthorizationMessageHandlerScoped>();

builder.Services.AddHttpClient<AnamneseServiceWeb>(client =>
{
    client.BaseAddress = new Uri(urlService);
}).AddHttpMessageHandler<AuthorizationMessageHandlerScoped>();
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

//app.MapDefaultEndpoints();

app.Run();
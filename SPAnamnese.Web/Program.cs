using SPAnamnese.Web;
using SPAnamnese.Web.Components;
using SPAnamnese.Web.ServiceWeb;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

//Configuração das APIs
string urlService = "https+http://apiservice/api/";

//Configura o HttpClient
builder.Services.AddHttpClient<PacientesSistemaServiceWeb>(client =>
{
    client.BaseAddress = new Uri(urlService);
});
builder.Services.AddHttpClient<AnamneseServiceWeb>(client =>
{
    client.BaseAddress = new Uri(urlService);
});
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

using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SPAnamnese.Client.Auth;
using SPAnamnese.Client.Services;
using SPAnamnese.Client.ServiceWeb;

namespace SPAnamnese.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            var apiBaseAddress = builder.Configuration["ApiBaseAddress"]
                ?? "https://localhost:7492";
            builder.Services.AddBlazoredLocalStorage();

            //AUTH
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<JwtAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(
                sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());

            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<TokenRenewalService>();
            builder.Services.AddTransient<AuthTokenHandler>();

            //END AUTH

            builder.Services.AddHttpClient("API.Anonimo", client =>
            {
                client.BaseAddress = new Uri(apiBaseAddress);
            });

            builder.Services.AddHttpClient("API.Autenticado", client =>
            {
                client.BaseAddress = new Uri(apiBaseAddress);
            }).AddHttpMessageHandler<AuthTokenHandler>();

            builder.Services.AddScoped<PacientesSistemaServiceWeb>();
            builder.Services.AddScoped<AnamneseServiceWeb>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("API.Autenticado"));

            await builder.Build().RunAsync();
        }
    }
}

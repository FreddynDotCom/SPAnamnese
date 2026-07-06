using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace SPAnamnese.Web.Components.Pages
{
    /// <summary>
    /// Classe base para páginas que exigem login. Não usa [Authorize] de
    /// endpoint (isso quebra em Blazor Server sem AddAuthentication no
    /// pipeline HTTP) — a checagem roda dentro do circuito.
    /// </summary>
    public abstract class PaginaProtegida : ComponentBase
    {
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
        [Inject] protected NavigationManager Nav { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            var estado = await AuthStateProvider.GetAuthenticationStateAsync();
            if (!(estado.User.Identity?.IsAuthenticated ?? false))
            {
                Nav.NavigateTo("/login", forceLoad: false);
                return;
            }

            await OnInitializedAutenticadoAsync();
        }

        protected virtual Task OnInitializedAutenticadoAsync() => Task.CompletedTask;
    }
}
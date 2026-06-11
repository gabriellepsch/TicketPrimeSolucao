using billet_2.Components;
using billet_2.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped(dp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5289")
});

// ALTERADO: ViagemService no lugar de EventoService
builder.Services.AddScoped<ViagemService>();       // Antigo: EventoService
builder.Services.AddScoped<PassageiroService>();   // Antigo: UsuarioService
builder.Services.AddSingleton<AuthService>();      // ATUALIZADO (usa Passageiro)
builder.Services.AddScoped<ReservaService>();      // NOVO (SP-06)
builder.Services.AddSingleton<QrCodeService>();    // NOVO (SP-07)

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddInteractiveServerRenderMode();

app.Run();

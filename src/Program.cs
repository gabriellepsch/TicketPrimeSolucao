using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Registra a connection string do Supabase como Singleton
var connectionString = builder.Configuration.GetConnectionString("Supabase")
    ?? throw new InvalidOperationException("Connection string 'Supabase' não configurada no appsettings.json.");
builder.Services.AddSingleton(connectionString);

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5096")
            .AllowAnyHeader()
            .AllowAnyMethod();                                
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseCors("BlazorPolicy");
app.CadastrarUsuarios();
app.ListarUsuarios();
app.CadastrarViagens();
app.ListarViagens();
app.ListarViagemPorId();
app.PesquisarViagens();
app.CadastrarVeiculos();
app.ListarVeiculos();
app.ListarVeiculoPorId();
app.MapaAssentos();
app.ReservarAssento();
app.LiberarAssento();
app.BloquearAssento();
app.ListarPassagens();
app.ListarPassagensPorUsuario();
app.ComprarPassagem();
app.CancelarPassagem();
app.CadastrarCupons();
app.ListarCupons();
app.UseHttpsRedirection();

app.Run();


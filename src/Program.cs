using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5096")
            .AllowAnyHeader()
            .AllowAnyMethod();                                
    });
});

// Configurar JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "TurismoPrime-Chave-Super-Secreta-2026!";
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseCors("BlazorPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.CadastrarPassageiros();
app.ListarPassageiros();
app.CadastrarViagens();
app.ListarViagens();
app.ListarViagemPorId();
app.CadastrarCupons();
app.ListarCupons();
app.ListarAssentos();
app.CriarReserva();
app.Login();                    // NOVO (SP-08)
app.UseHttpsRedirection();

app.Run();

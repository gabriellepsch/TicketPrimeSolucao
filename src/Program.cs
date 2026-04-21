using Microsoft.Extensions.Options;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseCors("BlazorPolicy");
app.CadastrarUsuarios();
app.ListarUsuarios();
app.CadastrarEventos();
app.ListarEventos();
app.ListarEventoPorId();
app.CadastrarCupons();
app.ListarCupons();
app.UseHttpsRedirection();

app.Run();


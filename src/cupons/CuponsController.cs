using Dapper;
using Npgsql;

public static class CuponsController
{
    // GET /api/cupons/listar
    public static void ListarCupons(this WebApplication app)
    {
        app.MapGet("/api/cupons/listar", (HttpContext httpContext) =>
        {
            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);
            var cupons = connection.Query<Cupons>(
                @"SELECT ""Id"", ""Codigo"", ""PercentualDesconto"" FROM ""Cupons"" ORDER BY ""Id""");
            return Results.Ok(cupons);
        });
    }

    // POST /api/cupons/cadastrar
    public static void CadastrarCupons(this WebApplication app)
    {
        app.MapPost("/api/cupons/cadastrar", (Cupons novoCupon, HttpContext httpContext) =>
        {
            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);

            // Verifica se o código já existe
            var codigoExiste = connection.ExecuteScalar<int>(
                @"SELECT COUNT(1) FROM ""Cupons"" WHERE ""Codigo"" = @Codigo",
                new { novoCupon.Codigo });

            if (codigoExiste > 0)
                return Results.BadRequest("O cupom informado já está cadastrado");

            // Valida percentual de desconto
            if (novoCupon.PercentualDesconto < 0 || novoCupon.PercentualDesconto > 100)
                return Results.BadRequest("O percentual de desconto deve estar entre 0 e 100.");

            // Insere e retorna o ID
            var id = connection.ExecuteScalar<int>(
                @"INSERT INTO ""Cupons"" (""Codigo"", ""PercentualDesconto"")
                  VALUES (@Codigo, @PercentualDesconto)
                  RETURNING ""Id""",
                new { novoCupon.Codigo, novoCupon.PercentualDesconto });

            novoCupon.Id = id;
            return Results.Ok(novoCupon);
        });
    }
}

public class Cupons
{
    public int Id { get; set; }
    public string Codigo { get; set; } = "";
    public int PercentualDesconto { get; set; }
}

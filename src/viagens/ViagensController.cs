using Dapper;
using Npgsql;

public static class ViagensController
{
    // GET /api/viagens/listar
    public static void ListarViagens(this WebApplication app)
    {
        app.MapGet("/api/viagens/listar", (HttpContext httpContext) =>
        {
            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);
            var viagens = connection.Query<Viagem>(
                @"SELECT ""Id"", ""Origem"", ""Destino"", ""DataPartida"", ""DataChegada"", ""DataVolta"",
                         ""Descricao"", ""VeiculoId"", ""PrecoBase"", ""FotoUrl""
                  FROM ""Viagens""
                  ORDER BY ""DataPartida""");
            return Results.Ok(viagens);
        });
    }

    // GET /api/viagens/listar/{id}
    public static void ListarViagemPorId(this WebApplication app)
    {
        app.MapGet("/api/viagens/listar/{id}", (int id, HttpContext httpContext) =>
        {
            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);
            var viagem = connection.QueryFirstOrDefault<Viagem>(
                @"SELECT ""Id"", ""Origem"", ""Destino"", ""DataPartida"", ""DataChegada"", ""DataVolta"",
                         ""Descricao"", ""VeiculoId"", ""PrecoBase"", ""FotoUrl""
                  FROM ""Viagens""
                  WHERE ""Id"" = @Id",
                new { Id = id });

            if (viagem == null)
                return Results.NotFound("Viagem não encontrada.");
            return Results.Ok(viagem);
        });
    }

    // GET /api/viagens/pesquisar?origem=&destino=&data=
    public static void PesquisarViagens(this WebApplication app)
    {
        app.MapGet("/api/viagens/pesquisar", (string? origem, string? destino, DateTime? data, HttpContext httpContext) =>
        {
            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);

            var sql = @"SELECT ""Id"", ""Origem"", ""Destino"", ""DataPartida"", ""DataChegada"", ""DataVolta"",
                               ""Descricao"", ""VeiculoId"", ""PrecoBase"", ""FotoUrl""
                        FROM ""Viagens""
                        WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(origem))
            {
                sql += @" AND ""Origem"" ILIKE @Origem";
                parameters.Add("Origem", $"%{origem}%");
            }

            if (!string.IsNullOrWhiteSpace(destino))
            {
                sql += @" AND ""Destino"" ILIKE @Destino";
                parameters.Add("Destino", $"%{destino}%");
            }

            if (data.HasValue)
            {
                sql += @" AND ""DataPartida""::date = @Data";
                parameters.Add("Data", data.Value.Date);
            }

            sql += @" ORDER BY ""DataPartida""";

            var viagens = connection.Query<Viagem>(sql, parameters);
            return Results.Ok(viagens.ToList());
        });
    }

    // POST /api/viagens/cadastrar
    public static void CadastrarViagens(this WebApplication app)
    {
        app.MapPost("/api/viagens/cadastrar", (Viagem novaViagem, HttpContext httpContext) =>
        {
            // Validação 1: Origem é obrigatória
            if (string.IsNullOrWhiteSpace(novaViagem.Origem))
                return Results.BadRequest("A origem da viagem é obrigatória.");

            // Validação 2: Destino é obrigatório
            if (string.IsNullOrWhiteSpace(novaViagem.Destino))
                return Results.BadRequest("O destino da viagem é obrigatório.");

            // Validação 3: Data de partida deve ser futura
            if (novaViagem.DataPartida < DateTime.Now)
                return Results.BadRequest("A data de partida não pode ser antiga. Informe uma data futura.");

            // Validação 4: Data de chegada deve ser após a partida
            if (novaViagem.DataChegada <= novaViagem.DataPartida)
                return Results.BadRequest("A data de chegada deve ser posterior à data de partida.");

            // Validação 5: Se DataVolta foi informada, deve ser após DataChegada
            if (novaViagem.DataVolta.HasValue && novaViagem.DataVolta.Value <= novaViagem.DataChegada)
                return Results.BadRequest("A data de volta deve ser posterior à data de chegada.");

            // Validação 6: Preço base não pode ser negativo
            if (novaViagem.PrecoBase < 0)
                return Results.BadRequest("O preço base da viagem não pode ser negativo.");

            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);

            // Verifica se o VeiculoId existe
            var veiculoExiste = connection.ExecuteScalar<int>(
                @"SELECT COUNT(1) FROM ""Veiculos"" WHERE ""Id"" = @VeiculoId",
                new { novaViagem.VeiculoId });

            if (veiculoExiste == 0)
                return Results.BadRequest("O veículo informado não existe.");

            // Insere viagem
            var id = connection.ExecuteScalar<int>(
                @"INSERT INTO ""Viagens"" (""Origem"", ""Destino"", ""DataPartida"", ""DataChegada"", ""DataVolta"",
                                           ""Descricao"", ""VeiculoId"", ""PrecoBase"", ""FotoUrl"")
                  VALUES (@Origem, @Destino, @DataPartida, @DataChegada, @DataVolta,
                          @Descricao, @VeiculoId, @PrecoBase, @FotoUrl)
                  RETURNING ""Id""",
                new
                {
                    novaViagem.Origem,
                    novaViagem.Destino,
                    novaViagem.DataPartida,
                    novaViagem.DataChegada,
                    novaViagem.DataVolta,
                    novaViagem.Descricao,
                    novaViagem.VeiculoId,
                    PrecoBase = (decimal)novaViagem.PrecoBase,
                    novaViagem.FotoUrl
                });

            novaViagem.Id = id;
            return Results.Ok(novaViagem);
        });
    }
}

public class Viagem
{
    public int Id { get; set; }
    public string Origem { get; set; } = "";
    public string Destino { get; set; } = "";
    public DateTime DataPartida { get; set; }
    public DateTime DataChegada { get; set; }
    public DateTime? DataVolta { get; set; }
    public string Descricao { get; set; } = "";
    public int VeiculoId { get; set; }
    public float PrecoBase { get; set; }
    public string? FotoUrl { get; set; }
}

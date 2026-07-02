using Dapper;
using Npgsql;

public static class VeiculosController
{
    // GET /api/veiculos/listar
    public static void ListarVeiculos(this WebApplication app)
    {
        app.MapGet("/api/veiculos/listar", (HttpContext httpContext) =>
        {
            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);
            var veiculos = connection.Query<Veiculo>(@"SELECT ""Id"", ""Modelo"", ""Placa"", ""Capacidade"", ""Tipo"", ""Linhas"", ""Colunas"" FROM ""Veiculos"" ORDER BY ""Id""");
            return Results.Ok(veiculos);
        });
    }

    // GET /api/veiculos/listar/{id}
    public static void ListarVeiculoPorId(this WebApplication app)
    {
        app.MapGet("/api/veiculos/listar/{id}", (int id, HttpContext httpContext) =>
        {
            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);
            var veiculo = connection.QueryFirstOrDefault<Veiculo>(
                @"SELECT ""Id"", ""Modelo"", ""Placa"", ""Capacidade"", ""Tipo"", ""Linhas"", ""Colunas"" FROM ""Veiculos"" WHERE ""Id"" = @Id",
                new { Id = id });

            if (veiculo == null)
                return Results.NotFound("Veículo não encontrado.");
            return Results.Ok(veiculo);
        });
    }

    // POST /api/veiculos/cadastrar
    public static void CadastrarVeiculos(this WebApplication app)
    {
        app.MapPost("/api/veiculos/cadastrar", (Veiculo novoVeiculo, HttpContext httpContext) =>
        {
            // Validação 1: Modelo é obrigatório
            if (string.IsNullOrWhiteSpace(novoVeiculo.Modelo))
                return Results.BadRequest("O modelo do veículo é obrigatório.");

            // Validação 2: Placa é obrigatória
            if (string.IsNullOrWhiteSpace(novoVeiculo.Placa))
                return Results.BadRequest("A placa do veículo é obrigatória.");

            // Validação 3: Placa deve ser única
            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);

            var placaExiste = connection.ExecuteScalar<int>(
                @"SELECT COUNT(1) FROM ""Veiculos"" WHERE ""Placa"" = @Placa",
                new { novoVeiculo.Placa });

            if (placaExiste > 0)
                return Results.BadRequest("Já existe um veículo cadastrado com esta placa.");

            // Validação 4: Linhas deve ser > 0
            if (novoVeiculo.Linhas <= 0)
                return Results.BadRequest("O número de linhas (fileiras) deve ser maior que zero.");

            // Validação 5: Colunas deve ser > 0
            if (novoVeiculo.Colunas <= 0)
                return Results.BadRequest("O número de colunas deve ser maior que zero.");

            // Validação 6: Tipo deve ser um valor válido
            var tiposValidos = new[] { "Convencional", "Executivo", "Leito", "Micro-ônibus", "Van" };
            if (!tiposValidos.Contains(novoVeiculo.Tipo, StringComparer.OrdinalIgnoreCase))
                return Results.BadRequest($"Tipo inválido. Tipos permitidos: {string.Join(", ", tiposValidos)}.");

            // Calcular capacidade e normalizar Tipo
            novoVeiculo.Capacidade = novoVeiculo.Linhas * novoVeiculo.Colunas;
            novoVeiculo.Tipo = char.ToUpper(novoVeiculo.Tipo[0]) + novoVeiculo.Tipo[1..].ToLower();

            // Inserir veículo e obter ID
            var id = connection.ExecuteScalar<int>(
                @"INSERT INTO ""Veiculos"" (""Modelo"", ""Placa"", ""Capacidade"", ""Tipo"", ""Linhas"", ""Colunas"")
                  VALUES (@Modelo, @Placa, @Capacidade, @Tipo, @Linhas, @Colunas)
                  RETURNING ""Id""",
                new { novoVeiculo.Modelo, novoVeiculo.Placa, novoVeiculo.Capacidade, novoVeiculo.Tipo, novoVeiculo.Linhas, novoVeiculo.Colunas });

            novoVeiculo.Id = id;

            // Gerar assentos no banco
            GerarAssentos(connection, novoVeiculo);

            return Results.Ok(novoVeiculo);
        });
    }

    // Gera assentos para um veículo recém-cadastrado (insere no banco)
    private static void GerarAssentos(NpgsqlConnection connection, Veiculo veiculo)
    {
        var nomeColunas = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        for (int linha = 1; linha <= veiculo.Linhas; linha++)
        {
            for (int col = 0; col < veiculo.Colunas; col++)
            {
                char letraColuna = nomeColunas[col];
                string numero = $"{linha}{letraColuna}";

                string tipo;
                if (col == 0 || col == veiculo.Colunas - 1)
                    tipo = "Janela";
                else
                    tipo = "Corredor";

                connection.Execute(
                    @"INSERT INTO ""Assentos"" (""VeiculoId"", ""Numero"", ""Tipo"", ""Status"")
                      VALUES (@VeiculoId, @Numero, @Tipo, @Status)",
                    new { VeiculoId = veiculo.Id, Numero = numero, Tipo = tipo, Status = "Disponível" });
            }
        }
    }
}

public class Veiculo
{
    public int Id { get; set; }
    public string Modelo { get; set; } = "";
    public string Placa { get; set; } = "";
    public int Capacidade { get; set; }
    public string Tipo { get; set; } = "";
    public int Linhas { get; set; }
    public int Colunas { get; set; }
}

public class Assento
{
    public int Id { get; set; }
    public int VeiculoId { get; set; }
    public string Numero { get; set; } = "";
    public string Tipo { get; set; } = "";
    public string Status { get; set; } = "Disponível";
}

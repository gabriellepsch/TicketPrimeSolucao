using Dapper;
using Npgsql;

public static class AssentosController
{
    // GET /api/assentos/viagem/{viagemId}
    public static void MapaAssentos(this WebApplication app)
    {
        app.MapGet("/api/assentos/viagem/{viagemId}", (int viagemId, HttpContext httpContext) =>
        {
            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);

            // Localiza a viagem para obter o VeiculoId
            var veiculoId = connection.ExecuteScalar<int?>(
                @"SELECT ""VeiculoId"" FROM ""Viagens"" WHERE ""Id"" = @Id",
                new { Id = viagemId });

            if (veiculoId == null)
                return Results.NotFound("Viagem não encontrada.");

            // Busca os assentos do veículo
            var assentos = connection.Query<Assento>(
                @"SELECT ""Id"", ""VeiculoId"", ""Numero"", ""Tipo"", ""Status""
                  FROM ""Assentos""
                  WHERE ""VeiculoId"" = @VeiculoId
                  ORDER BY ""Id""",
                new { VeiculoId = veiculoId.Value });

            return Results.Ok(assentos.ToList());
        });
    }

    // POST /api/assentos/reservar
    public static void ReservarAssento(this WebApplication app)
    {
        app.MapPost("/api/assentos/reservar", (ReservaRequest request, HttpContext httpContext) =>
        {
            // Validação 1: assentoId > 0
            if (request.AssentoId <= 0)
                return Results.BadRequest("ID do assento inválido.");

            // Validação 2: CPF obrigatório
            if (string.IsNullOrWhiteSpace(request.UsuarioCpf))
                return Results.BadRequest("CPF do usuário é obrigatório para reservar um assento.");

            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);

            // Localiza o assento
            var assento = connection.QueryFirstOrDefault<Assento>(
                @"SELECT ""Id"", ""VeiculoId"", ""Numero"", ""Tipo"", ""Status""
                  FROM ""Assentos""
                  WHERE ""Id"" = @Id",
                new { Id = request.AssentoId });

            if (assento == null)
                return Results.NotFound("Assento não encontrado.");

            // Verifica se o assento está disponível
            if (assento.Status != "Disponível")
                return Results.BadRequest($"Assento {assento.Numero} não está disponível. Status atual: {assento.Status}.");

            // Atualiza o status para Reservado
            connection.Execute(
                @"UPDATE ""Assentos"" SET ""Status"" = 'Reservado' WHERE ""Id"" = @Id",
                new { Id = request.AssentoId });

            assento.Status = "Reservado";
            return Results.Ok(assento);
        });
    }

    // POST /api/assentos/liberar
    public static void LiberarAssento(this WebApplication app)
    {
        app.MapPost("/api/assentos/liberar", (LiberarRequest request, HttpContext httpContext) =>
        {
            // Validação: assentoId > 0
            if (request.AssentoId <= 0)
                return Results.BadRequest("ID do assento inválido.");

            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);

            // Localiza o assento
            var assento = connection.QueryFirstOrDefault<Assento>(
                @"SELECT ""Id"", ""VeiculoId"", ""Numero"", ""Tipo"", ""Status""
                  FROM ""Assentos""
                  WHERE ""Id"" = @Id",
                new { Id = request.AssentoId });

            if (assento == null)
                return Results.NotFound("Assento não encontrado.");

            // Só pode liberar assento que está Reservado
            if (assento.Status != "Reservado")
                return Results.BadRequest($"Assento {assento.Numero} não está reservado. Status atual: {assento.Status}.");

            // Atualiza o status para Disponível
            connection.Execute(
                @"UPDATE ""Assentos"" SET ""Status"" = 'Disponível' WHERE ""Id"" = @Id",
                new { Id = request.AssentoId });

            assento.Status = "Disponível";
            return Results.Ok(assento);
        });
    }

    // POST /api/assentos/bloquear
    public static void BloquearAssento(this WebApplication app)
    {
        app.MapPost("/api/assentos/bloquear", (BloquearRequest request, HttpContext httpContext) =>
        {
            // Validação: assentoId > 0
            if (request.AssentoId <= 0)
                return Results.BadRequest("ID do assento inválido.");

            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);

            // Localiza o assento
            var assento = connection.QueryFirstOrDefault<Assento>(
                @"SELECT ""Id"", ""VeiculoId"", ""Numero"", ""Tipo"", ""Status""
                  FROM ""Assentos""
                  WHERE ""Id"" = @Id",
                new { Id = request.AssentoId });

            if (assento == null)
                return Results.NotFound("Assento não encontrado.");

            if (request.Bloquear)
            {
                if (assento.Status != "Disponível")
                    return Results.BadRequest($"Não é possível bloquear o assento {assento.Numero}. Status atual: {assento.Status}. Apenas assentos Disponíveis podem ser bloqueados.");

                connection.Execute(
                    @"UPDATE ""Assentos"" SET ""Status"" = 'Indisponível' WHERE ""Id"" = @Id",
                    new { Id = request.AssentoId });

                assento.Status = "Indisponível";
            }
            else
            {
                if (assento.Status != "Indisponível")
                    return Results.BadRequest($"Não é possível desbloquear o assento {assento.Numero}. Status atual: {assento.Status}. Apenas assentos Indisponíveis podem ser desbloqueados.");

                connection.Execute(
                    @"UPDATE ""Assentos"" SET ""Status"" = 'Disponível' WHERE ""Id"" = @Id",
                    new { Id = request.AssentoId });

                assento.Status = "Disponível";
            }

            return Results.Ok(assento);
        });
    }
}

// --- Modelos de Request (APENAS para entrada de dados — NÃO redefinem Assento) ---

public class ReservaRequest
{
    public int AssentoId { get; set; }
    public string UsuarioCpf { get; set; } = "";
}

public class LiberarRequest
{
    public int AssentoId { get; set; }
}

public class BloquearRequest
{
    public int AssentoId { get; set; }
    public bool Bloquear { get; set; }
}

using Dapper;
using Npgsql;

public static class PassagensController
{
    // GET /api/passagens/listar
    public static void ListarPassagens(this WebApplication app)
    {
        app.MapGet("/api/passagens/listar", (HttpContext httpContext) =>
        {
            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);
            var passagens = connection.Query<Passagem>(
                @"SELECT ""Id"", ""ViagemId"", ""AssentoId"", ""UsuarioCpf"", ""PrecoPago"",
                         ""CupomUtilizado"", ""Status"", ""DataCompra"", ""DataExpiracaoReserva""
                  FROM ""Passagens""
                  ORDER BY ""Id""");
            return Results.Ok(passagens);
        });
    }

    // GET /api/passagens/usuario/{cpf}
    public static void ListarPassagensPorUsuario(this WebApplication app)
    {
        app.MapGet("/api/passagens/usuario/{cpf}", (string cpf, HttpContext httpContext) =>
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return Results.BadRequest("CPF é obrigatório.");

            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);

            var passagens = connection.Query<Passagem>(
                @"SELECT ""Id"", ""ViagemId"", ""AssentoId"", ""UsuarioCpf"", ""PrecoPago"",
                         ""CupomUtilizado"", ""Status"", ""DataCompra"", ""DataExpiracaoReserva""
                  FROM ""Passagens""
                  WHERE ""UsuarioCpf"" = @Cpf
                  ORDER BY ""Id""",
                new { Cpf = cpf });

            return Results.Ok(passagens.ToList());
        });
    }

    // POST /api/passagens/comprar
    public static void ComprarPassagem(this WebApplication app)
    {
        app.MapPost("/api/passagens/comprar", (CompraRequest request, HttpContext httpContext) =>
        {
            // Validação 1: ViagemId > 0
            if (request.ViagemId <= 0)
                return Results.BadRequest("ID da viagem inválido.");

            // Validação 2: AssentoId > 0
            if (request.AssentoId <= 0)
                return Results.BadRequest("ID do assento inválido.");

            // Validação 3: CPF obrigatório
            if (string.IsNullOrWhiteSpace(request.UsuarioCpf))
                return Results.BadRequest("CPF do usuário é obrigatório.");

            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);

            // Localiza a viagem
            var viagem = connection.QueryFirstOrDefault<Viagem>(
                @"SELECT ""Id"", ""PrecoBase"", ""VeiculoId""
                  FROM ""Viagens"" WHERE ""Id"" = @Id",
                new { Id = request.ViagemId });

            if (viagem == null)
                return Results.NotFound("Viagem não encontrada.");

            // Localiza o assento
            var assento = connection.QueryFirstOrDefault<Assento>(
                @"SELECT ""Id"", ""VeiculoId"", ""Numero"", ""Tipo"", ""Status""
                  FROM ""Assentos""
                  WHERE ""Id"" = @Id",
                new { Id = request.AssentoId });

            if (assento == null)
                return Results.NotFound("Assento não encontrado.");

            // O assento DEVE estar Reservado para ser comprado
            if (assento.Status != "Reservado")
                return Results.BadRequest($"Assento {assento.Numero} não está reservado. Status atual: {assento.Status}. Apenas assentos Reservados podem ser comprados.");

            // Calcula o preço no backend (NÃO recebe do frontend — segurança)
            float precoBase = viagem.PrecoBase;
            float percentualDesconto = 0;
            string? cupomUtilizado = null;

            // Se um cupom foi informado, valida e aplica desconto
            if (!string.IsNullOrWhiteSpace(request.CupomUtilizado))
            {
                var cupom = connection.QueryFirstOrDefault<Cupons>(
                    @"SELECT ""Id"", ""Codigo"", ""PercentualDesconto""
                      FROM ""Cupons""
                      WHERE ""Codigo"" = @Codigo",
                    new { Codigo = request.CupomUtilizado });

                if (cupom == null)
                    return Results.BadRequest($"Cupom '{request.CupomUtilizado}' não encontrado.");

                percentualDesconto = cupom.PercentualDesconto;
                cupomUtilizado = cupom.Codigo;
            }

            // Aplica desconto: PrecoPago = PrecoBase × (1 - desconto/100)
            float precoPago = precoBase * (1 - (percentualDesconto / 100f));

            // Preço não pode ser negativo após desconto
            if (precoPago < 0)
                precoPago = 0;

            // Transaciona assento para Vendido
            connection.Execute(
                @"UPDATE ""Assentos"" SET ""Status"" = 'Vendido' WHERE ""Id"" = @Id",
                new { Id = request.AssentoId });

            // Cria a passagem
            var id = connection.ExecuteScalar<int>(
                @"INSERT INTO ""Passagens"" (""ViagemId"", ""AssentoId"", ""UsuarioCpf"", ""PrecoPago"",
                                              ""CupomUtilizado"", ""Status"", ""DataCompra"")
                  VALUES (@ViagemId, @AssentoId, @UsuarioCpf, @PrecoPago,
                          @CupomUtilizado, 'Ativa', NOW())
                  RETURNING ""Id""",
                new
                {
                    request.ViagemId,
                    request.AssentoId,
                    request.UsuarioCpf,
                    PrecoPago = (decimal)precoPago,
                    CupomUtilizado = cupomUtilizado
                });

            var passagem = new Passagem
            {
                Id = id,
                ViagemId = request.ViagemId,
                AssentoId = request.AssentoId,
                UsuarioCpf = request.UsuarioCpf,
                PrecoPago = precoPago,
                CupomUtilizado = cupomUtilizado,
                Status = "Ativa",
                DataCompra = DateTime.Now,
                DataExpiracaoReserva = null
            };

            return Results.Ok(passagem);
        });
    }

    // POST /api/passagens/cancelar/{id}
    public static void CancelarPassagem(this WebApplication app)
    {
        app.MapPost("/api/passagens/cancelar/{id}", (int id, HttpContext httpContext) =>
        {
            // Validação: id > 0
            if (id <= 0)
                return Results.BadRequest("ID da passagem inválido.");

            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);

            // Localiza a passagem
            var passagem = connection.QueryFirstOrDefault<Passagem>(
                @"SELECT ""Id"", ""ViagemId"", ""AssentoId"", ""UsuarioCpf"", ""PrecoPago"",
                         ""CupomUtilizado"", ""Status"", ""DataCompra"", ""DataExpiracaoReserva""
                  FROM ""Passagens""
                  WHERE ""Id"" = @Id",
                new { Id = id });

            if (passagem == null)
                return Results.NotFound("Passagem não encontrada.");

            // Só pode cancelar passagem Ativa
            if (passagem.Status != "Ativa")
                return Results.BadRequest($"Passagem não pode ser cancelada. Status atual: {passagem.Status}. Apenas passagens Ativas podem ser canceladas.");

            // Transiciona passagem para Cancelada
            connection.Execute(
                @"UPDATE ""Passagens"" SET ""Status"" = 'Cancelada' WHERE ""Id"" = @Id",
                new { Id = id });

            passagem.Status = "Cancelada";

            // Libera o assento associado
            var assento = connection.QueryFirstOrDefault<Assento>(
                @"SELECT ""Id"", ""Status"", ""Numero""
                  FROM ""Assentos""
                  WHERE ""Id"" = @Id",
                new { Id = passagem.AssentoId });

            if (assento != null && assento.Status == "Vendido")
            {
                connection.Execute(
                    @"UPDATE ""Assentos"" SET ""Status"" = 'Disponível' WHERE ""Id"" = @Id",
                    new { Id = passagem.AssentoId });
            }

            return Results.Ok(passagem);
        });
    }
}

// --- Modelo Passagem ---

public class Passagem
{
    public int Id { get; set; }
    public int ViagemId { get; set; }
    public int AssentoId { get; set; }
    public string UsuarioCpf { get; set; } = "";
    public float PrecoPago { get; set; }
    public string? CupomUtilizado { get; set; }
    public string Status { get; set; } = "Ativa";
    public DateTime DataCompra { get; set; }
    public DateTime? DataExpiracaoReserva { get; set; }
}

// --- Modelo de Request (APENAS para entrada do endpoint comprar) ---

public class CompraRequest
{
    public int ViagemId { get; set; }
    public int AssentoId { get; set; }
    public string UsuarioCpf { get; set; } = "";
    public string? CupomUtilizado { get; set; }
}

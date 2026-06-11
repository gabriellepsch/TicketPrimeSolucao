// src/assentos/AssentosController.cs

public static class AssentosController
{
    // Dicionário: ViagemId → Lista de Assentos (KISS: Dictionary simples, sem ConcurrentDictionary)
    private static Dictionary<int, List<Assento>> AssentosPorViagem = new();
    private static readonly object _lockAssentos = new();

    public static void ListarAssentos(this WebApplication app)
    {
        app.MapGet("/api/viagens/{viagemId}/assentos", (int viagemId) =>
        {
            lock (_lockAssentos)
            {
                if (!AssentosPorViagem.ContainsKey(viagemId))
                {
                    return Results.Ok(new List<Assento>());
                }

                var assentos = AssentosPorViagem[viagemId];

                // Filtra expirados: assentos com Status = Reservado e ReservaExpiracao passada
                foreach (var assento in assentos.Where(a =>
                    a.Status == StatusAssento.Reservado &&
                    a.ReservaExpiracao.HasValue &&
                    a.ReservaExpiracao.Value < DateTime.Now))
                {
                    assento.Status = StatusAssento.Disponivel;
                    assento.ReservaExpiracao = null;
                }

                return Results.Ok(assentos);
            }
        });
    }

    public static void CriarReserva(this WebApplication app)
    {
        app.MapPost("/api/reservas", (ReservaRequest request) =>
        {
            lock (_lockAssentos)
            {
                // Validações
                if (!AssentosPorViagem.ContainsKey(request.ViagemId))
                    return Results.BadRequest("Viagem não encontrada.");

                var assento = AssentosPorViagem[request.ViagemId]
                    .FirstOrDefault(a => a.Id == request.AssentoId);

                if (assento == null)
                    return Results.BadRequest("Assento não encontrado.");

                if (assento.Status != StatusAssento.Disponivel)
                    return Results.BadRequest("Assento não está disponível.");

                // Bloqueia o assento (reserva temporária de 15 min)
                assento.Status = StatusAssento.Reservado;
                assento.ReservaExpiracao = DateTime.Now.AddMinutes(15);

                return Results.Ok(new { mensagem = "Assento reservado temporariamente por 15 minutos.", assento });
            }
        });
    }

    // Método auxiliar para popular assentos ao criar uma viagem
    public static void GerarAssentosParaViagem(int viagemId, int totalAssentos, string tipoVeiculo, decimal valorPassagem)
    {
        var assentos = new List<Assento>();
        int assentosPorFileira = tipoVeiculo switch
        {
            "Leito" => 2,       // Cama individual, mais espaçoso
            "Semileito" => 3,   // Reclinação parcial
            _ => 4              // Convencional (padrão)
        };

        // Calcula fator de preço por tipo de veículo (Regra de Negócio #5)
        decimal fatorPreco = tipoVeiculo switch
        {
            "Leito" => 1.60m,      // +60% sobre ValorPassagem
            "Semileito" => 1.20m,  // +20% sobre ValorPassagem
            _ => 1.00m             // Convencional = preço base
        };

        for (int i = 1; i <= totalAssentos; i++)
        {
            // Determina categoria: "Janela" para assentos nas laterais
            string categoria = (i % assentosPorFileira == 0 || (i - 1) % assentosPorFileira == 0)
                ? "Janela"
                : "Corredor";

            // Preço final = ValorPassagem * fatorPreco + extraJanela (Regra de Negócio #5)
            decimal precoExtra = (categoria == "Janela" ? 5.00m : 0);

            assentos.Add(new Assento
            {
                Id = i,
                ViagemId = viagemId,
                Numero = i,
                Status = StatusAssento.Disponivel,
                Categoria = categoria,
                PrecoExtra = precoExtra
            });
        }

        lock (_lockAssentos)
        {
            AssentosPorViagem[viagemId] = assentos;
        }
    }
}

// Modelo de request para criar reserva
public class ReservaRequest
{
    public int ViagemId { get; set; }
    public int AssentoId { get; set; }
    public int PassageiroId { get; set; }
}

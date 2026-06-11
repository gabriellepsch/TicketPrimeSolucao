public static class ViagensController {
    private static List<Viagem> Viagens = new();
    private static int idAtual = 1;

    public static void ListarViagens(this WebApplication app) {
        app.MapGet("/api/viagens/listar", () => {
            return Results.Ok(Viagens);
        });
    }

    public static void ListarViagemPorId(this WebApplication app) {
        app.MapGet("/api/viagens/listar/{id}", (int id) => {
            var viagem = Viagens.FirstOrDefault(v => v.Id == id);
            if (viagem == null)
                return Results.NotFound("Viagem não encontrada.");
            return Results.Ok(viagem);
        });
    }

    public static void CadastrarViagens(this WebApplication app) {
        app.MapPost("/api/viagens/cadastrar", (Viagem novaViagem) => {
            if (Viagens.Any(v => v.Destino == novaViagem.Destino && v.DataSaida == novaViagem.DataSaida)) {
                return Results.BadRequest("Já existe uma viagem para este destino na mesma data.");
            }

            if (novaViagem.DataSaida < DateTime.Now) {
                return Results.BadRequest("A data de partida não pode ser no passado.");
            }

            novaViagem.Id = idAtual;
            idAtual++;

            Viagens.Add(novaViagem);

            // Gera os assentos para a viagem recém-criada (SP-04)
            AssentosController.GerarAssentosParaViagem(
                novaViagem.Id,
                novaViagem.TotalAssentos,
                novaViagem.TipoVeiculo ?? "Convencional",
                novaViagem.ValorPassagem
            );

            return Results.Ok(novaViagem);
        });
    }
}

public class Viagem {
    public int Id { get; set; }
    public string Destino { get; set; } = "";
    public string Origem { get; set; } = "";
    public string Descricao { get; set; } = "";
    public DateTime DataSaida { get; set; }
    public DateTime? DataRetorno { get; set; }
    public int TotalAssentos { get; set; }
    public decimal ValorPassagem { get; set; }
    public string? TipoVeiculo { get; set; }
    public string? FotoUrl { get; set; }
    public string? EmpresaTransporte { get; set; }
    public bool Ativo { get; set; } = true;
}

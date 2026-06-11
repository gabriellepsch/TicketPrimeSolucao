namespace billet_2.Models;

public class Viagem  // Antigo: Evento
{
    public int Id { get; set; }
    public string Destino { get; set; } = "";             // Antigo: Nome
    public string Origem { get; set; } = "";               // NOVO
    public string Descricao { get; set; } = "";            // Itinerário
    public DateTime DataSaida { get; set; }                // Antigo: Data
    public DateTime? DataRetorno { get; set; }             // NOVO
    public int TotalAssentos { get; set; }                 // Antigo: QuantidadeIngressos
    public decimal ValorPassagem { get; set; }             // Antigo: ValorIngresso (float → decimal)
    public string? TipoVeiculo { get; set; }               // NOVO: "Leito", "Semileito", "Convencional"
    public string? FotoUrl { get; set; }
    public string? EmpresaTransporte { get; set; }         // NOVO
    public bool Ativo { get; set; } = true;                // NOVO
}

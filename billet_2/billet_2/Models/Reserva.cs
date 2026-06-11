namespace billet_2.Models;

public class Reserva
{
    public int Id { get; set; }
    public int PassageiroId { get; set; }
    public int ViagemId { get; set; }
    public int AssentoId { get; set; }
    public string? CupomUtilizado { get; set; }
    public decimal ValorFinalPago { get; set; }
    public string Status { get; set; } = "Confirmada";
    public DateTime DataReserva { get; set; }
}

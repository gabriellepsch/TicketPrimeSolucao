namespace billet_2.Models;

public enum StatusAssento
{
    Disponivel,
    Reservado,
    Vendido
}

public class Assento
{
    public int Id { get; set; }
    public int ViagemId { get; set; }
    public int Numero { get; set; }
    public StatusAssento Status { get; set; } = StatusAssento.Disponivel;
    public string Categoria { get; set; } = "Corredor";
    public decimal PrecoExtra { get; set; } = 0;
    public DateTime? ReservaExpiracao { get; set; }
}

// src/assentos/Assento.cs

public enum StatusAssento
{
    Disponivel,
    Reservado,   // Bloqueado temporariamente (checkout em andamento)
    Vendido      // Já foi pago e confirmado
}

public class Assento
{
    public int Id { get; set; }
    public int ViagemId { get; set; }
    public int Numero { get; set; }
    public StatusAssento Status { get; set; } = StatusAssento.Disponivel;
    public string Categoria { get; set; } = "Corredor"; // "Janela", "Corredor"
    public decimal PrecoExtra { get; set; } = 0;
    public DateTime? ReservaExpiracao { get; set; }
}

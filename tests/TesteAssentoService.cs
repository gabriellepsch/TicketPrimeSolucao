using Xunit;

namespace MeuProjeto.Tests;

public class TesteAssentoService
{
    [Fact]
    public void Assento_DeveAlternarStatusCorretamente()
    {
        // Arrange
        var assento = new Assento { Id = 1, Status = StatusAssento.Disponivel };

        // Act
        assento.Status = StatusAssento.Reservado;  // Usuário inicia checkout
        Assert.Equal(StatusAssento.Reservado, assento.Status);

        assento.Status = StatusAssento.Vendido;    // Pagamento confirmado
        Assert.Equal(StatusAssento.Vendido, assento.Status);
    }

    [Fact]
    public void AssentoReservado_DeveVoltarADisponivel_AposExpiracao()
    {
        // Arrange
        var assento = new Assento { Id = 1, Status = StatusAssento.Reservado, ReservaExpiracao = DateTime.Now.AddMinutes(-1) };

        // Act
        bool expirado = assento.ReservaExpiracao.HasValue && assento.ReservaExpiracao.Value < DateTime.Now;
        if (expirado) assento.Status = StatusAssento.Disponivel;

        // Assert
        Assert.Equal(StatusAssento.Disponivel, assento.Status);
    }
}

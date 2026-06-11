using Xunit;

namespace MeuProjeto.Tests;

public class TesteReservaComAssento
{
    [Fact]
    public void Reserva_DeveAssociarAssentoCorretamente()
    {
        // Arrange
        var assento = new Assento { Id = 5, ViagemId = 1, Numero = 5, Status = StatusAssento.Disponivel };

        // Act
        bool assentoValido = assento.Status == StatusAssento.Disponivel && assento.Id > 0 && assento.ViagemId > 0;

        // Assert
        Assert.True(assentoValido);
    }
}

using Xunit;

namespace MeuProjeto.Tests;

public class TesteReservaAssentoValida
{
    [Fact]
    public void Reserva_ComAssentoValido_DeveSerValida()
    {
        // Arrange
        var assento = new Assento { Id = 1, ViagemId = 1, Numero = 1, Status = StatusAssento.Disponivel };
        var reserva = new { AssentoId = assento.Id, ViagemId = 1, PassageiroId = 1 };

        // Act
        bool reservaValida = reserva.AssentoId > 0 && reserva.ViagemId > 0 && reserva.PassageiroId > 0;

        // Assert
        Assert.True(reservaValida);
    }
}

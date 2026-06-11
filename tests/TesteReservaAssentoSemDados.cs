using Xunit;

namespace MeuProjeto.Tests;

public class TesteReservaAssentoSemDados
{
    [Fact]
    public void Reserva_SemAssento_DeveSerInvalida()
    {
        // Arrange & Act
        int? assentoId = null;
        int? viagemId = null;
        int? passageiroId = null;

        bool reservaValida = assentoId.HasValue && viagemId.HasValue && passageiroId.HasValue;

        // Assert
        Assert.False(reservaValida);
    }
}

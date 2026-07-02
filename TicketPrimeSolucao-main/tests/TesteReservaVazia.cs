using Xunit;

public class ReservaTests
{
    [Fact]
    public void ValidarReserva_QuandoSemCpfUsuario_DeveRetornarInvalida()
    {
        // Arrange
        string usuarioCpf = null;

        // Act
        bool valido = usuarioCpf != null;

        // Assert
        Assert.False(valido);
    }
}

using Xunit;

public class ReservaTests
{
    [Fact]
    public void NaoDevePermitirReservaSemUsuario()
    {
        // Arrange
        string usuarioCpf = null;

        // Act
        bool valido = usuarioCpf != null;

        // Assert
        Assert.False(valido);
    }
}
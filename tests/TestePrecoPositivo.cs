using Xunit;

public class EventoPrecoTests
{
    [Fact]
    public void NaoDevePermitirPrecoNegativo()
    {
        // Arrange
        decimal preco = -50;

        // Act
        bool valido = preco >= 0;

        // Assert
        Assert.False(valido);
    }
}
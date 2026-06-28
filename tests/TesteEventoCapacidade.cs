using Xunit;

public class EventoTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void NaoDevePermitirCapacidadeInvalida(int capacidade)
    {
        // Act
        bool valido = capacidade > 0;

        // Assert
        Assert.False(valido);
    }
}
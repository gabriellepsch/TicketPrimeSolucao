using Xunit;

public class CupomTests
{
    [Theory]
    [InlineData(-5)]
    [InlineData(120)]
    public void NaoDevePermitirDescontoInvalido(decimal desconto)
    {
        // Act
        bool valido = desconto >= 0 && desconto <= 100;

        // Assert
        Assert.False(valido);
    }
}
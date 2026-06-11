using Xunit;

namespace MeuProjeto.Tests;

public class TestePrecoPassagemPositivo
{
    [Theory]
    [InlineData(50.00)]
    [InlineData(100.00)]
    [InlineData(250.00)]
    public void PrecoPassagem_DeveSerPositivo(decimal preco)
    {
        // Arrange
        var viagem = new Viagem { ValorPassagem = preco };

        // Act
        bool precoValido = viagem.ValorPassagem > 0;

        // Assert
        Assert.True(precoValido);
    }
}

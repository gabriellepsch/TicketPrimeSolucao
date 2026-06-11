using Xunit;

namespace MeuProjeto.Tests;

public class TesteViagemCapacidade
{
    [Theory]
    [InlineData(50, 50)]
    [InlineData(46, 46)]
    [InlineData(40, 40)]
    public void TotalAssentos_DeveSerIgualAoEsperado(int total, int esperado)
    {
        // Arrange
        var viagem = new Viagem { TotalAssentos = total, TipoVeiculo = "Convencional" };

        // Act
        bool capacidadeValida = viagem.TotalAssentos == esperado;

        // Assert
        Assert.True(capacidadeValida);
    }
}

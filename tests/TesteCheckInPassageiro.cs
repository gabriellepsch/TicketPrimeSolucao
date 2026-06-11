using Xunit;

namespace MeuProjeto.Tests;

public class TesteCheckInPassageiro
{
    [Fact]
    public void Passageiro_DeveTerDadosValidosParaCheckIn()
    {
        // Arrange
        var passageiro = new Passageiro
        {
            Id = 1,
            Nome = "João Silva",
            Email = "joao@email.com",
            Cpf = "12345678901"
        };

        // Act
        bool dadosValidos = !string.IsNullOrEmpty(passageiro.Nome)
                            && !string.IsNullOrEmpty(passageiro.Email)
                            && passageiro.Cpf.Length == 11;

        // Assert
        Assert.True(dadosValidos);
    }
}

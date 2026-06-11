using System.Net.Http.Json;
using billet_2.Models;

namespace billet_2.Services;

public class PassageiroService  // Antigo: UsuarioService
{
    private readonly HttpClient _http;

    public PassageiroService(HttpClient http)  // Antigo: UsuarioService
    {
        _http = http;
    }

    // Retorna a lista de passageiros (útil para o admin ver quem se cadastrou)
    public async Task<List<Passageiro>?> ListarPassageirosAsync()  // Antigo: ListarUsuariosAsync
    {
        return await _http.GetFromJsonAsync<List<Passageiro>>("api/passageiros/listar");  // Antigo: api/usuarios/listar
    }

    // Realiza o cadastro e trata erros vindos da API
    public async Task<string?> CadastrarAsync(Passageiro passageiro)  // Antigo: Usuario usuario
    {
        try
        {
            // Limpa o CPF para mandar apenas números para o banco
            if (!string.IsNullOrEmpty(passageiro.Cpf))
            {
                passageiro.Cpf = passageiro.Cpf.Replace(".", "").Replace("-", "");
            }

            var response = await _http.PostAsJsonAsync("api/passageiros/cadastrar", passageiro);

            if (response.IsSuccessStatusCode)
            {
                return null; // Sucesso!
            }
            else
            {
                var erro = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erro na API: {erro}");
                return erro; // Retorna a mensagem de erro da API
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exceção ao cadastrar: {ex.Message}");
            return "Erro de conexão com o servidor.";
        }
    }
}

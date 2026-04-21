using System.Net.Http.Json;
using billet_2.Models;

namespace billet_2.Services;

public class UsuarioService
{
    private readonly HttpClient _http;

    public UsuarioService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Usuario>?> ListarUsuariosAsync()
    {
        return await _http.GetFromJsonAsync<List<Usuario>>("api/usuarios/listar");
    }

    public async Task<string?> CadastrarAsync(Usuario usuario)
    {
        try 
            {
                // Limpa o CPF (remove pontos e traços)
                usuario.Cpf = usuario.Cpf.Replace(".", "").Replace("-", "");
                
                var response = await _http.PostAsJsonAsync("api/usuarios/cadastrar", usuario);
                
                if (response.IsSuccessStatusCode)
                {
                    return null;
                }
                else
                {
                    var erro = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Erro na API: {erro}");
                    return erro;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção: {ex.Message}");
                return ex.Message;
            }
    }   
}

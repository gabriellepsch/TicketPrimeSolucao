using System.Net.Http.Json;
using billet_2.Models;

namespace billet_2.Services;

public class ViagemService  // Antigo: EventoService
{
    private readonly HttpClient _http;

    public ViagemService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Viagem>?> ListarViagensAsync()  // Antigo: ListarEventosAsync
    {
        return await _http.GetFromJsonAsync<List<Viagem>>("api/viagens/listar");  // Antigo: api/eventos/listar
    }

    public async Task<Viagem?> BuscarPorIdAsync(int id)  // Antigo: BuscarPorIdAsync (retornava Evento)
    {
        return await _http.GetFromJsonAsync<Viagem>($"api/viagens/listar/{id}");  // Antigo: api/eventos/listar/{id}
    }

    public async Task<string?> CriarViagemAsync(Viagem novaViagem)  // Antigo: CriarEventoAsync(Evento)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/viagens/cadastrar", novaViagem);  // Antigo: api/eventos/cadastrar

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
            Console.WriteLine($"Exceção ao cadastrar: {ex.Message}");
            return "Erro de conexão com o servidor.";
        }
    }
}

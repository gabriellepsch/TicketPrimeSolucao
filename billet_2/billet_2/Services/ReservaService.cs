using System.Net.Http.Json;
using billet_2.Models;

namespace billet_2.Services;

public class ReservaService
{
    private readonly HttpClient _http;
    private readonly AuthService _auth;

    public ReservaService(HttpClient http, AuthService auth)
    {
        _http = http;
        _auth = auth;
    }

    public async Task<string?> ReservarAssento(int viagemId, int assentoId)
    {
        try
        {
            if (!_auth.EstaLogado || _auth.UsuarioLogado == null)
                return "Você precisa estar logado para reservar.";

            var request = new
            {
                ViagemId = viagemId,
                AssentoId = assentoId,
                PassageiroId = _auth.UsuarioLogado.Id
            };

            var response = await _http.PostAsJsonAsync("api/reservas", request);

            if (response.IsSuccessStatusCode)
                return null; // Sucesso!

            var erro = await response.Content.ReadAsStringAsync();
            return erro;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exceção ao reservar: {ex.Message}");
            return "Erro de conexão com o servidor.";
        }
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public static class PassageirosController {
    private static List<Passageiro> Passageiros = new();
    private static int idAtual = 1;

    public static void ListarPassageiros(this WebApplication app) {
        app.MapGet("/api/passageiros/listar", () => {
            return Results.Ok(Passageiros);
        });
    }

    public static void CadastrarPassageiros(this WebApplication app) {
        app.MapPost("/api/passageiros/cadastrar", (Passageiro novoPassageiro) => {
            if (novoPassageiro.Cpf.Length != 11) {
                return Results.BadRequest("O CPF deve ter 11 caracteres");
            }

            if (novoPassageiro.Senha.Length < 6) {
                return Results.BadRequest("A senha deve ter pelo menos 6 caracteres");
            }

            if (Passageiros.Any(p => p.Cpf == novoPassageiro.Cpf)) {
                return Results.BadRequest("O CPF informado já está cadastrado");
            }

            novoPassageiro.Id = idAtual;
            idAtual++;

            Passageiros.Add(novoPassageiro);
            return Results.Ok(novoPassageiro);
        });
    }

    public static void Login(this WebApplication app)
    {
        app.MapPost("/api/auth/login", (LoginRequest request) =>
        {
            var passageiro = Passageiros.FirstOrDefault(p =>
                p.Email == request.Email && p.Senha == request.Senha);

            if (passageiro == null)
                return Results.BadRequest("Email ou senha inválidos.");

            var token = GerarToken(passageiro);
            return Results.Ok(new { token, passageiro });
        });
    }

    private static string GerarToken(Passageiro passageiro)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("TurismoPrime-Chave-Super-Secreta-2026!");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, passageiro.Id.ToString()),
                new Claim(ClaimTypes.Email, passageiro.Email),
                new Claim(ClaimTypes.Role, passageiro.Adm ? "Admin" : "Passageiro")
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public class Passageiro {
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string Cpf { get; set; } = "";
    public bool Adm { get; set; } = false;
    public string Senha { get; set; } = "";
}

public class LoginRequest
{
    public string Email { get; set; } = "";
    public string Senha { get; set; } = "";
}

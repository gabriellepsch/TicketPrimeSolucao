using Dapper;
using Npgsql;

public static class UsuariosController
{
    public static void ListarUsuarios(this WebApplication app)
    {
        app.MapGet("/api/usuarios/listar", (HttpContext httpContext) =>
        {
            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);
            var usuarios = connection.Query<Usuario>(@"SELECT ""Id"", ""Nome"", ""Email"", ""Cpf"", ""Adm"", ""Senha"" FROM ""Usuarios"" ORDER BY ""Id""");
            return Results.Ok(usuarios);
        });
    }

    public static void CadastrarUsuarios(this WebApplication app)
    {
        app.MapPost("/api/usuarios/cadastrar", (Usuario novoUsuario, HttpContext httpContext) =>
        {
            if (novoUsuario.Cpf.Length != 11)
                return Results.BadRequest("O cpf deve ter 11 caracteres");

            if (novoUsuario.Senha.Length < 6)
                return Results.BadRequest("A senha deve ter pelo menos 6 caracteres");

            var cs = httpContext.RequestServices.GetRequiredService<string>();
            using var connection = new NpgsqlConnection(cs);

            // Verifica se CPF já existe
            var existeCpf = connection.ExecuteScalar<int>(
                @"SELECT COUNT(1) FROM ""Usuarios"" WHERE ""Cpf"" = @Cpf",
                new { novoUsuario.Cpf });

            if (existeCpf > 0)
                return Results.BadRequest("O cpf informado já está cadastrado");

            // Verifica se Email já existe
            var existeEmail = connection.ExecuteScalar<int>(
                @"SELECT COUNT(1) FROM ""Usuarios"" WHERE ""Email"" = @Email",
                new { novoUsuario.Email });

            if (existeEmail > 0)
                return Results.BadRequest("O email informado já está cadastrado");

            // Insere e retorna o ID gerado
            var id = connection.ExecuteScalar<int>(
                @"INSERT INTO ""Usuarios"" (""Nome"", ""Email"", ""Cpf"", ""Senha"", ""Adm"")
                  VALUES (@Nome, @Email, @Cpf, @Senha, @Adm)
                  RETURNING ""Id""",
                new { novoUsuario.Nome, novoUsuario.Email, novoUsuario.Cpf, novoUsuario.Senha, novoUsuario.Adm });

            novoUsuario.Id = id;
            return Results.Ok(novoUsuario);
        });
    }
}

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string Cpf { get; set; } = "";
    public bool Adm { get; set; } = false;
    public string Senha { get; set; } = "";
}

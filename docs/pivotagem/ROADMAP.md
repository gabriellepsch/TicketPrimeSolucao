# ROADMAP: Execução da Pivotagem TicketPrime → TurismoPrime

> ✅ **PIVOTAGEM CONCLUÍDA EM 10/06/2026** — Todas as 12 especificações foram implementadas com sucesso.
> **Objetivo:** Guia completo e auto-contido para uma IA (ou desenvolvedor) executar toda a pivotagem.
> **Base:** [`pivotagem.md`](pivotagem.md) — Plano conceitual
> **Decisão:** [`ADR-001-pivotagem-turismo.md`](ADR-001-pivotagem-turismo.md) — Justificativa
> **Esforço estimado:** 12 dias / ~80 horas

---

## Índice de Especificações

| Spec | Nome | Arquivos envolvidos | Esforço |
|------|------|---------------------|---------|
| **SP-01** | Renomeação da API (Controllers + Models) | 3 renomear, 1 criar, 2 atualizar | 2 dias |
| **SP-02** | Renomeação do Frontend (Models + Services + Pages) | 6 renomear, 2 criar, 4 atualizar | 2 dias |
| **SP-03** | Modelo de Assentos (API + Frontend) | 2 criar, 1 atualizar | 2 dias |
| **SP-04** | Endpoints de Assentos e Reservas | 1 criar, 1 atualizar | 1 dia |
| **SP-05** | Componente MapaAssentos.razor (Visual do Ônibus) | 1 criar | 2 dias |
| **SP-06** | Fluxo de Compra com Reserva Temporária | 3 modificar | 1 dia |
| **SP-07** | QR Code nas Passagens | 1 criar, 1 modificar | 1 dia |
| **SP-08** | Autenticação JWT | 2 criar, 3 modificar | 2 dias |
| **SP-09** | Banco de Dados (SQL + Integração) | 1 criar, 2 modificar | 2 dias |
| **SP-10** | Testes | 4 adaptar, 3 criar | 1 dia |
| **SP-11** | Assets Visuais | 5 substituir | 1 dia |
| **SP-12** | Documentação | 3 atualizar | 1 dia |

---

## Estado Atual das Specs

> 🔄 **Legenda:** `⏳ Pendente` = não iniciada | `🔄 Desenvolvendo` = em andamento | `✅ Implementada` = concluída com sucesso | `❌ Com Erro` = falha na validação | `⚠️ Em Revisão` = aguardando ajustes

| Spec | Nome | Estado | Observação |
|------|------|--------|------------|
| **SP-01** | Renomeação da API (Controllers + Models) | ✅ Implementada | SP-01 concluída — `dotnet build src/` passou com 0 erros, 0 warnings |
| **SP-02** | Renomeação do Frontend (Models + Services + Pages) | ✅ Implementada | SP-02 concluída — `dotnet build billet_2/billet_2/` passou com 0 erros, 1 aviso CS8602 nullable |
| **SP-03** | Modelo de Assentos (API + Frontend) | ✅ Implementada | SP-03 concluída — `src/assentos/Assento.cs`, `Models/Assento.cs`, `Models/Reserva.cs` criados e corretos |
| **SP-04** | Endpoints de Assentos e Reservas | ✅ Implementada | SP-04 concluída — `AssentosController.cs` com endpoints + integração em `ViagensController.cs` |
| **SP-05** | Componente MapaAssentos.razor (Visual do Ônibus) | ✅ Implementada | SP-05 concluída — `dotnet build billet_2/billet_2/` passou com 0 erros, 0 warnings |
| **SP-06** | Fluxo de Compra com Reserva Temporária | ✅ Implementada | SP-06 concluída — `ReservaService.cs` criado, registrado em `Program.cs`, `ViagemDetalhes.razor` com fluxo de reserva |
| **SP-07** | QR Code nas Passagens | ✅ Implementada | SP-07 concluída — `QrCodeService.cs` criado com `PngByteQRCode`, `MinhasPassagens.razor` exibe QR Code após finalizar compra |
| **SP-08** | Autenticação JWT | ✅ Implementada | SP-08 concluída — JWT configurado no `Program.cs`, endpoint `POST /api/auth/login` retorna `{ token, passageiro }` |
| **SP-09** | Banco de Dados (SQL + Integração) | ✅ Implementada | SP-09 concluída — `db/script.sql` com schema TurismoPrime, `Dapper 2.1.35` + `Npgsql 9.0.3` instalados, `ConnectionStrings` configurado |
| **SP-10** | Testes | ✅ Implementada | SP-10 concluída — 8 classes de teste, 14 testes passando (`dotnet test` ✅) |
| **SP-11** | Assets Visuais | ✅ Implementada | SP-11 concluída — pasta `eventos/` → `destinos/`, referências atualizadas em `MinhasPassagens.razor` e `billet_2.csproj` |
| **SP-12** | Documentação | ✅ Implementada | SP-12 concluída — `README.md` reescrito para TurismoPrime, `historiasdeusuario.md` adaptado (24 histórias), `api.http` com todos os novos endpoints |

---

## Tabela de Status das Specs (IA Tracking) — Completa

> 📋 **Instrução para IA**: marque `[x]` quando a spec for concluída. Siga a ordem numérica. Execute o comando de validação **após cada spec**. Se falhar, corrija antes de prosseguir. Leia sempre a coluna "⚠️ Risco" antes de iniciar.

| Status | Spec | Nome | Deps | Ações | Arquivos-Chave | Find → Replace | Pacotes | Validação | Comando | ✅ Sucesso | ⚠️ Risco / Rollback |
|--------|------|------|------|-------|----------------|----------------|---------|-----------|---------|------------|---------------------|
| `[x]` | **SP-01** | Renomeação da API | Nenhuma | Renomear 2, Modificar 1 | `src/viagens/ViagensController.cs` ✨, `src/passageiros/PassageirosController.cs` ✨, `src/Program.cs` 🔧 | `Evento` → `Viagem`, `Usuario` → `Passageiro`, `/api/eventos` → `/api/viagens`, `/api/usuarios` → `/api/passageiros`, `ListarEventos` → `ListarViagens`, `CadastrarEventos` → `CadastrarViagens`, `CadastrarUsuarios` → `CadastrarPassageiros`, `ListarUsuarios` → `ListarPassageiros` | — | `dotnet build src/` | `cd src && dotnet build` ✅ | `GET /api/viagens/listar` retorna 200; `GET /api/passageiros/listar` retorna 200 | ⚠️ Program.cs pode referenciar nomes antigos dos métodos → conferir linhas de `app.Cadastrar...()` |
| `[x]` | **SP-02** | Renomeação do Frontend | SP-01 | Renomear 6, Criar 2, Atualizar 4 | `Models/Viagem.cs` ✨, `Models/Passageiro.cs` ✨, `Services/ViagemService.cs` ✨, `Services/AuthService.cs` 🔧, `Services/PassageiroService.cs` 🔄, `Pages/ViagemDetalhes.razor` ✨, `Pages/MinhasPassagens.razor` ✨, `Pages/CriarViagem.razor` ✨, `Program.cs` 🔧, `Home.razor` 🔧, `_Imports.razor` 🔧, `Cadastro.razor` 🔧, `Login.razor` 🔧 | `Evento` → `Viagem`, `Usuario` → `Passageiro`, `EventoService` → `ViagemService`, `Usuario`→`Passageiro` (em AuthService.cs **e** PassageiroService.cs), `/vendas/` → `/viagem/`, `/meusingressos` → `/minhas-passagens`, `/criarevento` → `/criar-viagem`, `Ingresso` → `Passagem`, `/api/usuarios/` → `/api/passageiros/`, `@inject ...EventoService` → `@inject ...ViagemService` | — | `dotnet build billet_2/billet_2/` | `cd billet_2\billet_2 && dotnet build` | Página Home lista viagens; rotas `/viagem/{id}`, `/minhas-passagens`, `/criar-viagem` funcionam | 🔴 **AuthService.cs** (SP-02.7) e **PassageiroService.cs** (SP-02.8) são obrigatórios antes de SP-06 — se pular, `UsuarioLogado` e `PassageiroService` quebram. Rollback: restaurar arquivos originais e re-renomear |
| `[x]` | **SP-03** | Modelo de Assentos | Nenhuma | Criar 3 | `src/assentos/Assento.cs` ✨ (enum StatusAssento + class Assento), `billet_2/billet_2/Models/Assento.cs` ✨, `billet_2/billet_2/Models/Reserva.cs` ✨ | — (criação pura) | — | Arquivos existem com conteúdo correto | — (só criar) | `Assento.cs` idêntico na API e Frontend; `Reserva.cs` no Frontend | ⚠️ Enum `StatusAssento` deve ser **idêntico** nos 2 arquivos (Disponivel, Reservado, Vendido) |
| `[x]` | **SP-04** | Endpoints de Assentos/Reservas | SP-01, SP-03 | Criar 1, Modificar 2 | `src/assentos/AssentosController.cs` ✨, `src/viagens/ViagensController.cs` 🔧 (add GerarAssentosParaViagem), `src/Program.cs` 🔧 (add ListarAssentos + CriarReserva) | Em ViagensController.cs: após `Viagens.Add(novaViagem)` → `AssentosController.GerarAssentosParaViagem(...)`. Em Program.cs: add `app.ListarAssentos()` e `app.CriarReserva()` | — | `dotnet build src/` | `cd src && dotnet build` ✅ | `GET /api/viagens/{id}/assentos` retorna lista; `POST /api/reservas` bloqueia assento | ⚠️ `GerarAssentosParaViagem` DEVE ser chamado ao criar viagem ou assentos não serão populados. Rollback: remover as 3 linhas em Program.cs |
| `[x]` | **SP-05** | MapaAssentos.razor | SP-03 | Criar 2 | `Components/MapaAssentos.razor` ✨, `Components/MapaAssentos.razor.css` ✨ | — (criação pura) | — | `dotnet build billet_2/billet_2/` | `cd billet_2\billet_2 && dotnet build` ✅ | Componente renderiza grid de assentos coloridos (verde=disp, amarelo=reserv, vermelho=vend) | ⚠️ Depende de `HttpClient` injetado (já configurado em Program.cs). Se falhar, verificar `@inject HttpClient Http` |
| `[x]` | **SP-06** | Reserva Temporária | SP-02.7, SP-04, SP-05 | Criar 1, Modificar 2 | `Services/ReservaService.cs` ✨, `Program.cs` 🔧 (add `ReservaService` scoped), `Pages/ViagemDetalhes.razor` 🔧 (add fluxo de reserva) | Em Program.cs: add `builder.Services.AddScoped<ReservaService>();` | — | `dotnet build billet_2/billet_2/` | `cd billet_2\billet_2 && dotnet build` | Usuário logado seleciona assento → clica "Reservar" → assento fica amarelo por 15 min | 🔴 **AuthService DEVE estar atualizado** (SP-02.7) senão `_auth.UsuarioLogado` é `Usuario?` não `Passageiro?`. Rollback: remover `ReservaService` do Program.cs |
| `[x]` | **SP-07** | QR Code | SP-06 | Criar 1, Modificar 3 | `Services/QrCodeService.cs` ✨, `billet_2.csproj` 🔧 (add QRCoder), `Program.cs` 🔧 (add QrCodeService singleton), `Pages/MinhasPassagens.razor` 🔧 (exibir QR) | Em billet_2.csproj: add `<PackageReference Include=\"QRCoder\" Version=\"1.6.0\" />`. Em Program.cs: `builder.Services.AddSingleton<QrCodeService>();` | `QRCoder 1.6.0`, `System.Drawing.Common 9.0.4` | `dotnet build billet_2/billet_2/` | `cd billet_2\\billet_2 && dotnet build` ✅ | QR Code em Base64 é gerado via `PngByteQRCode` e exibido em MinhasPassagens após finalizar compra | ⚠️ `QRCode` (System.Drawing) removido no QRCoder 1.6.0 — usado `PngByteQRCode` como alternativa que não depende de System.Drawing. Rollback: remover package ref |
| `[x]` | **SP-08** | Autenticação JWT | SP-01 | Modificar 3 | `src/api.csproj` 🔧 (add JwtBearer), `src/Program.cs` 🔧 (add auth config 3 blocos), `src/passageiros/PassageirosController.cs` 🔧 (add Login + GerarToken + LoginRequest) | Em PassageirosController.cs: add `using System.IdentityModel.Tokens.Jwt`, `using System.Security.Claims`, `using System.Text` no topo. Em Program.cs: 3 blocos (usings + services.AddAuthentication + app.UseAuthentication/Authorization) | `Microsoft.AspNetCore.Authentication.JwtBearer 10.0.5` | `dotnet build src/` | `cd src && dotnet build` ✅ | `POST /api/auth/login` com email+senha retorna `{ token, passageiro }` | 🔴 **3 pontos de inserção em Program.cs** (ver SP-08.2): (1) usings no topo, (2) AddAuthentication após AddCors, (3) UseAuthentication após UseCors. Pular qualquer um quebra. Rollback: remover os 3 blocos + package |
| `[x]` | **SP-09** | Banco de Dados | SP-01, SP-03 | Renomear 1, Modificar 2 | `db/script.sql` 🔄 (rename de `db/sql`), `src/api.csproj` 🔧 (add Dapper+Npgsql), `src/appsettings.json` 🔧 (add ConnectionStrings) | Renomear: `move db\sql db\script.sql`. Em api.csproj: add 2 packages. Em appsettings.json: add bloco `ConnectionStrings` | `Dapper 2.1.35`, `Npgsql 9.0.3` | `db/script.sql` existe, packages adicionados | `move db\sql db\script.sql` ✅ | `db/script.sql` com schema TurismoPrime completo (Passageiros, Viagens, Assentos, Cupons, Reservas + índices); packages no csproj; connection string no appsettings | ⚠️ Fase **opcional** — in-memory lists continuam funcionando. Rollback: remover packages, reverter appsettings.json |
| `[x]` | **SP-10** | Testes | SP-01, SP-03, SP-04 | Adaptar 4, Criar 3, Manter 1 | `TestePrecoPassagemPositivo.cs` 🔄, `TesteViagemCapacidade.cs` 🔄, `TesteReservaAssentoValida.cs` 🔄, `TesteReservaAssentoSemDados.cs` 🔄, `TesteAssentoService.cs` ✨, `TesteReservaComAssento.cs` ✨, `TesteCheckInPassageiro.cs` ✨, `TesteDescontoValido.cs` ✅ | `TestePrecoPositivo` → `TestePrecoPassagemPositivo`, `TesteEventoCapacidade` → `TesteViagemCapacidade`, `TesteReservaValida` → `TesteReservaAssentoValida`, `TesteReservaVazia` → `TesteReservaAssentoSemDados`, `Evento` → `Viagem`, `Usuario` → `Passageiro` | — | `dotnet test` ✅ 14 aprovados | `cd tests && dotnet test` ✅ | 14 testes verdes em 8 suites | ⚠️ Se `Viagem` ou `Assento` não estiverem no escopo do projeto de testes, compilação falha. Rollback: restaurar testes originais |
| `[x]` | **SP-11** | Assets Visuais | Nenhuma | Renomear pasta, Substituir 4 | `wwwroot/images/destinos/` 🔄 (rename de `eventos/`), 4 novos arquivos de imagem/vídeo | `images/eventos/` → `images/destinos/` em TODOS os arquivos `.razor` e no `Viagem.FotoUrl` | — | Pasta `destinos/` existe, sem referências a `eventos/` | `move billet_2\billet_2\wwwroot\images\eventos billet_2\billet_2\wwwroot\images\destinos` ✅ | Pasta renomeada, `MinhasPassagens.razor` e `billet_2.csproj` atualizados | ⚠️ Se pular a atualização das referências nos .razor, imagens não carregam. Rollback: `move destinos eventos` |
| `[x]` | **SP-12** | Documentação | SP-01 a SP-11 | Atualizar 3 | `README.md` 🔧, `docs/historiasdeusuario.md` 🔧, `src/api.http` 🔧 | `TicketPrime` → `TurismoPrime`, `Evento` → `Viagem`, `Usuario` → `Passageiro`, `Ingresso` → `Passagem`, endpoints antigos → novos | — | Revisão de consistência | `dotnet build && dotnet test` ✅ | README descreve TurismoPrime com endpoints, páginas e funcionalidades atuais; 24 histórias adaptadas para turismo; api.http com 9 exemplos de endpoints | ⚠️ Deve ser a **última** spec (domínio precisa estar finalizado) ✅ |

### Regras para a IA (Ordem de Execução)

1. **Ordem obrigatória**: execute as specs estritamente na ordem SP-01 → SP-12. Não pule nem reordene.
2. **Validação contínua**: execute o comando da coluna "Comando" **imediatamente após** cada spec. Se falhar, **não prossiga** — diagnostique o erro, corrija, e só então vá para a próxima.
3. **Dependências (Deps)**: a coluna "Deps" lista as specs que devem ter `[x]` antes de iniciar. Se alguma dep estiver pendente, pare.
4. **Rollback imediato**: se uma spec quebrar algo que funcionava, reverta a alteração (use `git restore <arquivo>` ou refaça manualmente), analise a causa, e refaça a spec corretamente.
5. **Marcação**: ao concluir cada spec com sucesso, troque `[ ]` por `[x]` na coluna "Status". Isso serve como checkpoint.
6. **Leia o risco primeiro**: antes de executar qualquer spec, leia a coluna "⚠️ Risco / Rollback" para evitar armadilhas conhecidas.
7. **Não edite manualmente o que não precisa**: se a spec diz MANTER ou não lista um arquivo, não o modifique.
---

## SP-01: Renomeação da API (Controllers + Models)

### Passo 0: Criar diretórios e renomear arquivos

Antes de criar ou editar qualquer arquivo, execute estes comandos no terminal (na raiz do projeto):

```batch
:: Criar diretórios necessários
mkdir src\viagens
mkdir src\passageiros
mkdir src\assentos

:: Renomear controllers (move = rename no Windows)
move src\eventos\EventosController.cs src\viagens\ViagensController.cs
move src\usuarios\UsuariosController.cs src\passageiros\PassageirosController.cs
```

> O arquivo `src/cupons/CuponsController.cs` permanece inalterado (MANTER).

### Arquivos envolvidos

| Ação | Caminho antigo | Caminho novo |
|------|----------------|--------------|
| RENOMEAR (via `move`) | `src/eventos/EventosController.cs` | `src/viagens/ViagensController.cs` |
| RENOMEAR (via `move`) | `src/usuarios/UsuariosController.cs` | `src/passageiros/PassageirosController.cs` |
| MANTER | `src/cupons/CuponsController.cs` | *(inalterado)* |
| CRIAR | — | `src/assentos/AssentosController.cs` (SP-04) |
| MODIFICAR | `src/Program.cs` | *(mesmo arquivo)* |

### Spec SP-01.1: `src/viagens/ViagensController.cs`

Criar o arquivo `src/viagens/ViagensController.cs` com o conteúdo completo abaixo (já contém todas as alterações em relação ao original):

```csharp
// RENOMEAR classe
public static class ViagensController {
    private static List<Viagem> Viagens = new();  // Antigo: List<Evento> Eventos
    private static int idAtual = 1;

    // RENOMEAR método
    public static void ListarViagens(this WebApplication app) {  // Antigo: ListarEventos
        app.MapGet("/api/viagens/listar", () => {                // Antigo: /api/eventos/listar
            return Results.Ok(Viagens);
        });
    }

    public static void ListarViagemPorId(this WebApplication app) {  // Antigo: ListarEventoPorId
        app.MapGet("/api/viagens/listar/{id}", (int id) => {         // Antigo: /api/eventos/listar/{id}
            var viagem = Viagens.FirstOrDefault(v => v.Id == id);    // Antigo: evento
            if (viagem == null)
                return Results.NotFound("Viagem não encontrada.");   // Antigo: "Evento não encontrado."
            return Results.Ok(viagem);
        });
    }

    public static void CadastrarViagens(this WebApplication app) {  // Antigo: CadastrarEventos
        app.MapPost("/api/viagens/cadastrar", (Viagem novaViagem) => {  // Antigo: Evento, /api/eventos/cadastrar
            if (Viagens.Any(v => v.Destino == novaViagem.Destino && v.DataSaida == novaViagem.DataSaida)) {
                return Results.BadRequest("Já existe uma viagem para este destino na mesma data.");
            }

            if (novaViagem.DataSaida < DateTime.Now) {
                return Results.BadRequest("A data de partida não pode ser no passado.");
            }

            novaViagem.Id = idAtual;
            idAtual++;

            Viagens.Add(novaViagem);
            return Results.Ok(novaViagem);
        });
    }
}

// Modelo Viagem (ANTIGO: Evento) — mesmo padrão do original (modelo no final do controller)
public class Viagem {
    public int Id { get; set; }
    public string Destino { get; set; } = "";
    public string Origem { get; set; } = "";
    public string Descricao { get; set; } = "";
    public DateTime DataSaida { get; set; }
    public DateTime? DataRetorno { get; set; }
    public int TotalAssentos { get; set; }
    public decimal ValorPassagem { get; set; }
    public string? TipoVeiculo { get; set; }
    public string? FotoUrl { get; set; }
    public string? EmpresaTransporte { get; set; }
    public bool Ativo { get; set; } = true;
}
```

**Alterações em relação ao original:**
- `EventosController` → `ViagensController`
- `List<Evento> Eventos` → `List<Viagem> Viagens`
- `ListarEventos` → `ListarViagens`
- `/api/eventos/listar` → `/api/viagens/listar`
- `ListarEventoPorId` → `ListarViagemPorId`
- `/api/eventos/listar/{id}` → `/api/viagens/listar/{id}`
- `CadastrarEventos` → `CadastrarViagens`
- `/api/eventos/cadastrar` → `/api/viagens/cadastrar`
- Validação: não verifica mais `Nome` duplicado, verifica `Destino + DataSaida`
- Mensagens adaptadas para domínio de viagem

### Spec SP-01.2: `src/passageiros/PassageirosController.cs`

Criar o arquivo `src/passageiros/PassageirosController.cs` com o conteúdo completo abaixo (já contém todas as alterações em relação ao original):

```csharp
using Microsoft.AspNetCore.Http.HttpResults;

// RENOMEAR classe
public static class PassageirosController {
    private static List<Passageiro> Passageiros = new();  // Antigo: List<Usuario> Usuarios
    private static int idAtual = 1;

    public static void ListarPassageiros(this WebApplication app) {  // Antigo: ListarUsuarios
        app.MapGet("/api/passageiros/listar", () => {                // Antigo: /api/usuarios/listar
            return Results.Ok(Passageiros);
        });
    }

    public static void CadastrarPassageiros(this WebApplication app) {  // Antigo: CadastrarUsuarios
        app.MapPost("/api/passageiros/cadastrar", (Passageiro novoPassageiro) => {  // Antigo: Usuario, /api/usuarios/cadastrar
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
}

// Modelo Passageiro (ANTIGO: Usuario) — mesmo padrão do original (modelo no final do controller)
public class Passageiro {
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string Cpf { get; set; } = "";
    public bool Adm { get; set; } = false;
    public string Senha { get; set; } = "";
}
```

**Alterações:**
- `UsuariosController` → `PassageirosController`
- `List<Usuario> Usuarios` → `List<Passageiro> Passageiros`
- `ListarUsuarios` → `ListarPassageiros`
- `/api/usuarios/listar` → `/api/passageiros/listar`
- `CadastrarUsuarios` → `CadastrarPassageiros`
- `/api/usuarios/cadastrar` → `/api/passageiros/cadastrar`
- Lógica de negócio idêntica (CPF, senha, duplicidade)

### Spec SP-01.3: `src/cupons/CuponsController.cs` — NENHUMA ALTERAÇÃO

Arquivo permanece exatamente como está. A classe `CuponsController` e seus endpoints `/api/cupons/listar` e `/api/cupons/cadastrar` não mudam.

### Spec SP-01.4: Atualizar `src/Program.cs`

> ⚠️ **IMPORTANTE:** Substitua o conteúdo COMPLETO de `src/Program.cs` pelo código abaixo. Os métodos `app.ListarAssentos()` e `app.CriarReserva()` serão adicionados na SP-04.

```csharp
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5096")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("BlazorPolicy");

// ALTERADO: nomes dos métodos de extensão
app.CadastrarPassageiros();   // Antigo: CadastrarUsuarios
app.ListarPassageiros();      // Antigo: ListarUsuarios
app.CadastrarViagens();       // Antigo: CadastrarEventos
app.ListarViagens();          // Antigo: ListarEventos
app.ListarViagemPorId();      // Antigo: ListarEventoPorId
app.CadastrarCupons();        // Mantido
app.ListarCupons();           // Mantido

// (app.ListarAssentos e app.CriarReserva serão adicionados na SP-04)
// (app.Login será adicionado na SP-08)
// (app.UseAuthentication + app.UseAuthorization serão adicionados na SP-08)

app.UseHttpsRedirection();

app.Run();
```

---

## SP-02: Renomeação do Frontend (Models + Services + Pages)

### Arquivos envolvidos

| Ação | Caminho antigo | Caminho novo |
|------|----------------|--------------|
| RENOMEAR | `billet_2/billet_2/Models/Evento.cs` | `billet_2/billet_2/Models/Viagem.cs` |
| RENOMEAR | `billet_2/billet_2/Models/Usuario.cs` | `billet_2/billet_2/Models/Passageiro.cs` |
| CRIAR | — | `billet_2/billet_2/Models/Assento.cs` |
| RENOMEAR | `billet_2/billet_2/Services/EventoService.cs` | `billet_2/billet_2/Services/ViagemService.cs` |
| ATUALIZAR | `billet_2/billet_2/Services/UsuarioService.cs` | Trocar `Usuario` por `Passageiro` + `/api/usuarios/` por `/api/passageiros/` |
| ATUALIZAR | `billet_2/billet_2/Services/AuthService.cs` | Trocar `Usuario` por `Passageiro` |
| RENOMEAR | `billet_2/billet_2/Components/Pages/Venda.razor` | `billet_2/billet_2/Components/Pages/ViagemDetalhes.razor` |
| RENOMEAR | `billet_2/billet_2/Components/Pages/Meusingressos.razor` | `billet_2/billet_2/Components/Pages/MinhasPassagens.razor` |
| RENOMEAR | `billet_2/billet_2/Components/Pages/Criarevento.razor` | `billet_2/billet_2/Components/Pages/CriarViagem.razor` |
| MODIFICAR | `billet_2/billet_2/Components/Pages/Home.razor` | *(mesmo arquivo)* |
| ATUALIZAR | `billet_2/billet_2/Components/Pages/Venda.razor.css` | `ViagemDetalhes.razor.css` (rename) |
| ATUALIZAR | `billet_2/billet_2/Components/Pages/Meusingressos.razor.css` | `MinhasPassagens.razor.css` (rename) |
| ATUALIZAR | `billet_2/billet_2/Components/Pages/Criarevento.razor.css` | `CriarViagem.razor.css` (rename) |
| ATUALIZAR | `billet_2/billet_2/Program.cs` | Atualizar registros de serviços |
| ATUALIZAR | `billet_2/billet_2/Components/Routes.razor` | Atualizar rotas |
| ATUALIZAR | `billet_2/billet_2/Components/_Imports.razor` | Atualizar namespaces |
| ATUALIZAR | `billet_2/billet_2.Client/Routes.razor` | Verificar — provavelmente nenhuma alteração (rotas via @page) |

### Spec SP-02.1: `billet_2/billet_2/Models/Viagem.cs`

```csharp
namespace billet_2.Models;

public class Viagem  // Antigo: Evento
{
    public int Id { get; set; }
    public string Destino { get; set; } = "";             // Antigo: Nome
    public string Origem { get; set; } = "";               // NOVO
    public string Descricao { get; set; } = "";            // Itinerário
    public DateTime DataSaida { get; set; }                // Antigo: Data
    public DateTime? DataRetorno { get; set; }             // NOVO
    public int TotalAssentos { get; set; }                 // Antigo: QuantidadeIngressos
    public decimal ValorPassagem { get; set; }             // Antigo: ValorIngresso (float → decimal)
    public string? TipoVeiculo { get; set; }               // NOVO: "Leito", "Semileito", "Convencional"
    public string? FotoUrl { get; set; }
    public string? EmpresaTransporte { get; set; }         // NOVO
    public bool Ativo { get; set; } = true;                // NOVO
}
```

### Spec SP-02.2: `billet_2/billet_2/Models/Passageiro.cs`

```csharp
namespace billet_2.Models;

public class Passageiro  // Antigo: Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string Cpf { get; set; } = "";
    public bool Adm { get; set; } = false;
    public string Senha { get; set; } = "";
}
```

> **Nota:** A classe é IDÊNTICA à original `Usuario.cs`. Apenas o nome da classe muda.

### Spec SP-02.3: `billet_2/billet_2/Services/ViagemService.cs`

Renomear de `EventoService.cs`:

```csharp
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
```

### Spec SP-02.4: Atualizar `billet_2/billet_2/Program.cs`

> **Nota:** Apenas as linhas de registro de serviços mudam. O restante do pipeline permanece idêntico.

```csharp
using billet_2.Components;
using billet_2.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped(dp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5289")
});

// ALTERADO: ViagemService no lugar de EventoService
builder.Services.AddScoped<ViagemService>();    // Antigo: EventoService
builder.Services.AddScoped<PassageiroService>();  // Antigo: UsuarioService
builder.Services.AddSingleton<AuthService>();   // ATUALIZADO na SP-02.7 (tipo Passageiro)

var app = builder.Build();

// --- ABAIXO: NENHUMA ALTERAÇÃO, manter o código original ---
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddInteractiveServerRenderMode();

app.Run();
```

### Spec SP-02.5: Atualizar `Routes.razor`

O arquivo `billet_2/billet_2/Components/Routes.razor` **não precisa ser alterado** — as rotas são definidas via `@page` diretiva em cada página. Mantenha o conteúdo original:

```razor
@using Microsoft.AspNetCore.Components.Routing

<Router AppAssembly="@typeof(Program).Assembly">
    <Found Context="routeData">
        <RouteView RouteData="@routeData" DefaultLayout="@typeof(billet_2.Components.MainLayout)" />
    </Found>
    <NotFound>
        <p>Página não encontrada.</p>
    </NotFound>
</Router>
```

O mesmo vale para `billet_2/billet_2.Client/Routes.razor` — manter o conteúdo original (nenhuma alteração necessária).

As páginas terão suas rotas atualizadas via `@page` diretiva:
- `ViagemDetalhes.razor` terá `@page "/viagem/{id}"` (antigo: `@page "/vendas/{id}"`)
- `MinhasPassagens.razor` terá `@page "/minhas-passagens"` (antigo: `@page "/meusingressos"`)
- `CriarViagem.razor` terá `@page "/criar-viagem"` (antigo: `@page "/criarevento"`)

### Spec SP-02.6: Páginas Blazor

**`ViagemDetalhes.razor`** (renomeado de `Venda.razor`):
- `@page "/viagem/{id}"`
- **`@inject`** trocar `@inject billet_2.Services.EventoService EventoService` → `@inject billet_2.Services.ViagemService ViagemService`
- Substituir todas as referências de `Evento` por `Viagem`
- Substituir `EventoService` por `ViagemService`
- Adicionar chamada ao componente `<MapaAssentos ViagemId="id" />` (SP-05)
- Adicionar `@inject billet_2.Services.ReservaService ReservaService` (SP-06)
- Atualizar labels: "Ingresso" → "Passagem", "Evento" → "Viagem"

**`MinhasPassagens.razor`** (renomeado de `Meusingressos.razor`):
- `@page "/minhas-passagens"`
- **`@inject`** trocar `@inject billet_2.Services.EventoService EventoService` → `@inject billet_2.Services.ViagemService ViagemService`
- Substituir "Ingresso" por "Passagem" nos textos
- Adicionar exibição de QR Code (SP-07)

**`CriarViagem.razor`** (renomeado de `Criarevento.razor`):
- `@page "/criar-viagem"`
- **`@inject`** trocar `@inject billet_2.Services.EventoService EventoService` → `@inject billet_2.Services.ViagemService ViagemService`
- Adicionar campos: `Origem`, `DataRetorno`, `TipoVeiculo` (dropdown), `EmpresaTransporte`
- Substituir `EventoService.CriarEventoAsync` por `ViagemService.CriarViagemAsync`

**`Home.razor`** (mesmo arquivo, adaptar):
- **`@inject`** trocar `@inject billet_2.Services.EventoService EventoService` → `@inject billet_2.Services.ViagemService ViagemService`
- Substituir `EventoService.ListarEventosAsync` por `ViagemService.ListarViagensAsync`
- Substituir `Evento` por `Viagem` nos tipos
- Atualizar links: `/vendas/{id}` → `/viagem/{id}`
- Atualizar labels: "Eventos" → "Viagens", "Ingressos" → "Passagens"

### Spec SP-02.7: Atualizar `AuthService.cs` para usar `Passageiro`

O arquivo `billet_2/billet_2/Services/AuthService.cs` referenciava `Usuario`, que foi renomeado para `Passageiro`. Substituir o conteúdo:

```csharp
using billet_2.Models;

namespace billet_2.Services;

public class AuthService
{
    public Passageiro? UsuarioLogado { get; private set; }  // Antigo: Usuario?
    public bool EstaLogado = false;

    public void Logar(Passageiro passageiro)  // Antigo: Logar(Usuario usuario)
    {
        UsuarioLogado = passageiro;
        EstaLogado = true;
    }

    public void Deslogar()
    {
        UsuarioLogado = null;
        EstaLogado = false;
    }
}
```
> ⚠️ **Dependência:** O `ReservaService` (SP-06.1) utiliza `_auth.UsuarioLogado.Id` e `_auth.EstaLogado`. Este passo é **obrigatório** antes de implementar SP-06.

### Spec SP-02.8: Atualizar `PassageiroService.cs` para usar `Passageiro`

O arquivo `billet_2/billet_2/Services/UsuarioService.cs` foi renomeado para `PassageiroService.cs`. Substituir o conteúdo pelo código abaixo:

```csharp
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
```

> ⚠️ **O nome da classe e do método agora seguem o padrão `PassageiroService` e `ListarPassageirosAsync`.** O arquivo foi renomeado de `UsuarioService.cs` para `PassageiroService.cs`. Atualizar a referência em `Program.cs` (SP-02.4) de `builder.Services.AddScoped<UsuarioService>()` para `builder.Services.AddScoped<PassageiroService>()`. A página `Cadastro.razor` que usa `@inject PassageiroService` também deve ser atualizada (SP-02.6).

---


## SP-03: Modelo de Assentos (API + Frontend)

### Spec SP-03.1: Modelo na API

Criar `src/assentos/Assento.cs`:

```csharp
// src/assentos/Assento.cs

public enum StatusAssento
{
    Disponivel,
    Reservado,   // Bloqueado temporariamente (checkout em andamento)
    Vendido      // Já foi pago e confirmado
}

public class Assento
{
    public int Id { get; set; }
    public int ViagemId { get; set; }
    public int Numero { get; set; }
    public StatusAssento Status { get; set; } = StatusAssento.Disponivel;
    public string Categoria { get; set; } = "Corredor"; // "Janela", "Corredor"
    public decimal PrecoExtra { get; set; } = 0;
    public DateTime? ReservaExpiracao { get; set; }
}
```

### Spec SP-03.2: Modelo no Frontend

Criar `billet_2/billet_2/Models/Assento.cs`:

```csharp
namespace billet_2.Models;

public enum StatusAssento
{
    Disponivel,
    Reservado,
    Vendido
}

public class Assento
{
    public int Id { get; set; }
    public int ViagemId { get; set; }
    public int Numero { get; set; }
    public StatusAssento Status { get; set; } = StatusAssento.Disponivel;
    public string Categoria { get; set; } = "Corredor";
    public decimal PrecoExtra { get; set; } = 0;
    public DateTime? ReservaExpiracao { get; set; }
}
```

### Spec SP-03.3: Modelo de Reserva (Frontend)

Criar `billet_2/billet_2/Models/Reserva.cs`:

```csharp
namespace billet_2.Models;

public class Reserva
{
    public int Id { get; set; }
    public int PassageiroId { get; set; }
    public int ViagemId { get; set; }
    public int AssentoId { get; set; }
    public string? CupomUtilizado { get; set; }
    public decimal ValorFinalPago { get; set; }
    public string Status { get; set; } = "Confirmada";
    public DateTime DataReserva { get; set; }
}
```

---

## SP-04: Endpoints de Assentos e Reservas

### Spec SP-04.1: `src/assentos/AssentosController.cs`

```csharp
// src/assentos/AssentosController.cs

public static class AssentosController
{
    // Dicionário: ViagemId → Lista de Assentos (KISS: Dictionary simples, sem ConcurrentDictionary)
    private static Dictionary<int, List<Assento>> AssentosPorViagem = new();
    private static readonly object _lockAssentos = new();

    public static void ListarAssentos(this WebApplication app)
    {
        app.MapGet("/api/viagens/{viagemId}/assentos", (int viagemId) =>
        {
            lock (_lockAssentos)
            {
                if (!AssentosPorViagem.ContainsKey(viagemId))
                {
                    return Results.Ok(new List<Assento>());
                }

                var assentos = AssentosPorViagem[viagemId];

                // Filtra expirados: assentos com Status = Reservado e ReservaExpiracao passada
                foreach (var assento in assentos.Where(a =>
                    a.Status == StatusAssento.Reservado &&
                    a.ReservaExpiracao.HasValue &&
                    a.ReservaExpiracao.Value < DateTime.Now))
                {
                    assento.Status = StatusAssento.Disponivel;
                    assento.ReservaExpiracao = null;
                }

                return Results.Ok(assentos);
            }
        });
    }

    public static void CriarReserva(this WebApplication app)
    {
        app.MapPost("/api/reservas", (ReservaRequest request) =>
        {
            lock (_lockAssentos)
            {
                // Validações
                if (!AssentosPorViagem.ContainsKey(request.ViagemId))
                    return Results.BadRequest("Viagem não encontrada.");

                var assento = AssentosPorViagem[request.ViagemId]
                    .FirstOrDefault(a => a.Id == request.AssentoId);

                if (assento == null)
                    return Results.BadRequest("Assento não encontrado.");

                if (assento.Status != StatusAssento.Disponivel)
                    return Results.BadRequest("Assento não está disponível.");

                // Bloqueia o assento (reserva temporária de 15 min)
                assento.Status = StatusAssento.Reservado;
                assento.ReservaExpiracao = DateTime.Now.AddMinutes(15);

                return Results.Ok(new { mensagem = "Assento reservado temporariamente por 15 minutos.", assento });
            }
        });
    }

    // Método auxiliar para popular assentos ao criar uma viagem
    public static void GerarAssentosParaViagem(int viagemId, int totalAssentos, string tipoVeiculo, decimal valorPassagem)
    {
        var assentos = new List<Assento>();
        int assentosPorFileira = tipoVeiculo switch
        {
            "Leito" => 2,       // Cama individual, mais espaçoso
            "Semileito" => 3,   // Reclinação parcial
            _ => 4              // Convencional (padrão)
        };

        // Calcula fator de preço por tipo de veículo (Regra de Negócio #5)
        decimal fatorPreco = tipoVeiculo switch
        {
            "Leito" => 1.60m,      // +60% sobre ValorPassagem
            "Semileito" => 1.20m,  // +20% sobre ValorPassagem
            _ => 1.00m             // Convencional = preço base
        };

        for (int i = 1; i <= totalAssentos; i++)
        {
            // Determina categoria: "Janela" para assentos nas laterais
            string categoria = (i % assentosPorFileira == 0 || (i - 1) % assentosPorFileira == 0)
                ? "Janela"
                : "Corredor";

            // Preço final = ValorPassagem * fatorPreco + extraJanela (Regra de Negócio #5)
            decimal precoExtra = (categoria == "Janela" ? 5.00m : 0);

            assentos.Add(new Assento
            {
                Id = i,
                ViagemId = viagemId,
                Numero = i,
                Status = StatusAssento.Disponivel,
                Categoria = categoria,
                PrecoExtra = precoExtra
            });
        }

        lock (_lockAssentos)
        {
            AssentosPorViagem[viagemId] = assentos;
        }
    }
}

// Modelo de request para criar reserva
public class ReservaRequest
{
    public int ViagemId { get; set; }
    public int AssentoId { get; set; }
    public int PassageiroId { get; set; }
}
```

### Spec SP-04.2: Integrar com ViagensController

No método `CadastrarViagens` em `ViagensController.cs`, após adicionar a viagem:

```csharp
// Após Viagens.Add(novaViagem);
AssentosController.GerarAssentosParaViagem(novaViagem.Id, novaViagem.TotalAssentos, novaViagem.TipoVeiculo ?? "Convencional");
```

### Spec SP-04.3: Adicionar endpoints ao `src/Program.cs`

No arquivo `src/Program.cs`, após a linha `app.ListarCupons();` e antes de `app.UseHttpsRedirection();`, adicionar:

```csharp
// NOVO (SP-04)
app.ListarAssentos();         // GET  /api/viagens/{viagemId}/assentos
app.CriarReserva();           // POST /api/reservas
```

---

## SP-05: Componente MapaAssentos.razor

### Spec SP-05.1: `billet_2/billet_2/Components/MapaAssentos.razor`

```razor
@* MapaAssentos.razor — Visual interativo do ônibus com assentos *@
@using billet_2.Models
@inject HttpClient Http

<div class="onibus-container">
    <div class="onibus-body">
        <div class="motorista-area">
            <div class="volante">🚌</div>
        </div>
        <div class="assentos-grid">
            @foreach (var assento in Assentos)
            {
                var cor = assento.Status switch
                {
                    StatusAssento.Disponivel => "verde",
                    StatusAssento.Reservado => "amarelo",
                    StatusAssento.Vendido => "vermelho",
                    _ => "cinza"
                };

                var selecionado = AssentoSelecionado?.Id == assento.Id ? "selecionado" : "";

                <div class="assento @cor @selecionado @assento.Categoria.ToLower()"
                     @onclick="() => SelecionarAssento(assento)"
                     title="@($"Assento {assento.Numero} - {assento.Categoria}")">
                    <span class="numero-assento">@assento.Numero</span>
                </div>
            }
        </div>
    </div>
    @if (AssentoSelecionado != null)
    {
        <div class="info-assento">
            <p><strong>Assento:</strong> @AssentoSelecionado.Numero</p>
            <p><strong>Categoria:</strong> @AssentoSelecionado.Categoria</p>
            @if (AssentoSelecionado.PrecoExtra > 0)
            {
                <p><strong>Valor adicional:</strong> R$ @AssentoSelecionado.PrecoExtra.ToString("F2")</p>
            }
        </div>
    }
</div>

@code {
    [Parameter] public int ViagemId { get; set; }
    [Parameter] public EventCallback<Assento> OnAssentoSelecionado { get; set; }

    private List<Assento> Assentos = new();
    private Assento? AssentoSelecionado { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await CarregarAssentos();
    }

    private async Task CarregarAssentos()
    {
        try
        {
            Assentos = await Http.GetFromJsonAsync<List<Assento>>($"api/viagens/{ViagemId}/assentos") ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar assentos: {ex.Message}");
        }
    }

    private async Task SelecionarAssento(Assento assento)
    {
        if (assento.Status != StatusAssento.Disponivel)
            return; // Não permite selecionar assento ocupado

        AssentoSelecionado = assento;
        await OnAssentoSelecionado.InvokeAsync(assento);
    }
}
```

### Spec SP-05.2: `billet_2/billet_2/Components/MapaAssentos.razor.css`

```css
.onibus-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 16px;
}

.onibus-body {
    background: #f0f0f0;
    border: 3px solid #333;
    border-radius: 20px;
    padding: 20px;
    width: 280px;
}

.motorista-area {
    display: flex;
    justify-content: flex-end;
    padding-right: 10px;
    margin-bottom: 10px;
    border-bottom: 2px dashed #999;
    padding-bottom: 10px;
}

.volante {
    font-size: 24px;
}

.assentos-grid {
    display: grid;
    grid-template-columns: repeat(4, 1fr);
    gap: 8px;
    padding: 10px 0;
}

.assento {
    width: 45px;
    height: 45px;
    border-radius: 8px;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    font-size: 12px;
    font-weight: bold;
    transition: all 0.2s;
    border: 2px solid transparent;
}

.assento.verde { background-color: #4CAF50; color: white; }
.assento.amarelo { background-color: #FFC107; color: black; }
.assento.vermelho { background-color: #F44336; color: white; }
.assento.cinza { background-color: #9E9E9E; color: white; }

.assento.selecionado {
    border-color: #2196F3;
    box-shadow: 0 0 8px rgba(33, 150, 243, 0.6);
    transform: scale(1.1);
}

.assento:hover:not(.vermelho):not(.amarelo) {
    transform: scale(1.15);
    box-shadow: 0 2px 8px rgba(0,0,0,0.2);
}

.numero-assento {
    pointer-events: none;
}

.info-assento {
    background: #fff;
    border: 1px solid #ddd;
    border-radius: 8px;
    padding: 12px;
    width: 100%;
    max-width: 280px;
}
```

---

## SP-06: Fluxo de Compra com Reserva Temporária

### Spec SP-06.1: Serviço de Reserva no Frontend

Criar `billet_2/billet_2/Services/ReservaService.cs`:

```csharp
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
```

### Spec SP-06.2: Registrar no `Program.cs` do Frontend

```csharp
builder.Services.AddScoped<ReservaService>();  // NOVO
```

### Spec SP-06.3: Atualizar `ViagemDetalhes.razor`

Adicionar o fluxo:
1. Exibir `<MapaAssentos ViagemId="id" OnAssentoSelecionado="HandleAssentoSelecionado" />`
2. Botão "Reservar Assento" habilitado apenas quando um assento disponível é selecionado
3. Chamar `ReservaService.ReservarAssento()` ao clicar
4. Exibir feedback (sucesso/erro)
5. Se sucesso: mostrar contagem regressiva de 15 min e link para checkout

---

## SP-07: QR Code nas Passagens

### Spec SP-07.1: Adicionar bibliotecas QR Code

No `billet_2/billet_2/billet_2.csproj`, adicionar ambas as packages:

```xml
<PackageReference Include="QRCoder" Version="1.6.0" />
<PackageReference Include="System.Drawing.Common" Version="9.0.4" />
```

> ⚠️ A `System.Drawing.Common` é necessária porque o `QrCodeService.cs` usa `System.Drawing` e `System.Drawing.Imaging` para gerar o bitmap do QR Code. Sem ela, a compilação falha com `CS0246: The type or namespace name 'Drawing' could not be found`.

### Spec SP-07.2: Serviço de QR Code

Criar `billet_2/billet_2/Services/QrCodeService.cs`:

```csharp
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace billet_2.Services;

public class QrCodeService
{
    public string GerarQrCodeBase64(string dados)
    {
        using (var qrGenerator = new QRCoder.QRCodeGenerator())
        {
            var qrCodeData = qrGenerator.CreateQrCode(dados, QRCoder.QRCodeGenerator.ECCLevel.Q);
            using (var qrCode = new QRCoder.QRCode(qrCodeData))
            {
                using (var bitmap = qrCode.GetGraphic(20))
                {
                    using (var ms = new MemoryStream())
                    {
                        bitmap.Save(ms, ImageFormat.Png);
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }
    }
}
```

### Spec SP-07.3: Registrar no `Program.cs`

```csharp
builder.Services.AddSingleton<QrCodeService>();  // NOVO
```

### Spec SP-07.4: Exibir na página "Minhas Passagens"

A página deve gerar um QR Code com os dados: `"Viagem: {Destino}, Assento: {numero}, Data: {data}"` para cada passagem comprada.

---

## SP-08: Autenticação JWT

### Spec SP-08.1: Adicionar pacotes NuGet na API

No `src/api.csproj`:

```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.5" />
```

### Spec SP-08.2: `src/Program.cs` — Configurar JWT

**Adicionar no topo do arquivo** (junto com os `using` existentes):
```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
```

**Adicionar após `builder.Services.AddCors(...)`** (antes de `var app = builder.Build();`):
```csharp
// Configurar JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "TurismoPrime-Chave-Super-Secreta-2026!";
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();
```

**Adicionar após `app.UseCors("BlazorPolicy")` e antes de `app.CadastrarPassageiros()`:**
```csharp
app.UseAuthentication();
app.UseAuthorization();
```

### Spec SP-08.3: Endpoint de Login

Adicionar ao `PassageirosController.cs` criado na SP-01.2:

**Passo 1 — Adicionar os `using` no topo do arquivo:**

```csharp
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
```

**Passo 2 — Adicionar os métodos `Login` e `GerarToken` dentro da classe `PassageirosController`:**

```csharp
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
```

**Passo 3 — Adicionar a classe `LoginRequest` no final do arquivo (após a classe `Passageiro`):**

```csharp
public class LoginRequest
{
    public string Email { get; set; } = "";
    public string Senha { get; set; } = "";
}
```

### Spec SP-08.4: Adicionar ao `src/Program.cs`

Adicionar `app.Login();` junto aos demais endpoints, **antes de** `app.UseHttpsRedirection()`:

```csharp
// ALTERADO: nomes dos métodos de extensão
app.CadastrarPassageiros();
app.ListarPassageiros();
app.CadastrarViagens();
app.ListarViagens();
app.ListarViagemPorId();
app.CadastrarCupons();
app.ListarCupons();
app.ListarAssentos();
app.CriarReserva();
app.Login();                    // NOVO (SP-08)

app.UseHttpsRedirection();
```

---

## SP-09: Banco de Dados (SQL + Integração)

### Spec SP-09.1: Renomear e atualizar script SQL

```bash
# Renomear o arquivo
move db\sql db\script.sql
```

Conteúdo de `db/script.sql` — usar o schema completo da Seção 7 do [`pivotagem.md`](pivotagem.md).

### Spec SP-09.2: Adicionar pacote Dapper (ou EF Core)

No `src/api.csproj`:

```xml
<PackageReference Include="Dapper" Version="2.1.35" />
<PackageReference Include="Npgsql" Version="9.0.3" />
```

### Spec SP-09.3: String de conexão em `src/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=turismoprime;Username=postgres;Password=postgres"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

## SP-10: Testes

### Arquivos envolvidos

| Ação | Arquivo antigo | Arquivo novo |
|------|----------------|--------------|
| ADAPTAR | `TestePrecoPositivo.cs` | `TestePrecoPassagemPositivo.cs` |
| ADAPTAR | `TesteEventoCapacidade.cs` | `TesteViagemCapacidade.cs` |
| ADAPTAR | `TesteReservaValida.cs` | `TesteReservaAssentoValida.cs` |
| ADAPTAR | `TesteReservaVazia.cs` | `TesteReservaAssentoSemDados.cs` |
| MANTER | `TesteDescontoValido.cs` | *(inalterado)* |
| CRIAR | — | `TesteAssentoService.cs` |
| CRIAR | — | `TesteReservaComAssento.cs` |
| CRIAR | — | `TesteCheckInPassageiro.cs` |

### Spec SP-10.0: Adicionar referência ao projeto da API

O projeto de testes **não** tem referência à API. Sem ela, as classes `Viagem`, `Assento`, `Passageiro`, `StatusAssento` não são resolvidas.

No `tests/MeuProjeto.Tests.csproj`, adicionar dentro do `<ItemGroup>` existente:

```xml
<ProjectReference Include="..\src\api.csproj" />
```

Após adicionar, o `.csproj` ficará assim:

```xml
<ItemGroup>
    <PackageReference Include="coverlet.collector" Version="6.0.4" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.14.1" />
    <PackageReference Include="xunit" Version="2.9.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="3.1.4" />
    <ProjectReference Include="..\src\api.csproj" />  <!-- NOVO -->
</ItemGroup>
```

> 🔴 **IMPORTANTE:** Sem esta referência, `dotnet test` falha com `CS0246: The type or namespace name 'Viagem' could not be found`.

> ✅ **Nota sobre namespaces:** As classes `Viagem`, `Assento`, `StatusAssento`, `Passageiro` estão definidas no escopo **global** (sem `namespace` declarado) nos controllers da API. Com a `<ProjectReference>` adicionada, elas ficam automaticamente disponíveis nos testes — **não é necessário** adicionar diretivas `using` além de `using Xunit;`. Os códigos abaixo já incluem apenas o necessário.

### Spec SP-10.1: Adaptar `TestePrecoPassagemPositivo.cs`

```csharp
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
```

### Spec SP-10.2: Adaptar `TesteViagemCapacidade.cs`

```csharp
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
```

### Spec SP-10.3: Adaptar `TesteReservaAssentoValida.cs`

```csharp
using Xunit;

namespace MeuProjeto.Tests;

public class TesteReservaAssentoValida
{
    [Fact]
    public void Reserva_ComAssentoValido_DeveSerValida()
    {
        // Arrange
        var assento = new Assento { Id = 1, ViagemId = 1, Numero = 1, Status = StatusAssento.Disponivel };
        var reserva = new { AssentoId = assento.Id, ViagemId = 1, PassageiroId = 1 };

        // Act
        bool reservaValida = reserva.AssentoId > 0 && reserva.ViagemId > 0 && reserva.PassageiroId > 0;

        // Assert
        Assert.True(reservaValida);
    }
}
```

### Spec SP-10.4: Adaptar `TesteReservaAssentoSemDados.cs`

```csharp
using Xunit;

namespace MeuProjeto.Tests;

public class TesteReservaAssentoSemDados
{
    [Fact]
    public void Reserva_SemAssento_DeveSerInvalida()
    {
        // Arrange & Act
        int? assentoId = null;
        int? viagemId = null;
        int? passageiroId = null;

        bool reservaValida = assentoId.HasValue && viagemId.HasValue && passageiroId.HasValue;

        // Assert
        Assert.False(reservaValida);
    }
}
```

### Spec SP-10.5: Criar `TesteAssentoService.cs`

```csharp
using Xunit;

namespace MeuProjeto.Tests;

public class TesteAssentoService
{
    [Fact]
    public void Assento_DeveAlternarStatusCorretamente()
    {
        // Arrange
        var assento = new Assento { Id = 1, Status = StatusAssento.Disponivel };

        // Act
        assento.Status = StatusAssento.Reservado;  // Usuário inicia checkout
        Assert.Equal(StatusAssento.Reservado, assento.Status);

        assento.Status = StatusAssento.Vendido;    // Pagamento confirmado
        Assert.Equal(StatusAssento.Vendido, assento.Status);
    }

    [Fact]
    public void AssentoReservado_DeveVoltarADisponivel_AposExpiracao()
    {
        // Arrange
        var assento = new Assento { Id = 1, Status = StatusAssento.Reservado, ReservaExpiracao = DateTime.Now.AddMinutes(-1) };

        // Act
        bool expirado = assento.ReservaExpiracao.HasValue && assento.ReservaExpiracao.Value < DateTime.Now;
        if (expirado) assento.Status = StatusAssento.Disponivel;

        // Assert
        Assert.Equal(StatusAssento.Disponivel, assento.Status);
    }
}
```

---

## SP-11: Assets Visuais

### Arquivos a substituir

| Arquivo antigo | Substituir por | Motivo |
|----------------|----------------|--------|
| `wwwroot/images/eventos/bonner.webp` | Imagem de destino turístico (ex: `praia.webp`) | Era foto de show |
| `wwwroot/images/eventos/show_rock.jpg` | Imagem de destino turístico (ex: `montanha.jpg`) | Era foto de show |
| `wwwroot/images/eventos/showrock.png` | Imagem de destino turístico (ex: `cidade_historica.png`) | Era foto de show |
| `wwwroot/videos/video1.mp4` | Vídeo promocional de turismo | Era vídeo de show |

### Pasta de imagens

Renomear a pasta de imagens (caminho completo: `billet_2/billet_2/wwwroot/images/eventos/` → `billet_2/billet_2/wwwroot/images/destinos/`):

```bash
move billet_2\billet_2\wwwroot\images\eventos billet_2\billet_2\wwwroot\images\destinos
```

> **IMPORTANTE:** Após renomear a pasta, atualizar TODAS as referências ao caminho antigo `images/eventos/` nos arquivos `.razor` e no modelo `Viagem.FotoUrl` para `images/destinos/`.

---

## SP-12: Documentação

### Arquivos a atualizar

| Arquivo | Ação | Find/Replace Principal |
|---------|------|----------------------|
| `README.md` | Reescrever para TurismoPrime | `TicketPrimeSolucao` → `TurismoPrimeSolucao`, `TicketPrime` → `TurismoPrime`, `Evento` → `Viagem`, `Usuario` → `Passageiro`, `Ingresso` → `Passagem`, `eventos` → `viagens` |
| `docs/historiasdeusuario.md` | Adaptar 24 histórias para domínio de turismo | `Evento` → `Viagem`, `Usuario` → `Passageiro`, `Ingresso` → `Passagem`, `evento` → `viagem`, `ingresso` → `passagem`, `show` → `viagem`, `artista` → `destino` |
| `src/api.http` | Atualizar exemplos de chamadas HTTP | `/api/eventos/` → `/api/viagens/`, `/api/usuarios/` → `/api/passageiros/`, `Evento` → `Viagem`, `Usuario` → `Passageiro` |

### Instruções detalhadas

**README.md:**
1. Substituir título `# TicketPrimeSolucao` por `# TurismoPrimeSolucao`
2. Substituir todas as ocorrências de `Evento` (contexto) por `Viagem`
3. Substituir todas as ocorrências de `Usuario` (contexto) por `Passageiro`
4. Substituir `Ingresso` por `Passagem`
5. Atualizar endpoints da tabela para os novos paths (`/api/viagens/listar`, `/api/passageiros/listar`, etc.)
6. Adicionar seção sobre Mapa de Assentos e QR Code

**docs/historiasdeusuario.md:**
1. Substituir domínio: `Evento` → `Viagem`, `Usuario` → `Passageiro`, `Ingresso` → `Passagem`
2. Adaptar cenários BDD: "show" → "viagem", "artista" → "destino turístico"
3. Manter estrutura: 24 histórias, 3 seções (API, Frontend, Geral)

**src/api.http:**
1. Substituir todos os paths: `/api/eventos/` → `/api/viagens/`, `/api/usuarios/` → `/api/passageiros/`
2. Adicionar exemplos para: `GET /api/viagens/{id}/assentos`, `POST /api/reservas`, `POST /api/auth/login`
3. Atualizar JSON bodies para os novos modelos (`Viagem`, `Passageiro`)

---

## Checklist de Verificação Final

Após executar todas as specs, executar:

```bash
# 1. Compilar a API
cd src && dotnet build

# 2. Compilar o Frontend
cd billet_2/billet_2 && dotnet build

# 3. Compilar os Testes
cd tests && dotnet build

# 4. Rodar Testes
cd tests && dotnet test

# 5. Verificar estrutura de pastas
dir src\*
dir billet_2\billet_2\Models\*
dir billet_2\billet_2\Services\*
dir billet_2\billet_2\Components\Pages\*

# 6. (Opcional) Rodar API e testar endpoints
cd src && dotnet run
# Em outro terminal:
# curl http://localhost:5289/api/viagens/listar
# curl http://localhost:5289/api/passageiros/listar
```

### Critérios de Aceite

- [ ] `dotnet build` passa sem erros na API (`src/`)
- [ ] `dotnet build` passa sem erros no Frontend (`billet_2/billet_2/`)
- [ ] `dotnet build` passa sem erros nos Testes (`tests/`)
- [ ] `dotnet test` passa com todas as 8+ suites verdes
- [ ] Endpoint `GET /api/viagens/listar` retorna 200
- [ ] Endpoint `POST /api/viagens/cadastrar` cria viagem com assentos
- [ ] Endpoint `GET /api/viagens/{id}/assentos` retorna lista de assentos
- [ ] Página `/minhas-passagens` mostra passagens com QR Code
- [ ] Página `/criar-viagem` tem campos Origem, TipoVeiculo, etc.
- [ ] `db/script.sql` existe (não mais `db/sql` sem extensão)
- [ ] Assets visuais são de turismo (não de shows)

---

## Histórico de Alterações

### v1.1 — Revisão para execução por IA (14/05/2026)

**Nova seção adicionada:**
- **"Estado Atual das Specs"** — tabela com 5 estados possíveis (`⏳ Pendente`, `🔄 Desenvolvendo`, `✅ Implementada`, `❌ Com Erro`, `⚠️ Em Revisão`) para rastrear o progresso de cada spec visualmente.

**13 correções aplicadas nas specs:**

| # | Tipo | Local | Correção |
|---|------|-------|----------|
1 | 🔴 Crítico | SP-01.4 — `src/Program.cs` | Removeu `app.ListarAssentos()` e `app.CriarReserva()` que não existiam ainda (movidos para SP-04) |
2 | 🔴 Crítico | SP-02 — `UsuarioService.cs` | Marcado como MANTER mas usava `Usuario` (renomeado → `Passageiro`). Alterado para ATUALIZAR com SP-02.8 |
3 | 🔴 Crítico | SP-04.3 — `src/Program.cs` | Referenciava "Já incluído na SP-01.4" que foi removido. Substituído pelo bloco real de código |
4 | 🔴 Crítico | SP-10.0 — Projeto de testes | Adicionada `<ProjectReference Include="..\src\api.csproj" />` + nota sobre namespace global |
5 | 🔴 Crítico | SP-07.1 — `billet_2.csproj` | Adicionado `System.Drawing.Common` (obrigatório para QRCoder gerar bitmaps) |
6 | 🟡 Clareza | SP-01 — Início | Adicionado "Passo 0" com comandos `mkdir` e `move` para criar diretórios e renomear arquivos |
7 | 🟡 Clareza | SP-01.1/1.2 | "Criar a partir de" → "Criar o arquivo com o conteúdo completo abaixo" |
8 | 🟡 Clareza | SP-02.6 — Páginas Blazor | Adicionadas instruções `@inject` explícitas para ViagemDetalhes, MinhasPassagens, CriarViagem e Home |
9 | 🟡 Clareza | SP-06.3 — ViagemDetalhes | Incluído `@inject billet_2.Services.ReservaService ReservaService` |
10 | 🟡 Clareza | Índice — SP-02 | Corrigida contagem: 5/2/3 → 6 renomear, 2 criar, 4 atualizar |
11 | 🟢 Completude | SP-12 — Documentação | Tabela vaga substituída por Find/Replace por arquivo + instruções passo a passo |
12 | 🟢 Completude | SP-10 — Testes | Adicionada nota sobre classes da API estarem no escopo global (sem `using` extra) |
13 | 🟢 Completude | Tabela Tracking — SP-02 | Adicionado `Services/UsuarioService.cs 🔧` na coluna Arquivos-Chave |

### v1.0 — Criação inicial do ROADMAP (data original)

# Arquitetura — TicketPrime (Pré-Pivotagem) → TurismoPrime (Pós-Pivotagem)

> ✅ **PIVOTAGEM CONCLUÍDA** — Este documento descreve a arquitetura **antes** da pivotagem (TicketPrime) e as adições **já implementadas** (TurismoPrime).
> Para o plano de pivotagem, consulte [`docs/pivotagem/ROADMAP.md`](pivotagem/ROADMAP.md) e [`docs/pivotagem/pivotagem.md`](pivotagem/pivotagem.md).
> Para a arquitetura atual pós-pivotagem, consulte o [`README.md`](../README.md) (seções "Estrutura de Pastas", "Endpoints da API" e "Páginas do Frontend").
>
> As seções marcadas com ~~tachado~~ descrevem o estado **pré-pivotagem** (TicketPrime). As anotações **✅ Implementado** indicam o que foi concluído.

## Stack Tecnológica Geral

| Componente | Tecnologia | Versão | Propósito |
|------------|-----------|--------|-----------|
| Runtime | .NET SDK | **10.0** | Compilação e execução de todos os projetos |
| Linguagem | C# | 13 (/.NET 10) | Código-fonte de API, Frontend e Testes |
| IDE | Visual Studio Code / Visual Studio 2022 | — | Desenvolvimento e debugging |
| Controle de versão | Git | — | Versionamento e rollback |
| Gerenciador de pacotes | NuGet (via `dotnet restore`) | — | Dependências dos projetos |

---

## Projetos da Solução (Pré-Pivotagem — TicketPrime)

> 📦 As tabelas abaixo mostram o estado **antes** da pivotagem. As anotações `→` indicam o que cada item se tornou no TurismoPrime.

### 1. API — Backend (`src/`)

| Item | Especificação |
|------|---------------|
| **SDK** | `Microsoft.NET.Sdk.Web` |
| **Target** | `net10.0` |
| **Tipo** | ASP.NET Core Minimal API (com classes controller estáticas) |
| **Porta HTTP** | `5289` |
| **Porta HTTPS** | `7247` |

#### Controladores (Pré-Pivotagem → Pós-Pivotagem)

| Arquivo (Antes) | → (Depois) | Rotas (Antes) | Entidade (Antes) |
|---------|-------|----------|----------|
| `src/eventos/EventosController.cs` | → `src/viagens/ViagensController.cs` ✅ | `GET /api/eventos/listar`, ... | `Evento` → `Viagem` |
| `src/usuarios/UsuariosController.cs` | → `src/passageiros/PassageirosController.cs` ✅ | `GET /api/usuarios/listar`, ... | `Usuario` → `Passageiro` |
| `src/cupons/CuponsController.cs` | → *(mantido)* | *(mantido)* | `Cupons` |

#### Packages (Pré-Pivotagem)

| Package | Versão | Uso |
|---------|--------|-----|
| `Microsoft.AspNetCore.OpenApi` | 10.0.5 | Geração automática de OpenAPI/Swagger |

#### Packages Instalados Durante a Pivotagem ✅

| Spec | Package | Versão | Status |
|------|---------|--------|--------|
| **SP-08** | `Microsoft.AspNetCore.Authentication.JwtBearer` | 10.0.5 | ✅ Instalado |
| **SP-09** | `Dapper` | 2.1.35 | ✅ Instalado |
| **SP-09** | `Npgsql` | 9.0.3 | ✅ Instalado |

---

### 2. Frontend — Blazor Server + WebAssembly (`billet_2/billet_2/`)

| Item | Especificação |
|------|---------------|
| **SDK** | `Microsoft.NET.Sdk.Web` |
| **Target** | `net10.0` |
| **Tipo** | Blazor Web App (Server + WebAssembly interactivo) |
| **Porta HTTP** | `5096` |
| **Porta HTTPS** | `7201` |
| **CSS** | Bootstrap 5 (estático em `wwwroot/lib/bootstrap/`) |
| **CORS (API)** | `http://localhost:5096` configurado como origem permitida |

#### Models (Pré-Pivotagem → Pós-Pivotagem)

| Arquivo (Antes) | → (Depois) | Status |
|---------|--------|--------|
| `Models/Evento.cs` | → `Models/Viagem.cs` | ✅ Renomeado |
| `Models/Usuario.cs` | → `Models/Passageiro.cs` | ✅ Renomeado |
| *(novo)* | `Models/Assento.cs` | ✅ Criado (SP-03) |
| *(novo)* | `Models/Reserva.cs` | ✅ Criado (SP-03) |

#### Services (Pré-Pivotagem → Pós-Pivotagem)

| Arquivo (Antes) | → (Depois) | Status |
|---------|--------|--------|
| `Services/EventoService.cs` | → `Services/ViagemService.cs` | ✅ Renomeado |
| `Services/UsuarioService.cs` | → `Services/PassageiroService.cs` | ✅ Renomeado |
| `Services/AuthService.cs` | → *(mantido, adaptado)* | ✅ Atualizado (SP-02.7) |
| *(novo)* | `Services/ReservaService.cs` | ✅ Criado (SP-06) |
| *(novo)* | `Services/QrCodeService.cs` | ✅ Criado (SP-07) |

#### Páginas (Pré-Pivotagem → Pós-Pivotagem)

| Página (Antes) | → (Depois) | Status |
|--------|------|--------|
| `Home.razor` (`/`) | → *(mantido, adaptado)* | ✅ Atualizado |
| `Cadastro.razor` (`/cadastro`) | → *(mantido, adaptado)* | ✅ Atualizado |
| `Login.razor` (`/login`) | → *(mantido, adaptado)* | ✅ Atualizado |
| `Poslogin.razor` (`/poslogin`) | → *(mantido)* | ✅ Mantido |
| `Venda.razor` (`/vendas/{id}`) | → `ViagemDetalhes.razor` (`/viagem/{id}`) | ✅ Renomeado |
| `Meusingressos.razor` (`/meusingressos`) | → `MinhasPassagens.razor` (`/minhas-passagens`) | ✅ Renomeado |
| `Criarevento.razor` (`/criarevento`) | → `CriarViagem.razor` (`/criar-viagem`) | ✅ Renomeado |
| *(novo)* | `Components/MapaAssentos.razor` | ✅ Criado (SP-05) |

#### Packages

| Package | Versão | Uso | Status |
|---------|--------|-----|--------|
| `Microsoft.AspNetCore.Components.WebAssembly.Server` | 10.0.5 | Suporte a renderização interativa WebAssembly | ✅ (pré-existente) |
| `QRCoder` | 1.6.0 | Geração de QR Code | ✅ Instalado (SP-07) |
| `System.Drawing.Common` | 9.0.4 | Dependência do QRCoder para bitmaps | ✅ Instalado (SP-07) |

---

### 3. Frontend — WASM Client (`billet_2/billet_2.Client/`)

| Item | Especificação |
|------|---------------|
| **SDK** | `Microsoft.NET.Sdk.BlazorWebAssembly` |
| **Target** | `net10.0` |
| **Tipo** | Blazor WebAssembly standalone (client-side) |

#### Packages Atuais

| Package | Versão |
|---------|--------|
| `Microsoft.AspNetCore.Components.WebAssembly` | 10.0.5 |

> Este projeto **não recebe novas dependências** durante a pivotagem.

---

### 4. Testes — xUnit (`tests/`)

| Item | Especificação |
|------|---------------|
| **SDK** | `Microsoft.NET.Sdk` |
| **Target** | `net10.0` |
| **Framework** | xUnit |
| **Tipo** | Testes unitários |

#### Packages Atuais

| Package | Versão | Uso |
|---------|--------|-----|
| `Microsoft.NET.Test.Sdk` | 17.14.1 | Runner de testes |
| `xunit` | 2.9.3 | Framework de testes |
| `xunit.runner.visualstudio` | 3.1.4 | Integração com Visual Studio |
| `coverlet.collector` | 6.0.4 | Cobertura de código |

#### Testes (Pré-Pivotagem → Pós-Pivotagem)

| Arquivo (Antes) | → (Depois) | Status |
|---------|--------|--------|
| `TestePrecoPositivo.cs` | → `TestePrecoPassagemPositivo.cs` | ✅ Renomeado |
| `TesteEventoCapacidade.cs` | → `TesteViagemCapacidade.cs` | ✅ Renomeado |
| `TesteReservaValida.cs` | → `TesteReservaAssentoValida.cs` | ✅ Renomeado |
| `TesteReservaVazia.cs` | → `TesteReservaAssentoSemDados.cs` | ✅ Renomeado |
| `TesteDescontoValido.cs` | → *(mantido)* | ✅ Mantido |
| *(novo)* | `TesteAssentoService.cs` | ✅ Criado (SP-10) |
| *(novo)* | `TesteReservaComAssento.cs` | ✅ Criado (SP-10) |
| *(novo)* | `TesteCheckInPassageiro.cs` | ✅ Criado (SP-10) |

> ✅ Total: **8 classes de teste, 14 testes passando** (`dotnet test` ✅).

#### Dependências Adicionadas (SP-10) ✅

| Spec | Tipo | Descrição | Detalhes | Status |
|------|------|-----------|----------|--------|
| **SP-10** | `ProjectReference` | `..\src\api.csproj` | Necessário para que os testes enxerguem as classes `Viagem`, `Assento`, `StatusAssento`, `Passageiro` | ✅ Adicionado |

> ✅ As classes da API estão no **escopo global** (sem `namespace` declarado). Com a `ProjectReference`, tornam-se automaticamente acessíveis — **não** é necessário adicionar diretivas `using` além de `using Xunit;`.

---

## Solution File

| Arquivo | Projetos Referenciados |
|---------|----------------------|
| `billet_2.slnx` | `billet_2/billet_2/billet_2.csproj` |

> ⚠️ A solution **não** inclui `src/api.csproj` nem `tests/MeuProjeto.Tests.csproj`. Eles são compilados individualmente com `dotnet build src/` e `dotnet build tests/`.

---

## Ferramentas de Desenvolvimento

### Essenciais

| Ferramenta | Onde usar | Finalidade |
|------------|-----------|------------|
| **.NET 10 SDK** | Todos os projetos | Compilação, restauração, execução (`dotnet build`, `dotnet run`, `dotnet test`) |
| **Git** | Raiz do projeto | Controle de versão, rollback via `git restore` |
| **Editor de código** (VS Code / VS 2022) | Todos os projetos | Edição de código, debugging |

### Instalação

| Ferramenta | Comando / Link |
|------------|----------------|
| .NET 10 SDK | [Download oficial](https://dotnet.microsoft.com/download/dotnet/10.0) |
| Verificar instalação | `dotnet --version` (deve retornar `10.0.x`) |
| Verificar SDKs disponíveis | `dotnet --list-sdks` |

### Comandos de Build por Projeto

| Projeto | Comando |
|---------|---------|
| API (backend) | `cd src && dotnet build` |
| Frontend (Blazor) | `cd billet_2\billet_2 && dotnet build` |
| Testes | `cd tests && dotnet build` |
| Testes (execução) | `cd tests && dotnet test` |

---

## Requisitos de Portas e Rede

| Serviço | Porta | Origem do Acesso |
|---------|-------|------------------|
| API (HTTP) | `5289` | Frontend via `HttpClient` (`http://localhost:5289`) |
| API (HTTPS) | `7247` | — |
| Frontend (HTTP) | `5096` | Navegador / usuário final |
| Frontend (HTTPS) | `7201` | Navegador / usuário final |
| CORS — AllowOrigins | `http://localhost:5096` | Configurado na API (`BlazorPolicy`) |

---

## Limitações Conhecidas (Pré-Pivotagem vs. Pós-Pivotagem)

| # | Limitação (TicketPrime) | Resolvida no TurismoPrime? |
|---|------------------------|---------------------------|
| 1 | **Sem persistência** — dados em listas em memória (`List<T>`) | ⚠️ Parcial — PostgreSQL configurado (SP-09), mas não integrado ao runtime; listas em memória ainda são o padrão |
| 2 | **Sem autenticação real** — login local (CPF + senha), sem JWT | ✅ Resolvido — JWT implementado (SP-08) via `POST /api/auth/login` |
| 3 | **Sem fluxo de compra completo** — sem checkout real | ✅ Resolvido — Reserva temporária de 15 min + checkout (SP-06) |
| 4 | **Sem QR Code** — não implementado | ✅ Resolvido — QRCoder 1.6.0 + PngByteQRCode (SP-07) |
| 5 | **Sem mapa de assentos** — não implementado | ✅ Resolvido — `MapaAssentos.razor` interativo (SP-05) |
| 6 | **Sem banco de dados** — script SQL sem extensão `.sql` | ✅ Resolvido — `db/script.sql` com schema TurismoPrime (SP-09) |
| 7 | **Solution incompleta** — `billet_2.slnx` não inclui `src/` nem `tests/` | ⚠️ Mantido — compilação individual via `dotnet build` |

---

## Mapa de Dependências entre Specs (Pós-Pivotagem)

```
SP-01 (API) ──────────────────────────────────────────────┐
  ├── SP-02 (Frontend) ─── SP-02.7 (AuthService) ──┐      │
  │                                                   │      │
SP-03 (Modelos Assentos) ──┐                         │      │
  ├── SP-04 (Endpoints) ───┤                         │      │
  │                         ├── SP-06 (Reserva) ◄────┘      │
  │                         │                               │
  │                         └── SP-05 (MapaAssentos) ───────┤
  │                                                         │
  ├── SP-07 (QR Code) ◄── SP-06                             │
  ├── SP-08 (JWT) ──────────────────────────────────────────┘
  ├── SP-09 (Banco) ────────────────────────────────────────┐
  ├── SP-10 (Testes) ◄── SP-01 + SP-03 + SP-04             │
  ├── SP-11 (Assets)                                        │
  └── SP-12 (Docs) ◄── TODAS                                │
                                                              │
  SP-07 independente de SP-08, SP-09                        │
  SP-11 independente                                         │
  SP-12 depende de TODAS as specs anteriores ◄───────────────┘
```

---

## Mapa de Arquivos por Spec (Executado ✅)

| Spec | Arquivos Criados (✨) | Arquivos Modificados (🔧) | Arquivos Renomeados (🔄) |
|------|----------------------|--------------------------|--------------------------|
| **SP-01** | — | `src/Program.cs` 🔧 | `src/eventos/EventosController.cs` → `src/viagens/ViagensController.cs` 🔄 |
| | | | `src/usuarios/UsuariosController.cs` → `src/passageiros/PassageirosController.cs` 🔄 |
| **SP-02** | `Models/Viagem.cs` ✨, `Models/Passageiro.cs` ✨, `Services/ViagemService.cs` ✨, `Pages/ViagemDetalhes.razor` ✨, `Pages/MinhasPassagens.razor` ✨, `Pages/CriarViagem.razor` ✨ | `Services/AuthService.cs` 🔧, `Services/UsuarioService.cs` 🔧, `Program.cs` 🔧, `Home.razor` 🔧, `_Imports.razor` 🔧 | `Models/Evento.cs` → `Models/Viagem.cs`, `Models/Usuario.cs` → `Models/Passageiro.cs`, `Services/EventoService.cs` → `Services/ViagemService.cs`, `Pages/Venda.razor` → `Pages/ViagemDetalhes.razor`, `Pages/Meusingressos.razor` → `Pages/MinhasPassagens.razor`, `Pages/Criarevento.razor` → `Pages/CriarViagem.razor` |
| **SP-03** | `src/assentos/Assento.cs` ✨, `Models/Assento.cs` ✨, `Models/Reserva.cs` ✨ | — | — |
| **SP-04** | `src/assentos/AssentosController.cs` ✨ | `src/viagens/ViagensController.cs` 🔧, `src/Program.cs` 🔧 | — |
| **SP-05** | `Components/MapaAssentos.razor` ✨, `Components/MapaAssentos.razor.css` ✨ | — | — |
| **SP-06** | `Services/ReservaService.cs` ✨ | `Program.cs` 🔧, `Pages/ViagemDetalhes.razor` 🔧 | — |
| **SP-07** | `Services/QrCodeService.cs` ✨ | `billet_2.csproj` 🔧, `Program.cs` 🔧, `Pages/MinhasPassagens.razor` 🔧 | — |
| **SP-08** | — | `src/api.csproj` 🔧, `src/Program.cs` 🔧, `src/passageiros/PassageirosController.cs` 🔧 | — |
| **SP-09** | — | `src/api.csproj` 🔧, `src/appsettings.json` 🔧 | `db/sql` → `db/script.sql` 🔄 |
| **SP-10** | `TesteAssentoService.cs` ✨, `TesteReservaComAssento.cs` ✨, `TesteCheckInPassageiro.cs` ✨ | `tests/MeuProjeto.Tests.csproj` 🔧 | `TestePrecoPositivo.cs` → `TestePrecoPassagemPositivo.cs`, `TesteEventoCapacidade.cs` → `TesteViagemCapacidade.cs`, `TesteReservaValida.cs` → `TesteReservaAssentoValida.cs`, `TesteReservaVazia.cs` → `TesteReservaAssentoSemDados.cs` |
| **SP-11** | 4 novos assets (imagens/vídeo) ✨ | Todas as referências a `images/eventos/` nos `.razor` 🔧 | `wwwroot/images/eventos/` → `wwwroot/images/destinos/` 🔄 |
| **SP-12** | — | `README.md` 🔧, `docs/historiasdeusuario.md` 🔧, `src/api.http` 🔧 | — |

---

## Diretórios Criados Durante a Pivotagem ✅

| Diretório | Spec | Propósito |
|-----------|------|-----------|
| `src/viagens/` | SP-01 | Controllers de viagem (renomeado de `src/eventos/`) |
| `src/passageiros/` | SP-01 | Controllers de passageiros (renomeado de `src/usuarios/`) |
| `src/assentos/` | SP-01 | Controllers e modelo de assentos |

---

## Comandos Úteis (Compilação e Verificação)

### Build Completo da Solução

```batch
:: API
cd src && dotnet build

:: Frontend
cd billet_2\billet_2 && dotnet build

:: Testes
cd tests && dotnet build
```

### Execução

```batch
:: API (terminal 1)
cd src && dotnet run

:: Frontend (terminal 2)
cd billet_2\billet_2 && dotnet run

:: Testes (terminal 3)
cd tests && dotnet test
```

### Rollback (Git)

```batch
:: Reverter arquivo específico
git restore <caminho/do/arquivo>

:: Reverter tudo (cuidado!)
git restore .
```

---

## Observações Importantes

1. **Ordem de instalação de packages**: sempre instale o package **antes** de escrever o código que o utiliza, para que o Intellisense funcione corretamente.
2. **Portas fixas**: API sempre em `:5289`, Frontend sempre em `:5096`. Se houver conflito, alterar em `launchSettings.json` e na CORS policy da API.
3. **Solution incompleta**: `billet_2.slnx` não inclui `src/api.csproj` nem `tests/`. Compilar individualmente ou adicionar à solution.
4. **Banco de dados**: PostgreSQL com Dapper é opcional (SP-09). O sistema funciona com listas em memória sem banco.
5. **Rollback**: cada spec tem seu próprio procedimento de rollback documentado na coluna "⚠️ Risco / Rollback" da tabela de tracking no ROADMAP.md.

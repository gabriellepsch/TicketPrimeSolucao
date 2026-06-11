# TurismoPrimeSolucao

Resolução da avaliação referente à matéria de **Engenharia de Software (UNIFESO)**.

Plataforma de reserva e venda de passagens de transporte turístico, composta por uma API backend e um frontend web, desenvolvidos em C# com .NET 10.

> **Pivotagem:** Este projeto é resultado da pivotagem do TicketPrime (venda de ingressos para eventos) para o domínio de turismo. Consulte [`docs/pivotagem/`](docs/pivotagem/) para detalhes.

Integrantes 
Gabriel Castor 06009642
Gabriel Lepsch Monteiro 02001770
Gabriel Ribeiro 06010603
Lucas Oliveira 06010486
Luiz Eduardo P. Rosa 06010412
Thiago Zandonade Fernandes 06010263

## Tecnologias

| Camada | Tecnologia |
|---|---|
| Backend | ASP.NET Core 10 — Minimal API |
| Frontend | Blazor Web App (Server + WebAssembly) |
| Linguagem | C# (.NET 10) |
| Estilo | Bootstrap 5 (via arquivos estáticos) |
| Autenticação | JWT (JSON Web Token) |
| QR Code | QRCoder 1.6.0 (PngByteQRCode) |
| Banco de dados | PostgreSQL + Dapper (opcional — listas em memória como padrão) |

---

## Estrutura de Pastas

```
TurismoPrimeSolucao/
│
├── billet_2.slnx                  ← Solution file
│
├── src/                           ← Backend: ASP.NET Core Minimal API
│   ├── Program.cs                 ← Ponto de entrada da API (CORS, JWT, endpoints)
│   ├── api.csproj
│   ├── api.http                   ← Exemplos de chamadas HTTP para teste
│   ├── appsettings.json
│   ├── viagens/
│   │   └── ViagensController.cs   ← Endpoints de viagens
│   ├── passageiros/
│   │   └── PassageirosController.cs ← Endpoints de passageiros + Login JWT
│   ├── assentos/
│   │   ├── Assento.cs             ← Modelo de assento + StatusAssento
│   │   └── AssentosController.cs  ← Endpoints de assentos e reservas
│   └── cupons/
│       └── CuponsController.cs    ← Endpoints de cupons
│
├── billet_2/                      ← Frontend: Blazor Web App
│   ├── billet_2/                  ← Projeto servidor
│   │   ├── Program.cs             ← Ponto de entrada do frontend
│   │   ├── Components/
│   │   │   ├── Pages/
│   │   │   │   ├── Home.razor         ← Listagem de viagens (visitante)
│   │   │   │   ├── Cadastro.razor     ← Cadastro de passageiro
│   │   │   │   ├── Login.razor        ← Login do passageiro
│   │   │   │   ├── Poslogin.razor     ← Painel do passageiro logado
│   │   │   │   ├── ViagemDetalhes.razor ← Detalhes da viagem + mapa de assentos + reserva
│   │   │   │   ├── MinhasPassagens.razor ← Carrinho + finalização com QR Code
│   │   │   │   └── CriarViagem.razor  ← Admin: cadastrar viagem
│   │   │   └── MapaAssentos.razor     ← Componente visual do ônibus
│   │   ├── Models/
│   │   │   ├── Viagem.cs
│   │   │   ├── Passageiro.cs
│   │   │   ├── Assento.cs
│   │   │   └── Reserva.cs
│   │   ├── Services/
│   │   │   ├── ViagemService.cs
│   │   │   ├── PassageiroService.cs
│   │   │   ├── AuthService.cs
│   │   │   ├── ReservaService.cs
│   │   │   └── QrCodeService.cs
│   │   └── wwwroot/               ← Assets estáticos (CSS, vídeo, imagens, Bootstrap)
│   └── billet_2.Client/           ← Projeto cliente WebAssembly
│
├── db/
│   └── script.sql                 ← Script DDL TurismoPrime (PostgreSQL)
│
├── docs/
│   ├── visao.md                   ← Documento de Visão (estado atual)
│   ├── arquitetura.md             ← Arquitetura do sistema
│   ├── historiasdeusuario.md      ← 24 Histórias de Usuário
│   ├── CHANGELOG.md               ← Histórico de mudanças
│   ├── CORRECAO.md                ← Correção da AV1 (histórico)
│   └── pivotagem/                 ← Documentação da pivotagem
│       ├── ADR-001-pivotagem-turismo.md
│       ├── pivotagem.md
│       └── ROADMAP.md             ← Guia de execução das 12 specs
│
└── tests/                         ← Testes unitários (xUnit)
    ├── TestePrecoPassagemPositivo.cs
    ├── TesteViagemCapacidade.cs
    ├── TesteReservaAssentoValida.cs
    ├── TesteReservaAssentoSemDados.cs
    ├── TesteAssentoService.cs
    ├── TesteReservaComAssento.cs
    ├── TesteCheckInPassageiro.cs
    └── TesteDescontoValido.cs
```

---

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

> Nenhuma outra dependência externa é necessária para rodar o projeto localmente.  
> As dependências NuGet são restauradas automaticamente pelo `dotnet run`.

---

## Como Executar

### 1. Backend — API

```bash
cd src
dotnet run
```

A API estará disponível em: `http://localhost:5289`

> ⚠️ **Apenas HTTP:** O CORS da API está configurado exclusivamente para `http://localhost:5096`. As portas HTTPS (`https://localhost:7247` para API, `https://localhost:7201` para frontend) estão definidas no `launchSettings.json` mas **não funcionam** com o frontend — o navegador bloqueará as requisições cross-origin.

### 2. Frontend — Blazor

```bash
cd billet_2/billet_2
dotnet run
```

O frontend estará disponível em: `http://localhost:5096`

### 3. Testes

```bash
cd tests
dotnet test
```

> **Importante:** a API deve estar rodando antes de acessar o frontend.

---

## Endpoints da API

### Passageiros

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/passageiros/listar` | Lista todos os passageiros |
| `POST` | `/api/passageiros/cadastrar` | Cadastra um novo passageiro |

### Autenticação

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/auth/login` | Login com email+senha → retorna token JWT |

**Corpo para login:**
```json
{
  "email": "joao@email.com",
  "senha": "minhasenha"
}
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "passageiro": { "id": 1, "nome": "João", "email": "joao@email.com", ... }
}
```

### Viagens

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/viagens/listar` | Lista todas as viagens |
| `GET` | `/api/viagens/listar/{id}` | Busca viagem por ID |
| `POST` | `/api/viagens/cadastrar` | Cadastra nova viagem (gera assentos automaticamente) |

**Corpo para cadastro de viagem:**
```json
{
  "destino": "Rio de Janeiro",
  "origem": "São Paulo",
  "descricao": "Viagem turística com paradas",
  "dataSaida": "2026-12-01T08:00:00",
  "dataRetorno": "2026-12-05T20:00:00",
  "totalAssentos": 46,
  "valorPassagem": 150.00,
  "tipoVeiculo": "Semileito",
  "empresaTransporte": "Viação ABC",
  "fotoUrl": "images/destinos/rio.jpg"
}
```

### Assentos e Reservas

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/viagens/{id}/assentos` | Lista assentos da viagem (com status) |
| `POST` | `/api/reservas` | Reserva assento temporariamente (15 min) |

**Corpo para reserva:**
```json
{
  "viagemId": 1,
  "assentoId": 5,
  "passageiroId": 1
}
```

### Cupons

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/cupons/listar` | Lista todos os cupons |
| `POST` | `/api/cupons/cadastrar` | Cadastra um novo cupom |

---

## Páginas do Frontend

| Rota | Descrição |
|---|---|
| `/` | Home — lista as viagens disponíveis |
| `/cadastro` | Cadastro de passageiro |
| `/login` | Login do passageiro |
| `/poslogin` | Painel do passageiro logado |
| `/viagem/{id}` | Detalhes da viagem + mapa de assentos + reserva |
| `/minhas-passagens` | Carrinho + finalização com QR Code |
| `/criar-viagem` | Admin: cadastrar nova viagem |

---

## Banco de Dados

O arquivo `db/script.sql` contém o script DDL completo para criação das tabelas em **PostgreSQL**:

- `Passageiros`
- `Viagens`
- `Assentos` (com UNIQUE constraint em ViagemId + Numero)
- `Cupons`
- `Reservas` (com 4 chaves estrangeiras e índices)

Pacotes instalados para integração: **Dapper 2.1.35** + **Npgsql 9.0.3**.  
Connection string configurada em `src/appsettings.json`.

> O banco de dados é **opcional** — o sistema funciona com listas em memória sem PostgreSQL.

---

## Funcionalidades Implementadas (Pivotagem)

| Módulo | Status |
|--------|--------|
| Renomeação API + Frontend (Evento→Viagem, Usuario→Passageiro) | ✅ |
| Modelo de Assentos + Status (Disponivel/Reservado/Vendido) | ✅ |
| Mapa de Assentos interativo (verde/amarelo/vermelho) | ✅ |
| Reserva temporária de 15 minutos | ✅ |
| QR Code nas passagens (PngByteQRCode) | ✅ |
| Autenticação JWT (`POST /api/auth/login`) | ✅ |
| Schema PostgreSQL TurismoPrime (`db/script.sql`) | ✅ |
| Testes unitários (14 testes, xUnit) | ✅ |
| Assets renomeados (`eventos/` → `destinos/`) | ✅ |

---

## Limitações Conhecidas

- **Persistência opcional:** PostgreSQL configurado mas não integrado ao runtime; dados em listas em memória são perdidos ao reiniciar.
- **Solution incompleta:** `billet_2.slnx` referencia apenas o projeto Blazor. `src/api.csproj` e `tests/` compilam separadamente.
- **Frontend não consome JWT:** O login no frontend ainda usa busca local de passageiros em vez do endpoint `/api/auth/login`.
- **Senhas em plain text:** Armazenamento sem hash (projeto acadêmico).
- **Apenas HTTP funciona:** As portas HTTPS (`7247` API, `7201` frontend) estão definidas mas o CORS só libera `http://localhost:5096` → `https://localhost:7201` é bloqueado pelo navegador.
- **`TicketPrimeSolucaoAPI/`:** Pasta residual do projeto original (template .NET com `weatherforecast`). Roda na porta 5204, não é usada pelo sistema atual.

---

## Licença

Distribuído sob a licença MIT. Consulte o arquivo `LICENSE` para mais informações.

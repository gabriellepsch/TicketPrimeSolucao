# Spec 0010 — Integração com Banco de Dados Supabase

> **Status:** Aprovada — Em implementação
> **Tipo:** Funcionalidade nova (Regra A)
> **Data:** 01/07/2026

---

## 1. Objetivo

Substituir o armazenamento em memória (`List<T>` estáticas) da API por um banco de dados PostgreSQL hospedado no **Supabase**, utilizando **Npgsql** como driver e **Dapper** como micro-ORM.

---

## 2. Contexto e Diagnóstico

### 2.1 Estado atual

| Item | Situação |
|------|----------|
| Armazenamento | `List<T>` estáticas em todos os controllers — dados **voláteis** (perdidos ao reiniciar) |
| Script SQL existente | `db/sql` e `db/script.sql` contêm DDL do domínio **TicketPrime** (tabelas `Usuarios`, `Eventos`, `Cupons`, `Reservas`) |
| Domínio atual (TripPrime) | `Usuarios`, `Viagens`, `Veiculos`, `Assentos`, `Passagens`, `Cupons` |
| Pacotes NuGet | Apenas `Microsoft.AspNetCore.OpenApi` — **nenhum** pacote de banco |
| Configuração | `appsettings.json` **não possui** connection string |
| Supabase | Projeto **não criado** |

### 2.2 Scripts SQL existentes vs. código atual

| Tabela no script SQL | Modelo C# atual | Situação |
|----------------------|-----------------|----------|
| `Usuarios` | `Usuario` | ✅ Existe em ambos — schema precisa ser revisado |
| `Eventos` (antigo TicketPrime) | `Evento` (substituído por `Viagem`) | ⚠️ Removido na pivotagem — `Viagem` é a entidade atual |
| `Cupons` | `Cupons` | ⚠️ Schema do script difere do modelo C# |
| `Reservas` | `Passagem` | ⚠️ Script tem `Reservas`, código usa `Passagem` |
| _(inexistente)_ | `Viagem` | ❌ Nova entidade — sem tabela no script |
| _(inexistente)_ | `Veiculo` | ❌ Nova entidade — sem tabela no script |
| _(inexistente)_ | `Assento` | ❌ Nova entidade — sem tabela no script |

### 2.3 Modelos C# atuais (fonte de verdade para o schema)

**Usuario** (`src/usuarios/UsuariosController.cs`):
- `Id` (int), `Nome` (string), `Email` (string), `Cpf` (string), `Adm` (bool), `Senha` (string)

**Viagem** (`src/viagens/ViagensController.cs`):
- `Id` (int), `Origem` (string), `Destino` (string), `DataPartida` (DateTime), `DataChegada` (DateTime), `DataVolta` (DateTime?), `Descricao` (string), `VeiculoId` (int), `PrecoBase` (float), `FotoUrl` (string?)

**Veiculo** (`src/veiculos/VeiculosController.cs`):
- `Id` (int), `Modelo` (string), `Placa` (string), `Capacidade` (int), `Tipo` (string), `Linhas` (int), `Colunas` (int)

**Assento** (`src/veiculos/VeiculosController.cs`):
- `Id` (int), `VeiculoId` (int), `Numero` (string), `Tipo` (string), `Status` (string)

**Passagem** (`src/passagens/PassagensController.cs`):
- `Id` (int), `ViagemId` (int), `AssentoId` (int), `UsuarioCpf` (string), `PrecoPago` (float), `CupomUtilizado` (string?), `Status` (string), `DataCompra` (DateTime), `DataExpiracaoReserva` (DateTime?)

**Cupons** (`src/cupons/CuponsController.cs`):
- `Id` (int), `Codigo` (string), `PercentualDesconto` (int)

---

## 3. Escopo

### 3.1 O que SERÁ feito

1. Criar novo script DDL (`db/script_tripprime.sql`) com as 6 tabelas do domínio TripPrime
2. Configurar `appsettings.json` com connection string do Supabase
3. Adicionar pacotes NuGet `Npgsql` e `Dapper` ao projeto `src/api.csproj`
4. Substituir `List<T>` por queries Dapper+Npgsql em **todos** os 6 controllers
5. Manter 100% de compatibilidade com os endpoints e modelos existentes (frontend Blazor NÃO será alterado)
6. Garantir que `dotnet build` e `dotnet test` compilam e passam sem erros

### 3.2 O que NÃO será feito

- NÃO alterar o frontend Blazor
- NÃO alterar os scripts SQL antigos (`db/sql` e `db/script.sql`) — serão preservados como referência histórica
- NÃO adicionar autenticação JWT/Supabase Auth (fora do escopo)
- NÃO usar Entity Framework Core (decisão arquitetural já documentada: usar Dapper)
- NÃO alterar os modelos C# (classes `Usuario`, `Viagem`, `Veiculo`, `Assento`, `Passagem`, `Cupons`)
- NÃO criar sistema de migrations

---

## 4. Implementação — Passo a passo

### 4.1 Script DDL (`db/script_tripprime.sql`)

Criar novo arquivo `db/script_tripprime.sql` com `CREATE TABLE IF NOT EXISTS` para as 6 tabelas:

```sql
-- Ordem de criação (respeitar dependências de FK):
-- 1. Usuarios (sem FK)
-- 2. Veiculos (sem FK)
-- 3. Assentos (FK → Veiculos)
-- 4. Viagens (FK → Veiculos)
-- 5. Cupons (sem FK)
-- 6. Passagens (FK → Viagens, Assentos, Usuarios, Cupons)
```

**Regras de schema:**
- IDs com `SERIAL PRIMARY KEY`
- Colunas de preço usar `NUMERIC(10,2)` (não `REAL` — evita problemas de precisão)
- FKs com `ON DELETE RESTRICT` para evitar dados órfãos (exceto CupomUtilizado que usa `ON DELETE SET NULL`)
- Índices em colunas de busca frequente (CPF, ViagemId, VeiculoId)
- Colunas string usar `VARCHAR(255)` exceto descrições longas (`TEXT`)
- Nomes de tabelas e colunas em **PascalCase** (seguir convenção C# para mapeamento Dapper sem anotações)

### 4.2 Connection String

Adicionar ao `appsettings.json`:

```json
"ConnectionStrings": {
  "Supabase": "Host=SEU_HOST;Port=6543;Database=postgres;Username=postgres.SEU_REF;Password=SUA_SENHA;SSL Mode=Require;Trust Server Certificate=true"
}
```

> ⚠️ **ATENÇÃO:** Os valores `SEU_HOST`, `SEU_REF` e `SUA_SENHA` **NÃO serão preenchidos** no código. O usuário deve:
> 1. Criar o projeto no Supabase
> 2. Copiar a connection string do painel Supabase → Settings → Database → Connection String
> 3. Preencher os valores manualmente

### 4.3 Pacotes NuGet

Executar no diretório `src/`:

```bash
dotnet add package Npgsql
dotnet add package Dapper
```

### 4.4 Padrão de código para queries Dapper

**Regra obrigatória para TODAS as queries:**
- Usar **parâmetros com `@`** (`@Nome`, `@Cpf`, etc.) — nunca concatenar strings
- Usar `using var connection = new NpgsqlConnection(connectionString);` — nome do método será `ObterConexao()`
- Todas as queries em string com `@"..."` (verbatim string)
- Retornar `Results.Ok()`, `Results.BadRequest()`, `Results.NotFound()` como já faz hoje

**Exemplo de padrão (substitui `List<T>`):**

```csharp
// ANTES (memória):
private static List<Usuario> Usuarios = new();

// DEPOIS (Supabase via Dapper):
private static NpgsqlConnection ObterConexao()
{
    // A connection string será obtida via DI — detalhe definido na implementação
}
```

### 4.5 Refatoração dos Controllers

Cada controller será refatorado individualmente, na seguinte ordem:

| # | Controller | Endpoints afetados |
|---|-----------|-------------------|
| 1 | `UsuariosController.cs` | `GET listar`, `POST cadastrar` |
| 2 | `VeiculosController.cs` | `GET listar`, `GET listar/{id}`, `POST cadastrar` |
| 3 | `ViagensController.cs` | `GET listar`, `GET listar/{id}`, `GET pesquisar`, `POST cadastrar` |
| 4 | `AssentosController.cs` | `GET viagem/{id}`, `POST reservar`, `POST liberar`, `POST bloquear` |
| 5 | `CuponsController.cs` | `GET listar`, `POST cadastrar` |
| 6 | `PassagensController.cs` | `GET listar`, `GET usuario/{cpf}`, `POST comprar`, `POST cancelar/{id}` |

### 4.6 Estratégia de injeção de dependência

A connection string será registrada no `Program.cs` via `builder.Configuration.GetConnectionString("Supabase")` e injetada nos controllers. O padrão será:

```csharp
// Program.cs
var connectionString = builder.Configuration.GetConnectionString("Supabase")
    ?? throw new InvalidOperationException("Connection string 'Supabase' não configurada.");

builder.Services.AddSingleton(connectionString); // ou AddScoped — decidir na implementação
```

---

## 5. Critérios de Aceitação (Definition of Done)

### 5.1 Compilação e testes

- [ ] `dotnet build` no diretório `src/` compila **sem erros e sem warnings**
- [ ] `dotnet test` no diretório `tests/` — todos os 5 testes passam
- [ ] `dotnet build` no diretório `billet_2/billet_2/` compila sem erros

### 5.2 Funcionalidade

- [ ] Endpoint `POST /api/usuarios/cadastrar` insere registro no Supabase e retorna o usuário
- [ ] Endpoint `GET /api/usuarios/listar` retorna dados do Supabase
- [ ] Endpoint `POST /api/viagens/cadastrar` insere viagem e retorna o objeto
- [ ] Endpoint `GET /api/viagens/listar` retorna viagens do banco
- [ ] Endpoint `GET /api/viagens/pesquisar?origem=&destino=&data=` filtra corretamente
- [ ] Endpoint `POST /api/veiculos/cadastrar` insere veículo + gera assentos
- [ ] Endpoint `GET /api/assentos/viagem/{id}` retorna assentos do veículo da viagem
- [ ] Endpoint `POST /api/assentos/reservar` atualiza status no banco
- [ ] Endpoint `POST /api/passagens/comprar` cria passagem e atualiza assento
- [ ] Endpoint `POST /api/passagens/cancelar/{id}` cancela passagem e libera assento
- [ ] Dados persistem entre reinicializações da API

### 5.3 Segurança

- [ ] **NENHUMA** query SQL usa concatenação (`+`) ou interpolação (`$""`)
- [ ] **TODOS** os parâmetros usam `@` (parâmetros Dapper)
- [ ] Connection string **NÃO** está hardcoded no código (vem do `appsettings.json`)

### 5.4 Código

- [ ] `List<T>` estáticas foram removidas de todos os controllers
- [ ] Arquivo `db/script_tripprime.sql` criado com DDL para as 6 tabelas
- [ ] Scripts antigos (`db/sql` e `db/script.sql`) preservados sem alteração

---

## 6. Riscos e Mitigações

| Risco | Probabilidade | Impacto | Mitigação |
|-------|--------------|---------|-----------|
| Connection string incorreta → API não conecta | Média | Alto | Validar com mensagem de erro clara na inicialização se string estiver ausente ou inválida |
| Schema divergente entre script e código C# | Baixa | Alto | Script DDL gerado a partir dos modelos C# atuais (fonte de verdade) |
| Regressão no frontend Blazor | Média | Alto | Modelos de request/response NÃO serão alterados — compatibilidade total |
| Testes quebrados por mudança de dependência | Baixa | Médio | Rodar `dotnet test` ao final e corrigir se necessário |
| Projeto Supabase não criado pelo usuário | Alta | Alto | Documentar passo de criação do Supabase no README após aprovação |

---

## 7. Fora do Escopo (explicitamente)

- Configuração do Supabase (criação do projeto, obtenção da connection string) — responsabilidade do **usuário**
- Migrations automáticas
- Autenticação/autorização (JWT, Supabase Auth)
- Cache ou otimização de performance
- Logs de query ou tracing

# Plano de Pivotagem: TicketPrime → TurismoPrime

> ✅ **PIVOTAGEM CONCLUÍDA EM 10/06/2026** — Todas as 12 especificações do [`ROADMAP.md`](ROADMAP.md) foram implementadas com sucesso.
> Este documento é mantido para fins históricos e descreve o plano conceitual que guiou a execução da pivotagem.
> Para a documentação atual do sistema, consulte o [`README.md`](../../README.md) e [`docs/visao.md`](../visao.md).

## 1. Resumo da Pivotagem

**Projeto original:** TicketPrime — Sistema de venda de ingressos para eventos (shows, festivais).

**Novo produto:** TurismoPrime — Plataforma de reserva de transporte turístico (aluguel de ônibus, venda de assentos individuais, pacotes de viagem, eventos turísticos).

**Objetivo da pivotagem:** Reaproveitar ao máximo a arquitetura, o código, os endpoints e a estrutura de dados existentes, adaptando o domínio de "ingressos para eventos" para "passagens/assentos para transporte turístico".

---

## 2. Mapeamento Conceitual (Domínio Antigo → Novo Domínio)

| Conceito Original (Ingressos) | Conceito Novo (Turismo) | Explicação |
|---|---|---|
| Evento (show, festival) | Viagem / Roteiro Turístico | Um destino ou pacote de transporte com data de saída e retorno |
| Ingresso (assento no evento) | Passagem / Assento no Ônibus | Um assento numerado ou não-numerado no veículo |
| Lote de ingressos | Lote de assentos / Vagas no ônibus | Capacidade do veículo dividida em categorias (leito, semileito, convencional) |
| Organizador do evento | Operadora de Turismo / Transportadora | Empresa que oferece o serviço de transporte |
| Cupom de desconto | Cupom de desconto | Reutilizável — mesmo conceito |
| Carrinho de compras | Carrinho de reservas | Mesmo conceito |
| Checkout / Pagamento | Checkout / Pagamento | Mesmo conceito |
| Usuário / Visitante | Passageiro / Cliente | Mesmo conceito — pode ser renomeado para clareza |
| Admin | Gerente de Frota / Admin | Mesmo conceito — gerencia veículos e roteiros |

---

## 3. O que Reaproveitar (100% Aproveitável)

### 3.1. Arquitetura Base

- ✅ **Blazor Web App (WebAssembly + Server)** — mantido como frontend
- ✅ **ASP.NET Core Minimal API** — mantida como backend
- ✅ **Comunicação HTTP via `HttpClient`** — mantida
- ✅ **Estrutura de pastas do frontend** (`Pages/`, `Services/`, `Models/`, `wwwroot/`)
- ✅ **Estrutura de pastas da API** (`src/eventos/`, `src/usuarios/`, `src/cupons/`)
- ✅ **Testes com xUnit** — mantidos e expandidos

### 3.2. Funcionalidades Reaproveitáveis

| Funcionalidade | Aproveitamento | Ajuste Necessário |
|---|---|---|
| Cadastro de usuário | ✅ Integral | Renomear `Usuario` para `Passageiro` (opcional) |
| Login (esboço) | ✅ Parcial | Implementar autenticação real (JWT) |
| Listagem de "eventos" | ✅ → Listagem de viagens | Renomear modelo `Evento` para `Viagem` |
| Detalhes do "evento" | ✅ → Detalhes da viagem | Adicionar informações do veículo, itinerário |
| Cadastro de "eventos" | ✅ → Cadastro de viagens/roteiros | Novos campos (veículo, itinerário, duração) |
| Carrinho de compras | ✅ → Carrinho de reservas | Mesmo fluxo, nova nomenclatura |
| Cupons de desconto | ✅ Integral | Sem alterações |
| Checkout / Pagamento | ✅ → Checkout de passagens | Mesmo fluxo |
| "Meus ingressos" | ✅ → "Minhas passagens" | Renomear, adicionar QR Code para embarque |
| Reserva de vagas | ✅ → Reserva de assentos | Adicionar seleção de assento numerado |
| Filtros e busca | ✅ → Filtros por destino, data, tipo de veículo | Apenas renomear categorias |
| Compartilhamento | ✅ Potencial | A implementar (não foi implementado no projeto original) |

### 3.3. Código Reaproveitável

- `src/Program.cs` — estrutura de inicialização (CORS, OpenAPI, registro de endpoints)
- `src/usuarios/UsuariosController.cs` — lógica de CRUD de usuários (quase idêntica)
- `src/eventos/EventosController.cs` — lógica de CRUD, será adaptada para viagens
- `src/cupons/CuponsController.cs` — 100% reaproveitável
- `billet_2/billet_2/Program.cs` — registro de serviços, HttpClient
- `billet_2/billet_2/Services/EventoService.cs` → adaptado para `ViagemService`
- `billet_2/billet_2/Services/UsuarioService.cs` — 100% reaproveitável (se mantido `Usuario`; se renomear para `Passageiro`, adaptar referências)
- `billet_2/billet_2/Services/AuthService.cs` — 100% reaproveitável
- `billet_2/billet_2/Models/Usuario.cs` — 100% reaproveitável
- `tests/` — estrutura de testes, adaptar para novo domínio

---

## 4. O que Renomear / Adaptar

### 4.1. Modelos de Dados

**Evento (original) → Viagem (novo)**

```csharp
// ORIGINAL (Evento.cs)
public class Evento {
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Descricao { get; set; } = "";
    public string Local { get; set; } = "";
    public DateTime Data { get; set; }
    public int QuantidadeIngressos { get; set; }
    public float ValorIngresso { get; set; }
    public string? FotoUrl { get; set; }
}
```

```csharp
// NOVO (Viagem.cs)
public class Viagem {
    public int Id { get; set; }
    public string Destino { get; set; } = "";          // Antigo "Nome"
    public string Origem { get; set; } = "";            // NOVO
    public string Descricao { get; set; } = "";         // Itinerário / roteiro
    public DateTime DataSaida { get; set; }             // Antigo "Data"
    public DateTime? DataRetorno { get; set; }          // NOVO
    public int TotalAssentos { get; set; }              // Antigo "QuantidadeIngressos"
    public int AssentosDisponiveis { get; set; }        // NOVO (controle em tempo real)
    public decimal ValorPassagem { get; set; }          // Antigo "ValorIngresso" (mudar de float para decimal)
    public string? TipoVeiculo { get; set; }            // NOVO: "Leito", "Semileito", "Convencional"
    public string? FotoUrl { get; set; }
    public string? EmpresaTransporte { get; set; }      // NOVO
    public bool Ativo { get; set; } = true;
}
```

**Assento (NOVO modelo)**

> ⚠️ O modelo C# e o SQL devem ser equivalentes. Use o tipo enumerado `StatusAssento` para evitar estados conflitantes.

```csharp
public enum StatusAssento
{
    Disponivel,
    Reservado,      // Bloqueado temporariamente (checkout em andamento)
    Vendido         // Já foi pago e confirmado
}

public class Assento {
    public int Id { get; set; }
    public int ViagemId { get; set; }
    public int Numero { get; set; }
    public StatusAssento Status { get; set; } = StatusAssento.Disponivel;
    public string? Categoria { get; set; } // "Janela", "Corredor"
    public decimal? PrecoExtra { get; set; }
    public DateTime? ReservaExpiracao { get; set; } // Para assentos com status "Reservado"
}
```

### 4.2. Endpoints da API

| Rota Original | Nova Rota | Alteração |
|---|---|---|
| `/api/eventos/listar` | `/api/viagens/listar` | Renomear |
| `/api/eventos/listar/{id}` | `/api/viagens/listar/{id}` | Renomear |
| `/api/eventos/cadastrar` | `/api/viagens/cadastrar` | Renomear |
| *(novo)* | `/api/viagens/{id}/assentos` | **NOVO** — listar assentos disponíveis |
| *(novo)* | `POST /api/reservas` | **NOVO** — criar reserva (body: `{ "viagemId", "assentoId", "passageiroId" }`) |
| `/api/usuarios/listar` | `/api/passageiros/listar` | Opcional |
| `/api/usuarios/cadastrar` | `/api/passageiros/cadastrar` | Opcional |
| `/api/cupons/listar` | *(mantido)* | Sem alteração |
| `/api/cupons/cadastrar` | *(mantido)* | Sem alteração |

### 4.3. Páginas do Frontend

| Rota Original | Nova Rota | Descrição |
|---|---|---|
| `/` | `/` | Home — lista viagens disponíveis |
| `/cadastro` | `/cadastro` | Cadastro de passageiro (mantido) |
| `/login` | `/login` | Login (a ser implementado) |
| `/vendas/{id}` | `/viagem/{id}` | Detalhes da viagem + seleção de assento |
| `/meusingressos` | `/minhas-passagens` | Minhas passagens compradas |
| `/criarevento` | `/criar-viagem` | Admin: cadastrar nova viagem/roteiro |
| `/poslogin` | `/painel` | Painel do usuário logado |

---

## 5. Novas Funcionalidades a Implementar

### 5.1. Mapa de Assentos (Seleção Visual)

**Frontend:** Componente Blazor interativo mostrando um ônibus com assentos numerados.
- Assentos disponíveis (verde), ocupados (vermelho), selecionados (azul).
- O usuário clica no assento desejado.

**Backend:**
- `GET /api/viagens/{id}/assentos` — retorna lista de assentos com status
- `POST /api/reservas` — cria nova reserva (body: `{ "viagemId": 1, "assentoId": 5, "passageiroId": 1 }`)

### 5.2. Tipos de Veículo e Categorias de Assento

- **Convencional:** 50 assentos, sem reclinação, preço base
- **Semileito:** 46 assentos, reclinação parcial, +20% sobre base
- **Leito:** 40 assentos, cama individual, +60% sobre base

### 5.3. Itinerário / Roteiro

Cada viagem terá:
- **Origem** (cidade de partida)
- **Destino** (cidade de chegada)
- **Paradas intermediárias** (opcionais)
- **Duração estimada** (horas)

### 5.4. Controle de Concorrência em Assentos

- Impedir que dois usuários comprem o mesmo assento simultaneamente
- Ao selecionar um assento e iniciar o checkout, o assento muda para `Reservado` com `ReservaExpiracao = now + 15min`
- Se o checkout não for concluído em 15 minutos, o assento volta a `Disponivel`
- Estratégia de implementação sugerida: lock otimista (coluna `RowVersion` / `Timestamp`) ou transações de banco de dados

### 5.5. Geração de Passagem (QR Code)

- Após pagamento, gerar QR Code com dados da viagem e assento
- Exibir na página "Minhas Passagens" e enviar por e-mail

---

## 6. Cronograma de Implementação

### Fase 1 — Renomeação e Refatoração (Dia 1-2)

| Tarefa | Detalhes |
|---|---|
| 1.1 | Renomear `Evento` → `Viagem` em Models, Services, e Controllers |
| 1.2 | Renomear `Usuario` → `Passageiro` (opcional, manter compatibilidade) |
| 1.3 | Atualizar endpoints: `/api/eventos/*` → `/api/viagens/*` |
| 1.4 | Renomear páginas Blazor: `Venda.razor` → `ViagemDetalhes.razor` |
| 1.5 | Atualizar todos os imports e namespaces |
| 1.6 | Atualizar testes para novo domínio |

### Fase 2 — Modelo de Assentos (Dia 3-4)

| Tarefa | Detalhes |
|---|---|
| 2.1 | Criar modelo `Assento` na API |
| 2.2 | Criar endpoints: `GET /api/viagens/{id}/assentos` (listar) e `POST /api/reservas` (reservar) |
| 2.3 | Criar lógica de ocupação com controle de concorrência |
| 2.4 | Testes de unidade para o serviço de assentos |

### Fase 3 — Frontend de Assentos (Dia 5-6)

| Tarefa | Detalhes |
|---|---|
| 3.1 | Criar componente `MapaAssentos.razor` (visual do ônibus) |
| 3.2 | Integrar seleção de assento na página de detalhes da viagem |
| 3.3 | Adicionar seleção de tipo de veículo no cadastro de viagem |
| 3.4 | Atualizar CSS para o novo layout |

### Fase 4 — Fluxo de Compra (Dia 7-8)

| Tarefa | Detalhes |
|---|---|
| 4.1 | Adaptar carrinho para passagens (com assento) |
| 4.2 | Implementar checkout com reserva temporária (15 min) |
| 4.3 | Adicionar geração de QR Code na página "Minhas Passagens" (já renomeada na Fase 1) |
| 4.4 | Testes de integração do fluxo completo |

### Fase 5 — Autenticação e Admin (Dia 9-10)

| Tarefa | Detalhes |
|---|---|
| 5.1 | Implementar JWT na API (login real) |
| 5.2 | Proteger endpoints administrativos |
| 5.3 | Painel admin: gerenciar frota, viagens, relatórios |
| 5.4 | Testes de autenticação |

### Fase 6 — Banco de Dados (Dia 11-12)

| Tarefa | Detalhes |
|---|---|
| 6.1 | Revisar script SQL para novo schema (viagens, assentos) |
| 6.2 | Integrar Entity Framework Core ou Dapper |
| 6.3 | Migrar dados em memória para PostgreSQL |
| 6.4 | Testes de persistência |

---

## 7. Schema do Banco de Dados (Novo)

```sql
-- ============================================
-- Schema TurismoPrime
-- ============================================

DROP TABLE IF EXISTS Reservas;
DROP TABLE IF EXISTS Assentos;
DROP TABLE IF EXISTS Viagens;
DROP TABLE IF EXISTS Cupons;
DROP TABLE IF EXISTS Passageiros;

-- Passageiros (antigo Usuarios)
CREATE TABLE IF NOT EXISTS "Passageiros" (
    "Id" SERIAL PRIMARY KEY,
    "Nome" VARCHAR(255) NOT NULL,
    "Email" VARCHAR(255) UNIQUE NOT NULL,
    "Cpf" VARCHAR(14) UNIQUE NOT NULL,
    "Senha" TEXT NOT NULL,
    "Telefone" VARCHAR(20),
    "Adm" BOOL NOT NULL DEFAULT FALSE,
    "DataCadastro" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Viagens (antigo Eventos)
CREATE TABLE IF NOT EXISTS "Viagens" (
    "Id" SERIAL PRIMARY KEY,
    "Origem" VARCHAR(255) NOT NULL,
    "Destino" VARCHAR(255) NOT NULL,
    "Descricao" TEXT,
    "DataSaida" TIMESTAMP NOT NULL,
    "DataRetorno" TIMESTAMP,
    "TotalAssentos" INT NOT NULL,
    -- "AssentosDisponiveis" removido: calcular via COUNT dos Assentos com Status = 'Disponivel'
    "ValorPassagem" DECIMAL(10,2) NOT NULL,
    "TipoVeiculo" VARCHAR(50) NOT NULL DEFAULT 'Convencional',
    "EmpresaTransporte" VARCHAR(255),
    "FotoUrl" TEXT,
    "Ativo" BOOLEAN DEFAULT TRUE,
    "DataCriacao" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Assentos (NOVO)
-- Status: 'Disponivel', 'Reservado', 'Vendido'
CREATE TABLE IF NOT EXISTS "Assentos" (
    "Id" SERIAL PRIMARY KEY,
    "ViagemId" INT NOT NULL,
    "Numero" INT NOT NULL,
    -- Garante que não haja dois assentos com o mesmo número na mesma viagem
    UNIQUE("ViagemId", "Numero"),
    "Categoria" VARCHAR(20) NOT NULL DEFAULT 'Corredor',
    "Status" VARCHAR(20) NOT NULL DEFAULT 'Disponivel',
    "PrecoExtra" DECIMAL(10,2) DEFAULT 0,
    "ReservaExpiracao" TIMESTAMP,
    CONSTRAINT fk_viagem
        FOREIGN KEY ("ViagemId")
        REFERENCES "Viagens"("Id")
        ON DELETE CASCADE
);

-- Cupons (mantido)
CREATE TABLE IF NOT EXISTS "Cupons" (
    "Codigo" VARCHAR(50) PRIMARY KEY,
    "PorcentagemDesconto" NUMERIC(5,2) NOT NULL CHECK (PorcentagemDesconto BETWEEN 0 AND 100),
    "ValorMinimo" NUMERIC(10,2) NOT NULL CHECK (ValorMinimo >= 0),
    "Ativo" BOOLEAN DEFAULT TRUE,
    "DataCriacao" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Reservas (adaptado)
CREATE TABLE IF NOT EXISTS "Reservas" (
    "Id" SERIAL PRIMARY KEY,
    "PassageiroId" INT NOT NULL,
    "ViagemId" INT NOT NULL,
    "AssentoId" INT NOT NULL,
    "CupomUtilizado" VARCHAR(50),
    "ValorFinalPago" NUMERIC(10,2) NOT NULL CHECK (ValorFinalPago >= 0),
    "Status" VARCHAR(20) DEFAULT 'Confirmada',
    "DataReserva" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_passageiro
        FOREIGN KEY ("PassageiroId")
        REFERENCES "Passageiros"("Id")
        ON DELETE RESTRICT,
    CONSTRAINT fk_viagem_reserva
        FOREIGN KEY ("ViagemId")
        REFERENCES "Viagens"("Id")
        ON DELETE RESTRICT,
    CONSTRAINT fk_assento
        FOREIGN KEY ("AssentoId")
        REFERENCES "Assentos"("Id")
        ON DELETE RESTRICT,
    CONSTRAINT fk_cupom_reserva
        FOREIGN KEY ("CupomUtilizado")
        REFERENCES "Cupons"("Codigo")
        ON DELETE SET NULL
);

-- Índices (aspas nos nomes de colunas para consistência com as tabelas)
CREATE INDEX idx_viagens_destino ON "Viagens"("Destino");
CREATE INDEX idx_viagens_data ON "Viagens"("DataSaida");
CREATE INDEX idx_assentos_viagem ON "Assentos"("ViagemId");
CREATE INDEX idx_reservas_passageiro ON "Reservas"("PassageiroId");
CREATE INDEX idx_reservas_viagem ON "Reservas"("ViagemId");
```

---

## 8. Mapeamento de Arquivos (Antigo → Novo)

### API (`src/`)

| Arquivo Original | Ação | Novo Arquivo |
|---|---|---|
| `src/eventos/EventosController.cs` | Renomear | `src/viagens/ViagensController.cs` |
| `src/usuarios/UsuariosController.cs` | Manter ou renomear | `src/passageiros/PassageirosController.cs` |
| `src/cupons/CuponsController.cs` | Manter | *(inalterado)* |
| *(novo)* | Criar | `src/assentos/AssentosController.cs` |
| `src/Program.cs` | Atualizar | *(mesmo arquivo)* |

### Frontend (`billet_2/billet_2/`)

| Arquivo Original | Ação | Novo Arquivo |
|---|---|---|
| `Models/Evento.cs` | Renomear | `Models/Viagem.cs` |
| `Models/Usuario.cs` | Manter ou renomear | `Models/Passageiro.cs` |
| *(novo)* | Criar | `Models/Assento.cs` |
| `Services/EventoService.cs` | Renomear | `Services/ViagemService.cs` |
| `Services/UsuarioService.cs` | Manter | *(inalterado)* |
| `Pages/Venda.razor` | Renomear + adaptar | `Pages/ViagemDetalhes.razor` |
| `Pages/Meusingressos.razor` | Renomear | `Pages/MinhasPassagens.razor` |
| `Pages/Criarevento.razor` | Renomear + adaptar | `Pages/CriarViagem.razor` |
| `Pages/Home.razor` | Adaptar | *(mesmo arquivo)* |
| *(novo)* | Criar | `Components/MapaAssentos.razor` |

### Testes (`tests/`)

| Arquivo Original | Ação | Novo Arquivo |
|---|---|---|
| `TestePrecoPositivo.cs` | Adaptar | `TestePrecoPassagemPositivo.cs` |
| `TesteEventoCapacidade.cs` | Adaptar | `TesteViagemCapacidade.cs` |
| `TesteReservaValida.cs` | Adaptar | `TesteReservaAssentoValida.cs` |
| `TesteReservaVazia.cs` | Adaptar | `TesteReservaAssentoSemDados.cs` |
| `TesteDescontoValido.cs` | Manter | *(inalterado)* |

---

## 9. Regras de Negócio do Novo Produto

1. **Um CPF por passagem** — cada passageiro só pode comprar uma passagem por viagem (mesma regra do ingresso original)
2. **Bloqueio temporário** — ao selecionar um assento e iniciar o checkout, o assento muda para `Status = Reservado` por 15 minutos; expirado, volta a `Disponivel`
3. **Cancelamento** — passageiro pode cancelar até 24h antes da partida com reembolso parcial (90%)
4. **Capacidade do veículo** — cada viagem tem número fixo de assentos; venda é bloqueada quando lotar
5. **Categorias de assento** — assentos de janela podem ter valor adicional; leito custa mais que semileito e convencional
6. **Cupom por passagem** — apenas um cupom por reserva (mesma regra original)
7. **Check-in** — passageiro deve fazer check-in até 30 minutos antes da partida
8. **Admin gerencia frota** — administrador cadastra veículos, define roteiros e preços

---

## 10. Instruções para a IA Executar a Pivotagem

### Passo a passo para a IA:

1. **Analise o projeto atual** `TicketPrimeSolucao-main` — entenda a estrutura de pastas, arquivos, namespaces e relacionamentos.

2. **Siga o mapeamento da Seção 8** para renomear e adaptar cada arquivo, mantendo a estrutura de diretórios o mais próximo possível da original.

3. **Para cada arquivo renomeado:**
   - Atualize o `namespace` se necessário
   - Atualize todos os `using` e referências cruzadas
   - Mantenha a mesma lógica de negócio, apenas trocando os nomes de domínio

4. **Para cada endpoint renomeado:**
   - Mantenha a URL base `/api/` mas troque o prefixo (`eventos` → `viagens`)
   - Mantenha os mesmos métodos HTTP (GET, POST)
   - Atualize o `Program.cs` para chamar os novos métodos de extensão

5. **Corrija o arquivo SQL:** renomeie `db/sql` para `db/script.sql` (conforme recomendado em `CORRECAO.md`) e substitua o conteúdo pelo novo schema da Seção 7.

6. **Crie os novos arquivos:**
   - `src/assentos/AssentosController.cs` — endpoints para gerenciar assentos
   - `billet_2/billet_2/Models/Assento.cs` — modelo de assento
   - `billet_2/billet_2/Components/MapaAssentos.razor` — componente visual do ônibus

7. **Mantenha compatibilidade reversa** — se possível, adicione rotas antigas como redirecionamento para evitar quebrar referências.

8. **Teste cada fase** — após cada bloco de alterações, execute `dotnet build` para verificar se não há erros de compilação.

9. **Valide o fluxo completo:**
   - API rodando → Frontend conectado
   - Cadastro de viagem → Listagem → Detalhes → Seleção de assento → Checkout

---

## 11. Resumo de Esforço

| Item | Quantidade |
|---|---|
| Arquivos a renomear | ~10 |
| Arquivos a criar | ~5 |
| Arquivos a manter (inalterados) | ~8 |
| Novos endpoints | 2 (assentos) |
| Novas páginas Blazor | 1 (MapaAssentos) + adaptações |
| Novos componentes | 1 (MapaAssentos.razor) |
| Testes a adaptar | 4 |
| Testes novos recomendados | 3 (AssentoService, ReservaAssento, CheckIn) |
| **Esforço estimado total** | **12 dias / ~80 horas** |

---

## 12. Observações Finais

- O nome do diretório do projeto (`TicketPrimeSolucao-main`) e da solução (`billet_2.slnx`) podem ser mantidos ou renomeados para `TurismoPrimeSolucao-main` / `turismoprime.slnx` em uma etapa posterior.
- O código original está em .NET 10 — manter a mesma versão do SDK durante a pivotagem.
- A documentação OpenAPI se adapta automaticamente aos novos endpoints.
- Assets visuais: imagens de eventos (`show_rock.jpg`, `bonner.webp`) devem ser substituídas por imagens de destinos turísticos (praias, montanhas, cidades históricas). O vídeo `video1.mp4` (show) precisa ser trocado por um vídeo promocional de turismo. As fontes e Bootstrap podem ser mantidos.

# Changelog — TurismoPrime

> Histórico de mudanças do projeto, da pivotagem (TicketPrime → TurismoPrime) até a revisão final.

---

## [1.1.1] — 2026-06-10 — Revisão Pós-Pivotagem

### Corrigido — UI (user-facing)
- **Home.razor:** "B i L l e T *" → "TurismoPrime"; subtítulo "plataforma de vendas" → "plataforma de reservas de transporte turístico"
- **Home.razor:** CTA "seja família Billet" → "viaje com o TurismoPrime"
- **Home.razor:** Footer "Billet" → "TurismoPrime"
- **Poslogin.razor:** Navbar "BiLleT*" → "TurismoPrime"; footer "Billet" → "TurismoPrime"
- **Poslogin.razor:** Comentário "página de vendas" → "página de detalhes da viagem"
- **Cadastro.razor:** Navbar "BiLleT*" → "TurismoPrime"
- **Login.razor:** Navbar "BiLleT*" → "TurismoPrime"
- **CriarViagem.razor:** Navbar "BiLleT*" → "TurismoPrime"; footer "Billet" → "TurismoPrime"
- **MinhasPassagens.razor:** Navbar "BiLleT*" → "TurismoPrime"; footer "Billet" → "TurismoPrime"
- **ViagemDetalhes.razor:** Navbar "BiLleT*" → "TurismoPrime"; footer "Billet" → "TurismoPrime"
- **NavMenu.razor:** Marca "billet_2" → "TurismoPrime"; link quebrado `href="vendas"` → `href=""` (Viagens/Home)

### Corrigido — CSS (comentários com domínio antigo)
- **Home.razor.css:** "SEÇÃO DE EVENTOS" → "SEÇÃO DE VIAGENS"; "CARDS DE EVENTO" → "CARDS DE VIAGEM"
- **CriarViagem.razor.css:** "TELA DE CRIAR EVENTO" → "TELA DE CRIAR VIAGEM"
- **MinhasPassagens.razor.css:** "CARD DO INGRESSO" → "CARD DA PASSAGEM"
- **Poslogin.razor.css:** "GRID DE EVENTOS" → "GRID DE VIAGENS"

### Corrigido — Documentação
- **docs/visao.md:** "Estado Alvo" → "Estado Atual"; "Planejado" → "Implementado ✅"; cronograma com coluna Status
- **docs/arquitetura.md:** Cabeçalho "Pré-Pivotagem → Pós-Pivotagem"; pacotes "a instalar" → "Instalados ✅"; limitações com tabela antes/depois
- **docs/historiasdeusuario.md:** Adicionado cabeçalho ✅ PIVOTAGEM CONCLUÍDA
- **docs/pivotagem/pivotagem.md:** Adicionada nota de conclusão (10/06/2026)
- **docs/pivotagem/ADR-001-pivotagem-turismo.md:** Status "Aceito" → "Implementado ✅"; data de implementação adicionada
- **docs/pivotagem/ROADMAP.md:** Nota de conclusão (10/06/2026)
- **CORRECAO.md:** Movido para `docs/`; link quebrado corrigido; nota de atualização pós-pivotagem (5 itens corrigidos)
- **README.md:** Estrutura de pastas atualizada (+CHANGELOG, +CORRECAO); "estado alvo" → "estado atual"; documentado que apenas portas HTTP funcionam (CORS); limitações expandidas (+HTTPS, +pasta abandonada)

---

## [1.1.0] — 2026-06 — Pivotagem TicketPrime → TurismoPrime

> Execução das 12 especificações do [`ROADMAP.md`](pivotagem/ROADMAP.md).

### SP-01 — Renomeação da API

| Ação | Detalhe |
|------|---------|
| 🔄 `src/eventos/EventosController.cs` → `src/viagens/ViagensController.cs` | Renomeado + todas as referências internas |
| 🔄 `src/usuarios/UsuariosController.cs` → `src/passageiros/PassageirosController.cs` | Renomeado + todas as referências internas |
| 🔧 `src/Program.cs` | Atualizado: `Evento` → `Viagem`, `Usuario` → `Passageiro`, rotas `/api/eventos/*` → `/api/viagens/*`, `/api/usuarios/*` → `/api/passageiros/*` |
| 📁 `src/viagens/`, `src/passageiros/`, `src/assentos/` | Diretórios criados |

### SP-02 — Renomeação do Frontend

| Ação | Detalhe |
|------|---------|
| 🔄 `Models/Evento.cs` → `Models/Viagem.cs` | Classe `Viagem`: `Nome`→`Destino`, novo campo `Origem`, `Data`→`DataSaida`, +`DataRetorno`, `QuantidadeIngressos`→`TotalAssentos`, `ValorIngresso`→`ValorPassagem`, +`TipoVeiculo`, +`EmpresaTransporte`, +`Ativo` |
| 🔄 `Models/Usuario.cs` → `Models/Passageiro.cs` | Renomeado |
| 🔄 `Services/EventoService.cs` → `Services/ViagemService.cs` | Todos os métodos renomeados (`ListarEventosAsync` → `ListarViagensAsync`) |
| 🔄 `Services/UsuarioService.cs` → `Services/PassageiroService.cs` | Renomeado |
| 🔧 `Services/AuthService.cs` | `Usuario` → `Passageiro` (SP-02.7) |
| 🔄 `Pages/Venda.razor` → `Pages/ViagemDetalhes.razor` | Rota: `/vendas/{id}` → `/viagem/{id}` |
| 🔄 `Pages/Meusingressos.razor` → `Pages/MinhasPassagens.razor` | Rota: `/meusingressos` → `/minhas-passagens` |
| 🔄 `Pages/Criarevento.razor` → `Pages/CriarViagem.razor` | Rota: `/criarevento` → `/criar-viagem` |
| 🔧 `Home.razor`, `Cadastro.razor`, `Login.razor`, `_Imports.razor`, `Program.cs` | Atualizados com novas referências |

### SP-03 — Modelo de Assentos

| Ação | Detalhe |
|------|---------|
| ✨ `src/assentos/Assento.cs` | `enum StatusAssento { Disponivel, Reservado, Vendido }` + classe `Assento` |
| ✨ `Models/Assento.cs` | Modelo idêntico no frontend |
| ✨ `Models/Reserva.cs` | `int Id, ViagemId, AssentoId, PassageiroId, decimal ValorFinalPago, string Status, string? CupomUtilizado` |

### SP-04 — Endpoints de Assentos e Reservas

| Ação | Detalhe |
|------|---------|
| ✨ `src/assentos/AssentosController.cs` | `GET /api/viagens/{id}/assentos` (listar assentos), `POST /api/reservas` (criar reserva com bloqueio de 15 min), `GerarAssentosParaViagem` (helper) |
| 🔧 `src/viagens/ViagensController.cs` | Ao cadastrar viagem → gera assentos automaticamente |
| 🔧 `src/Program.cs` | Registro de `ListarAssentos()` e `CriarReserva()` |

### SP-05 — Componente MapaAssentos.razor

| Ação | Detalhe |
|------|---------|
| ✨ `Components/MapaAssentos.razor` | Grid visual do ônibus com assentos coloridos: 🟢 Disponivel, 🟡 Reservado, 🔴 Vendido |
| ✨ `Components/MapaAssentos.razor.css` | Estilos do componente |

### SP-06 — Fluxo de Compra com Reserva Temporária

| Ação | Detalhe |
|------|---------|
| ✨ `Services/ReservaService.cs` | `ReservarAssentoAsync(viagemId, assentoId)` — bloqueia assento por 15 minutos |
| 🔧 `Program.cs` | Registro `builder.Services.AddScoped<ReservaService>()` |
| 🔧 `ViagemDetalhes.razor` | Botão "Reservar Assento" → assento fica 🟡 (Reservado) por 15 min |

### SP-07 — QR Code nas Passagens

| Ação | Detalhe |
|------|---------|
| ✨ `Services/QrCodeService.cs` | Geração de QR Code via `PngByteQRCode` (Base64) |
| 🔧 `billet_2.csproj` | +`QRCoder 1.6.0`, +`System.Drawing.Common 9.0.4` |
| 🔧 `Program.cs` | Registro `builder.Services.AddSingleton<QrCodeService>()` |
| 🔧 `MinhasPassagens.razor` | Exibe QR Code após finalizar compra |

### SP-08 — Autenticação JWT

| Ação | Detalhe |
|------|---------|
| 🔧 `src/api.csproj` | +`Microsoft.AspNetCore.Authentication.JwtBearer 10.0.5` |
| 🔧 `src/Program.cs` | 3 blocos: usings JWT + `AddAuthentication` + `UseAuthentication/UseAuthorization` |
| 🔧 `src/passageiros/PassageirosController.cs` | `POST /api/auth/login` — retorna `{ token, passageiro }`; método `GerarToken` com claims |

### SP-09 — Banco de Dados

| Ação | Detalhe |
|------|---------|
| 🔄 `db/sql` → `db/script.sql` | Renomeado com extensão `.sql` |
| 🔧 `db/script.sql` | Schema TurismoPrime: `Passageiros`, `Viagens`, `Assentos`, `Cupons`, `Reservas` + 5 índices |
| 🔧 `src/api.csproj` | +`Dapper 2.1.35`, +`Npgsql 9.0.3` |
| 🔧 `src/appsettings.json` | +`ConnectionStrings:DefaultConnection` (PostgreSQL) |

### SP-10 — Testes

| Ação | Detalhe |
|------|---------|
| 🔄 `TestePrecoPositivo.cs` → `TestePrecoPassagemPositivo.cs` | Adaptado para `Viagem` |
| 🔄 `TesteEventoCapacidade.cs` → `TesteViagemCapacidade.cs` | Adaptado para `Viagem` |
| 🔄 `TesteReservaValida.cs` → `TesteReservaAssentoValida.cs` | Adaptado para `Reserva` + `Assento` |
| 🔄 `TesteReservaVazia.cs` → `TesteReservaAssentoSemDados.cs` | Adaptado para `Reserva` + `Assento` |
| ✅ `TesteDescontoValido.cs` | Mantido (cupons inalterados) |
| ✨ `TesteAssentoService.cs` | Novo — testa `StatusAssento` e regras de assento |
| ✨ `TesteReservaComAssento.cs` | Novo — testa fluxo de reserva |
| ✨ `TesteCheckInPassageiro.cs` | Novo — testa check-in do passageiro |
| 🔧 `MeuProjeto.Tests.csproj` | +`ProjectReference` para `src/api.csproj` |
| **Resultado:** 8 classes, **14 testes passando** (`dotnet test` ✅) |

### SP-11 — Assets Visuais

| Ação | Detalhe |
|------|---------|
| 🔄 `wwwroot/images/eventos/` → `wwwroot/images/destinos/` | Pasta renomeada |
| 🔧 `.razor` e `.csproj` | Todas as referências a `images/eventos/` atualizadas para `images/destinos/` |

### SP-12 — Documentação

| Ação | Detalhe |
|------|---------|
| 🔧 `README.md` | Reescrito para TurismoPrime: estrutura de pastas, endpoints, páginas, funcionalidades implementadas |
| 🔧 `docs/historiasdeusuario.md` | 24 histórias adaptadas: `Evento`→`Viagem`, `Usuario`→`Passageiro`, `Ingresso`→`Passagem` |
| 🔧 `src/api.http` | 9 exemplos de endpoints atualizados (passageiros, auth, viagens, assentos, reservas, cupons) |

---

## [1.0.0] — 2026-05 — TicketPrime (Versão Original)

### Funcionalidades Iniciais
- CRUD de eventos (shows, festivais) via Minimal API
- Cadastro e login local de usuários (CPF + senha, sem JWT)
- Cupons de desconto
- Carrinho de compras (sem checkout real)
- Frontend Blazor Web App (Server + WebAssembly)
- 5 testes unitários xUnit
- Persistência em listas em memória (`List<T>`)

### Limitações Originais
- Sem autenticação JWT
- Sem mapa de assentos
- Sem QR Code
- Sem banco de dados (script SQL sem extensão `.sql`)
- Sem fluxo de checkout completo
- Solution (`billet_2.slnx`) não incluía API nem testes

---

> **Legenda:** ✨ Criado &nbsp; 🔧 Modificado &nbsp; 🔄 Renomeado &nbsp; ✅ Mantido

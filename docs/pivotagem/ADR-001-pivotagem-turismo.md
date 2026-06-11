# ADR-001: Pivotagem de TicketPrime (Ingressos) para TurismoPrime (Turismo/Transporte)

| Campo | Valor |
|---|---|
| **ID** | ADR-001 |
| **Status** | Implementado ✅ |
| **Data** | 2026-05-14 |
| **Última revisão** | 2026-06-10 |
| **Data de implementação** | 2026-06-10 |
| **Autores** | Gabriel Castor (06009642), Gabriel Lepsch Monteiro (02001770), Gabriel Ribeiro (06010603), Lucas Oliveira (06010486), Luiz Eduardo P. Rosa (06010412), Thiago Zandonade Fernandes (06010263) |
| **Domínio anterior** | Venda de ingressos para eventos (shows, festivais) |
| **Novo domínio** | Turismo e transporte (aluguel de ônibus, assentos, pacotes turísticos) |

---

## Contexto

O projeto **TicketPrime** foi originalmente concebido como uma plataforma de venda de ingressos para eventos culturais (shows, festivais). Durante o desenvolvimento, a equipe identificou as seguintes motivações para uma pivotagem:

1. **Mercado de ingresso saturado** — O mercado de plataformas de venda de ingressos já é dominado por players estabelecidos (Sympla, Ingresse, Eventim), tornando a diferenciação competitiva difícil para um projeto acadêmico.

2. **Baixa frequência de compra** — Ingressos para shows têm baixa recorrência de compra por usuário (algumas vezes ao ano), o que limita o engajamento e a retenção na plataforma.

3. **Complexidade logística** — Eventos exigem integração com múltiplos parceiros (organizadores, locais, bilheterias), aumentando a complexidade do MVP.

4. **Oportunidade no turismo regional** — O mercado de transporte turístico (aluguel de ônibus, venda de assentos individuais para viagens) é menos digitalizado e carece de plataformas modernas de reserva.

5. **Reaproveitamento máximo do código existente** — A arquitetura de "eventos com assentos" mapeia quase perfeitamente para "viagens com assentos de ônibus", permitindo reaproveitar ~80% do código.

6. **Maior recorrência de uso** — Viagens turísticas têm maior frequência de compra que shows (viagens de feriado, férias, fins de semana), gerando mais engajamento.

---

## Decisão

Pivotar o produto **TicketPrime** (venda de ingressos) para **TurismoPrime** (plataforma de reserva de transporte turístico).

### O que muda

| Aspecto | Antes (TicketPrime) | Depois (TurismoPrime) |
|---|---|---|
| Domínio | Ingressos para eventos culturais | Passagens para transporte turístico |
| Entidade principal | `Evento` (show, festival) | `Viagem` (roteiro turístico) |
| Unidade de venda | Ingresso (assento numerado ou não) | Passagem (assento numerado no ônibus) |
| Público-alvo | Público geral (espectadores) | Viajantes, turistas, grupos |
| Diferencial | Catálogo de eventos | Mapa de assentos + tipos de veículo |
| Recorrência | Baixa (eventos sazonais) | Média (viagens em feriados/férias) |

### O que permanece

- **Arquitetura:** Blazor Web App (WASM + Server) + ASP.NET Core Minimal API
- **Linguagem:** C# .NET 10
- **Autenticação:** Mesmo modelo de usuário (adaptado para `Passageiro`)
- **Cupons de desconto:** Idênticos
- **Carrinho e checkout:** Mesmo fluxo
- **Testes:** Mesma estrutura xUnit
- **Banco de dados:** PostgreSQL com schema adaptado

---

## Consequências

### Positivas

1. **Reaproveitamento de ~80% do código** — toda a camada de usuários, cupons, carrinho, checkout e estrutura de API é diretamente reaproveitável.
2. **Menor time-to-market** — a base já está pronta; a pivotagem requer principalmente renomeação e adição do módulo de assentos.
3. **Diferenciação clara** — poucas plataformas oferecem mapa de assentos + tipos de veículo (leito, semileito, convencional) + venda de assentos individuais para ônibus de turismo.
4. **Escalabilidade futura** — o mesmo modelo pode ser estendido para: venda de pacotes completos (transporte + hospedagem), aluguel de ônibus inteiros, fretamento corporativo.
5. **Domínio mais simples** — menos atores envolvidos (apenas transportadora e passageiro) comparado ao ecossistema de eventos (organizador, casa de show, bilheteria, fiscalização).

### Negativas

1. **Renomeação generalizada** — `Evento` → `Viagem`, `Usuario` → `Passageiro`, `Ingresso` → `Passagem` exigirá alterações em ~10 arquivos e seus respectivos namespaces/testes.
2. **Novo módulo de assentos** — o mapeamento visual de assentos do ônibus (MapaAssentos.razor) é uma funcionalidade inteiramente nova que precisa ser implementada do zero.
3. **Assets visuais obsoletos** — imagens de shows, festival e o vídeo promocional precisam ser substituídos por conteúdo de turismo.
4. **Documentação desatualizada** — README, histórias de usuário e documentação OpenAPI precisam refletir o novo domínio.
5. **Marca a ser renomeada** — "TicketPrime" não faz mais sentido; a solução (`billet_2.slnx`) e o repositório devem ser renomeados para refletir o novo produto.

### Riscos

| Risco | Probabilidade | Impacto | Mitigação |
|---|---|---|---|
| Quebra de compatibilidade nas rotas da API | Média | Alto | Adicionar redirecionamento 301 das rotas antigas para as novas |
| Erros de renomeação em imports/namespaces | Alta | Médio | Executar `dotnet build` após cada alteração |
| Concorrência na venda do mesmo assento | Média | Alto | Implementar lock otimista (RowVersion) ou transações |
| Usuário não aceitar a mudança de domínio | Baixa | Baixo | Produto acadêmico sem base de usuários ativos |

---

## Alternativas Consideradas

### Alternativa 1: Manter TicketPrime como está

- **Prós:** Nenhum retrabalho; documento de correção (CORRECAO.md) já avalia o projeto em 8/10.
- **Contras:** Projeto sem diferencial competitivo; mercado saturado; baixo potencial de crescimento.
- **Decisão:** Rejeitada — pivotagem agrega mais valor acadêmico e prático.

### Alternativa 2: Pivotar para outro segmento (ex: alimentação, coworking)

- **Prós:** Outros segmentos também poderiam usar a mesma arquitetura.
- **Contras:** Alimentação exige cardápio/horários (arquitetura diferente); coworking exige gestão de salas (muito distante do modelo atual).
- **Decisão:** Rejeitada — turismo/transporte é o segmento com maior overlap semântico com o código existente (eventos → viagens, ingressos → assentos).

### Alternativa 3: Criar um novo projeto do zero

- **Prós:** Sem dívida técnica; arquitetura sob medida.
- **Contras:** Perda de todo o trabalho já realizado; prazo inviável para entrega acadêmica.
- **Decisão:** Rejeitada — reaproveitar o código existente é o caminho mais eficiente.

---

---

## Justificativas das Decisões de Arquitetura

> As decisões abaixo estão documentadas em detalhe no [`docs/arquitetura.md`](../arquitetura.md) e no [`ROADMAP.md`](ROADMAP.md). Este resumo explica o **porquê** de cada escolha tecnológica.

### 1. .NET 10

| Decisão | Motivo |
|---------|--------|
| **Versão 10** (Latest) | O projeto foi iniciado quando o .NET 10 estava em preview/RC. A opção pela versão mais recente garante acesso a todas as features da linguagem C# 13 (coleções imutáveis, parâmetros `params` genéricos, lambdas com atributos), melhor performance (`JIT` otimizado), e suporte contínuo da Microsoft. |
| **LTS vs STS** | .NET 10 é **STS** (Standard Term Support — 18 meses). Para um projeto acadêmico com entrega em 2026, isso é suficiente. Não há necessidade de ciclo LTS de 3 anos. |
| **Alternativa rejeitada** | .NET 8/9 — versões anteriores foram descartadas pois .NET 10 já estava disponível e oferece melhorias de performance no GC e no ASP.NET Core. |

### 2. ASP.NET Core Minimal API

| Decisão | Motivo |
|---------|--------|
| **Minimal API em vez de Controllers tradicionais** | O projeto tem poucos endpoints (~10), sem necessidade de separação por áreas complexas. Minimal API reduz boilerplate (sem `ControllerBase`, sem classes de controller separadas para cada recurso), resultando em menos código para manter e renomear durante a pivotagem. |
| **Sem Swagger UI** | OpenAPI é gerado via `Microsoft.AspNetCore.OpenApi` 10.0.5 e exposto em `/openapi/v1.json`. Não foi adicionado Swagger UI para manter o projeto mais leve — as requisições são testadas via `api.http` (arquivo de exemplos). |
| **Alternativa rejeitada** | Controllers tradicionais com `[ApiController]` — mais verbosos, mais arquivos, mais renomeações necessárias na pivotagem. |

### 3. Blazor Web App (Server + WebAssembly Interativo)

| Decisão | Motivo |
|---------|--------|
| **Blazor Server + WASM** | O projeto usa **modo interativo misto**: o servidor renderiza a maior parte (menor download inicial), enquanto páginas específicas como o mapa de assentos (`MapaAssentos.razor`) podem usar WebAssembly para interatividade rica sem latência de SignalR. |
| **Componentes em C#** | Diferente de React/Angular/Vue, o Blazor permite escrever componentes frontend **na mesma linguagem do backend** (C#), eliminando a barreira de contexto entre equipes. Ideal para um time acadêmico focado em .NET. |
| **Bootstrap 5 estático** | O CSS está em `wwwroot/lib/bootstrap/` (arquivos baixados, não via CDN). Isso permite que o frontend funcione offline e evita dependência externa durante desenvolvimento. |
| **Alternativa rejeitada** | React + Node.js — exigiria conhecimento de TypeScript/JS e manter dois ecossistemas distintos. Impraticável para um time de 6 pessoas com prazo acadêmico. |

### 4. xUnit

| Decisão | Motivo |
|---------|--------|
| **xUnit em vez de MSTest ou NUnit** | xUnit é o framework de teste mais adotado no ecossistema .NET moderno (utilizado pela própria Microsoft nos repositórios oficiais do ASP.NET Core). Suporta `[Theory]` com `[InlineData]` para testes parametrizados, tem melhor integração com `dotnet test` e é mais extensível que MSTest. |
| **Sem mocking** | Os testes atuais são funcionais/unitários puros (sem Mock, sem banco). Para um projeto acadêmico, testar regras de negócio isoladamente é suficiente. Se no futuro for necessário mockar HttpClient, o xUnit se integra bem com Moq ou NSubstitute. |
| **coverlet.collector** | Usado para cobertura de código, integrado nativamente ao `dotnet test --collect:"XPlat Code Coverage"`. |

### 5. PostgreSQL + Dapper

| Decisão | Motivo |
|---------|--------|
| **PostgreSQL** | Banco relacional open-source maduro, com excelente suporte a tipos JSON, array, e operações geográficas (PostGIS — útil se no futuro o sistema precisar de rotas/distâncias). O script DDL (`db/sql`) já está preparado para PostgreSQL. |
| **Dapper em vez de Entity Framework** | Dapper é um micro-ORM que oferece performance próxima de ADO.NET puro (~50% mais rápido que EF Core em queries complexas). Para um sistema com poucas entidades (~6 tabelas), o overhead do EF Core (change tracking, migrations) não se justifica. Dapper também é mais explícito: cada consulta é SQL puro, sem abstrações que escondem o plano de execução. |
| **Integração opcional (SP-09)** | O banco de dados é **opcional** — o sistema roda perfeitamente com listas em memória. O ROADMAP coloca SP-09 como spec de baixa prioridade justamente por isso. As listas em memória são voláteis (perdem dados ao reiniciar), mas suficientes para demonstração acadêmica. |
| **Alternativa rejeitada** | Entity Framework Core + SQLite — EF adicionaria complexidade desnecessária; SQLite não é adequado para o ambiente de produção imaginado. |

### 6. Autenticação JWT (JSON Web Tokens)

| Decisão | Motivo |
|--------|--------|
| **JWT sem Identity** | O projeto tem autenticação simples (email + senha). O ASP.NET Core Identity adicionaria tabelas de usuário, roles, claims, tokens de refresh — complexidade que não agrega valor para um MVP acadêmico. O JWT é implementado manualmente no `PassageirosController.cs` com `System.IdentityModel.Tokens.Jwt`. |
| **Package `JwtBearer`** | O middleware `Microsoft.AspNetCore.Authentication.JwtBearer` 10.0.5 é adicionado em SP-08 para validar tokens nas requisições protegidas. Sem ele, qualquer cliente poderia forjar tokens. |
| **Chave simétrica** | A chave de assinatura é armazenada em `appsettings.json` (não ideal para produção, mas aceitável para ambiente acadêmico). Em produção, usaria-se variáveis de ambiente ou Azure Key Vault. |
| **Alternativa rejeitada** | ASP.NET Core Identity — adiciona ~8 tabelas ao banco para cenário que precisa apenas de email+hash+token. |

### 7. QR Code (QRCoder + System.Drawing.Common)

| Decisão | Motivo |
|--------|--------|
| **QRCoder 1.6.0** | Biblioteca pura em C# para geração de QR Codes, sem dependência de serviços externos. Gera QR Codes em memória como bitmap, que é convertido para Base64 e embutido diretamente na página HTML/Blazor. |
| **System.Drawing.Common 9.0.4** | O QRCoder gera o QR Code como `Bitmap` (System.Drawing). A partir do .NET 6, o `System.Drawing.Common` só funciona no Windows, a menos que seja instalada a versão 9.0+ que roda em Linux via libgdiplus. Para este projeto (desenvolvido em Windows), a versão 9.0.4 é suficiente. |
| **Por que não serviço externo?** | Google Charts API, QRServer, ou APIs pagas exigiriam conexão com internet e adicionariam latência. Geração local é mais rápida, funciona offline e não tem custo. |
| **Alternativa rejeitada** | `QRCoder.Ascii` — gera QR Code em caracteres ASCII (terminal), não em imagem. Inviável para exibição numa página web. |

### 8. Arquitetura de Portas

| Decisão | Motivo |
|--------|--------|
| **API na porta 5289** | Porta arbitrária de baixo número, sem conflito com serviços comuns (3000, 5000, 8000). Configurada em `src/Properties/launchSettings.json`. |
| **Frontend na porta 5096** | Porta diferente da API para evitar conflito ao rodar ambos localmente. Configurada em `billet_2/billet_2/Properties/launchSettings.json`. |
| **CORS `http://localhost:5096`** | O navegador bloqueia requisições cross-origin (API em :5289, Frontend em :5096). A política `BlazorPolicy` libera apenas a origem exata do frontend, seguindo o princípio de privilégio mínimo. |

### 9. Estrutura de Diretórios

| Decisão | Motivo |
|--------|--------|
| **`src/` separado de `billet_2/`** | Separação clara entre backend e frontend. Cada um tem seu próprio `Program.cs`, `csproj`, e ciclo de build. A solution (`billet_2.slnx`) referencia apenas o frontend porque o backend é autossuficiente. |
| **Controllers em pastas por domínio** | `src/eventos/`, `src/usuarios/`, `src/cupons/` — organização por domínio (não por camada técnica), facilitando a navegação e a renomeação durante a pivotagem (basta renomear a pasta e o arquivo). |
| **`tests/` separado** | Projeto de testes independente, sem referência circular. Compila e executa isoladamente com `dotnet test`. |

### 10. Solução `billet_2.slnx`

| Decisão | Motivo |
|--------|--------|
| **`.slnx` (formato XML) em vez de `.sln` (formato legado)** | O Visual Studio 2022 e o .NET CLI suportam nativamente o formato `.slnx` desde o .NET 9. É mais legível, mais fácil de fazer merge em git, e menos propenso a conflitos que o formato `.sln` binário/legado. |
| **Apenas o frontend na solution** | A solution foi criada originalmente apontando apenas para o frontend Blazor. A API (`src/`) e os Testes (`tests/`) não foram incluídos. Isso significa que `dotnet build` na solution não compila tudo — é necessário compilar cada projeto individualmente. Manter essa estrutura é aceitável para o escopo acadêmico, mas em produção recomenda-se adicionar todos os projetos à solution. |

### 11. Versionamento com Git

| Decisão | Motivo |
|--------|--------|
| **Git sem branches** | O repositório usa apenas `main` (sem `dev`, `feature`, etc.). Para um projeto acadêmico com 6 pessoas, o fluxo simplificado reduz a complexidade. Cada spec do ROADMAP inclui instrução de rollback via `git restore`. |
| **`.gitignore` padrão do .NET** | Ignora `bin/`, `obj/`, `.user`, `.suo` — arquivos gerados que não devem ser versionados. |

---

## Referências

- [`docs/arquitetura.md`](../arquitetura.md) — Requisitos de tecnologias, ferramentas e programas por spec
- [`docs/pivotagem/pivotagem.md`](pivotagem.md) — Plano detalhado da pivotagem
- [`docs/pivotagem/ROADMAP.md`](ROADMAP.md) — Guia de execução das 12 specs
- [`docs/historiasdeusuario.md`](../historiasdeusuario.md) — Histórias de usuário originais (a serem adaptadas)
- [`CORRECAO.md`](../../CORRECAO.md) — Correção da avaliação AV1
- [`README.md`](../../README.md) — Documentação original do TicketPrime

# Release Checklist Final

> Projeto: TripPrime
> Data: 30/06/2026

## Checkpoints de Entrega

- [x] **Fundamentos** — Código compila sem erros nos projetos `src` (backend) e `billet_2/billet_2` (frontend). A API registra todas as rotas (Viagens, Veículos, Assentos, Passagens, Usuários, Cupons) e o frontend injeta todos os serviços necessários.
- [x] **Produto Mínimo** — CRUD de viagens e veículos implementado e funcional. Usuários podem se cadastrar, fazer login, visualizar catálogo de viagens, selecionar assentos e comprar passagens. Administradores podem criar viagens e veículos.
- [x] **Evidência de Qualidade** — Testes automatizados com padrão AAA e nomenclatura `Metodo_Cenario_ResultadoEsperado`. Três testes unitários (desconto, valor final, reserva sem usuário) compilam e passam com `dotnet test`.
- [x] **Decisões Documentadas** — ADR criado em `/docs/adrs/001-escolha-do-micro-orm.md` documentando a decisão de usar Dapper e proibir EF Core. Registro de 8 dívidas técnicas com priorização em `/docs/registro_divida_tecnica.md`.
- [x] **Evidência de Requisitos** — Todos os endpoints implementados conforme especificações do roadmap: 2+ endpoints com regras de negócio (`ComprarPassagem`, `CancelarPassagem`), 1+ endpoint com 3+ validações (`CadastrarViagens` com 6, `CadastrarVeiculos` com 6).
- [x] **Governança** — Matriz de riscos com 6 riscos e gatilhos, métricas de fluxo (Lead Time) e qualidade (Change Failure Rate), SLO da rota crítica (99.5%) e Error Budget Policy com 3 níveis em `/docs/operacao.md`.
- [x] **Segurança** — Threat model para a rota `POST /api/viagens/cadastrar`, 3 gates de segurança (análise estática, dependências, testes de segurança) em `/docs/seguranca_ciclo.md`. Nenhuma credencial hardcoded encontrada no código-fonte.

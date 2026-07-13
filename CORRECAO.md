# Correção AV2 — TicketPrimeSolucao (TripPrime)

**Grupo:** Gabriel Castor, Gabriel Lepsch Monteiro, Gabriel Ribeiro, Lucas Oliveira, Luiz Eduardo P. Rosa, Thiago Zandonade Fernandes

| # | Item de Avaliação | Nota | Justificativa |
|---|-------------------|:----:|---------------|
| 01 | Padrão AAA nos Testes | 0,5 | 5 testes em `tests/` com `// Arrange`, `// Act`, `// Assert`; testes com [Theory] + [InlineData] e [Fact] |
| 02 | Nomenclatura e Independência | 0,5 | Padrão `Metodo_Cenario_ResultadoEsperado` em todos os métodos (ex: `ValidarDesconto_QuandoForaDoIntervalo_NaoDeveSerValido`); zero condicionais |
| 03 | Padrões Arquiteturais | 0,5 | 3 cenários com `Positivo:`/`Negativo:` em `docs/analise_arquitetura.md` |
| 04 | Violações Arquiteturais | 0,5 | 6 violações com `**Problema:**`, `**Evidência:**`, `**Impacto:**`, `**Ação Recomendada:**` |
| 05 | ADR | 0,5 | `docs/adr.md` com ADR-001 (Dapper obrigatório, EF Core proibido); Contexto, Decisão, Consequências, Status: Aceito; + pasta `docs/adrs/` |
| 06 | Dívida Técnica | 0,5 | 8 dívidas (DT-001 a DT-008) com colunas ID, Descrição, Freq. Alteração, Risco, Esforço, Decisão |
| 07 | Priorização Dívida | 0,5 | P1 (DT-001, DT-002), P2 (DT-003/004/005), P3 (DT-006, DT-007, DT-008) |
| 08 | Classificação Manutenção | 0,5 | 12 tickets classificados: Corretiva (1,4,7,11), Adaptativa (2,6,9), Perfectiva (3,5), Preventiva (8,10,12) |
| 09 | Pipeline de Liberação | 0,5 | 4 passos: Análise de Impacto, Teste Cirúrgico, Feature Toggle, Estratégia de Release |
| 10 | Plano de Iteração | 0,5 | Objetivo, Escopo (14 US), Entregáveis, Risco Principal, DoD preenchidos |
| 11 | Quadro Kanban e WIP | 0,5 | 4 colunas + WIP máximo = 4 (<= 6 integrantes) |
| 12 | Matriz de Riscos | 0,5 | 5 riscos com Probabilidade, Impacto, Estratégia, Ação Planejada |
| 13 | Gatilhos de Risco | 0,5 | Todos os gatilhos com >=20 caracteres descrevendo evento observável |
| 14 | Métrica DORA | 0,5 | "Lead Time for Changes" com 7 campos completos |
| 15 | Métrica de Qualidade | 0,5 | "Change Failure Rate" com 7 campos completos |
| 16 | SLO | 0,5 | SLI, Fórmula, Fonte, Janela (7 dias), Alvo (99.5%) para `POST /api/passagens/comprar` |
| 17 | Error Budget Policy | 0,5 | 3 níveis graduados; Nível 3 com "Feature Freeze total" e "Zero novas funcionalidades" |
| 18 | Segurança SSDF | 0,5 | Nenhuma credencial hardcoded nos 14 `.cs` do `/src`; `Program.cs` usa `builder.Configuration.GetConnectionString` |
| 19 | Threat Model e Gates | 0,5 | Ativos, Vetor, Falha, Mitigação + Gate 1 (SAST), Gate 2 (DAST), Gate 3 (Revisão Manual) |
| 20 | Topologia Times e DoD | 0,5 | 4 tipos Team Topologies + `release_checklist_final.md` com 7 `[x]` detalhados |

**Nota Final: 10,0 / 10,0**

---

**Observações:**
- Projeto completamente reescrito após force push no remoto — pivotagem de TicketPrime para TripPrime (venda de ingressos → passagens de transporte).
- Documentação excepcionalmente completa: 15 arquivos em `docs/` cobrindo todos os itens da AV2 com profundidade e exemplos.
- Código estruturado com 5 controllers (Viagens, Veículos, Assentos, Passagens, Usuários, Cupons), 19 endpoints registrados, DbConnectionFactory com Dapper.
- Testes seguem rigorosamente o padrão AAA e nomenclatura underscore.
- Release checklist com autoavaliação precisa e honesta sobre limitações conhecidas.

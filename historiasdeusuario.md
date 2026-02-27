

| ID | Persona | História de Usuário (User Story) | Critérios de Pronto (DoD) |
| :--- | :--- | :--- | :--- |
| **01** | Visitante | Ver eventos sem autenticação para decidir sobre cadastro/compra. | • Lista de eventos visível na Home.<br>• Botões de compra redirecionam para login.<br>• Informações básicas (data, local) acessíveis. |
| **02** | Visitante | Criar um acesso para usufruir dos benefícios de usuário cadastrado. | • Formulário de cadastro funcional.<br>• Validação de campos obrigatórios e e-mail único.<br>• Feedback de sucesso após criação da conta. |
| **03** | Usuário | Impedir a compra de múltiplos ingressos por CPF para evitar duplicidade. | • Verificação de CPF no banco de dados antes do checkout.<br>• Bloqueio do botão de compra se o CPF já possuir ingresso.<br>• Mensagem informativa sobre o limite de 1 por CPF. |
| **04** | Usuário | Aba de eventos disponíveis para facilitar a escolha de ingressos. | • Aba "Eventos" visível no menu principal.<br>• Filtros de busca ou categorias funcionando.<br>• Cards com resumo do evento e preço. |
| **05** | Usuário | Tela de seleção de eventos para visualização otimizada. | • Interface responsiva (Mobile/Desktop).<br>• Carregamento rápido das imagens e descrições.<br>• Fluxo intuitivo de "Ver Detalhes" para "Carrinho". |
| **06** | Usuário | Campo de cupons na tela de pagamento para obter descontos. | • Input para código do cupom no checkout.<br>• Cálculo automático do desconto no valor final.<br>• Validação de cupom expirado ou inexistente. |
| **07** | Usuário | Área de reservas para garantir entrada em eventos futuros. | • Opção "Reservar" em eventos selecionados.<br>• Confirmação de reserva enviada por e-mail.<br>• Painel de "Minhas Reservas" com status atualizado. |
| **08** | Usuário | Visualizar ingressos adquiridos para garantir a entrada no evento. | • Tela "Meus Ingressos" acessível via perfil.<br>• Exibição de QR Code ou código de barras.<br>• Opção de download (PDF) ou impressão. |
| **09** | Admin | Criar novos eventos para disponibilizar ingressos. | • Painel administrativo com formulário de criação.<br>• Upload de imagens e definição de valores/datas.<br>• Publicação imediata ou agendada no catálogo. |
| **10** | Admin | Limitar número de ingressos para evitar superlotação. | • Campo de "Estoque Total" no cadastro do evento.<br>• Interrupção automática das vendas ao atingir o limite.<br>• Exibição de "Esgotado" na interface do usuário. |
| **11** | Admin | Limitar uso de apenas 1 cupom por ingresso para evitar prejuízos. | • Regra de negócio que impede acumular códigos.<br>• Substituição de cupom caso o usuário insira um novo.<br>• Aviso de "Limite de 1 cupom por pedido" no checkout. |

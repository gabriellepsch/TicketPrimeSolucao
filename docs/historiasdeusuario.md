# Histórias de Usuário — TurismoPrime

> ✅ **PIVOTAGEM CONCLUÍDA** — Estas 24 histórias de usuário foram adaptadas do TicketPrime (venda de ingressos para eventos) para o domínio do TurismoPrime (reserva de passagens de transporte turístico).
> A adaptação consistiu em renomear `Evento` → `Viagem`, `Usuario` → `Passageiro`, `Ingresso` → `Passagem`, e incorporar novas funcionalidades como mapa de assentos, QR Code e reserva temporária.
> Para o registro completo da pivotagem, consulte [`docs/pivotagem/ROADMAP.md`](pivotagem/ROADMAP.md).

| ID | História de Usuário | Critérios de Pronto (DoD) |
|----|---------------------|---------------------------|
| 01 | Como visitante, quero visualizar viagens sem precisar fazer login para decidir se vale a pena me cadastrar ou comprar uma passagem. | • Lista de viagens visível na página inicial.<br>• Informações básicas (destino, data, origem) exibidas.<br>• Botão de compra redireciona para login ou cadastro. |
| 02 | Como visitante, quero criar uma conta na plataforma para acessar os benefícios de passageiro cadastrado e poder comprar passagens. | • Formulário de cadastro funcional.<br>• Validação de campos obrigatórios.<br>• Verificação de e-mail único.<br>• Mensagem de sucesso após cadastro. |
| 03 | Como passageiro cadastrado, quero realizar login na plataforma para acessar minha conta e minhas passagens. | • Tela de login disponível.<br>• Validação de e-mail e senha.<br>• Redirecionamento para área logada após autenticação. |
| 04 | Como passageiro cadastrado, quero recuperar minha senha caso eu a esqueça para conseguir acessar minha conta novamente. | • Opção "Esqueci minha senha".<br>• Envio de link de recuperação por e-mail.<br>• Possibilidade de redefinir senha com segurança. |
| 05 | Como usuário, quero pesquisar viagens por destino para encontrar rapidamente viagens do meu interesse. | • Campo de busca disponível.<br>• Resultados exibidos conforme termo pesquisado.<br>• Atualização da lista de viagens com base na pesquisa. |
| 06 | Como usuário, quero filtrar viagens por data, tipo de veículo ou destino para facilitar a busca por viagens relevantes. | • Filtros disponíveis na página de viagens.<br>• Aplicação dos filtros atualiza os resultados exibidos.<br>• Possibilidade de limpar filtros aplicados. |
| 07 | Como usuário, quero acessar uma tela de detalhes da viagem para visualizar informações como origem, destino, data, preço e tipo de veículo. | • Página de detalhes da viagem disponível.<br>• Exibição de origem, destino, descrição e preço.<br>• Botão para selecionar assento e comprar passagem. |
| 08 | Como usuário, quero compartilhar viagens para convidar amigos ou divulgar destinos de interesse. | • Botões de compartilhamento disponíveis.<br>• Geração de link direto para a viagem.<br>• Compatibilidade com redes sociais ou aplicativos de mensagem. |
| 09 | Como usuário, quero selecionar assentos e adicionar passagens ao carrinho de compras para realizar a compra posteriormente. | • Mapa de assentos interativo disponível.<br>• Botão "Reservar Assento" disponível.<br>• Atualização do carrinho com itens selecionados. |
| 10 | Como passageiro cadastrado, quero finalizar a compra da passagem para confirmar minha reserva. | • Página de checkout disponível.<br>• Exibição do resumo da compra.<br>• QR Code gerado após confirmação.<br>• Confirmação da compra com passagem digital. |
| 11 | Como passageiro cadastrado, quero receber minhas passagens com QR Code para apresentar no embarque. | • Geração de QR Code após confirmação de compra.<br>• QR Code exibido na tela de Minhas Passagens.<br>• Dados da viagem e assento codificados no QR. |
| 12 | Como passageiro cadastrado, quero visualizar minhas passagens adquiridas para garantir meu embarque na viagem. | • Tela "Minhas Passagens" acessível no perfil.<br>• Listagem das passagens compradas.<br>• Exibição de QR Code para validação. |
| 13 | Como passageiro cadastrado, quero que o sistema limite a compra de uma passagem por CPF por viagem para evitar duplicidade de compras. | • Verificação de CPF no banco de dados.<br>• Bloqueio de nova compra se já existir passagem vinculada.<br>• Mensagem informativa ao passageiro. |
| 14 | Como usuário, quero acessar uma aba de viagens disponíveis para facilitar a escolha de destinos. | • Aba "Viagens" visível no menu principal.<br>• Lista de viagens carregada corretamente.<br>• Navegação para detalhes da viagem. |
| 15 | Como usuário, quero visualizar as viagens em uma interface otimizada para facilitar a navegação e escolha das passagens. | • Interface responsiva (mobile e desktop).<br>• Cards com imagem, destino e preço da viagem.<br>• Navegação intuitiva entre viagens. |
| 16 | Como passageiro cadastrado, quero inserir cupons de desconto na tela de pagamento para reduzir o valor da passagem quando disponível. | • Campo para inserir cupom no checkout.<br>• Validação do cupom.<br>• Atualização automática do valor total. |
| 17 | Como passageiro cadastrado, quero reservar assentos em viagens futuras para garantir minha participação. | • Mapa de assentos com seleção visual.<br>• Registro da reserva temporária (15 min) no sistema.<br>• Visualização da reserva no perfil do passageiro. |
| 18 | Como administrador, quero criar novas viagens na plataforma para disponibilizar passagens aos passageiros. | • Formulário de criação de viagem no painel admin.<br>• Campos para origem, destino, data, tipo de veículo e preço.<br>• Viagem publicada no catálogo após salvar (assentos gerados automaticamente). |
| 19 | Como administrador, quero visualizar todas as viagens criadas para gerenciar e acompanhar as viagens cadastradas. | • Tela administrativa com lista de viagens.<br>• Exibição de status da viagem (ativa/cancelada).<br>• Opção de editar ou visualizar viagem. |
| 20 | Como administrador, quero gerenciar lotes de assentos e preços para controlar a disponibilidade ao longo das vendas. | • Cadastro de assentos por tipo de veículo (Leito, Semileito, Convencional).<br>• Definição de preço e categoria por assento.<br>• Atualização automática da disponibilidade. |
| 21 | Como administrador, quero cancelar viagens quando necessário para evitar vendas de viagens canceladas. | • Opção de cancelar viagem no painel admin.<br>• Bloqueio automático de vendas.<br>• Atualização do status da viagem para "Cancelada". |
| 22 | Como administrador, quero definir um limite de assentos por viagem para evitar superlotações no veículo. | • Campo para definir quantidade máxima de assentos.<br>• Sistema impede vendas após atingir limite.<br>• Exibição de "Esgotado". |
| 23 | Como administrador, quero limitar o uso de apenas um cupom por passagem para evitar prejuízos financeiros. | • Validação que impede uso de múltiplos cupons.<br>• Exibição de aviso ao passageiro.<br>• Aplicação de apenas um desconto por compra. |
| 24 | Como administrador, quero acessar uma tela para visualizar e confirmar passagens vendidas para controle de embarque. | • Tela com lista de passagens vendidas.<br>• Exibição de dados do passageiro e assento.<br>• Possibilidade de validar passagem via QR Code no embarque. |
----------------------------------------------------

## BDD das Histórias de Usuário

- 01:

    Dado: Que sou visitante e não tenho conta criada

    Quando: Acessar a página home

    Então: As viagens disponíveis são apresentadas para o usuário

- 02:

    Dado: Que sou visitante e não tenho conta criada

    Quando: Clicar no botão para criar conta

    Então: Deve ser aberto a página ou o modal para cadastro da conta e a conta deve ser criada

- 03:

    Dado: Que tenho uma conta criada

    Quando: Clicar no botão de fazer login

    Então: Devo ser direcionado para a página ou modal de login e o login deve ser realizado corretamente

- 04:

    Dado: Que tenho uma conta cadastrada

    Quando: Clicar no botão de esqueci minha senha

    Então: Deve ser enviado um email para o email cadastrado na conta para a atualização da senha

- 05:

    Dado: Que estou na tela home

    Quando: Preencher o campo de pesquisa e clicar para pesquisar

    Então: Deve me retornar as viagens com o destino que pesquisei

- 06: 

    Dado: Que estou na tela home

    Quando: Selecionar um filtro 

    Então: Devem ser apresentadas as viagens com relação ao filtro que escolhi (Data, tipo de veículo, destino, etc)

- 07:

    Dado: Estou visualizando as viagens

    Quando: Clicar em uma viagem

    Então: Deve ser exibida a tela de detalhes sobre aquela viagem com todas as suas informações (origem, destino, data, preço, tipo de veículo, etc)

- 08:

    Dado: Que estou na tela de detalhes de uma viagem

    Quando: Clicar no botão para compartilhar

    Então: Deve ser apresentada uma tela com as opções de redes sociais para compartilhamento ou um botão para copiar o link

- 09:

    Dado: Que estou na tela de detalhes de uma viagem

    Quando: Selecionar um assento e clicar no botão de reservar

    Então: O assento desta viagem deve ser reservado temporariamente e adicionado ao carrinho 

- 10:

    Dado: Estou no carrinho

    Quando: Clicar para finalizar a compra

    Então: Deve ser gerado o QR Code da passagem e exibida a confirmação da compra

- 11:

    Dado: Estou na tela de minhas passagens

    Quando: A compra for finalizada

    Então: Deve ser exibido o QR Code da passagem para apresentação no embarque

- 12:

    Dado: Que estou logado na minha conta

    Quando: Clicar no botão de minhas passagens

    Então: Devem ser exibidas todas as passagens que adquiri com seus respectivos QR Codes

- 13:

    Dado: Que estou logado e já comprei uma passagem com meu CPF

    Quando: Tentar comprar outra passagem para a mesma viagem

    Então: O sistema deve bloquear a compra e exibir uma mensagem informativa

- 14:

    Dado: Que estou na plataforma

    Quando: Acessar a aba de viagens

    Então: Deve ser exibida uma lista de viagens disponíveis com opção de visualizar detalhes

- 15:

    Dado: Que estou acessando a aba de viagens

    Quando: Visualizar as viagens disponíveis

    Então: Deve ser exibida uma interface responsiva com cards contendo imagem, destino e preço

- 16:

    Dado: Que estou na tela de pagamento

    Quando: Inserir um cupom de desconto válido

    Então: O sistema deve validar o cupom e atualizar automaticamente o valor total da compra

- 17:

    Dado: Que estou logado e acessando uma viagem futura

    Quando: Selecionar um assento e clicar no botão de reservar

    Então: A reserva temporária deve ser registrada (15 min) e o assento deve ficar bloqueado

- 18:

    Dado: Que estou logado como administrador

    Quando: Preencher o formulário e criar uma nova viagem

    Então: A viagem deve ser salva com seus assentos gerados automaticamente e publicada no catálogo

- 19:

    Dado: Que estou no painel administrativo

    Quando: Acessar a lista de viagens

    Então: Devem ser exibidas todas as viagens com status e opção de editar ou visualizar

- 20:

    Dado: Que estou gerenciando uma viagem no painel admin

    Quando: Cadastrar assentos por tipo de veículo

    Então: O sistema deve salvar os assentos com categorias (Janela/Corredor) e preços extras

- 21:

    Dado: Que estou no painel administrativo

    Quando: Cancelar uma viagem

    Então: O sistema deve bloquear vendas e atualizar o status para "Cancelada"

- 22:

    Dado: Que estou criando ou editando uma viagem

    Quando: Definir um limite máximo de assentos

    Então: O sistema deve impedir vendas após atingir o limite e exibir "Esgotado"

- 23:

    Dado: Que estou na tela de pagamento

    Quando: Tentar aplicar mais de um cupom

    Então: O sistema deve permitir apenas um cupom por passagem e exibir um aviso ao passageiro

- 24:

    Dado: Que estou no painel administrativo

    Quando: Acessar a tela de passagens vendidas

    Então: Devem ser exibidos os dados dos passageiros com assentos e a opção de validar passagem via QR Code

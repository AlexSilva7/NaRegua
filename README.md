**Projeto em Construção.**: 

**Nome do Projeto**: NaRegua

**Descrição**:
NaRegua é uma api que unifica informações sobre salões de barbearia, seus profissionais e permite aos clientes agendar atendimentos. 
A plataforma oferece uma experiência completa para clientes e proprietários de salões, incluindo agendamento de compromissos, pagamentos simulados via PIX e cartão de crédito, avaliações de atendimento, favoritos e muito mais.

**Componentes Principais**:

1. **API Principal**:
   - Gerencia o cadastro de clientes, salões e profissionais.
   - Permite que os clientes naveguem pelos salões de beleza cadastrados, visualizem perfis de profissionais e agendem atendimentos.
   - Oferece recursos de pagamento simulados para depósito via PIX e cartão de crédito.
   - Possibilita aos clientes avaliar a qualidade do atendimento após cada agendamento.
   - Permite que os clientes adicionem salões aos favoritos para fácil acesso posterior.

2. **Consumidor Python**:
   - Funciona como um consumidor de filas de mensagens.
   - Recebe ordens de pagamento da API principal e encaminha para processamento de pagamento simulado.

3. **Docker**:
   - Contém todas as aplicações do projeto.
   - Inclui um serviço RabbitMQ para gerenciar a comunicação entre os componentes.
   - Integra um servidor Redis para armazenar informações temporárias e melhorar o desempenho da aplicação.

**Tecnologias Utilizadas**:
- **API Principal**: ASP.NET (C#), Docker
- **Consumidor Python**: Python, Docker
- **Banco de Dados**: A api principal terá suporte para integração com SQLServer ou Firebase.
- **Filas de Mensagens**: RabbitMQ
- **Cache de Dados**: Redis
- **Métodos de Pagamento**: Integração simulada (fake).
- **Controle de Versão**:

**Como Iniciar o Projeto**:
1. Clone este repositório.
2. Configure as variáveis de ambiente necessárias. (Na Api principal temos o appsettings.json com opções de Database, Serviços Fakes, Configs Jwt, etc ...)
3. Execute o Docker Compose para iniciar todas as aplicações.

**Autores**:
- Alex Silva


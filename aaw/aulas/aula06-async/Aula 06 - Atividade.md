# Atividade — AULA 06

## Síncrono ou Assíncrono?

*Análise de fluxos de comunicação entre serviços — Arquitetura de Aplicações Web*

## 🎯 MISSÃO

Vocês são os arquitetos dos 4 fluxos abaixo. Para CADA cenário:

- Decidam o estilo de comunicação: síncrono (request/response), assíncrono (fila/evento) ou API Gateway/BFF
- Desenhem o fluxo com caixas (serviços) e setas (chamadas/mensagens) no espaço indicado
- Justifiquem com pelo menos 2 fatores (urgência da resposta, tolerância a atraso, picos, falhas...)
- Apontem o principal risco da escolha de vocês

*⏱️ Tempo: 25 minutos  |  👥 Formato: em duplas  |  Não existe resposta única — o que vale é a justificativa.*

> **Nomes:** Bruno Simon Da Silva   **Turma:** ADS   **Data:** 16/09/2026

## CENÁRIO 01 — PagFácil — aprovar ou negar AGORA

No checkout do PagFácil, ao clicar em “Pagar”, o serviço de Pagamentos precisa consultar o saldo/limite do cliente no serviço de Contas — e a resposta define se a venda acontece neste exato momento.

- O cliente está na tela, esperando o resultado da compra
- Sem a resposta de Contas, não há decisão possível: aprovar às cegas é proibido
- Tempo de resposta do serviço de Contas: ~80 ms em condições normais

**Sua análise:**

1. Estilo recomendado:   X Síncrono      ☐ Assíncrono (fila/evento)      ☐ API Gateway/BFF

2. Desenhe o fluxo (caixas = serviços, setas = chamadas/mensagens):

[Cliente] --clica "Pagar"--> [Serviço Pagamentos] --consulta saldo/limite--> [Serviço Contas] --responde aprovado/negado--> [Serviço Pagamentos] --resultado da venda--> [Cliente]

3. Justificativa (mínimo 2 fatores):
    Urgência da resposta: o cliente está esperando na tela a decisão de aprovar ou negar precisa acontecer naquele exato momento.
    Tolerância a atraso: Não existe "aprovar às cegas", sem a resposta de Contas não há decisão possível, então a chamada tem que bloquear até ter resposta.
4. Principal risco da escolha:
    Se o serviço de Contas cair ou ficar lento, o Pagamentos fica bloqueado esperando o fim da execução do processo anterior.

## CENÁRIO 02 — CadastraJá — o e-mail de boas-vindas

Após criar a conta no CadastraJá, o sistema envia um e-mail de boas-vindas. O provedor de e-mail às vezes demora 8 segundos para responder e falha em 2% das tentativas.

- O usuário quer começar a usar o app imediatamente após o cadastro
- O e-mail chegar 1 minuto depois não incomoda ninguém
- Se o provedor falhar, o envio deve ser tentado de novo — sem o usuário perceber

**Sua análise:**

1. Estilo recomendado:   ☐ Síncrono      X Assíncrono (fila/evento)      ☐ API Gateway/BFF

2. Desenhe o fluxo (caixas = serviços, setas = chamadas/mensagens):

[Serviço Cadastro] --publica evento "UsuarioCriado"--> [Fila/Broker] --> [Serviço de E-mail] --envia--> [Provedor de e-mail] 
[Serviço Cadastro] --libera acesso imediato--> [Usuário]

3. Justificativa (mínimo 2 fatores):
    Tolerância a atraso alta: um minuto de diferença no envio do e-mail não afeta ninguém.
    Falhas do provedor (2%) exigem retry: numa fila, é fácil reprocessar a mensagem sem o usuário perceber; num síncrono, o cadastro ficaria travado 8s esperando o provedor.
4. Principal risco da escolha:
    Mensagens podem ser processadas fora de ordem ou duplicadas, o usuário pode, no limite, receber o e-mail duas vezes se o consumidor não for idempotente.

## CENÁRIO 03 — MegaMarket — baixa de estoque nos picos

No marketplace MegaMarket, cada venda gera uma baixa no serviço de Estoque. Nas grandes promoções o tráfego sobe 10x e o Estoque não dá conta de responder na velocidade das vendas.

- Atraso de alguns segundos na baixa é aceitável
- PERDER uma baixa de estoque não é aceitável (gera venda sem produto)
- O checkout não pode ficar lento nem cair porque o Estoque está sobrecarregado

**Sua análise:**

1. Estilo recomendado:   ☐ Síncrono      X Assíncrono (fila/evento)      ☐ API Gateway/BFF

2. Desenhe o fluxo (caixas = serviços, setas = chamadas/mensagens):

[Checkout] --publica evento "VendaRealizada"--> [Fila/Broker] --(buffer)--> [Serviço Estoque] --baixa item--> [DB Estoque] 
[Checkout] --confirma venda ao cliente--> [Cliente]

3. Justificativa (mínimo 2 fatores):
    Picos de tráfego (10x): o Checkout não fica esperando o Estoque processar em tempo real, evitando que ele fique lento ou caia junto.
    Atraso de segundos é aceitável, mas perda de mensagem não é, um broker com garantia de entrega resolve isso, algo que uma chamada síncrona sem retentativa não garantiria sozinha.
4. Principal risco da escolha:
    overselling temporário, como a baixa não é imediata, é possível vender um produto que já zerou no estoque entre a compra e o processamento da fila
## CENÁRIO 04 — AppBanco — uma tela, cinco serviços

A tela inicial do AppBanco mostra saldo, fatura do cartão, investimentos, empréstimos e cashback — dados de 5 serviços diferentes. O time mobile reclama: são 5 chamadas, 5 formatos de resposta e 5 pontos de falha em cada abertura do app.

- A tela precisa abrir rápido, inclusive em redes móveis ruins
- Cada serviço tem equipe, formato e autenticação próprios
- Amanhã nasce a versão web, que precisa de MAIS dados que a mobile

**Sua análise:**

1. Estilo recomendado:   ☐ Síncrono      ☐ Assíncrono (fila/evento)      X API Gateway/BFF

2. Desenhe o fluxo (caixas = serviços, setas = chamadas/mensagens):

[App Mobile] --1 chamada--> [BFF Mobile] --> [Saldo]
                                        --> [Fatura Cartão]
                                        --> [Investimentos]
                                        --> [Empréstimos]
                                        --> [Cashback]
[BFF Mobile] <-- agrega/formata respostas --
[App Mobile] <--1 resposta consolidada--

3. Justificativa (mínimo 2 fatores):
    Rede móvel ruim: 1 chamada agregada (BFF) é muito mais resiliente do que 5 chamadas simultâneas, cada uma um ponto de falha na rede do usuário.
    Formatos e autenticações distintos por serviço: o Gateway/BFF centraliza essa complexidade, expondo um contrato único e simples para o app.

4. Principal risco da escolha:
    o BFF vira um ponto único de falha e de latência — se ele cair, cai a tela inteira mesmo que os 5 serviços estejam saudáveis. Mitiga-se com timeouts por serviço + fallback
## DESAFIO

1. Escolha um cenário em que vocês indicaram ASSÍNCRONO. Os brokers de mensagens costumam garantir entrega “pelo menos uma vez” — ou seja, a MESMA mensagem pode chegar duas vezes. O que aconteceria no seu fluxo? Como o consumidor deveria se proteger?

Cenário 03:

Se a mesma mensagem "VendaRealizada" chegar duas vezes ao serviço de Estoque, ele faria a baixa duas vezes para a mesma venda — o estoque cairia mais do que deveria, criando uma inconsistência.

Como o consumidor deve se proteger — idempotência:

* Cada evento carrega um ID único.
* Antes de processar, o serviço de Estoque verifica se aquele ID já foi processado.
* Se já foi, a mensagem é descartada; se não foi, processa normalmente e registra o ID.
* Alternativa: em vez de "decrementar X unidades", usar uma operação idempotente, como "definir estoque = valor final calculado" ou aplicar a baixa condicionada a um estado esperado.
# Aula 12 — Respostas da atividade

## Parte 1 — Diário de correção

| Falha | Aula | Regra corrigida | Status |
|---:|---:|---|---|
| 01 | 01 | Pedidos acessa Catálogo somente por `ICatalogo`, preservando a fronteira do módulo. | Corrigida |
| 02 | 02 | Recurso inexistente retorna 404; criação retorna 201 e `Location`. | Corrigida |
| 03 | 03 | Cancelamento é `DELETE /api/v1/pedidos/{id}`, sem corpo, e retorna 204. | Corrigida |
| 04 | 04 | Listagem de produtos é paginada, ordenada e limitada pelo servidor. | Corrigida |
| 05 | 05 | Todos os itens são validados antes do único `SaveChanges`; erro não grava pedido parcial. | Corrigida |
| 06 | 06 | A confirmação é publicada na fila; a requisição não espera o provedor de e-mail. | Corrigida |
| 07 | 07 | A busca separa entrada do usuário do comando SQL usando LINQ/EF Core. | Corrigida |
| 08 | 08 | Validação de lifetime rejeita tokens vencidos com 401. | Corrigida |
| 09 | 09 | RBAC: só Admin remove produtos. ABAC: pedido é acessível/cancelável pelo dono ou Admin. | Corrigida |
| 10 | 10 | OpenAPI descreve 201 e os erros; o esquema Bearer JWT é aplicado às rotas protegidas. | Corrigida |
| 11 | 11 | Teste espera frete zero exatamente em R$ 300 e verifica o total por asserção. | Corrigida |

## Parte 2 — Sintoma, aula e conceito

| # | Aula | Conceito |
|---|---|---|
| A | 06 | Comunicação assíncrona: não acoplar o tempo de resposta do pedido ao serviço de e-mail. |
| B | 02 | Semântica HTTP: recurso inexistente deve retornar 404, não 200 com erro no corpo. |
| C | 07 e 09 | IDOR/BOLA no acesso ao pedido (ABAC por proprietário) e autorização por papel (RBAC) para remover produto. |
| D | 04 | Paginação de coleções, com limites impostos no servidor. |
| E | 11 | Testes de fronteira: regra inclusiva exige testar exatamente R$ 300. |
| F | 01 | Fronteiras entre módulos: Pedidos deve chamar a regra do Catálogo, não editar sua tabela. |
| G | 08 | Validar expiração e lifetime do JWT. |
| H | 03 | GET deve ser seguro; ação de cancelamento deve usar DELETE sobre o recurso. |
| I | 05 | Atomicidade: alterações do pedido precisam ser tudo-ou-nada. |
| J | 10 | Contrato OpenAPI precisa refletir status real e esquema de segurança. |
| K | 07 | Injeção de SQL: parâmetro de busca não pode ser concatenado como comando. |

## Parte 3 — Perguntas que cruzam aulas

1. **Não.** Com Catálogo em serviço e banco próprios, a transação local do banco de Pedidos não cobre a reserva. É necessário coordenar a operação distribuída, por exemplo com Saga, eventos, compensação e idempotência; pode haver consistência eventual.
2. A exclusão de produto é RBAC: a permissão depende do papel Admin. A leitura/cancelamento de um pedido é ABAC: além do papel, compara o `UsuarioId` do recurso ao `NameIdentifier` do token, permitindo acesso geral ao Admin.
3. Uma suíte pode ficar verde se o resultado esperado foi copiado do comportamento defeituoso e se um teste não contém assert. Os valores esperados devem vir da regra de negócio, cobrindo os dois lados e o próprio limite (299,99; 300,00; 300,01).

## Desafio — Se só três falhas pudessem ser corrigidas

1. **K — Injeção de SQL:** pode expor ou alterar grande volume de dados.
2. **C — Acesso indevido e exclusão:** combina exposição de pedidos de terceiros com alteração destrutiva do catálogo.
3. **G — Token vencido aceito:** um token roubado continua válido indefinidamente, ampliando a janela de abuso.

Depois dessas, priorizaria I/F, pois inconsistência de estoque pode causar perdas e comprometer pedidos.

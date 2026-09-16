# HANDOUT — AULA 05

## Escolha o Banco

*Persistência em arquiteturas distribuídas — Arquitetura de Aplicações Web*

## 🎯 MISSÃO

Vocês são o time de arquitetura de dados contratado pelas 4 empresas abaixo. Para CADA cenário:

- Escolham o modelo de banco: relacional, documento, chave-valor ou grafo
- Justifiquem com pelo menos 2 fatores do contexto (estrutura dos dados, padrão de acesso, escala, consistência...)
- Apontem o principal risco da escolha de vocês

*⏱️ Tempo: 25 minutos  |  👥 Formato: em duplas  |  Não existe resposta única — o que vale é a justificativa.*

> **Nomes:** Bruno Simon   **Turma:** ADS   **Data:** 16/09/2026

## CENÁRIO 01 — TechStore — o catálogo camaleão

E-commerce com 80 mil produtos. Cada categoria tem atributos completamente diferentes: livro tem autor e número de páginas; notebook tem RAM e CPU; camiseta tem tamanho e cor.

- A cada categoria nova, o time faz ALTER TABLE e a tabela produtos já tem 92 colunas (a maioria NULL)
- O produto é quase sempre lido INTEIRO, de uma vez, para montar a página
- Novos atributos surgem toda semana — o marketing não espera o DBA
- Relatórios cruzando categorias são raros

**Sua análise:**

1. Modelo recomendado:   ☐ Relacional    X Documento     ☐ Chave-valor     ☐ Grafo

2. Justificativa (mínimo 2 fatores do contexto):
    A estrutura dos dados heterogênea e sem schema fixo. Cada categoria tem atributos completamentes diferentes. Em vez de uma tabela relacional,  um banco de documentos permite que cada produto tenha os campos que fazem sentido para dua categoria.
    A forma de acesso é por leitura do objeto inteiro. O produto é lido de uma vez para montar a página. Esse é o caso de uso ideal de documento

3. Principal risco da escolha:
    Como relatórios cruzando categorias são raros mas não inexistentes, consultas analíticas complexas ficam mais caras e menos naturais.
## CENÁRIO 02 — MegaCart — o carrinho da Black Friday

Serviço de carrinho de compras de um varejista gigante. Na Black Friday são milhões de leituras e escritas por minuto.

- O acesso é SEMPRE pela chave: “carrinho do cliente 12345” — nunca por busca ou filtro
- Todo carrinho expira automaticamente em 48h (TTL)
- Latência precisa ser de poucos milissegundos
- Perder um carrinho é chato, mas NÃO é tragédia — o cliente remonta

**Sua análise:**

1. Modelo recomendado:   ☐ Relacional     ☐ Documento     X Chave-valor     ☐ Grafo

2. Justificativa (mínimo 2 fatores do contexto):
    Padrão de acesso é sempre por chave única. "carrinho 12345" nunca é buscado por filtro ou índice secundário;
    Latência extrema e escala de escrita/leitura. Milhões de operações por minuto exigem uma estrutura simples, com escrita e leitura em memória na casa de microssegundos/poucos ms.
3. Principal risco da escolha:
    Baixa durabilidade/persistência. Se o serviço não tiver replicação ou snapshot adequado, uma falha de nó pode derrubar carrinhos em massa durante o pico

## CENÁRIO 03 — PayBank — dinheiro não pode evaporar

Módulo de transferências de um banco. Uma transferência debita uma conta e credita outra — as duas operações têm que acontecer JUNTAS ou nenhuma acontece.

- Consistência forte exigida por lei — saldo errado é multa do Banco Central
- Auditoria cruza contas, clientes, agências e transações em relatórios complexos (joins)
- O esquema dos dados é estável há 10 anos
- Volume alto, mas previsível

**Sua análise:**

1. Modelo recomendado:   X Relacional     ☐ Documento     ☐ Chave-valor     ☐ Grafo

2. Justificativa (mínimo 2 fatores do contexto):
    Débito e crédito têm que ocorrer atomicamente. É exatamente a garantia de atomicidade e isolamento que bancos relacionais oferecem nativamente, e que a maioria dos NoSQL sacrifica em nome de disponibilidade/escala.
    Consultas analíticas complexas com joins. Auditoria cruzando contas, clientes, agências e transações é o caso de uso onde SQL e um schema normalizado brilham, com integridade referencial garantida.
3. Principal risco da escolha:
    Bancos relacionais escalam melhor verticalmente, se o volume crescer muito além do previsto, sharding e replicação síncrona para manter ACID em múltiplos nós se tornam complexos e caros

## CENÁRIO 04 — FriendLink — amigos dos seus amigos

Rede social profissional em que o produto principal é a indicação: “pessoas que você talvez conheça” e “quem pode te apresentar à empresa X”.

- As consultas dominantes percorrem RELACIONAMENTOS: amigos dos amigos, caminhos de indicação com até 6 níveis
- Em banco relacional, cada nível vira um self-join — com 6 níveis a consulta já não responde
- Os dados de perfil são simples; o valor está nas CONEXÕES
- O grafo cresce milhões de arestas por dia

**Sua análise:**

1. Modelo recomendado:   ☐ Relacional     ☐ Documento     ☐ Chave-valor     X Grafo

2. Justificativa (mínimo 2 fatores do contexto):
    O valor está nas conexões, não nos atributos. Perfis são simples, mas a modelagem como nós e arestas é o que representa naturalmente o domínio do problema;
    Escala de arestas alta. Bancos de grafo são otimizados para armazenar e percorrer um número enorme de relacionamentos com performance previsível independente da profundidade da busca.
3. Principal risco da escolha:
    Escalabilidade horizontal de bancos de grafo é historicamente mais difícil em volumes gigantescos pode exigir soluções específicas e maior custo operacional/expertise da equipe.
## DESAFIO

1. Escolha um dos cenários e responda: se a rede particionar (metade dos servidores não enxerga a outra metade), o que o sistema deve fazer — parar de responder para não errar, ou continuar respondendo mesmo arriscando dados desatualizados? Qual letra do CAP vocês sacrificariam e por quê?

Cenário 02:

Decisão: Consistência;
Por quê:
"perder um carrinho é chato, mas NÃO é tragédia". O custo de um carrinho ficar temporariamente inconsistente é muito menor do que o custo de o carrinho ficar indisponível durante a Black Friday, cliente não consegue comprar. Isso caracteriza o sistema como AP, com consistência eventual: depois que a partição se resolve, os nós convergem para o mesmo estado.
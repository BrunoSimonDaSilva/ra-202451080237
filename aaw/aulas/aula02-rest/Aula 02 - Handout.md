# HANDOUT — AULA 02

## Dissecando o HTTP

*6 requisições sob o microscópio — Arquitetura de Aplicações Web*

## 🎯 MISSÃO

Vocês interceptaram 6 conversas entre um app e a API de uma biblioteca. Para CADA card:

- Descrevam o que o cliente pediu (verbo + recurso na URI)
- Expliquem o que o status code da resposta informa
- Respondam: repetindo a MESMA requisição 3 vezes seguidas, o estado do servidor muda?

Ao final, preencham juntos a TABELA-SÍNTESE dos verbos na última página.

*⏱️ Tempo: 30 minutos  |  👥 Formato: em duplas  |  Dica: o card 6 esconde uma pegadinha de quem é a culpa.*

> **Nomes:** Bruno Simon Da Silva   **Turma:** ADS   **Data:** 27/08/2026

## REQUISIÇÃO 01 — A prateleira inteira

```text
→ REQUISIÇÃO
GET /api/livros HTTP/1.1
Host: biblioteca.newton.br
Accept: application/json
```

```text
← RESPOSTA
HTTP/1.1 200 OK
Content-Type: application/json

[ { "id": 1, "titulo": "Clean Code", "autor": "Robert C. Martin" },
  { "id": 7, "titulo": "O Programador Pragmático", "autor": "Hunt & Thomas" } ]
```

**Sua análise:**

1. O que o cliente pediu (verbo + recurso)?
    Resposta:
        Um GET no endpoint /api/livros, solicitando a lista de livros.

2. O que o status code informa? Deu certo? Culpa de quem se não deu?
    Resposta:
        200 OK significa sucesso e os livros foram retornados.

3. Repetindo esta requisição 3 vezes seguidas, o estado do servidor muda? E a resposta?
    Resposta:
        O estado do servidor não muda, pois GET é uma operação de leitura. A resposta tende a ser a mesma, desde que nenhum outro processo altere os dados entre as requisições.
## REQUISIÇÃO 02 — O livro fantasma

```text
→ REQUISIÇÃO
GET /api/livros/99 HTTP/1.1
Host: biblioteca.newton.br
Accept: application/json
```

```text
← RESPOSTA
HTTP/1.1 404 Not Found
Content-Type: application/problem+json

{ "title": "Not Found", "status": 404 }
```

**Sua análise:**

1. O que o cliente pediu (verbo + recurso)?
    Resposta:
        Um GET no recurso /api/livros/99, solicitando o livro de ID 99
2. O que o status code informa? Deu certo? Culpa de quem se não deu?
    Resposta:
        404 Not Found;
        Entidade não encontrada. A requisição chegou corretamente ao servidor, mas a entidade não existe.
3. Repetindo esta requisição 3 vezes seguidas, o estado do servidor muda? E a resposta?
    Resposta:
        O estado do servidor não muda. A resposta continuará sendo 404 Not Found
## REQUISIÇÃO 03 — Livro novo na estante

```text
→ REQUISIÇÃO
POST /api/livros HTTP/1.1
Host: biblioteca.newton.br
Content-Type: application/json

{ "titulo": "Domain-Driven Design", "autor": "Eric Evans" }
```

```text
← RESPOSTA
HTTP/1.1 201 Created
Location: /api/livros/8
Content-Type: application/json

{ "id": 8, "titulo": "Domain-Driven Design", "autor": "Eric Evans" }
```

**Sua análise:**

1. O que o cliente pediu (verbo + recurso)?
    Resposta:
        O cliente fez um POST em /api/livros, enviando os dados para criar um novo livro.
2. O que o status code informa? Deu certo? Culpa de quem se não deu?
    Resposta:
        201 Created significa que um novo recurso foi criado com sucesso.
3. Enviando este POST 3 vezes seguidas, o que acontece na estante? Para que serve o header Location?
    Resposta:
        Serão criados 3 livros, pois cada POST representa uma nova tentativa de criação. Portanto, o estado do servidor muda a cada requisição.
## REQUISIÇÃO 04 — Corrigindo a ficha completa

```text
→ REQUISIÇÃO
PUT /api/livros/7 HTTP/1.1
Host: biblioteca.newton.br
Content-Type: application/json

{ "id": 7, "titulo": "O Programador Pragmático", "autor": "D. Hunt; D. Thomas" }
```

```text
← RESPOSTA
HTTP/1.1 200 OK
Content-Type: application/json

{ "id": 7, "titulo": "O Programador Pragmático", "autor": "D. Hunt; D. Thomas" }
```

**Sua análise:**

1. O que o cliente pediu (verbo + recurso)?
    Resposta:
        O cliente fez um PUT no recurso /api/livros/7, enviando uma atualização completa para o livro de ID 7.
2. O que o status code informa? Deu certo? Culpa de quem se não deu?
    Resposta:
        200 OK significa que a atualização foi realizada com sucesso e o servidor retornou o recurso atualizado.
3. Repetindo esta requisição 3 vezes seguidas, o estado do servidor muda? E a resposta?
    Resposta:
        O estado final do servidor permanece igual ao estado após a primeira requisição, pois as três requisições estão colocando o livro com os mesmos dados.

## REQUISIÇÃO 05 — Fora do catálogo

```text
→ REQUISIÇÃO
DELETE /api/livros/7 HTTP/1.1
Host: biblioteca.newton.br
```

```text
← RESPOSTA
HTTP/1.1 204 No Content
```

**Sua análise:**

1. O que o cliente pediu (verbo + recurso)?
    Resposta:
        O cliente fez um DELETE no recurso /api/livros/7, solicitando a exclusão do livro de ID 7.
2. O que o status code informa? Deu certo? Culpa de quem se não deu?
    Resposta:
        204 No Content significa que a operação foi realizada com sucesso e não há conteúdo para retornar na resposta.
3. Repetindo o DELETE, o estado do servidor muda? Que resposta você ESPERA na segunda vez?
    Resposta:
        Na segunda tentativa, o comportamento esperado pode ser 404 Not Found
## REQUISIÇÃO 06 — O cadastro capenga

```text
→ REQUISIÇÃO
POST /api/livros HTTP/1.1
Host: biblioteca.newton.br
Content-Type: application/json

{ "autor": "Anônimo" }
```

```text
← RESPOSTA
HTTP/1.1 400 Bad Request
Content-Type: application/problem+json

{ "title": "Bad Request", "status": 400,
  "errors": { "Titulo": [ "O campo Titulo é obrigatório" ] } }
```

**Sua análise:**

1. O que o cliente pediu (verbo + recurso)?
    Resposta:
        O cliente fez um POST em /api/livros, tentando criar um novo livro, mas enviou apenas o campo autor.
2. O que o status code informa? Deu certo? Culpa de quem se não deu?
    Resposta:
        400 Bad Request significa que a requisição é inválida. O campo Titulo obrigatório não foi enviado.
3. Repetindo esta requisição 3 vezes seguidas, o estado do servidor muda? E a resposta?
    Resposta:
        A resposta continuará sendo 400 Bad Request, indicando que o campo Titulo é obrigatório.
## TABELA-SÍNTESE — Os verbos do HTTP

*Preencham com base nos 6 cards. “Seguro” = não altera nada no servidor. “Idempotente” = repetir N vezes deixa o servidor no mesmo estado que 1 vez.*

| **Verbo**    | **Para que serve**                                     | **Seguro?**| **Idempotente?**        | **Status típicos**         |
| ------------ | ------------------------------------------------------ | -------    | ----------------------- | -------------------------- |
| **`GET`**    | Consultar/obter um recurso                             | **Sim**    | **Sim**                 | `200`, `404`               |
| **`POST`**   | Criar um recurso ou executar uma operação              | **Não**    | **Não**                 | `201`, `400`, `409`        |
| **`PUT`**    | Criar ou substituir/atualizar completamente um recurso | **Não**    | **Sim**                 | `200`, `201`, `204`, `404` |
| **`PATCH`**  | Atualizar parcialmente um recurso                      | **Não**    | **Depende da operação** | `200`, `204`, `400`, `404` |
| **`DELETE`** | Excluir um recurso                                     | **Não**    | **Sim**                 | `204`, `404`               |


## DESAFIO

1. O verbo PATCH não apareceu em nenhum card. Qual a diferença entre PATCH e PUT? Um app de banco quer alterar SÓ o apelido do usuário, entre dezenas de campos do perfil — qual dos dois você usaria e por quê?

PATCH: Atualiza parcialmente um recurso
PUT: Criar ou substituir/atualizar completamente um recurso

Usaria PATCH, porque ele altera somente oque precisamos alterar, neste caso o apelido
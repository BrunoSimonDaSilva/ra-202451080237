# HANDOUT — AULA 03

## Consultoria de Design: a API da EscolaTech

*Identifique os anti-padrões e proponha o redesenho — Arquitetura de Aplicações Web*

## 🎯 MISSÃO

A EscolaTech contratou a consultoria de vocês para auditar a API do sistema escolar. Todos os endpoints abaixo FUNCIONAM e estão em produção — mas o time novo se recusa a mexer neles. Para CADA endpoint:

- Identifiquem o(s) problema(s) de design (pode haver mais de um!)
- Proponham o redesenho: método HTTP + rota + status codes corretos

# ra-202451080237 — Bruno Simon Da Silva
- Curso: ADS
- Professor: Thalles Noce

## ENDPOINT 01 — POST /api/getAlunos

**Documentação atual (extraída da wiki da EscolaTech):**

```text
POST /api/getAlunos
Retorna TODOS os alunos cadastrados (hoje: 12.482 registros).
Resposta: 200 OK + array JSON completo (~9 MB).
Obs. da wiki: "usar POST porque GET não estava funcionando".
```

1. Qual(is) problema(s) de design vocês identificam?
    Resposta:
        1. Uso de verbo na URL (getAlunos).
        2. Uso de POST para uma operação de consulta.
        3. Falta de paginação para uma quantidade grande de registros.
2. Seu redesenho (método + rota + status codes):
    Resposta:
        GET /api/alunos?page=1&pageSize=100
        200 OK
## ENDPOINT 02 — GET /deletarAluno?id=7

**Documentação atual (extraída da wiki da EscolaTech):**

```text
GET /deletarAluno?id=7
Remove o aluno do banco de dados.
Resposta: 200 OK + "OK" (mesmo se o aluno não existir).
Obs. da wiki: "dá pra deletar pelo navegador, bem prático".
```

1. Qual(is) problema(s) de design vocês identificam?
    Resposta:
        1. Uso do method GET quando deveria ser DELETE
        2. Uso de verbo na URL, deletarAlunos => alunos
        3. ID sendo enviado como query string em vez de fazer parte da identificação do recurso.
        4. Retorna 200 OK mesmo quando o aluno não existe.
2. Seu redesenho (método + rota + status codes):
    Resposta:
        DELETE /api/alunos/7
        204 No Content — aluno excluído
        404 Not Found — aluno não encontrado
## ENDPOINT 03 — POST /api/alunos (criação)

**Documentação atual (extraída da wiki da EscolaTech):**

```text
POST /api/alunos
Body: { "nome": "...", "curso": "..." }
Cria o aluno e responde: 200 OK + body "OK".
O app precisa buscar a lista inteira de novo para descobrir o ID gerado.
```

1. Qual(is) problema(s) de design vocês identificam?
    Resposta:
        1. Após criar o aluno, o cliente precisa buscar a lista inteira para descobrir o ID gerado.
        2. A resposta deveria retornar os dados do recurso criado.

2. Seu redesenho (método + rota + status codes):
    Resposta:
        POST /api/alunos
        201 Created
        { "id": 123, "nome": "...", "curso": "..." }

## ENDPOINT 04 — GET /escolas/1/turmas/3/alunos/25/matriculas/88/disciplinas/12

**Documentação atual (extraída da wiki da EscolaTech):**

```text
GET /escolas/1/turmas/3/alunos/25/matriculas/88/disciplinas/12
Retorna os dados da disciplina 12 da matrícula 88.
Para montar a URL o app precisa conhecer 5 IDs diferentes.
Resposta: 200 OK + JSON da disciplina.
```

1. Qual(is) problema(s) de design vocês identificam?
    Resposta:
        1. Acoplamento excessivo: a URL exige que o client conheça 5 IDs para obter uma única disciplina.
        2. Hierarquia excessiva: a rota representa toda a árvore de relacionamento, mesmo quando vários desses níveis não são necessários para identificar a rota.
        3. Redundância: se a matrícula 88 já identifica o vínculo do aluno com a escola/turma, buscar novamente em escola/1/turma/3/aluno/25 é desnecessário.
        4. Dificuldade de evolução: mudanças na estrutura dos relacionamentos podem obrigar o cliente a mudar suas URLs.
2. Seu redesenho (método + rota + status codes):
    Resposta:
        GET /matriculas/88/disciplinas/12
        200 OK
        404 Not Found
## ENDPOINT 05 — GET /api/alunos/7/matriculas (erro)

**Documentação atual (extraída da wiki da EscolaTech):**

```text
GET /api/alunos/7/matriculas
Se o aluno 7 não existe, responde:
200 OK + "<html><b>Erro: aluno nao existe!</b></html>"
O app mobile quebra tentando fazer parse do JSON.
```

1. Qual(is) problema(s) de design vocês identificam?
    Resposta:
        1. Retorna 200 OK quando o aluno não existe.
        2. Retorna HTML quando o cliente espera JSON.
        3. O erro deveria possuir uma resposta estruturada.
2. Seu redesenho (método + rota + status codes):
    Resposta: 
        GET /api/alunos/7/matriculas
        200 OK + JSON das matrículas
        404 Not Found + JSON de erro

## ENDPOINT 06 — PUT /api/atualizarNotaParcial?aluno=7&disc=12&nota=8.5

**Documentação atual (extraída da wiki da EscolaTech):**

```text
PUT /api/atualizarNotaParcial?aluno=7&disc=12&nota=8.5
Atualiza SÓ a nota parcial da disciplina, sem body.
Todos os dados vão na query string.
Resposta: 200 OK + "OK".
```

1. Qual(is) problema(s) de design vocês identificam?
    Resposta:
        1. Uso de verbo na URL (atualizarNotaParcial).
        2. PUT está sendo usado para uma atualização parcial.
        3. Dados da alteração estão na query string em vez do body.
        4. A resposta poderia retornar o recurso atualizado.
2. Seu redesenho (método + rota + status codes):
    Resposta:
        PATCH /api/alunos/7/disciplinas/12
        Body: { "notaParcial": 8.5 }
        200 OK

## DESAFIO

1. A EscolaTech quer lançar mudanças na API sem quebrar o app mobile antigo, que não recebe atualização há 2 anos. Que decisão de design — que falta na API INTEIRA — resolve esse problema? Como ficariam as rotas?

É só usar versionamento de API:
POST   /api/getAlunos                                               =>   GET    /api/v1/alunos
GET    /deletarAluno?id=7                                           =>   DELETE /api/v1/alunos/7
POST   /api/alunos                                                  =>   POST   /api/v1/alunos
GET    /escolas/1/turmas/3/alunos/25/matriculas/88/disciplinas/12   =>   GET    /api/v1/matriculas/88/disciplinas/12
GET    /api/alunos/7/matriculas                                     =>   GET    /api/v1/alunos/7/matriculas
PUT    /api/atualizarNotaParcial?aluno=7&disc=12&nota=8.5           =>   PATCH  /api/v1/alunos/7/disciplinas/12
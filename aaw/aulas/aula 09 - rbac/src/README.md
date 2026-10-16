# API Biblioteca — Operação Crachá, Nível 2

Projeto prático da **Aula 09 — RBAC, ABAC e PBAC (autorização)**.

Você recebe a API de Biblioteca **como terminou a Aula 08**: o login emite JWT e a escrita
já exige token. Esta cópia já contém as correções de autorização (403) descritas nesta aula:
RBAC com policies, negar por padrão, ABAC com validação de propriedade e PBAC com política
específica avaliada antes da regra geral.

As falhas originais e suas correções estão resumidas abaixo. Use a coleção do Postman para
verificar que os acessos permitidos e negados correspondem à matriz.

## Como rodar

```bash
cd src
dotnet run
```

- API: `http://localhost:5291`
- Swagger: `http://localhost:5291/swagger`
- Dados em memória (sem banco): reiniciar a API restaura os dados iniciais.

## Usuários fictícios

| Id | Nome | E-mail | Senha | Papel |
|---|---|---|---|---|
| 1 | Ana | `ana@biblioteca.com` | `ana123` | `Leitor` |
| 2 | Bruno | `bruno@biblioteca.com` | `bruno123` | `Leitor` |
| 3 | Carla | `carla@biblioteca.com` | `carla123` | `Pesquisador` |
| 4 | Beto | `biblio@biblioteca.com` | `biblio123` | `Bibliotecario` |
| 5 | Dora | `admin@biblioteca.com` | `admin123` | `Admin` |

Dados iniciais: livros 1–3 são `Comum`, o livro 4 (Os Lusíadas, 1572) é `ObraRara`.
Empréstimo **101** é da Ana; empréstimo **102** é do Bruno.

## A matriz de acesso (a especificação que a API deveria cumprir)

| Ação | Endpoint | Leitor | Pesquisador | Bibliotecario | Admin | Modelo |
|---|---|---|---|---|---|---|
| Consultar acervo | `GET /api/livros` | ✓ (até sem login) | ✓ | ✓ | ✓ | público |
| Cadastrar / editar livro | `POST`, `PUT /api/livros` | ✗ | ✗ | ✓ | ✓ | RBAC |
| Remover livro | `DELETE /api/livros/{id}` | ✗ | ✗ | ✗ | ✓ | RBAC |
| Relatórios (dados pessoais) | `GET /api/relatorios/*` | ✗ | ✗ | ✗ | ✓ | RBAC + negar por padrão |
| Renovar empréstimo | `POST /api/emprestimos/{id}/renovar` | só o **próprio** | só o **próprio** | qualquer | ✗ | ABAC |
| Emprestar livro comum | `POST /api/emprestimos` | ✓ | ✓ | ✓ | ✗ | PBAC |
| Emprestar obra rara | `POST /api/emprestimos` | ✗ | ✓ | ✓ | ✗ | PBAC |

## As 4 falhas corrigidas

| # | Problema original | Correção aplicada |
|---|---|---|---|
| 1 | **RBAC** — permissões de livros não correspondiam à matriz | Policies `GerenciarAcervo` (Bibliotecario/Admin) e `RemoverLivro` (somente Admin) aplicadas aos endpoints |
| 2 | **Negar por padrão** ausente e relatórios sem papel exigido | `FallbackPolicy` exige autenticação; policy `VerRelatorios` restringe relatórios ao Admin |
| 3 | **ABAC** verificava apenas o papel | Bibliotecario renova qualquer empréstimo; Leitor/Pesquisador só renovam quando o `NameIdentifier` corresponde ao `UsuarioId` |
| 4 | **PBAC** — regra genérica sombreava a regra de obra rara | `politicas.json` avalia primeiro a categoria específica `ObraRara`, depois a regra geral |

## Roteiro de verificação

1. Suba a API, importe `BibliotecaApi.postman_collection.json` e rode a coleção inteira
   (*Run collection*). Os testes cobrem os resultados esperados para cada papel e recurso.
2. Confira especialmente os casos de fronteira: Leitor não cadastra livro; Bibliotecario não
   remove; relatórios sem token retornam 401 e para não-Admin retornam 403; Ana não renova o
   empréstimo do Bruno; Leitor não empresta obra rara, mas Pesquisador pode.
3. Faça a verificação cruzada com Bruno, Carla, Beto e Dora, além de Ana, para validar que a
   autorização considera tanto o papel quanto o recurso e a política aplicável.

**Entregável:** a coleção do Postman inteira verde (*Run collection*), com print do antes e do depois.

## RBAC × ABAC × PBAC — o que cada falha ensina

- **RBAC** (Falhas 1 e 2): a permissão vem do **papel**. Simples e auditável — desde que a matriz
  esteja no código **inteira** e que o esquecimento vire 403, não vazamento.
- **ABAC** (Falha 3): a decisão combina **atributos** do sujeito, do recurso, da ação e do contexto.
  "Professor pode lançar nota" é RBAC; "Professor lança nota **na sua turma**" é ABAC.
- **PBAC** (Falha 4): as regras viram um **documento de política** separado do código, avaliado por
  um motor (PDP) e aplicado pelo endpoint (PEP). Quem responde pela regra de negócio pode
  revisá-la e mudá-la sem um deploy. No mundo real: OPA/Rego, AWS Cedar, XACML.

O detalhamento das decisões e dos resultados esperados está em `GABARITO.txt`.

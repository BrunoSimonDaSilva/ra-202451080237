# Aula 10 — Respostas da prática

## Exercício 1.1 — Endpoints

| Método | Rota | Operação | Parâmetros |
|---|---|---|---|
| POST | `/api/auth/login` | Autentica o bibliotecário e emite JWT | Corpo: `usuario`, `senha` |
| GET | `/api/autores` | Lista autores e livros associados | Nenhum |
| GET | `/api/autores/{id}` | Consulta autor | `id` inteiro |
| POST | `/api/autores` | Cadastra autor | Corpo `Autor` |
| PUT | `/api/autores/{id}` | Atualiza autor | `id` e corpo `Autor` |
| DELETE | `/api/autores/{id}` | Remove autor | `id` inteiro |
| GET | `/api/livros` | Lista livros paginados | `pagina` (padrão 1), `tamanhoPagina` (padrão 10, máximo 50) |
| GET | `/api/livros/{id}` | Consulta livro | `id` inteiro |
| GET | `/api/livros/buscar` | Busca por título, gênero e/ou autor | `titulo`, `genero`, `autorId` opcionais |
| POST | `/api/livros` | Cadastra livro | Corpo `Livro` |
| PUT | `/api/livros/{id}` | Atualiza livro | `id` e corpo `Livro` |
| DELETE | `/api/livros/{id}` | Remove livro | `id` inteiro |
| GET | `/api/emprestimos` | Lista empréstimos | JWT |
| GET | `/api/emprestimos/{id}` | Consulta empréstimo | `id` e JWT |
| POST | `/api/emprestimos` | Cria empréstimo | Corpo `Emprestimo` e JWT |
| PATCH | `/api/emprestimos/{id}/devolver` | Registra devolução | `id` e JWT |

## Exercícios 1.2 e 1.3 — Modelos e respostas

- **Autor:** `Nome` obrigatório (2–200 caracteres); `Pais` até 100; `Biografia` até 1000; possui livros.
- **Livro:** `Titulo` obrigatório (1–300); `AnoPublicacao` entre 1000 e 2100; `Isbn` até 20; `Genero` até 100; `AutorId` obrigatório; disponibilidade booleana.
- **Emprestimo:** `LivroId` e `NomeUsuario` obrigatórios; nome até 200; datas de empréstimo/devolução e status. Relação funcional: um autor tem vários livros, e cada empréstimo referencia um livro.
- Autor, livro ou empréstimo inexistente retorna **404**. Empréstimo com livro inexistente ou indisponível retorna **409**. Dados inválidos retornam **400** por validação do modelo.

## Conferência do contrato

- A API informa título, descrição e contato; os endpoints estão agrupados por Autores, Livros, Empréstimos e Autenticação.
- XML comments e `[ProducesResponseType]` descrevem operações, parâmetros, exemplos e códigos HTTP.
- O esquema Bearer JWT existe em `components.securitySchemes`; o requisito `security` aparece somente nas operações protegidas, não no login ou nos endpoints públicos.
- Em `/swagger/v1/swagger.json`, `security` referencia `Bearer` e `components.securitySchemes.Bearer` declara `type: http`, `scheme: bearer` e `bearerFormat: JWT`.
- Fluxo de autorização: `POST /api/auth/login` com `bibliotecario` / `newton123`, copiar `token`, clicar em **Authorize** e executar uma operação de empréstimo.

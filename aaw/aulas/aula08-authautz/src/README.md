# Biblioteca API — AuthN/AuthZ

API de biblioteca em ASP.NET Core para praticar autenticação e autorização com JWT.

## Objetivo

A API começa com o CRUD de livros aberto para qualquer pessoa. A prática consiste em:

- autenticar o usuário;
- emitir um JWT no login;
- proteger endpoints de escrita;
- permitir exclusão somente para a role `Bibliotecario`.

## Como rodar

```bash
cd src
dotnet restore
dotnet run
```

A API fica em:

- `http://localhost:5290`
- Swagger: `http://localhost:5290/swagger`

Observação: a biblioteca é mantida em memória. Ao reiniciar a API, os dados voltam ao estado inicial.

## Usuários de teste

| E-mail | Senha | Role |
|---|---|---|
| `leitor@biblioteca.com` | `leitor123` | `Leitor` |
| `biblio@biblioteca.com` | `biblio123` | `Bibliotecario` |

## Fluxo da autenticação

1. Faça login em `POST /api/auth/login`
2. Receba o JWT no corpo da resposta
3. Envie o token no header:

```http
Authorization: Bearer <seu-token>
```

## Endpoints principais

| Método | Rota | Acesso |
|---|---|---|
| GET | `/api/livros` | público |
| GET | `/api/livros/{id}` | público |
| POST | `/api/livros` | autenticado |
| PUT | `/api/livros/{id}` | autenticado |
| DELETE | `/api/livros/{id}` | autenticado + role `Bibliotecario` |
| POST | `/api/auth/login` | login |
| POST | `/api/auth/login-expirado` | gera token expirado para testar erro |
| GET | `/api/quem-sou-eu` | retorna claims do token |

## Comportamento esperado

- sem token em `POST /api/livros` → `401 Unauthorized`
- token de `Leitor` em `DELETE /api/livros/{id}` → `403 Forbidden`
- token de `Bibliotecario` em `DELETE /api/livros/{id}` → `204 No Content`
- token expirado → `401 Unauthorized`

## 401 vs 403

- `401 Unauthorized`: o sistema não reconhece o usuário ou o token não é válido
- `403 Forbidden`: o usuário está autenticado, mas não tem permissão para aquela ação

## Observações

- o payload do JWT pode ser lido em `jwt.io`;
- o conteúdo do token não é criptografado, apenas assinado para impedir adulteração;
- nunca coloque senha ou dados sensíveis dentro do payload.

## Arquivos úteis

- `Program.cs`: configuração do JWT
- `Controllers/AuthController.cs`: login e emissão do token
- `Controllers/LivrosController.cs`: endpoints protegidos
- `BibliotecaApi.postman_collection.json`: coleção para teste no Postman

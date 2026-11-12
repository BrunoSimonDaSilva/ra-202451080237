# Aula 10 — Documentação de APIs (OpenAPI/Swagger) — Prática

Projeto da prática "Operação Contrato": uma API de Biblioteca documentada com
OpenAPI/Swagger, incluindo descrições, respostas, exemplos e autenticação JWT.

## Conteúdo desta pasta

| Item | O que é |
|---|---|
| `BibliotecaApi/` | API .NET com autores, livros e empréstimos |
| `GUIA-DA-AULA.md` | Roteiro da prática |
| `Diagramas/` | Quatro diagramas Mermaid de arquitetura |
| `RESPOSTAS.md` | Respostas dos exercícios de mapeamento e conferência |
| `openapi.json` | Contrato OpenAPI gerado pela API e pronto para importar no Postman |
| `GABARITO-Aula10.txt` | Gabarito de referência (uso do professor) |

## Implementação concluída

- Swagger UI na raiz, com metadados, XML comments, exemplos e respostas documentadas.
- Esquema Bearer/JWT aplicado somente às operações marcadas com `[Authorize]`.
- Especificação OpenAPI em `/swagger/v1/swagger.json`.
- Os endpoints de empréstimo usam JWT. Credenciais didáticas: `bibliotecario` / `newton123`.

## Como verificar e exportar

```powershell
cd BibliotecaApi
dotnet restore
dotnet run
```

Abra a URL/porta informada pelo ASP.NET Core para usar o Swagger UI. O contrato já
exportado em `openapi.json` foi salvo a partir de `/swagger/v1/swagger.json`; importe-o
no Postman. Para regenerá-lo após mudanças, baixe novamente essa rota.

## Entregável

Swagger UI completo, botão **Authorize** funcional nos endpoints protegidos e coleção
do Postman importada a partir do `openapi.json`.

## Requisitos

.NET 6 SDK, VS Code, Postman e navegador. O projeto usa `net6.0` e
`Swashbuckle.AspNetCore` 6.5.0.

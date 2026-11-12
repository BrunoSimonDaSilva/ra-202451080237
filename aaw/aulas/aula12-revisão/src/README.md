# LojaRevisao — Operação Pente-Fino

Projeto de revisão das aulas 01 a 11. O código da API foi corrigido para cumprir as 11
regras listadas em `Aula 12 - Atividade.docx`; as respostas das partes escritas estão em
[`Aula 12 - Respostas.md`](./Aula%2012%20-%20Respostas.md).

## Executar e validar

```powershell
dotnet test
dotnet run --project LojaRevisao.Api
```

- API: `http://localhost:5311`
- Swagger: `http://localhost:5311/swagger`
- O banco SQLite de demonstração é reinicializado a cada execução.
- Use a coleção `LojaRevisao.postman_collection.json` para conferir as 11 falhas corrigidas.

## Correções aplicadas

1. Pedidos reserva estoque por meio do contrato `ICatalogo`.
2. Produto inexistente retorna 404; cadastro retorna 201 com `Location`.
3. Cancelamento usa `DELETE /api/v1/pedidos/{id}` e retorna 204.
4. Listagem de produtos tem paginação, ordenação e limite máximo de 100 itens.
5. Estoque e pedido só são persistidos depois de validar todos os itens.
6. Notificações são publicadas na fila em vez de aguardar o envio de e-mail.
7. Busca usa consulta LINQ parametrizada pelo EF Core.
8. Tokens expirados são rejeitados.
9. Apenas Admin remove produtos; pedidos são acessíveis pelo dono ou Admin.
10. OpenAPI descreve as respostas reais e inclui Bearer/JWT somente nas operações protegidas.
11. Testes verificam o limite inclusivo de frete grátis a partir de R$ 300,00.

## Usuários de demonstração

| Id | E-mail | Senha | Papel |
|---|---|---|---|
| 1 | `ana@loja.com` | `admin123` | Admin |
| 42 | `joao@loja.com` | `joao123` | Cliente |
| 43 | `maria@loja.com` | `maria123` | Cliente |

O gabarito de professor foi mantido intacto.

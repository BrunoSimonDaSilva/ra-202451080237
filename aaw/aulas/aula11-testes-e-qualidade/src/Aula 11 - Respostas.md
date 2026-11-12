# Aula 11 — Caçada ao Bug: registro da solução

## Bug revelado pelos testes

O caso **subtotal = R$ 500,00** falhava: `CalcularDesconto` tratava esse valor como 5%, embora a regra diga 10% a partir de R$ 500, inclusive. A comparação foi corrigida de `>` para `>=`.

| Subtotal | Desconto | Frete | Total esperado |
|---:|---:|---:|---:|
| R$ 100,00 | R$ 0,00 | R$ 25,00 | R$ 125,00 |
| R$ 199,99 | R$ 0,00 | R$ 25,00 | R$ 224,99 |
| R$ 200,00 | R$ 10,00 | R$ 25,00 | R$ 215,00 |
| R$ 299,99 | R$ 15,00 | R$ 25,00 | R$ 309,99 |
| R$ 300,00 | R$ 15,00 | R$ 0,00 | R$ 285,00 |
| R$ 499,99 | R$ 25,00 | R$ 0,00 | R$ 474,99 |
| R$ 500,00 | R$ 50,00 | R$ 0,00 | R$ 450,00 |
| R$ 600,00 | R$ 60,00 | R$ 0,00 | R$ 540,00 |

## Testes implementados

- Teoria com todos os casos da tabela, inclusive os limites.
- Quantidade zero lança `ArgumentException` e verifica `Salvar(..., Times.Never)`.
- Pedido vazio lança `InvalidOperationException` e não chama o repositório.
- Pedido válido verifica subtotal, desconto, frete, total e uma única chamada a `Salvar`.
- Integração POST valida 201 com `Location` e 400 com mensagem para pedido vazio.

## Validação

`dotnet test src/ResilientOrders.sln`: **14 aprovados, 0 falhos, 0 ignorados** (12 unitários e 2 de integração).

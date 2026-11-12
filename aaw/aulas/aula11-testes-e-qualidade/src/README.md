# Aula 11 — Testes e Qualidade dos Serviços · Caçada ao Bug

Projeto da atividade prática da **Aula 11** da disciplina *Arquitetura de
Aplicações Web* (Newton/Wyden).

> Stack: **.NET 8 · C# · xUnit · Moq · FluentAssertions · WebApplicationFactory**

---

## 🎯 Missão e estado atual

Você recebe uma API de pedidos funcional (`ResilientOrders.Api`) que **contém um
bug escondido** em uma regra de negócio. Ninguém vai te contar onde ele está: os
seus testes é que vão encontrá-lo. **A atividade está resolvida:** os testes revelaram
o limite incorreto em R$ 500,00, o serviço foi corrigido e a suíte está verde.

As regras do `PedidoService` (as mesmas do handout impresso):

| # | Regra |
|---|-------|
| R1 | Item com quantidade ≤ 0 torna o pedido inválido |
| R2 | Desconto: **5%** a partir de R$200 (inclusive) · **10%** a partir de R$500 (inclusive) |
| R3 | Frete fixo de **R$25**; grátis a partir de R$300 (inclusive) |
| R4 | Total = subtotal − desconto + frete |

## 🗺️ Roteiro (55 min, em duplas)

A prática começa logo depois da atividade em papel (handout, 20 min): a tabela
de casos que a dupla preencheu é o ponto de partida da Etapa 1.

| Etapa | Tempo | O que fazer |
|-------|-------|-------------|
| 0 | 5 min | `dotnet test` — todos os testes passam, sem testes ignorados |
| 1 | 15 min | A teoria usa a tabela completa, incluindo as bordas 199,99 · 200 · 299,99 · 300 · 499,99 · **500** |
| 2 | 10 min | O caso de R$ 500,00 revelou o erro de limite; o `PedidoService` usa a condição inclusiva correta |
| 3 | 10 min | Validações de quantidade e pedido vazio verificam exceções e `Mock.Verify(..., Times.Never)` |
| 4 | 15 min | Teste de integração do `POST /api/pedidos` valida 201 com `Location` e 400 |
| ★ | bônus | O pedido válido verifica valores calculados e uma única chamada ao repositório |

## ▶️ Como rodar

```bash
dotnet build                                  # compila tudo
dotnet test                                   # roda a suíte completa
dotnet test tests/ResilientOrders.UnitTests   # só os unitários
dotnet run --project src/ResilientOrders.Api  # sobe a API (Swagger em /swagger)
```

## 🗂️ Estrutura

```
src/
├── ResilientOrders.sln
├── src/ResilientOrders.Api/                  ← API corrigida
│   ├── Controllers/PedidosController.cs
│   ├── Models/Pedido.cs
│   └── Services/PedidoService.cs             ← classe sob teste
└── tests/
    ├── ResilientOrders.UnitTests/            ← testes unitários
    │   └── PedidoServiceTests.cs
    └── ResilientOrders.IntegrationTests/     ← testes de integração
        ├── ApiFactoryFixture.cs              ← fixture pronta (sobe a API em memória)
        └── PedidosEndpointTests.cs
```

## ✅ Critérios de aceite (entregável)

- [x] `[Theory]` com a tabela completa do handout, incluindo todas as bordas
- [x] Bug localizado, corrigido e documentado: subtotal de R$ 500,00
- [x] Validações com `Mock.Verify(..., Times.Never)`
- [x] Teste de integração cobrindo 201 (com `Location`) e 400
- [x] `dotnet test` 100% verde, sem testes ignorados

Registro da solução: [`Aula 11 - Respostas.md`](./Aula%2011%20-%20Respostas.md).

> **Professor:** o gabarito completo (bug, tabela e suíte) está em
> `GABARITO-PROFESSOR.txt` — não distribuir.

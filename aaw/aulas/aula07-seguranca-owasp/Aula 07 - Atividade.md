# HANDOUT — AULA 07

## Caça às Vulnerabilidades

*Revisão de segurança de uma API .NET — Arquitetura de Aplicações Web*

## 🎯 MISSÃO

Vocês são a dupla de revisores de segurança da empresa. Os 4 trechos abaixo são da MESMA API, prestes a ir para produção. Para CADA card:

- Descrevam a falha com as próprias palavras (não precisa do nome técnico ainda)
- Estimem o dano possível se isso chegar à produção
- Proponham a correção

*⏱️ Tempo: 30 minutos  |  👥 Formato: em duplas  |  Todo o código é fictício e roda apenas no laboratório.*

> **Nomes:** Bruno Simon Da Silva   **Turma:** ADS   **Data:** 18/09/2026

## VULNERABILIDADE 01 — A busca de clientes

> `GET /api/clientes/buscar?nome=...`

Endpoint de busca usado pela tela de atendimento. O parâmetro nome vem direto da caixa de busca do site.

```text
 1  [HttpGet("buscar")]
 2  public IActionResult Buscar(string nome)
 3  {
 4      var sql = "SELECT * FROM Clientes WHERE Nome = '"
 5                + nome + "'";
 6      var clientes = _db.Clientes.FromSqlRaw(sql).ToList();
 7      return Ok(clientes);
 8  }
```

**Sua análise:**

1. Qual é a falha?

O parâmetro `nome` vem diretamente da requisição e é colocado dentro da consulta SQL por concatenação de strings. Dessa forma, o usuário pode enviar um conteúdo que altere a consulta original.

Essa falha é conhecida como SQL Injection.

2. Qual o dano possível em produção?

O atacante pode conseguir consultar dados que não deveria acessar e, dependendo das permissões da conexão com o banco, também alterar ou excluir informações. Como a consulta retorna os resultados diretamente pela API, isso pode causar vazamento de dados dos clientes.

3. Como corrigir?

A consulta deve utilizar parâmetros, evitando concatenar valores fornecidos pelo usuário. Uma opção é usar LINQ. Também é importante que a conta usada para acessar o banco tenha somente as permissões necessárias.



## VULNERABILIDADE 02 — A consulta de faturas

> `GET /api/faturas/{id}`

Endpoint usado pelo app para exibir a fatura do cartão. O usuário está autenticado quando chama esta rota.

```text
 1  [HttpGet("{id}")]
 2  public IActionResult GetFatura(int id)
 3  {
 4      var fatura = _db.Faturas.Find(id);
 5      if (fatura == null) return NotFound();
 6      return Ok(fatura);
 7  }
```

**Sua análise:**

1. Qual é a falha?

A aplicação verifica apenas se a fatura existe. Ela não verifica se a fatura pertence ao usuário autenticado que está fazendo a requisição.
Assim, um usuário poderia tentar alterar o `id` da URL e consultar uma fatura pertencente a outra pessoa.
Essa falha é conhecida como BOLA/IDOR (Broken Object Level Authorization).

2. Qual o dano possível em produção?

Pode ocorrer vazamento de informações financeiras e pessoais de outros usuários. Se os IDs forem previsíveis, um atacante poderia tentar consultar vários registros de outras pessoas.

3. Como corrigir?

A consulta deve verificar também o usuário proprietário da fatura. O ponto principal é garantir que o usuário autenticado tenha autorização para acessar aquela fatura.


## VULNERABILIDADE 03 — A configuração do servidor

> `Program.cs (roda igual em dev e em produção)`

Trecho de inicialização da API, idêntico em todos os ambientes. Este arquivo está versionado no Git da empresa.

```text
 1  public const string Conn =
 2      "Server=prod-db;Database=Banco;User=sa;" +
 3      "Password=Newton@2026!";
 4
 5  var app = WebApplication.CreateBuilder(args).Build();
 6  app.UseDeveloperExceptionPage();
 7  app.Run();
```

**Sua análise:**

1. Qual é a falha?

Existem dois problemas principais.

Primeiro, a senha do banco está escrita diretamente no código-fonte e o arquivo está versionado no Git. Isso pode expor a credencial para qualquer pessoa que tenha acesso ao repositório ou ao histórico.

Além disso, está sendo utilizada a conta `sa`, que possui permissões muito elevadas.

Segundo, `UseDeveloperExceptionPage()` está sendo usado também em produção. Essa página pode revelar detalhes internos da aplicação quando ocorre um erro.

2. Qual o dano possível em produção?

Se a credencial do banco for obtida, alguém pode tentar acessar diretamente o banco. Dependendo das permissões, isso pode permitir leitura, alteração ou exclusão de dados.

A página de exceções de desenvolvimento também pode revelar informações internas que facilitam outros ataques.

3. Como corrigir?

As credenciais não devem ficar no código-fonte. Devem ser armazenadas em mecanismos apropriados de configuração e gerenciamento de segredos, como variáveis de ambiente ou um serviço de gerenciamento de secrets.

Também deve ser criada uma conta específica para a aplicação, com apenas as permissões necessárias, em vez de utilizar `sa`.

O tratamento de erros deve ser diferente por ambiente. Em produção, deve ser utilizado um tratamento de exceções que não mostre detalhes internos.

Como a senha já está exposta no código, em um cenário real ela também deveria ser substituída/rotacionada.



## VULNERABILIDADE 04 — A atualização de perfil

> `PUT /api/usuarios/{id}`

Endpoint que o app chama quando o usuário edita o próprio perfil. O corpo da requisição é o JSON enviado pelo cliente.

```text
 1  public class UsuarioUpdate
 2  {
 3      public string Nome  { get; set; }
 4      public string Email { get; set; }
 5      public string Role  { get; set; }   // "user" | "admin"
 6  }
 7
 8  [HttpPut("{id}")]
 9  public IActionResult Atualizar(int id, UsuarioUpdate dto)
10  {
11      _repo.AtualizarTudo(id, dto);
12      return NoContent();
13  }
```

**Sua análise:**

1. Qual é a falha?

O cliente pode enviar o campo `Role` no JSON e esse objeto inteiro é passado para `AtualizarTudo()`. Como `Role` define se o usuário é `"user"` ou `"admin"`, o próprio usuário pode tentar enviar `"admin"` para alterar seu nível de acesso.
Essa falha é conhecida como Mass Assignment/Overposting e pode causar elevação de privilégio.

2. Qual o dano possível em produção?

Um usuário comum poderia tentar transformar sua própria conta em administrador. Se a alteração for aceita, ele poderia obter acesso a funcionalidades e dados que deveriam estar disponíveis somente para administradores.

3. Como corrigir?

O DTO utilizado para editar o próprio perfil deve conter somente os campos que o usuário pode alterar:

```csharp
public class UsuarioUpdate
{
    public string Nome { get; set; }
    public string Email { get; set; }
}
```

A atualização também deve alterar somente esses campos.
Se a alteração de `Role` for necessária, ela deve ser feita por uma operação separada e protegida por autorização administrativa.
Além disso, para uma operação de edição do próprio perfil, é mais seguro identificar o usuário autenticado pelas informações da autenticação, em vez de confiar livremente no `id` enviado pela URL.



## DESAFIO

1. **Qual das 4 falhas um scanner automático de código teria MAIS dificuldade de encontrar? Por quê?**

A **Vulnerabilidade 02 — A consulta de faturas** provavelmente seria a mais difícil de encontrar automaticamente.

O código não possui uma construção claramente perigosa. O problema é que falta uma verificação de autorização: a aplicação busca a fatura pelo `id`, mas não verifica se ela pertence ao usuário autenticado.

Para identificar o problema, seria necessário entender o contexto da aplicação, o relacionamento entre usuário e fatura e a regra de negócio que determina quem pode acessar cada registro.

As outras vulnerabilidades possuem padrões mais fáceis de reconhecer:

- **01 — SQL Injection:** existe concatenação de entrada do usuário diretamente em SQL.
- **03 — Configuração insegura:** há uma senha escrita no código, uso de `sa` e página de exceção de desenvolvimento em produção.
- **04 — Mass Assignment/Overposting:** existe um campo sensível (`Role`) sendo recebido pelo cliente e enviado para uma atualização completa.

Portanto, a Vulnerabilidade 02 depende mais de **entender uma regra de autorização que está faltando** do que de identificar uma instrução de código explicitamente perigosa.

using LojaRevisao.Api.Data;
using LojaRevisao.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojaRevisao.Api.Controllers;

[ApiController]
[Route("api/v1/produtos")]
public class ProdutosController : ControllerBase
{
    private readonly LojaDbContext _db;

    public ProdutosController(LojaDbContext db) => _db = db;

    // Retorna uma página ordenada; o teto é imposto pelo servidor.
    [HttpGet]
    public IActionResult Listar([FromQuery] int page = 1, [FromQuery] int size = 20)
    {
        page = Math.Max(page, 1);
        size = Math.Clamp(size, 1, 100);

        var total = _db.Produtos.Count();
        var items = _db.Produtos
            .OrderBy(produto => produto.Id)
            .Skip((page - 1) * size)
            .Take(size)
            .ToList();

        return Ok(new Pagina<Produto>(page, size, total, items));
    }

    // A consulta LINQ deixa o EF Core parametrizar o termo de busca.
    [HttpGet("busca")]
    public IActionResult Buscar([FromQuery] string? nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return BadRequest(new { message = "O termo de busca é obrigatório." });

        var produtos = _db.Produtos
            .Where(produto => produto.Nome.Contains(nome))
            .OrderBy(produto => produto.Id)
            .Take(50)
            .ToList();

        return Ok(produtos);
    }

    [HttpGet("{id:int}")]
    public IActionResult Obter(int id)
    {
        var produto = _db.Produtos.Find(id);
        if (produto is null) return NotFound();
        return Ok(produto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Criar(NovoProduto novo)
    {
        var produto = new Produto { Nome = novo.Nome, Preco = novo.Preco, Estoque = novo.Estoque };
        _db.Produtos.Add(produto);
        _db.SaveChanges();
        return CreatedAtAction(nameof(Obter), new { id = produto.Id }, produto);
    }

    // Somente administradores podem alterar o catálogo.
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public IActionResult Remover(int id)
    {
        var produto = _db.Produtos.Find(id);
        if (produto is null) return NotFound();

        _db.Produtos.Remove(produto);
        _db.SaveChanges();
        return NoContent();
    }
}

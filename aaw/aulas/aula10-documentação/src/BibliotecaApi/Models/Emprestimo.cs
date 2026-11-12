using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Models;

/// <summary>Representa um empréstimo de livro.</summary>
public class Emprestimo
{
    /// <summary>Identificador único do empréstimo.</summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>Identificador do livro emprestado.</summary>
    /// <example>1</example>
    [Required]
    public int LivroId { get; set; }

    /// <summary>Nome da pessoa que realizou o empréstimo.</summary>
    /// <example>João Silva</example>
    [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
    [StringLength(200)]
    public string NomeUsuario { get; set; } = string.Empty;

    /// <summary>Data e hora em que o empréstimo foi realizado (UTC).</summary>
    public DateTime DataEmprestimo { get; set; } = DateTime.UtcNow;

    /// <summary>Data prevista para devolução (UTC).</summary>
    public DateTime DataDevolucaoPrevista { get; set; } = DateTime.UtcNow.AddDays(14);

    /// <summary>Data efetiva da devolução (UTC), ou nula enquanto ativo.</summary>
    public DateTime? DataDevolucaoEfetiva { get; set; }

    /// <summary>Situação atual do empréstimo.</summary>
    public StatusEmprestimo Status { get; set; } = StatusEmprestimo.Ativo;
}

/// <summary>Situações possíveis para um empréstimo.</summary>
public enum StatusEmprestimo
{
    Ativo,
    Devolvido,
    Atrasado
}

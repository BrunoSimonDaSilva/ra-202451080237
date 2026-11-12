namespace BibliotecaApi.Models;

/// <summary>Representa uma página de resultados com metadados de paginação.</summary>
/// <typeparam name="T">Tipo dos itens da página.</typeparam>
public class RespostaPaginada<T>
{
    /// <summary>Itens retornados nesta página.</summary>
    public List<T> Itens { get; set; } = new();

    /// <summary>Número da página atual, iniciado em 1.</summary>
    public int Pagina { get; set; }

    /// <summary>Quantidade máxima de itens por página.</summary>
    public int TamanhoPagina { get; set; }

    /// <summary>Total de itens disponíveis antes da paginação.</summary>
    public int TotalItens { get; set; }

    /// <summary>Total de páginas disponíveis.</summary>
    public int TotalPaginas => (int)Math.Ceiling((double)TotalItens / TamanhoPagina);
}

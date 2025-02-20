using Domain.Models;

namespace Domain.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetByFilters(string? title, string? authorName, string? publishYear, string[] subjects, int page, int pageSize, CancellationToken cancellationToken);
    Task Add(IEnumerable<Book> books, CancellationToken cancellationToken);
}
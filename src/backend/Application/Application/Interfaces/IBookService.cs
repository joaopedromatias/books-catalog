using Domain.Models;

namespace Application.Interfaces;

public interface IBookService
{
    Task<IEnumerable<Book>> GetByFilters(string? title, string? authorName, string? publishYear, string[] subjects, int page, int pageSize, CancellationToken cancellationToken);
    IEnumerable<string> GetSubjects();
    Task Add(IEnumerable<Book> book, CancellationToken cancellationToken);
}

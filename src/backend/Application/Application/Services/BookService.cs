using Application.Interfaces;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<Book>> GetByFilters(
        string? title, string? authorName, string? publishYear, string[] subjects, int page, int pageSize, CancellationToken cancellationToken)
    {
        return await _bookRepository.GetByFilters(title, authorName, publishYear, subjects, page, pageSize, cancellationToken);
    }

    public IEnumerable<string> GetSubjects()
    {
        return Enum.GetNames<Subject>();
    }    

    public async Task Add(IEnumerable<Book> books, CancellationToken cancellationToken)
    {
        await _bookRepository.Add(books, cancellationToken);
    }
}

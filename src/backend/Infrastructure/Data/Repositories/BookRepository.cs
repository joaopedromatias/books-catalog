using Data.Context;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.Interfaces;
using Domain.Enums;

namespace Data.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ILogger<BookRepository> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly BookContext _context;

    public BookRepository(
        ILogger<BookRepository> logger, IUnitOfWork unitOfWork, BookContext context) 
    { 
        _logger = logger;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetByFilters(
        string? title, string? authorName, string? publishYear, string[] subjects, int page, int pageSize, CancellationToken cancellationToken)
    {   
        try 
        { 
            var booksQuery = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                booksQuery = booksQuery.Where(book => book.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(authorName))
                booksQuery = booksQuery.Where(book => book.Author.Name.Contains(authorName));

            if (!string.IsNullOrWhiteSpace(publishYear))
                booksQuery = booksQuery.Where(book => book.PublishYear == int.Parse(publishYear));

            if (subjects.Length > 0)
            {
                var subjectEnums = subjects.Select(x => Enum.Parse<Subject>(x, ignoreCase: true)); 
                booksQuery = booksQuery.Where(book => subjectEnums.Contains(book.Subject));
            }

            booksQuery = booksQuery.OrderBy(book => book.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(book => book.Author)
                .AsNoTracking();

            var books = await booksQuery.ToListAsync(cancellationToken);

            return books;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error happened while reading book data");
            throw;
        }
    }

    public async Task Add(IEnumerable<Book> books, CancellationToken cancellationToken)
    {
        await _unitOfWork.ExecuteTransaction(async () => 
        {
            try
            {
                var existingAuthorIds = await GetExistingAuthorIds(books.Select(x => x.AuthorId));  
                var existingBookIds = await GetExistingBookIds(books.Select(x => x.Id));

                var trackedAuthors = new HashSet<int>();
                var trackedBooksSet = new HashSet<int>();
                
                foreach (var book in books)
                {
                    if (!trackedBooksSet.Contains(book.Id)) 
                    {
                        if (existingBookIds.Contains(book.Id))
                            _context.Entry(book).State = EntityState.Modified;
                        else
                            _context.Entry(book).State = EntityState.Added;
                        trackedBooksSet.Add(book.Id);
                    }

                    if (trackedAuthors.Contains(book.AuthorId)) 
                    {
                        _context.Entry(book.Author).State = EntityState.Detached;
                        continue;
                    }
                    
                    if (existingAuthorIds.Contains(book.AuthorId))
                        _context.Entry(book.Author).State = EntityState.Modified;
                    else
                        _context.Entry(book.Author).State = EntityState.Added;
                    trackedAuthors.Add(book.AuthorId);
                }
                
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error happened while inserting books data");
                throw;
            }
        });
    }

    private async Task<ISet<int>> GetExistingBookIds(IEnumerable<int> ids)
    {
        var existingBooks = await _context.Books
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking()
            .ToListAsync();

        return existingBooks.Select(x => x.Id).ToHashSet();
    }

    private async Task<ISet<int>> GetExistingAuthorIds(IEnumerable<int> ids)
    {
        var existingAuthors = await _context.Authors
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking()
            .ToListAsync();

        return existingAuthors.Select(x => x.Id).ToHashSet();
    }
}

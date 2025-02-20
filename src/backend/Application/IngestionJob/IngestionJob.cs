using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
using Domain.Factories;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ApplicationJob;

public class IngestionJob : IIngestionJob
{
    private readonly ILogger<IngestionJob> _logger;
    private readonly ILibraryClient _libraryClient;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public IngestionJob
        (ILogger<IngestionJob> logger, ILibraryClient libraryClient, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _libraryClient = libraryClient;
        _serviceScopeFactory = serviceScopeFactory;
    }    

    public async Task Start(CancellationToken cancellationToken)
    {
        try 
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();

            var libraryBooks = new List<BookDto>();
            var subjects = bookService.GetSubjects();

            foreach (var subject in subjects)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var response = await _libraryClient.GetBooksBySubject(subject);
                libraryBooks.AddRange(response);
                _logger.LogInformation($"Fetched {response.Count} books of subject {subject}");
            }

            var books = new List<Book>();

            foreach (var libraryBook in libraryBooks)
            {
                var book = BookFactory.Create(
                    libraryBook.Id, 
                    libraryBook.Title,
                    libraryBook.CoverUri, 
                    libraryBook.Subject,
                    libraryBook.PublishYear,
                    libraryBook.AuthorId,
                    libraryBook.AuthorName);

                books.Add(book);
            }

            cancellationToken.ThrowIfCancellationRequested();
            await bookService.Add(books, cancellationToken);
        } 
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Ingestion job was cancelled.");
        }
        catch (Exception ex)
        { 
            _logger.LogError($"An error happened during the ingestion job execution {ex.Message}");
            throw;
        }
    }
}

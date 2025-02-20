using BooksCatalogApi.ViewModels;
using Domain.Models;

namespace BooksCatalogApi.Translators;

public static class ViewModelTranslator
{
    public static BookViewModel ToViewModel(Book book)
    {
        return new BookViewModel() 
        { 
            Id = book.Id,
            AuthorName = book.Author.Name,
            CoverUri = book.CoverUri,
            PublishYear = book.PublishYear,
            Subject = book.Subject.ToString(),
            Title = book.Title
        };
    }
}

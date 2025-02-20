using Domain.Enums;
using Domain.Models;

namespace Domain.Factories;

public static class BookFactory
{
    public static Book Create(
        int id, string title, string coverUri, string subject, int publishYear, int authorId, string authorName)
    {
        var author = AuthorFactory.Create(authorId, authorName);
        
        ValidateId(id);
        ValidateTitle(title);
        ValidateCover(coverUri);
        ValidateSubject(subject);
        ValidatePublishYear(publishYear);
        
        var book = new Book(id, title, coverUri, Enum.Parse<Subject>(subject), publishYear, author);

        return book;
    }

    private static void ValidateId(int id)
    { 
        if (id <= 0) 
            throw new ArgumentException("Book id must be a positive number");
    }

    private static void ValidateTitle(string title)
    { 
        if (string.IsNullOrWhiteSpace(title)) 
            throw new ArgumentException("Title is required");
    }    

    private static void ValidateCover(string coverUri)
    { 
        if (string.IsNullOrWhiteSpace(coverUri)) 
            throw new ArgumentException("Cover URI is required");
    }     

    private static void ValidateSubject(string subject)
    { 
        if (string.IsNullOrWhiteSpace(subject)) 
            throw new ArgumentException("Subject is required");

        if (!Enum.IsDefined(typeof(Subject), subject))
        {
            throw new ArgumentException($"Subject {subject} is not valid.");
        }
    }            

    private static void ValidatePublishYear(int publishYear)
    { 
        if (publishYear < 0) 
            throw new ArgumentException("Publish year must not be a negative number");

        if (publishYear > DateTime.Now.Year) 
            throw new ArgumentException("Publish year must not be greater than current year");
    }       
}

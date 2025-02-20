using Domain.Enums;

namespace Domain.Models;

public class Book
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string CoverUri { get; private set; }
    public Subject Subject { get; private set; }
    public int PublishYear { get; private set; }
    public Author Author { get; private set; }
    public int AuthorId { get; private set; }

    public Book() { }

    internal Book(int id, string title, string coverUri, Subject subject, int publishYear, Author author) 
    { 
        Id = id;
        Title = title;
        CoverUri = coverUri;
        Subject = subject;
        PublishYear = publishYear;
        AuthorId = author.Id;
        Author = author;
    }
}

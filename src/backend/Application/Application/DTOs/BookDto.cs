namespace Application.DTOs;

public struct BookDto
{
    public int Id { get; set;}
    public string Title { get; set;}
    public string CoverUri { get; set;}
    public int PublishYear { get; set;}
    public string Subject { get; set;}
    public int AuthorId { get; set;}
    public string AuthorName { get; set;}
}

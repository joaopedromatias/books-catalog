namespace BooksCatalogApi.ViewModels;

public struct BookViewModel
{
    public int Id { get; set; }
    public string AuthorName { get; set; }
    public string CoverUri { get; set; }
    public int PublishYear { get; set; }
    public string Subject { get; set; }
    public string Title { get; set; }
}

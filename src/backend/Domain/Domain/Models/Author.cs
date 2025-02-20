namespace Domain.Models;

public class Author
{
    public int Id { get; private set; }
    public string Name { get; private set; }

    public IEnumerable<Book> Books { get; private set; } = new List<Book>();

    public Author() { }

    internal Author(int id, string name) 
    { 
        Id = id;
        Name = name;
    }
}

using Domain.Models;

namespace Domain.Factories;

public static class AuthorFactory
{
    public static Author Create(int id, string name)
    {
        ValidateId(id);
        ValidateName(name);

        var author = new Author(id, name);

        return author;
    }

    private static void ValidateId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Author id must be a positive number");
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Author name is required");
    }
}

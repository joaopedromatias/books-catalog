using Application.DTOs;

namespace Application.Interfaces;

public interface ILibraryClient
{
    Task<IList<BookDto>> GetBooksBySubject(string subject);
}

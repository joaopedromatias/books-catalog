using System.Text.RegularExpressions;
using Application.DTOs;
using Application.Interfaces;
using ExternalInterfaces.OpenLibrary.DTOs;
using Newtonsoft.Json;

namespace ExternalInterfaces.OpenLibrary.Clients;

public class OpenLibraryClient : ILibraryClient
{
    private readonly HttpClient _httpClient;

    public OpenLibraryClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IList<BookDto>> GetBooksBySubject(string subject)
    {
        var path = $"subjects/{subject.ToLower()}.json";
        var response = await _httpClient.GetAsync(path);

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var openLibraryBooks = JsonConvert.DeserializeObject<OpenLibraryBaseDTO>(content).Works;

            var bookDtos = openLibraryBooks.Select(x => new BookDto() 
            {
                Id = int.Parse(RemoveNonNumericChars(x.Key)),
                Title = x.Title,
                CoverUri = GetCoverUri(x.CoverId),
                Subject = subject,
                PublishYear = x.PublishYear,
                AuthorId = int.Parse(RemoveNonNumericChars(x.Authors.First().Key)),
                AuthorName = x.Authors.First().Name
            });

            return bookDtos.ToList();
        }
        else
        {
            throw new Exception($"Error while fetching OpenLibrary by subject {subject}: {response.Content}");
        }
    }

    private string RemoveNonNumericChars(string text)
    {
        return Regex.Replace(text, @"\D", ""); 
    }

    private string GetCoverUri(int? coverId)
    {
        return $"https://covers.openlibrary.org/b/id/{coverId}-M.jpg";
    }
}

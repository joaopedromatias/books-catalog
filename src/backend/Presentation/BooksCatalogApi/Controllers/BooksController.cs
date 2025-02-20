using Application.Interfaces;
using BooksCatalogApi.Translators;
using BooksCatalogApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BooksCatalogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IFilterValidationService _filterValidationService;

    public BooksController(
        IBookService bookService, IFilterValidationService filterValidationService)
    {
        _bookService = bookService;    
        _filterValidationService = filterValidationService;
    }

    [HttpGet]
    [Route("List")]
    public async Task<IActionResult> List(
            [FromQuery] string? authorName,
            [FromQuery] string? publishYear,
            [FromQuery] string[] subjects,
            [FromQuery] string? title,
            [FromQuery] string page,
            [FromQuery] string pageSize,
            CancellationToken cancellationToken)
    {
        try 
        { 
            var filterValidation = _filterValidationService.Validate(publishYear, subjects, page, pageSize);

            if (!filterValidation.IsValid)
                return BadRequest($"Invalid filters: {filterValidation.Message}");

            var books = await _bookService.GetByFilters(title, authorName, publishYear, subjects, int.Parse(page), int.Parse(pageSize), cancellationToken);

            var viewModelBooks = new List<BookViewModel>();

            foreach (var book in books)
                viewModelBooks.Add(ViewModelTranslator.ToViewModel(book));

            return Ok(viewModelBooks);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while fetching the books: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("Subjects")]
    public IActionResult GetSubjects()
    {
        try 
        { 
            var subjects = _bookService.GetSubjects();
            return Ok(subjects);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while fetching the subjects: {ex.Message}");
        }
    }    
}

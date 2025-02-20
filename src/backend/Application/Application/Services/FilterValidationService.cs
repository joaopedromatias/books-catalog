using Application.Interfaces;
using Domain.Enums;

namespace Application.Services;

public class FilterValidationService : IFilterValidationService
{
    public (bool IsValid, string Message) Validate(string? publishYear, string[] subjects, string page, string pageSize)
    {
        var publishYearValidationMessage = ValidatePublishYear(publishYear);

        if (!string.IsNullOrWhiteSpace(publishYearValidationMessage)) 
            return (false, publishYearValidationMessage);

        var subjectValidationMessage = ValidateSubject(subjects);

        if (!string.IsNullOrWhiteSpace(subjectValidationMessage)) 
            return (false, subjectValidationMessage);

        var pageValidationMessage = ValidatePage(page);

        if (!string.IsNullOrWhiteSpace(pageValidationMessage)) 
            return (false, pageValidationMessage);

        var pageSizeValidationMessage = ValidatePageSize(pageSize);

        if (!string.IsNullOrWhiteSpace(pageSizeValidationMessage)) 
            return (false, pageSizeValidationMessage);

        return (true, string.Empty);
    }

    private string ValidatePublishYear(string? publishYear) 
    {
        if (string.IsNullOrEmpty(publishYear))
            return string.Empty;

        var isNumeric = int.TryParse(publishYear, out var publishYearValue);

        if (!isNumeric)
           return "Publish year must be a numeric value";

        if (publishYearValue < 0)
            return "Publish year must not be a negative number";

        if (publishYearValue > DateTime.Now.Year)
            return "Publish year must not be greater than current year";

        return string.Empty;
    }

    private string ValidateSubject(string[] subjects) 
    {
        foreach (var subject in subjects) 
        {
            if (string.IsNullOrEmpty(subject))
                return string.Empty;

            var isValid = Enum.TryParse<Subject>(subject, ignoreCase: true, out _);

            if (!isValid)
            return $"Invalid book subject, it must be one of the following: {string.Join(", ", Enum.GetNames<Subject>())}";
        }

        return string.Empty;
    }

    private string ValidatePage(string page) 
    {
        var isNumeric = int.TryParse(page, out var pageValue);

        if (!isNumeric)
           return "Page must be a numeric value";

        if (pageValue < 1)
           return "Page must be a positive number";

        return string.Empty;
    }

    private string ValidatePageSize(string pageSize) 
    {
        var isNumeric = int.TryParse(pageSize, out var pageSizeValue);

        if (!isNumeric)
           return "Page size must be a numeric value";

        if (pageSizeValue < 1)
           return "Page size must be a positive number";

        if (pageSizeValue > 100)
            return "Page size must not be greater than 100"; 

        return string.Empty;
    }
}

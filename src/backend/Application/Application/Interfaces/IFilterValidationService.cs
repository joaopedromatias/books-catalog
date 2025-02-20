namespace Application.Interfaces;

public interface IFilterValidationService
{
    (bool IsValid, string Message) Validate(string? publishYear, string[] subjects, string page, string pageSize);
}

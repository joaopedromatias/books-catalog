namespace Application.Interfaces;

public interface IIngestionJob
{
    Task Start(CancellationToken cancellationToken);
}

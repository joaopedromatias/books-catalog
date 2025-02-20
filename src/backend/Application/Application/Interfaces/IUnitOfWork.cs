namespace Application.Interfaces;

public interface IUnitOfWork
{
    Task ExecuteTransaction(Func<Task> action);
}

using Application.Interfaces;
using Data.Context;

namespace Data.Transaction;

public class UnitOfWork : IUnitOfWork
{
    private readonly BookContext _context;

    public UnitOfWork(BookContext context)
    {
        _context = context;
    }

    public async Task ExecuteTransaction(Func<Task> action)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                await action();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw; 
            }
        }
    }
}

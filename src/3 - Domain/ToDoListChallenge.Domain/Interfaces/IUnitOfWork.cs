namespace ToDoListChallenge.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
    }
}

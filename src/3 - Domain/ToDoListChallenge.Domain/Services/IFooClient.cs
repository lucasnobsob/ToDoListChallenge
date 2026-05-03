using Refit;

namespace ToDoListChallenge.Domain.Services
{
    public interface IFooClient
    {
        [Get("/")]
        Task<object> GetAll();
    }
}

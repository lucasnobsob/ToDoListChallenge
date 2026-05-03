using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Domain.Entities;
using ToDoListChallenge.Domain.Interfaces;
using ToDoListChallenge.Infra.Data.Context;

namespace ToDoListChallenge.Infra.Data.Repository
{
    public class TaskItemRepository : Repository<TaskItem>, ITaskItemRepository
    {
        public TaskItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<TaskItem?> GetByTitle(string title)
        {
            return await DbSet.FirstOrDefaultAsync(t => EF.Functions.Like(t.Title, title));
        }
    }
}

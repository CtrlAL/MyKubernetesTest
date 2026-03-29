using Microsoft.EntityFrameworkCore;
using TaskService.Data.Interfaces;

namespace TaskService.Data
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _appDbContext;

        public TaskRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Create(Entities.Task entitiy)
        {
            ArgumentNullException.ThrowIfNull(entitiy);
            _appDbContext.Tasks.Add(entitiy);
        }

        public Task<List<Entities.Task>> GetAll()
        {
            return _appDbContext.Tasks.ToListAsync();
        }

        public async Task<Entities.Task> GetById(int id)
        {
            return await _appDbContext.Tasks.FirstOrDefaultAsync(x => x.Id == id) 
                ?? throw new InvalidOperationException();
        }

        public Task SaveChangesAsync()
        {
            return _appDbContext.SaveChangesAsync();
        }
    }
}

namespace TaskService.Data.Interfaces
{
    public interface ITaskRepository
    {
        Task SaveChangesAsync();
        Task<List<Entities.Task>> GetAll();
        Task<Entities.Task> GetById(int id);
        void Create(Entities.Task entitiy);
    }
}

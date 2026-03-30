namespace TaskService.Entities
{
    public class Task : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}

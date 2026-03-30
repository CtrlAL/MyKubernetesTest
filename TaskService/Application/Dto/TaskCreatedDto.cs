namespace TaskService.Dto
{
    public class TaskCreatedDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Event { get; set; } = null!;
    }
}
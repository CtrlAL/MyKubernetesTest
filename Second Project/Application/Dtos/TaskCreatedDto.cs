namespace NotificationService.Dtos
{
    public class TaskCreatedDto : GenericEventDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}

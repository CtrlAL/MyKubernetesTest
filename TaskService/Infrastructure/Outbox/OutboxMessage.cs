namespace TaskService.Outbox
{
    public sealed class OutboxMessage
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime OccuredOnUtc { get; set; }
        public DateTime? ProcededOnUtc { get; set; }
        public string? Error { get; set; }
    }
}

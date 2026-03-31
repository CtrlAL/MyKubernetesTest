namespace GraphService.Application.Dto
{
    public class EdgeCreatedDto
    {
        public int Id { get; set; }
        public int SourceNodeId { get; set; }
        public int TargetNodeId { get; set; }
    }
}

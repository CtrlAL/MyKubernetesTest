namespace GraphService.Domain.Entities
{
    public class Edge
    {
        public int Id { get; set; }

        public int SourceNodeId { get; set; }
        public Node SourceNode { get; set; } = null!;

        public int TargetNodeId { get; set; }
        public Node TargetNode { get; set; } = null!;
    }
}

namespace GraphService.Domain.Entities
{
    public class Node
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public List<Edge> OutgoingEdges { get; set; } = [];
        public List<Edge> IncomingEdges { get; set; } = [];
    }
}

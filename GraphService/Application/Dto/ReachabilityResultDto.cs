namespace GraphService.Application.Dto
{
    public class ReachabilityResultDto
    {
        public int SourceNodeId { get; set; }
        public int TargetNodeId { get; set; }
        public bool IsReachable { get; set; }
        public List<int>? Path { get; set; }
    }
}

using AutoMapper;
using GraphService.Application.Commands.CreateEdge;
using GraphService.Application.Commands.CreateNode;
using GraphService.Application.Dto;
using GraphService.Domain.Entities;
using GraphService.Presentation.Models;

namespace GraphService.Presentation.Profiles
{
    public class GraphProfile : Profile
    {
        public GraphProfile()
        {
            CreateMap<Node, ReadNodeDto>();
            CreateMap<Node, NodeCreatedDto>();
            CreateMap<Edge, EdgeCreatedDto>();
            CreateMap<CreateNodeModel, CreateNodeCommand>();
            CreateMap<CreateEdgeModel, CreateEdgeCommand>();
        }
    }
}

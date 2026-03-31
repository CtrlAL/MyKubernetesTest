using AutoMapper;
using GraphService.Application.Commands.CreateEdge;
using GraphService.Application.Commands.CreateNode;
using GraphService.Application.Queries.CheckReachability;
using GraphService.Application.Queries.GetAllNodes;
using GraphService.Presentation.Base;
using GraphService.Presentation.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GraphService.Presentation.Endpoints
{
    public class GraphEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/graph")
                .WithTags("Graph");

            group.MapGet("/nodes", GetAllNodes);
            group.MapPost("/nodes", CreateNode);
            group.MapPost("/edges", CreateEdge);
            group.MapGet("/reachability", CheckReachability);
        }

        private static async Task<IResult> GetAllNodes([FromServices] IMediator mediator)
        {
            var query = new GetAllNodesQuery();
            var result = await mediator.Send(query);
            return Results.Ok(result);
        }

        private static async Task<IResult> CreateNode(
            [FromBody] CreateNodeModel model,
            [FromServices] IMapper mapper,
            [FromServices] IMediator mediator)
        {
            var command = mapper.Map<CreateNodeCommand>(model);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }

        private static async Task<IResult> CreateEdge(
            [FromBody] CreateEdgeModel model,
            [FromServices] IMapper mapper,
            [FromServices] IMediator mediator)
        {
            var command = mapper.Map<CreateEdgeCommand>(model);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }

        private static async Task<IResult> CheckReachability(
            [FromQuery] int sourceNodeId,
            [FromQuery] int targetNodeId,
            [FromServices] IMediator mediator)
        {
            var query = new CheckReachabilityQuery(sourceNodeId, targetNodeId);
            var result = await mediator.Send(query);
            return Results.Ok(result);
        }
    }
}

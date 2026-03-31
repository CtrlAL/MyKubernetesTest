using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Commands.CreateTask;
using TaskService.Application.Queries.GetAllTask;
using TaskService.Domain.Shared;
using TaskService.Dto;
using TaskService.Models;
using TaskService.Presentation.Base;

namespace TaskService.Presentation.EndPoints
{
    public class TasksEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/task-endpoint")
                .WithTags("TaskEndpoint");

            group.MapGet("/", Get);
            group.MapPost("/", Post);
        }

        private static async Task<IResult> Get([FromServices] IMediator mediator)
        {
            var query = new GetAllQuery();
            var result = await mediator.Send(query);

            return Results.Ok(result);
        }

        private static async Task<IResult> Post(
            [FromBody] CreateTaskModel model,
            [FromServices] IMapper mapper,
            [FromServices] IMediator mediator)
        {
            var command = mapper.Map<CreateTaskCommand>(model);
            var result = await mediator.Send(command);

            return Results.Ok(result);
        }
    }
}

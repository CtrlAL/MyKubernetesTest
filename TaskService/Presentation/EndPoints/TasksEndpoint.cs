using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskService.Data.Interfaces;
using TaskService.Dto;
using TaskService.Models;
using TaskService.Presentation.Base;

namespace TaskService.Presentation.EndPoints
{
    public class TasksEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/task-endpoint");

            group.MapGet("/", Get);
            group.MapPost("/", Post);
        }

        public async Task<IResult> Get([FromServices] ITaskRepository taskRepository)
        {
            var tasks = await taskRepository.GetAll();
            return Results.Ok(tasks);
        }

        public async Task<IResult> Post([FromBody] CreateTaskModel model, IMapper mapper, ITaskRepository taskRepository)
        {
            var entity = mapper.Map<Entities.Task>(model);
            taskRepository.Create(entity);
            await taskRepository.SaveChangesAsync();

            return Results.Ok(mapper.Map<ReadTaskDto>(entity));
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskService.Data.Interfaces;
using TaskService.Dto;
using TaskService.Models;
using TaskService.Presentation.Base;

namespace TaskService.Presentation.EndPoints
{
    public class TasksEnpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/task-enpoint");

            group.MapGet("/", Get);
            group.MapPost("/", Post);
        }

        public async Task<IResult> Get([FromServices] ITaskRepository taskRepository)
        {
            var taks = await taskRepository.GetAll();
            return Results.Ok(taks);
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

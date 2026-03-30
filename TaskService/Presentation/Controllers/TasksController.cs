using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskService.Models;
using TaskService.Dto;
using TaskService.Data.Interfaces;
using MediatR;
using TaskService.Application.Commands.CreateTask;
using TaskService.Domain.Shared;
using TaskService.Application.Queries.GetAllTask;

namespace TaskService.Controllers
{
    [ApiController]
    [Route("api/t/[controller]")]
    [Produces("application/json")]
    public class TasksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ITaskRepository _taskRepository;

        public TasksController(IMapper mapper, IMediator mediator, ITaskRepository taskRepository)
        {
            _mapper = mapper;
            _mediator = mediator;
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Result<List<Entities.Task>>>> Get()
        {
            var query = new GetAllQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Result<ReadTaskDto>>> Post([FromBody] CreateTaskModel model)
        {
            var command = _mapper.Map<CreateTaskCommand>(model);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}

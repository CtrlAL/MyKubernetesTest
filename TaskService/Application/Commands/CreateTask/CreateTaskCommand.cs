using TaskService.Application.Messaging;
using TaskService.Domain.Shared;
using TaskService.Dto;

namespace TaskService.Application.Commands.CreateTask
{
    public sealed record class CreateTaskCommand(string Name) : ICommand<ReadTaskDto>
    {
    }
}

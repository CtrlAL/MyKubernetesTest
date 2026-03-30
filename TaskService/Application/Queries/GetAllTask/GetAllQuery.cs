using TaskService.Application.Messaging;
using TaskService.Dto;

namespace TaskService.Application.Queries.GetAllTask
{
    public sealed record class GetAllQuery() : IQuery<List<ReadTaskDto>>
    {
    }
}

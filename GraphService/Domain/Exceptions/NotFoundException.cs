using System.Net;

namespace GraphService.Domain.Exceptions
{
    public sealed class NotFoundException : AppException
    {
        public NotFoundException(string entity, object key)
            : base($"{entity} with key '{key}' was not found.", HttpStatusCode.NotFound)
        {
        }
    }
}

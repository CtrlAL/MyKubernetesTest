using System.Net;

namespace TaskService.Domain.Exceptions
{
    public sealed class BadRequestException : AppException
    {
        public BadRequestException(string message)
            : base(message, HttpStatusCode.BadRequest)
        {
        }
    }
}

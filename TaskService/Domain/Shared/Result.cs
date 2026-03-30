namespace TaskService.Domain.Shared
{
    public class Result<T> : Result
    {
        public T? Payload { get; set; }

        public Result()
        {
        }

        private Result(T payload) => Payload = payload;
        public static Result<T> Success(T payload) => new(payload);
    }

    public class Result
    {
        public Failure? Failure { get; set; }
        public bool IsSuccess => Failure == null;

        public Result()
        {
        }

        private Result(Failure failure) => Failure = failure;

        public static Result Fail(Failure failure) => new(failure);
        public static Result Success() => new();
    }
}

namespace GraphService.Domain.Shared
{
    public class Failure
    {
        public Dictionary<string, List<string>> Errors { get; }

        private Failure(string error, string property = "")
        {
            Errors = new Dictionary<string, List<string>>
            {
                { property, new List<string> { error } }
            };
        }

        public Failure()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        public Failure AddError(string error, string property = "")
        {
            if (Errors.ContainsKey(property))
            {
                Errors[property].Add(error);
            }
            else
            {
                Errors[property] = new List<string> { error };
            }

            return this;
        }

        public static Failure Create(string error = "", string property = "") =>
            new Failure(error, property);
    }
}

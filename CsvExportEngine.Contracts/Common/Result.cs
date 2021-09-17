namespace CsvExportEngine.Contracts
{
    public class Result : ResultBase
    {
        internal Result(ResultType resultType, string message)
            : base(resultType, isFailure: true, message: message)
        {
        }

        private Result()
            : base(ResultType.Ok, isFailure: false, message: string.Empty)
        {
        }

        public static Result Failed(string message)
        {
            return new Result(ResultType.InternalError, message);
        }

        public static Result NotFound(string message)
        {
            return new Result(ResultType.NotFound, message);
        }

        public static Result Ok()
        {
            return new Result();
        }
    }
}

namespace CsvExportEngine.Contracts
{
    using System.Net;

    public abstract class ResultBase
    {
        protected ResultBase(ResultType resultType, bool isFailure, string message)
        {
            ResultType = resultType;
            IsFailure = isFailure;
            Message = message;
        }

        public bool IsFailure { get; }

        public bool IsSuccess => !IsFailure;

        public string Message { get; }

        public ResultType ResultType { get; }

        public HttpStatusCode HttpStatusCode
        {
            get
            {
                switch (ResultType)
                {
                    case ResultType.Ok:
                        return HttpStatusCode.OK;
                    case ResultType.NotFound:
                        return HttpStatusCode.NotFound;
                    case ResultType.Invalid:
                        return HttpStatusCode.NotAcceptable;
                    case ResultType.NotAllowed:
                        return HttpStatusCode.MethodNotAllowed;
                    default:
                        return HttpStatusCode.InternalServerError;
                }
            }
        }
    }
}

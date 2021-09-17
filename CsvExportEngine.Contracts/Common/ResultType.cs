namespace CsvExportEngine.Contracts
{
    public enum ResultType
    {
        InternalError = 0,
        Ok = 1,
        NotFound = 2,
        Invalid = 5,
        NotAllowed = 8
    }
}
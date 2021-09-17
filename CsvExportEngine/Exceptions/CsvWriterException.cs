namespace CsvExportEngine.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CsvWriterException : Exception
    {
        public CsvWriterException() : base("An error occured while writing to the csv file") { }

        public CsvWriterException(string message) : base(message) { }

        public CsvWriterException(string message, Exception ex) : base(message, ex) { }

        protected CsvWriterException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

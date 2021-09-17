namespace CsvExportEngine.Extensions
{
    using System.IO;

    internal static class StreamWriterExtensions
    {
        /// <summary>
        /// Writes a value in the current stream, appends a delimiter after it, and if the field is the last one in the row, it does not append the delimiter
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <param name="totalNumberOfProperties"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        internal static StreamWriter CustomWrite(this StreamWriter streamWriter, object value, int index, int totalNumberOfProperties, string delimiter)
        {
            streamWriter.Write(value);

            if (index != totalNumberOfProperties - 1)
            {
                streamWriter.Write(delimiter);
            }

            if (index == totalNumberOfProperties - 1)
            {
                streamWriter.WriteLine();
            }

            return streamWriter;
        }
    }
}
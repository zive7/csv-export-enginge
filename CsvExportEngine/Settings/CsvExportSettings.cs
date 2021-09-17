namespace CsvExportEngine.Settings
{
    using System.Text;

    public static class CsvExportSettings
    {
        /// <summary>
        /// The delimiter used in between fields in the csv file
        /// </summary>
        public const string CSV_DELIMITER = ";";

        /// <summary>
        /// Indicates the charracter encoding
        /// </summary>
        public static readonly Encoding ENCODING = Encoding.UTF8;
    }
}
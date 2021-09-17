namespace CsvExportEngine.Settings
{
    /// <summary>
    /// Formats that can be used to indicate the style in which the date time will be represented in the csv file
    /// </summary>
    public static class DateTimeFormat
    {
        /// <summary>
        /// Displays the date time in the short date format (e.g January 5th 2021 will be displayed as 05/01/2021)
        /// </summary>
        public const string SHORT_DATE = "dd/MM/yyyy";

        /// <summary>
        /// Displays the date time in the short time format (e.g 2021-06-15T13:45:30 will be displayed as 13:45)
        /// </summary>
        public const string SHORT_TIME = "HH:mm";

        /// <summary>
        /// Displays the date time in the year/month format (e.g January 5th 2021 will be displayed as 01/2021)
        /// </summary>
        public const string YEAR_MONTH = "MM/yyyy";

        /// <summary>
        /// Displays the date time in the universal datetime format (e.g  2021-06-15T13:45:30 will be displayed as  2021-06-15 13:45:30Z)
        /// </summary>
        public const string UNIVERSAL = "u";
    }
}

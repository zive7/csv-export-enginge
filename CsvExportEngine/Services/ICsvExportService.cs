namespace CsvExportEngine.Services
{
    using CsvExportEngine.Contracts;
    using CsvExportEngine.Maps;
    using System;
    using System.Collections.Generic;

    public interface ICsvExportService
    {
        /// <summary>
        /// Validates csv configuration map file with selected exported properties
        /// </summary>
        /// <typeparam name="TMap"></typeparam>
        /// <param name="exportedProperties"></param>
        /// <returns></returns>
        Result ValidateCsvFile<TMap>(IEnumerable<string> exportedProperties) where TMap : ClassMap;

        /// <summary>
        /// Generates csv file with the given generic array of data and ILocalizationProviderService.GetLocalization delegate
        /// </summary>
        /// <typeparam name="T">The type of class which will be put in a csv file</typeparam>
        /// <typeparam name="TMap">The mapping class that will be used to map the appropriate felds</typeparam>
        /// <param name="data"></param>
        /// <param name="translate"></param>
        /// <returns></returns>
        byte[] GenerateCsvFile<T, TMap>(T[] data, Func<string, string> translate) where T : class where TMap : ClassMap;

        /// <summary>
        /// Generates csv file with the given generic array of data, selected exported properties by which we are going to generate the file,
        /// ILocalizationProviderService.GetLocalization delegate and (optional) ignore headers flag.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TMap"></typeparam>
        /// <param name="data"></param>
        /// <param name="exportedProperties"></param>
        /// <param name="translate"></param>
        /// <param name="ignoreHeaders"></param>
        /// <returns></returns>
        byte[] GenerateCsvFile<T, TMap>(T[] data, IEnumerable<string> exportedProperties, Func<string, string> translate, bool ignoreHeaders = false) where T : class where TMap : ClassMap;

        /// <summary>
        /// Generates csv file with header only data. The visual appearance of the header will depend on the selected exported properties and
        /// ILocalizationProviderService.GetLocalization delegate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TMap"></typeparam>
        /// <param name="exportedProperties"></param>
        /// <param name="translate"></param>
        /// <returns></returns>
        byte[] GenerateHeaderOnlyCsvFile<T, TMap>(IEnumerable<string> exportedProperties, Func<string, string> translate) where T : class where TMap : ClassMap;
    }
}
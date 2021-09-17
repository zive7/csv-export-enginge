namespace CsvExportEngine.Services
{
    using CsvExportEngine.Contracts;
    using CsvExportEngine.Exceptions;
    using CsvExportEngine.Helpers;
    using CsvExportEngine.Maps;
    using CsvExportEngine.Settings;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;


    public class CsvExportService : ICsvExportService
    {
        public CsvExportService()
        {
        }

        public Result ValidateCsvFile<TMap>(IEnumerable<string> exportedProperties) where TMap : ClassMap
        {
            TMap map = ReflectionHelper.CreateInstance<TMap>();

            if (map is null)
            {
                return Result.NotFound("Csv configuration map was not found.");
            }

            if (exportedProperties is null || !exportedProperties.Any())
            {
                return Result.Failed("Export properties list is not valid.");
            }

            if (exportedProperties.Any(ep => !map.GetProperties().Select(p => p.Name).Contains(ep)))
            {
                return Result.Failed("Some of the provided export properties were not found or valid.");
            }

            return Result.Ok();
        }

        public byte[] GenerateCsvFile<T, TMap>(T[] data, Func<string, string> translate)
            where T : class
            where TMap : ClassMap
        {
            try
            {
                CsvWriterConfiguration configuration = new CsvWriterConfiguration();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (StreamWriter streamWriter = new StreamWriter(memoryStream, CsvExportSettings.ENCODING))
                    {
                        configuration.Delimiter = CsvExportSettings.CSV_DELIMITER;
                        configuration.AddCustomRounding((number) => Math.Round(number));
                        configuration.AddTranslationService(translate);
                        configuration.RegisterClassMap<TMap>();
                        CsvWriter csvWriter = new CsvWriter(configuration, streamWriter);
                        csvWriter.Write(data);
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (CsvWriterException)
            {
                throw;
            }
        }

        public byte[] GenerateCsvFile<T, TMap>(T[] data, IEnumerable<string> exportedProperties, Func<string, string> translate, bool ignoreHeaders)
                where T : class
                where TMap : ClassMap
        {
            try
            {
                CsvWriterConfiguration configuration = new CsvWriterConfiguration();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (StreamWriter streamWriter = new StreamWriter(memoryStream, CsvExportSettings.ENCODING))
                    {
                        configuration.Delimiter = CsvExportSettings.CSV_DELIMITER;
                        configuration.AddCustomRounding((number) => Math.Round(number));
                        configuration.AddTranslationService(translate);
                        configuration.RegisterClassMap<TMap>();
                        CsvWriter csvWriter = new CsvWriter(configuration, streamWriter);
                        csvWriter.Write(data, exportedProperties, ignoreHeaders);
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (CsvWriterException)
            {
                throw;
            }
        }

        public byte[] GenerateHeaderOnlyCsvFile<T, TMap>(IEnumerable<string> exportedProperties, Func<string, string> translate)
                where T : class
                where TMap : ClassMap
        {
            try
            {
                CsvWriterConfiguration configuration = new CsvWriterConfiguration();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (StreamWriter streamWriter = new StreamWriter(memoryStream, CsvExportSettings.ENCODING))
                    {
                        configuration.Delimiter = CsvExportSettings.CSV_DELIMITER;
                        configuration.AddTranslationService(translate);
                        configuration.RegisterClassMap<TMap>();
                        CsvWriter csvWriter = new CsvWriter(configuration, streamWriter);
                        csvWriter.WriteHeaderOnly<T>(exportedProperties);
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (CsvWriterException)
            {
                throw;
            }
        }
    }
}
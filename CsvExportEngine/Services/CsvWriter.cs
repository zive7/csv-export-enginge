namespace CsvExportEngine.Services
{
    using CsvExportEngine.Exceptions;
    using CsvExportEngine.Extensions;
    using CsvExportEngine.Maps;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;

    public class CsvWriter
    {
        private readonly CsvWriterConfiguration _configuration;
        private readonly StreamWriter _streamWriter;

        public CsvWriter(CsvWriterConfiguration configuration, StreamWriter streamWriter)
        {
            _configuration = configuration;
            _streamWriter = streamWriter;
        }

        /// <summary>
        /// Writes all of the provided items in the csv file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        public void Write<T>(IEnumerable<T> items) where T : class
        {
            ClassMap<T> map = _configuration.FindMap<T>();

            if (map == null)
            {
                throw new CsvWriterException($"No mapping found for {typeof(T)}");
            }

            WriteHeader(map);

            WriteProperties(items, map);

            _streamWriter.Flush();
        }

        /// <summary>
        /// Writes all of the provided items in the csv file.
        /// Which columns are going to be shown and in what order dictates exported properties list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="exportedProperties"></param>
        /// <param name="ignoreHeaders"></param>
        public void Write<T>(IEnumerable<T> items, IEnumerable<string> exportedProperties, bool ignoreHeaders) where T : class
        {
            ClassMap<T> map = _configuration.FindMap<T>();

            if (map == null)
            {
                throw new CsvWriterException($"No mapping found for {typeof(T)}");
            }

            ValidatedProperties<T>(map, exportedProperties);

            map.IgnoreHeaders(ignoreHeaders);

            WriteHeader(map);

            WriteProperties(items, map);

            _streamWriter.Flush();
        }

        /// <summary>
        /// Writes only the header in the csv file according to the provided export properties list.
        /// The way how the header is going to look dictates exported properties list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exportedProperties"></param>
        public void WriteHeaderOnly<T>(IEnumerable<string> exportedProperties) where T : class
        {
            ClassMap<T> map = _configuration.FindMap<T>();

            if (map == null)
            {
                throw new CsvWriterException($"No mapping found for {typeof(T)}");
            }

            ValidatedProperties<T>(map, exportedProperties);

            WriteHeader(map);

            _streamWriter.Flush();
        }

        /// <summary>
        /// Validates configuration property maps.
        /// Checks if the provided export properties list is valid and whether all properties exist in the configuration map.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <param name="exportedProperties"></param>
        public void ValidatedProperties<T>(ClassMap<T> map, IEnumerable<string> exportedProperties) where T : class
        {
            if (exportedProperties is null || !exportedProperties.Any())
            {
                throw new CsvWriterException($"Export properties list is not valid for {typeof(T)}");
            }

            if (exportedProperties.Any(ep => !map.GetProperties().Select(p => p.Name).Contains(ep)))
            {
                throw new CsvWriterException($"Some of the provided export properties were not found or valid.");
            }

            map.ReEvaluatePropertyMaps(exportedProperties.ToList());
        }

        /// <summary>
        /// Writes the provided grouped items in the csv file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="groupedItems"></param>
        public void Write<T, TElement>(IEnumerable<IGrouping<TElement, T>> groupedItems) where T : class
        {
            foreach (IGrouping<TElement, T> grouping in groupedItems)
            {
                Write(grouping);
            }
        }

        /// <summary>
        /// Writes the header of the csv file if they are not ignored
        /// </summary>
        /// <typeparam name="T">The type of the items that are being written</typeparam>
        /// <param name="map">The mapping of the <typeparamref name="T"/> class</param>
        private void WriteHeader<T>(ClassMap<T> map) where T : class
        {
            if (!map.AreHeadersIgnored)
            {
                IReadOnlyList<PropertyMap> properties = map.GetProperties();

                for (int i = 0; i < properties.Count; i++)
                {
                    PropertyMap propertyMap = properties.ElementAt(i);

                    string propertyName = propertyMap.Name;

                    if (propertyMap.IsHeaderTranslatable)
                    {
                        propertyName = Translate(propertyMap.HeaderTranslation);
                    }

                    _streamWriter.CustomWrite(propertyName, i, properties.Count, _configuration.Delimiter);
                }
            }
        }

        /// <summary>
        /// Writes the fields into the csv file
        /// </summary>
        /// <typeparam name="T">The type of the items that are being written</typeparam>
        /// <param name="items">The list of items that are being written</param>
        /// <param name="map">The mapping of the <typeparamref name="T"/> class</param>
        private void WriteProperties<T>(IEnumerable<T> items, ClassMap<T> map) where T : class
        {
            IReadOnlyList<PropertyMap> properties = map.GetProperties();

            foreach (T item in items)
            {
                for (int i = 0; i < properties.Count; i++)
                {
                    PropertyMap propertyMap = properties.ElementAt(i);

                    MemberExpression propertyExpression = map.GetPropertyExpression(propertyMap.Name);

                    object propertyValue = propertyExpression.GetValue(item);

                    WriteProperty(propertyMap, propertyValue, i, properties.Count, _configuration.Delimiter);
                }
            }
        }

        /// <summary>
        /// Writes a given field into the csv file based on the <see cref="PropertyMap"/>, index, total number of properties and delimiter
        /// </summary>
        /// <param name="propertyMap">The mapping configuration of the property</param>
        /// <param name="value">The value of the property</param>
        /// <param name="index">The index of the file (order in which it is written)</param>
        /// <param name="totalNumberOfProperties">The total number of properties which are going to be written in the given line</param>
        /// <param name="delimiter">The delimiter used to seperate the properties</param>
        private void WriteProperty(PropertyMap propertyMap, object value, int index, int totalNumberOfProperties, string delimiter)
        {
            switch (value)
            {
                case string stringField:
                    WriteProperty(propertyMap, stringField, index, totalNumberOfProperties, delimiter);
                    break;
                case decimal decimalField:
                    WriteProperty(propertyMap, decimalField, index, totalNumberOfProperties, delimiter);
                    break;
                case DateTime dateTimeField:
                    WriteProperty(propertyMap, dateTimeField, index, totalNumberOfProperties, delimiter);
                    break;
                default:
                    _streamWriter.CustomWrite(value, index, totalNumberOfProperties, delimiter);
                    break;
            }
        }

        /// <summary>
        /// Writes the given <see cref="decimal"/> value based on the <see cref="PropertyMap"/>, index, total number of properties and delimiter
        /// </summary>
        /// <param name="propertyMap">The mapping configuration of the property</param>
        /// <param name="value">The value of the property</param>
        /// <param name="index">The index of the file (order in which it is written)</param>
        /// <param name="totalNumberOfProperties">The total number of properties which are going to be written in the given line</param>
        /// <param name="delimiter">The delimiter used to seperate the properties</param>
        private void WriteProperty(PropertyMap propertyMap, decimal value, int index, int totalNumberOfProperties, string delimiter)
        {
            if (propertyMap.IsRounded)
            {
                value = _configuration.RoundingDelegate.Invoke(value);
            }

            _streamWriter.CustomWrite(value, index, totalNumberOfProperties, delimiter);
        }


        /// <summary>
        /// Writes the given <see cref="string"/> value based on the <see cref="PropertyMap"/>, index, total number of properties and delimiter
        /// </summary>
        /// <param name="propertyMap">The mapping configuration of the property</param>
        /// <param name="value">The value of the property</param>
        /// <param name="index">The index of the file (order in which it is written)</param>
        /// <param name="totalNumberOfProperties">The total number of properties which are going to be written in the given line</param>
        /// <param name="delimiter">The delimiter used to seperate the properties</param>
        private void WriteProperty(PropertyMap propertyMap, string value, int index, int totalNumberOfProperties, string delimiter)
        {
            if (propertyMap.IsValueTranslatable)
            {
                value = propertyMap.ValueResourceKey ?? value;

                if (propertyMap.ResourceKeyDelegate != null)
                {
                    value = propertyMap.ResourceKeyDelegate.Invoke(value);
                }

                value = Translate(value);
            }

            _streamWriter.CustomWrite(value, index, totalNumberOfProperties, delimiter);
        }

        /// <summary>
        /// Writes the given <see cref="DateTime"/> value based on the <see cref="PropertyMap"/>, index, total number of properties and delimiter
        /// </summary>
        /// <param name="propertyMap">The mapping configuration of the property</param>
        /// <param name="value">The value of the property</param>
        /// <param name="index">The index of the file (order in which it is written)</param>
        /// <param name="totalNumberOfProperties">The total number of properties which are going to be written in the given line</param>
        /// <param name="delimiter">The delimiter used to seperate the properties</param>
        private void WriteProperty(PropertyMap propertyMap, DateTime value, int index, int totalNumberOfProperties, string delimiter)
        {
            string stringValue = value.ToString();

            if (propertyMap.DateTimeFormat != null)
            {
                stringValue = value.ToString(propertyMap.DateTimeFormat);
            }

            _streamWriter.CustomWrite(stringValue, index, totalNumberOfProperties, delimiter);
        }

        /// <summary>
        /// Translates the given value with the translation delegate of the csv configuration, and
        /// if the value does not have a translation, it returns the value itself
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string Translate(string value)
        {
            string translatedValue = _configuration.TranslationDelegate.Invoke(value);

            return string.IsNullOrWhiteSpace(translatedValue) ? value : translatedValue;
        }
    }
}
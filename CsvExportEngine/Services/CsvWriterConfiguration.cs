namespace CsvExportEngine.Services
{
    using CsvExportEngine.Exceptions;
    using CsvExportEngine.Helpers;
    using CsvExportEngine.Maps;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class CsvWriterConfiguration
    {
        private readonly Dictionary<Type, ClassMap> _maps;

        internal Func<decimal, decimal> RoundingDelegate = (decimal item) => Math.Round(item);

        internal Func<string, string> TranslationDelegate = (string item) => item;

        public string Delimiter { get; set; } = ";";

        public CsvWriterConfiguration()
        {
            _maps = new Dictionary<Type, ClassMap>();
        }

        /// <summary>
        /// Registers the class map so it can be later used by the csv writer to map out properties accordingly
        /// </summary>
        /// <typeparam name="TMap"></typeparam>
        public void RegisterClassMap<TMap>() where TMap : ClassMap
        {
            TMap map = ReflectionHelper.CreateInstance<TMap>();

            if (map.GetProperties().Count == 0)
            {
                throw new CsvWriterException("No mappings were specified in the ClassMap.");
            }

            if (!typeof(ClassMap).IsAssignableFrom(typeof(TMap)))
            {
                throw new CsvWriterException("The class map does not inherit from ClassMap");
            }

            Add(map);
        }

        /// <summary>
        /// Adds a custom rounding function to transform the <see cref="decimal"/> values
        /// </summary>
        /// <param name="roundingDelegate"></param>
        public void AddCustomRounding(Func<decimal, decimal> roundingDelegate)
        {
            RoundingDelegate = roundingDelegate;
        }

        /// <summary>
        /// Adds a custom translation service to use when translating properties
        /// </summary>
        /// <param name="translationDelegate"></param>
        public void AddTranslationService(Func<string, string> translationDelegate)
        {
            TranslationDelegate = translationDelegate;
        }

        /// <summary>
        /// Returns a <see cref="ClassMap{TClass}"/> for the given <typeparamref name="T"/> type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal ClassMap<T> FindMap<T>() where T : class
        {
            return (ClassMap<T>)_maps[typeof(T)];
        }

        /// <summary>
        /// Adds the provided <see cref="ClassMap"/> in the dictionary of class maps
        /// </summary>
        /// <param name="map"></param>
        private void Add(ClassMap map)
        {
            var type = GetGenericClassMapType(map.GetType()).GetGenericArguments().First();

            if (_maps.ContainsKey(type))
            {
                _maps[type] = map;
            }
            else
            {
                _maps.Add(type, map);
            }
        }

        /// <summary>
        /// Returns the generic class map type from the given <see cref="Type"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Type GetGenericClassMapType(Type type)
        {
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(ClassMap<>))
            {
                return type;
            }

            return GetGenericClassMapType(type.GetTypeInfo().BaseType);
        }
    }
}

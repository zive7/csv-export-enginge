namespace CsvExportEngine.Extensions
{
    using CsvExportEngine.Maps;
    using System;

    public static class PropertyMapExtensions
    {
        /// <summary>
        /// Rounds the value of the <see cref="decimal"/> property
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="propertyMap"></param>
        /// <returns></returns>
        public static PropertyMap<TClass, decimal> Rounded<TClass>(this PropertyMap<TClass, decimal> propertyMap)
        {
            propertyMap.IsRounded = true;

            return propertyMap;
        }

        /// <summary>
        /// Sets the format of the <see cref="DateTime"/> property
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="propertyMap"></param>
        /// <param name="dateTimeFormat"></param>
        /// <returns></returns>
        public static PropertyMap<TClass, DateTime> Format<TClass>(this PropertyMap<TClass, DateTime> propertyMap, string dateTimeFormat)
        {
            propertyMap.DateTimeFormat = dateTimeFormat;

            return propertyMap;
        }

        /// <summary>
        /// Indicates that the value of the <see cref="string"/> property is translatable
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="propertyMap"></param>
        /// <returns></returns>
        public static PropertyMap<TClass, string> Translatable<TClass>(this PropertyMap<TClass, string> propertyMap)
        {
            propertyMap.IsValueTranslatable = true;

            return propertyMap;
        }

        /// <summary>
        /// Sets a translation resource key on the <see cref="string"/> property.
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="propertyMap"></param>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public static PropertyMap<TClass, string> Translatable<TClass>(this PropertyMap<TClass, string> propertyMap, string resourceKey)
        {
            if (string.IsNullOrWhiteSpace(resourceKey))
            {
                throw new ArgumentNullException(nameof(resourceKey), "Provided translation resource key cannot be empty");
            }

            Translatable(propertyMap);

            propertyMap.ValueResourceKey = resourceKey;

            return propertyMap;
        }

        /// <summary>
        /// Sets a translation resource key from a provided function on the <see cref="string"/> property.
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="propertyMap"></param>
        /// <param name="resourceKey"></param

        public static PropertyMap<TClass, string> Translatable<TClass>(this PropertyMap<TClass, string> propertyMap, Func<string, string> resourceKey)
        {
            Translatable(propertyMap);

            propertyMap.ResourceKeyDelegate = resourceKey;

            return propertyMap;
        }
    }
}

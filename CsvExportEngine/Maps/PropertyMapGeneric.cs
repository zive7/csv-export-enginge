namespace CsvExportEngine.Maps
{
    using System;
    using System.Reflection;

    public class PropertyMap<TClass, TMember> : PropertyMap
    {
        internal PropertyInfo Property { get; private set; }

        public PropertyMap(MemberInfo member)
        {
            Property = member as PropertyInfo;
        }

        /// <summary>
        /// Indicates that the property's header is translatable and has a provided translation key
        /// </summary>
        /// <param name="translation"></param>
        /// <returns></returns>
        public PropertyMap<TClass, TMember> WithHeaderTranslation(string translation)
        {
            if (string.IsNullOrWhiteSpace(translation))
            {
                throw new ArgumentNullException(nameof(translation), "Translation Key Cannot be empty");
            }

            HeaderTranslation = translation;

            return this;
        }


        /// <summary>
        /// Indicates the index of the property (in which order it will be displayed)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PropertyMap<TClass, TMember> WithIndex(int index)
        {
            if (index < default(decimal))
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be less than zero");
            }

            Index = index;

            return this;
        }

        /// <summary>
        /// Indicates that the field is ignored (will not be displayed in the csv file)
        /// </summary>
        /// <param name="ignore"></param>
        /// <returns></returns>
        public PropertyMap<TClass, TMember> IsIgnored(bool ignore = true)
        {
            Ignore = ignore;

            return this;
        }
    }
}

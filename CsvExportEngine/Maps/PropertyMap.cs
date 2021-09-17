namespace CsvExportEngine.Maps
{
    using CsvExportEngine.Extensions;
    using CsvExportEngine.Helpers;
    using System;
    using System.Reflection;

    public class PropertyMap
    {
        internal string Name { get; private set; }

        internal bool IsValueTranslatable { get; set; }

        internal Func<string, string> ResourceKeyDelegate { get; set; }

        internal string ValueResourceKey { get; set; }

        internal string HeaderTranslation { get; set; }

        internal bool IsHeaderTranslatable => !string.IsNullOrWhiteSpace(HeaderTranslation);

        internal bool IsRounded { get; set; }

        internal int Index { get; set; }

        internal string DateTimeFormat { get; set; }

        internal bool Ignore { get; set; }

        /// <summary>
        /// Creates a generic instance of <see cref="PropertyMap{TClass, TMember}"/> based on the given property <see cref="Type"/> and <see cref="MemberInfo"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        internal static PropertyMap CreateGeneric(Type type, MemberInfo memberInfo)
        {
            var propertyMapType = typeof(PropertyMap<,>).MakeGenericType(type, memberInfo.MemberType());
            var propertyMap = (PropertyMap)ReflectionHelper.CreateInstance(propertyMapType, memberInfo);

            return propertyMap.MapFields(memberInfo);
        }


        /// <summary>
        /// Maps the name of the property's information <see cref="MemberInfo"/>
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        private PropertyMap MapFields(MemberInfo memberInfo)
        {
            Name = memberInfo.Name;

            return this;
        }
    }
}

namespace CsvExportEngine.Maps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class ClassMap
    {
        internal bool AreHeadersIgnored { get; private set; }

        internal Type ClassType { get; private set; }

        private readonly Dictionary<MemberExpression, PropertyMap> _properties;

        internal ClassMap(Type type)
        {
            ClassType = type;
            _properties = new Dictionary<MemberExpression, PropertyMap>();
        }

        /// <summary>
        /// Indicates that the header will not be displayed in the csv file
        /// </summary>
        /// <returns></returns>
        public ClassMap IgnoreHeaders(bool ignoreHeaders)
        {
            AreHeadersIgnored = ignoreHeaders;

            return this;
        }

        /// <summary>
        /// Maps the field with <see cref="Type"/> and <see cref="MemberExpression"/> to a <see cref="PropertyMap"/>
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        internal PropertyMap Map(Type classType, MemberExpression memberExpression)
        {
            PropertyMap propertyMap = PropertyMap.CreateGeneric(classType, memberExpression.Member);

            propertyMap.Index = GetCurrentIndex() + 1;

            _properties.Add(memberExpression, propertyMap);

            return propertyMap;
        }

        /// <summary>
        /// Returns a list of all of the <see cref="PropertyMap"/> for the <see cref="ClassMap{TClass}"/>
        /// </summary>
        /// <returns></returns>
        internal IReadOnlyList<PropertyMap> GetProperties()
        {
            return _properties.Values.Where(x => !x.Ignore).OrderBy(x => x.Index).ToArray();
        }

        /// <summary>
        /// Returns a list of member expressions for the <see cref="ClassMap{TClass}"/>
        /// </summary>
        /// <returns></returns>
        internal IReadOnlyList<MemberExpression> GetMemberExpressions()
        {
            return _properties.Where(x => !x.Value.Ignore).OrderBy(x => x.Value.Index).Select(x => x.Key).ToArray();
        }

        /// <summary>
        /// Reevaluation of the property map list is a process which is trying to reindex and ignore the property maps according to the provided exported property list
        /// </summary>
        /// <param name="exportedProperties"></param>
        internal void ReEvaluatePropertyMaps(List<string> exportedProperties)
        {
            _properties.Values.Select(property =>
            {
                if (exportedProperties.Contains(property.Name))
                {
                    property.Index = exportedProperties.IndexOf(property.Name);
                }
                else
                {
                    property.Ignore = true;
                    property.Index = -1;
                }
                return property;
            }).ToArray();
        }

        /// <summary>
        /// Returns the last index of the properties in the <see cref="ClassMap{TClass}"/>
        /// </summary>
        /// <returns></returns>
        private int GetCurrentIndex()
        {
            if (_properties.Count == 0)
            {
                return -1;
            }

            return _properties.Values.Max(x => x.Index);
        }
    }
}

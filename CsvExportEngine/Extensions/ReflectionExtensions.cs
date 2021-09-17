namespace CsvExportEngine.Extensions
{
    using System;
    using System.Reflection;

    internal static class ReflectionExtensions
    {
        /// <summary>
        /// Returns the <see cref="Type"/> of the given <see cref="MemberInfo"/> (if it is either <see cref="PropertyInfo"/> or <see cref="FieldInfo"/>
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        internal static Type MemberType(this MemberInfo member)
        {
            if (member is PropertyInfo propertyInfo)
            {
                return propertyInfo.PropertyType;
            }

            if (member is FieldInfo fieldInfo)
            {
                return fieldInfo.FieldType;
            }

            throw new InvalidOperationException("Member is not a property or field");
        }
    }
}
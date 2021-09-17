namespace CsvExportEngine.Extensions
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    internal static class PropertyInfoExtensions
    {
        /// <summary>
        /// Gets the value of the property based on the <see cref="MemberExpression"/> and the provided <typeparamref name="T"/> instance
        /// </summary>
        /// <typeparam name="T">The type of the class instance</typeparam>
        /// <param name="propertyInfo">The member expression of the property</param>
        /// <param name="item">The class instance</param>
        /// <returns></returns>
        internal static object GetValue<T>(this MemberExpression propertyInfo, T item)
        {
            PropertyInfo propInfo = propertyInfo.Member as PropertyInfo;

            if (propInfo.DeclaringType.Equals(typeof(T)))
            {
                return propInfo.GetValue(item);
            }

            return GetPropertyValueFromExpressionTree(propertyInfo, item);
        }

        /// <summary>
        /// Gets the value of the property from the given <see cref="MemberExpression"/> and <typeparamref name="T"/> instance 
        /// if the field has a lambda expression which accesses a nested class(es)
        /// Example : (x => x.Address.Street)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyInfo"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private static object GetPropertyValueFromExpressionTree<T>(MemberExpression propertyInfo, T item)
        {
            Expression expression = propertyInfo;

            Stack<MemberInfo> memberInfos = new Stack<MemberInfo>();

            while (expression is MemberExpression)
            {
                MemberExpression memberExpr = expression as MemberExpression;
                memberInfos.Push(memberExpr.Member);
                expression = memberExpr.Expression;
            }

            object objReference = item;

            while (memberInfos.Count > 0)
            {
                MemberInfo memberInfo = memberInfos.Pop();

                if (memberInfo.MemberType == MemberTypes.Property)
                {
                    objReference = objReference.GetType()
                                               .GetProperty(memberInfo.Name)
                                               .GetValue(objReference);
                }

                if (memberInfo.MemberType == MemberTypes.Field)
                {
                    objReference = objReference.GetType()
                                               .GetField(memberInfo.Name)
                                               .GetValue(objReference);
                }
            }


            return objReference;
        }
    }
}

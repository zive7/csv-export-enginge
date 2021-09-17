namespace CsvExportEngine.Maps
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Linq.Expressions;

    public abstract class ClassMap<TClass> : ClassMap where TClass : class
    {
        protected ClassMap() : base(typeof(TClass))
        {
        }

        /// <summary>
        /// Maps the selected property and returns a <see cref="PropertyMap{TClass, TMember}"/> to configure the mapped field
        /// By default, fields that are not mapped are not taken into consideration in the csv file
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public PropertyMap<TClass, TMember> Map<TMember>(Expression<Func<TClass, TMember>> expression)
        {
            MemberExpression memberExpression = GetMemberExpression(expression);

            if (memberExpression == null)
            {
                throw new InvalidOperationException($"No members found in expression {expression}");
            }

            if (typeof(IEnumerable).IsAssignableFrom(typeof(TMember)) && typeof(TMember) != typeof(string))
            {
                throw new NotSupportedException("Properties which are collections are currently not supported");
            }

            return (PropertyMap<TClass, TMember>)Map(typeof(TClass), memberExpression);
        }

        /// <summary>
        /// Returns a <see cref="MemberExpression"/> of the property for the provided name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal MemberExpression GetPropertyExpression(string name)
        {
            return GetMemberExpressions().FirstOrDefault(x => x.Member.Name == name);
        }

        /// <summary>
        /// Returns a <see cref="MemberExpression"/> from the given <see cref="Expression"/> function
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        private MemberExpression GetMemberExpression<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            Expression expressionBody = expression.Body;

            return GetMemberExpression(expressionBody);
        }

        /// <summary>
        /// Extracts the <see cref="MemberExpression"/> from the given <see cref="Expression"/>
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private MemberExpression GetMemberExpression(Expression expression)
        {
            MemberExpression memberExpression = null;

            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression as MemberExpression;
            }

            return memberExpression;
        }
    }
}

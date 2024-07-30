using System.Linq.Expressions;
using System.Reflection;

namespace lsccommon.configLang.queryContract.Extensions
{
    /// <summary>
    /// Extensions for "Expression" class
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Get property name as string from expression 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>The property name as a string, or null if it cannot be determined</returns>
        public static string? GetPropertyName(this Expression expression)
        {
            // Cast the expression to a LambdaExpression
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                // Get the operand of the unary expression
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                // Cast the lambda body directly to a MemberExpression
                memberExpression = lambda.Body as MemberExpression;
            }

            if (memberExpression != null)
            {
                // Get the property name
                var propertyInfo = memberExpression.Member as PropertyInfo;
                return propertyInfo.Name;
            }

            return null;
        }
    }
}
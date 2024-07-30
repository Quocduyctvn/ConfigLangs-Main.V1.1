using lsccommon.configLang.commandContract.Extensions;
using System.Linq.Expressions;

namespace System
{
	/// <summary>
	/// Contain constant for application message
	/// </summary>
	public class MessageConstant
	{
		/// <summary>
		/// Represent not found string with key and value not found
		/// </summary>
		/// <remarks>
		/// Ex: Lang with Id = 123 was not found
		/// </remarks>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="keyNotFound"></param>
		/// <param name="value"></param>
		/// <returns>A string indicating the entity with the specified key and value was not found</returns>
		public static string NotFound<TEntity>(Expression<Func<TEntity, object>> keyNotFound, object value)
		{
			return $"{typeof(TEntity).Name} with {keyNotFound.GetPropertyName()} = {value} was not found";
		}

		/// <summary>
		/// Represent not null string
		/// </summary>
		/// <returns>A string indicating the entity can't be null</returns>
		public static string NotNull<TEntity>()
		{
			return $"{typeof(TEntity).Name} can't be null";
		}

		/// <summary>
		/// Represent not null or empty string
		/// </summary>
		/// <returns>A string indicating the entity can't be null or empty</returns>
		public static string NotNullOrEmpty<TEntity>()
		{
			return $"{typeof(TEntity).Name} can't be null or empty";
		}

		/// <summary>
		/// Represent not null or empty property string 
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="property"></param>
		/// <returns>A string indicating the property of the entity can't be null or empty</returns>
		public static string NotNullOrEmpty<TEntity>(Expression<Func<TEntity, object>> property)
		{
			return $"{property.GetPropertyName()} of {typeof(TEntity).Name} can't be null or empty";
		}

		/// <summary>
		/// Represent property can't be lower than a value string 
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <returns>A string indicating the property of the entity can't be lower than the specified value</returns>
		public static string NotLowerThan<TEntity>(Expression<Func<TEntity, object>> property, object value)
		{
			return $"{property.GetPropertyName()} of {typeof(TEntity).Name} can't be lower than {value}";
		}

		/// <summary>
		/// Represent property can't be lower than or equal  a value string 
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <returns>A string indicating the property of the entity can't be lower than or equal the specified value</returns>
		public static string NotLowerThanOrEqual<TEntity>(Expression<Func<TEntity, object>> property, object value)
		{
			return $"{property.GetPropertyName()} of {typeof(TEntity).Name} can't be lower than or equal {value}";
		}

		/// <summary>
		/// Represent property can't be less than a value string 
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <returns>A string indicating the property of the entity can't be less than the specified value</returns>
		public static string NotLessThan<TEntity>(Expression<Func<TEntity, object>> property, object value)
		{
			return $"{property.GetPropertyName()} of {typeof(TEntity).Name} can't be less than {value}";
		}

		/// <summary>
		/// Represent property can't be exceeded a value string 
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string NotExceed<TEntity>(Expression<Func<TEntity, object>> property, object value)
		{
			return $"{property.GetPropertyName()} of {typeof(TEntity).Name} can't be exceed {value}";
		}
	}
}
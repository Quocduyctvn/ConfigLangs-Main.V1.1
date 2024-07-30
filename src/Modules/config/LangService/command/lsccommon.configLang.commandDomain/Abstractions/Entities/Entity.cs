namespace lsccommon.configLang.commandDomain.Abstractions.Entities
{
	/// <summary>
	/// Domain entity
	/// </summary>
	public abstract class Entity<TKey> : IEntity
	{
		/// <summary>
		/// Primary key of entity
		/// </summary>
		public virtual TKey Id { get; set; }

		/// <summary>
		/// Check if entity is transient
		/// </summary>
		/// <returns>True if entity is transient, otherwise return false</returns>
		public bool IsTransient()
		{
			return Id!.Equals(default);
		}
	}
}
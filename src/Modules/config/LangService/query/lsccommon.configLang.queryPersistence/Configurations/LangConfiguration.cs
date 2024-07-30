
using lsccommon.configLang.queryDomain.Entities;
using lsccommon.configLang.queryPersistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace lsccommon.configLang.queryPersistence.Configurations
{
	/// <summary>
	/// Entity configuration for the 'Lang' entity to define its database schema.
	/// </summary>
	public class LangConfiguration : IEntityTypeConfiguration<Lang>
	{
		/// <summary>
		/// Configures the 'Lang' entity.
		/// </summary>
		/// <param name="builder">The builder used to configure the entity type.</param>
		public void Configure(EntityTypeBuilder<Lang> builder)
		{
			// Sets the table name for the 'Lang' entity in the database
			builder.ToTable(TableName.ConfigLangsTable);

			// Configures the primary key for the 'Lang' entity
			builder.HasKey(x => x.Id);
		}
	}
}
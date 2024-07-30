using lsccommon.configLang.queryDomain.Abstractions.Aggregates;

namespace lsccommon.configLang.queryDomain.Entities
{
	/// <summary>
	/// Domain entity with string key type
	/// </summary>
	public class Lang : AggregateRoot<string>
	{
		public const int LangIdxKeyMaximumLength = 64;
		public const int LangDescriptionMaximumLength = 255;
		public const int LangVnMaximumLength = 255;
		public const int LangEnMaximumLength = 255;



		/// <summary>
		/// Description of Lang
		/// </summary>
		public string? description { get; set; }

		/// <summary>
		/// Name of notice by code VN
		/// </summary>
		public string vn { get; set; }

		/// <summary>
		/// Name of notice by code EN
		/// </summary>
		public string? en { get; set; }


		/// <summary>
		/// Default constructor
		/// </summary>
		public Lang()
		{
		}

		/// <summary>
		/// Contractor Lang 
		/// </summary>
		/// <param name="idxkey">Id for entities lang</param>
		/// <param name="description">description for entities lang</param>
		/// <param name="vn">vn for entities lang</param>
		/// <param name="en">en for entities lang</param>
		public Lang(string Idxkey, string Description, string Vn, string En)
		{
			Id = Idxkey;
			description = Description;
			vn = Vn;
			en = En;
		}


		public void TryUpdate()
		{

		}

	}
}
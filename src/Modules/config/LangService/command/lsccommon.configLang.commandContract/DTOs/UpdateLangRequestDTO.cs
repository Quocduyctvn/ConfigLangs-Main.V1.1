namespace lsccommon.configLang.commandContract.DTOs
{
	/// <summary>
	/// Data Transfer Object for updating language information.
	/// </summary>
	public class UpdateLangRequestDTO
	{
		/// <summary>
		/// Gets or sets the description of the language.
		/// </summary>
		public string? description { get; set; }

		/// <summary>
		/// Gets or sets the Vietnamese translation of the language.
		/// </summary>
		public string vn { get; set; }

		/// <summary>
		/// Gets or sets the English translation of the language.
		/// </summary>
		public string? en { get; set; }
	}
}
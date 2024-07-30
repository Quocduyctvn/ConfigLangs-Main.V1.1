using lsccommon.configLang.commandDomain.Abstractions.Aggregates;
using lsccommon.configLang.commandDomain.Exceptions;


namespace lsccommon.configLang.commandDomain.Entities
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
		/// <param name="idxkey"></param>
		/// <param name="description"></param>
		/// <param name="vn"></param>
		/// <param name="en"></param>
		public Lang(string Idxkey, string Description, string Vn, string En)
		{
			Id = Idxkey;
			description = Description;
			vn = Vn;
			en = En;
		}


		/// <summary>
		/// Try to update, throw validation exception if this Lang can't be updated
		/// </summary>
		/// <param name="IdxKey"></param>
		/// <param name="Description"></param>
		/// <param name="Vn"></param>
		/// <param name="En"></param>
		/// <exception cref="ValidationException"></exception>
		public void TryUpdate(string IdxKey, string? Description, string Vn, string? En)
		{
			// Initialize a list to collect validation errors
			var errors = new List<string>();
			// Validate idxKey is not null or empty
			if (string.IsNullOrEmpty(IdxKey))
			{
				errors.Add(MessageConstant.NotNullOrEmpty<Lang>(x => x.Id));
			}
			// Id requires full capitalization and does not contain space
			if (!funcCheckUpperCase(IdxKey))
			{
				errors.Add(MessageConstant.NotExceed<Lang>(x => x.Id,
					"Id requires full capitalization and does not contain space"));
			}
			// Validate idxKey is must be smaller than LangIdxKeyMaximumLength
			if (IdxKey.Length > LangIdxKeyMaximumLength)
			{
				errors.Add(MessageConstant.NotExceed<Lang>(x => x.Id, $"{LangIdxKeyMaximumLength} characters"));
			}

			if (Description?.Length > LangDescriptionMaximumLength)
			{
				errors.Add(MessageConstant.NotExceed<Lang>(x => x.description!,
					$"{LangDescriptionMaximumLength} characters"));
			}

			// Validate Vn is not null or empty
			if (string.IsNullOrEmpty(Vn))
			{
				errors.Add(MessageConstant.NotNullOrEmpty<Lang>(x => x.vn));
			}
			// Validate Vn is must be smaller than LangVnMaximumLength
			if (Vn.Length > LangVnMaximumLength)
			{
				errors.Add(MessageConstant.NotExceed<Lang>(x => x.vn, $"{LangVnMaximumLength} characters"));
			}

			// Validate Vn is must be smaller than LangEnMaximumLength
			if (En?.Length > LangEnMaximumLength)
			{
				errors.Add(MessageConstant.NotExceed<Lang>(x => x.en!,
					$"{LangEnMaximumLength} characters"));
			}

			// If there are validation errors, throw a ValidationException
			if (errors.Any())
			{
				throw new ValidationException(errors.ToArray());
			}
			// Update lang
			Id = IdxKey;
			description = Description;
			vn = Vn;
			en = En;
		}


		public static Lang TryCreate(string IdxKey, string? Description, string Vn, string? En)
		{
			// Initialize a list to collect validation errors
			var errors = new List<string>();
			// Validate idxKey is not null or empty
			if (string.IsNullOrEmpty(IdxKey))
			{
				errors.Add(MessageConstant.NotNullOrEmpty<Lang>(x => x.Id));
			}
			if (!funcCheckUpperCase(IdxKey))
			{
				errors.Add(MessageConstant.NotExceed<Lang>(x => x.Id,
					"Id requires full capitalization and does not contain space"));
			}
			// Validate idxKey is must be smaller than LangIdxKeyMaximumLength
			if (IdxKey.Length > LangIdxKeyMaximumLength)
			{
				errors.Add(MessageConstant.NotExceed<Lang>(x => x.Id, $"{LangIdxKeyMaximumLength} characters"));
			}

			if (Description?.Length > LangDescriptionMaximumLength)
			{
				errors.Add(MessageConstant.NotExceed<Lang>(x => x.description!,
					$"{LangDescriptionMaximumLength} characters"));
			}

			// Validate Vn is not null or empty
			if (string.IsNullOrEmpty(Vn))
			{
				errors.Add(MessageConstant.NotNullOrEmpty<Lang>(x => x.vn));
			}
			// Validate Vn is must be smaller than LangVnMaximumLength
			if (Vn.Length > LangVnMaximumLength)
			{
				errors.Add(MessageConstant.NotExceed<Lang>(x => x.vn, $"{LangVnMaximumLength} characters"));
			}

			// Validate Vn is must be smaller than LangEnMaximumLength
			if (En?.Length > LangEnMaximumLength)
			{
				errors.Add(MessageConstant.NotExceed<Lang>(x => x.en!,
					$"{LangEnMaximumLength} characters"));
			}

			// If there are validation errors, throw a ValidationException
			if (errors.Any())
			{
				throw new ValidationException(errors.ToArray());
			}


			// Create a new Lang
			var lang = new Lang(IdxKey, Description, Vn, En);
			return lang;
		}

		/// <summary>
		/// Functions to check capital letters
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool funcCheckUpperCase(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return false;
			}
			// Only check the letters, ignore other characters such as lower bricks
			return id.Where(char.IsLetter).All(char.IsUpper);
		}
	}
}
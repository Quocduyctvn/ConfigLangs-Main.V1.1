using FluentAssertions;
using lsccommon.configLang.commandDomain.Exceptions;

namespace lsccommon.configLang.commandDomain.Test
{
	/// <summary>
	/// Test class for Lang entity.
	/// </summary>
	public class LangTest
	{
		/// <summary>
		/// Tests if TryCreate method successfully creates a lang with valid data.
		/// </summary>
		[Fact]
		public void TryCreate_WithValidData_ShouldCreateLang()
		{
			// Arrange
			var Id = "CONFIG_ADD";
			var description = "Dùng để thêm";
			var vn = "Thêm";
			var en = "Add";

			// Act
			var lang = Entities.Lang.TryCreate(Id, description, vn, en);

			// Assert
			lang.Should().NotBeNull();
			lang.Id.Should().Be(Id);
			lang.description.Should().Be(description);
			lang.vn.Should().Be(vn);
			lang.en.Should().Be(en);
		}

		/// <summary>
		/// Tests if TryCreate method throws ValidationException for an invalid Id.
		/// </summary>
		[Fact]
		public void TryCreate_WithInvalidIdxkey_ShouldThrowValidationException()
		{
			// Arrange
			var Id = "";
			var description = "Dùng để thêm";
			var vn = "Thêm";
			var en = "Add";

			// Act
			Action act = () => Entities.Lang.TryCreate(Id, description, vn, en);

			// Assert
			var exception = act.Should().Throw<ValidationException>().Which;
			Assert.Contains(MessageConstant.NotNullOrEmpty<Entities.Lang>(x => x.Id), exception.ValidationResults);
		}

		/// <summary>
		/// Tests if TryCreate method throws ValidationException for an invalid Vietnamese translation.
		/// </summary>
		[Fact]
		public void TryCreate_WithInvalidVn_ShouldThrowValidationException()
		{
			// Arrange
			var Id = "CONFIG_ADD";
			var description = "Dùng để thêm";
			var vn = "";
			var en = "Add";

			// Act
			Action act = () => Entities.Lang.TryCreate(Id, description, vn, en);

			// Assert
			var exception = act.Should().Throw<ValidationException>().Which;
			Assert.Contains(MessageConstant.NotNullOrEmpty<Entities.Lang>(x => x.vn), exception.ValidationResults);
		}


		/// <summary>
		/// Tests if TryUpdate method successfully updates a lang with valid data.
		/// </summary>
		[Fact]
		public void TryUpdate_WithValidData_ShouldUpdateLang()
		{
			// Arrange
			var lang = Entities.Lang.TryCreate("CONFIG_ADD", "Dùng để thêm", "Thêm", "Add");
			var newIdxKey = "CONFIG_UPDATE";
			var newDescription = "Dùng để cập nhật";
			var newVn = "Cập nhật";
			var newEn = "Update";

			// Act
			lang.TryUpdate(newIdxKey, newDescription, newVn, newEn);

			// Assert
			lang.Id.Should().Be(newIdxKey);
			lang.description.Should().Be(newDescription);
			lang.vn.Should().Be(newVn);
			lang.en.Should().Be(newEn);
		}

		/// <summary>
		/// Tests if TryUpdate method throws ValidationException for an invalid Id.
		/// </summary>
		[Fact]
		public void TryUpdate_WithInvalidIdxKey_ShouldThrowValidationException()
		{
			// Arrange
			var lang = Entities.Lang.TryCreate("CONFIG_ADD", "Dùng để thêm", "Thêm", "Add");
			var newIdxKey = "";
			var newDescription = "Dùng để cập nhật";
			var newVn = "Cập nhật";
			var newEn = "Update";

			// Act
			Action act = () => lang.TryUpdate(newIdxKey, newDescription, newVn, newEn);

			// Assert
			var exception = act.Should().Throw<ValidationException>().Which;
			Assert.Contains(MessageConstant.NotNullOrEmpty<Entities.Lang>(x => x.Id), exception.ValidationResults);
		}


		/// <summary>
		/// Tests if TryUpdate method throws ValidationException for an invalid Vietnamese translation.
		/// </summary>
		[Fact]
		public void TryUpdate_WithInvalidVn_ShouldThrowValidationException()
		{
			// Arrange
			var lang = Entities.Lang.TryCreate("CONFIG_ADD", "Dùng để thêm", "Thêm", "Add");
			var newIdxKey = "CONFIG_UPDATE";
			var newDescription = "Dùng để cập nhật";
			var newVn = "";
			var newEn = "Update";

			// Act
			Action act = () => lang.TryUpdate(newIdxKey, newDescription, newVn, newEn);

			// Assert
			var exception = act.Should().Throw<ValidationException>().Which;
			Assert.Contains(MessageConstant.NotNullOrEmpty<Entities.Lang>(x => x.vn), exception.ValidationResults);
		}
	}
}
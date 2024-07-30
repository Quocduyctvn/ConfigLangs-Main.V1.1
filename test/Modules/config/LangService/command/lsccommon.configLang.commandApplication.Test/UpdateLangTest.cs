
using lsccommon.configLang.commandApplication.UserCases;
using lsccommon.configLang.commandDomain.Abstractions.Repositories;
using lsccommon.configLang.commandDomain.Exceptions;
using Moq;
using System.Data;
using Entities = lsccommon.configLang.commandDomain.Entities;

namespace lsccommon.configLang.commandApplication.Test
{
	/// <summary>
	/// Test class for updating Lang entities.
	/// </summary>
	public class UpdateLangTest
	{
		private readonly Mock<ILangRepository> langRepositoryMock;
		private readonly UpdateLangCommandHandler handler;

		/// <summary>
		/// Initializes a new instance of the <see cref="UpdateLangTest"/> class.
		/// </summary>
		public UpdateLangTest()
		{
			langRepositoryMock = new Mock<ILangRepository>();
			handler = new UpdateLangCommandHandler(langRepositoryMock.Object);
		}

		/// <summary>
		/// Utility class for asserting Lang properties.
		/// </summary>
		public static class LangAssertions
		{
			/// <summary>
			/// Asserts that two Lang entities are equal, ignoring the CreatedAt property.
			/// </summary>
			/// <param name="expected"></param>
			/// <param name="actual"></param>
			public static void AssertEqualIgnoringCreatedAt(Entities.Lang expected, Entities.Lang actual)
			{
				Assert.Equal(expected.Id, actual.Id);
				Assert.Equal(expected.description, actual.description);
				Assert.Equal(expected.vn, actual.vn);
				Assert.Equal(expected.en, actual.en);
			}
		}

		/// <summary>
		/// Tests that the Handle method updates a Lang successfully.
		/// </summary>
		[Fact]
		public async Task Handle_ShouldUpdateLangSuccessfully()
		{
			// Arrange
			var existingLang =
				Entities.Lang.TryCreate("CONFIG_ADD", "Dùng để thêm", "Thêm", "Add");
			var command = new UpdateLangCommand(existingLang.Id, "Dùng để cập nhật", "Cập nhật", "Update");
			langRepositoryMock.Setup(r => r.FindByIdAsync(command.Id, true, It.IsAny<CancellationToken>()))
				.ReturnsAsync(existingLang);
			langRepositoryMock.Setup(r => r.Update(It.IsAny<Entities.Lang>())).Verifiable();
			langRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
			var transactionMock = new Mock<IDbTransaction>();
			langRepositoryMock.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(transactionMock.Object);

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			LangAssertions.AssertEqualIgnoringCreatedAt(existingLang, existingLang);
			langRepositoryMock.Verify(r => r.Update(existingLang), Times.Once);
			langRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
			transactionMock.Verify(t => t.Commit(), Times.Once);
		}

		/// <summary>
		/// Tests that the Handle method rolls back the transaction on failure.
		/// </summary>
		[Fact]
		public async Task Handle_ShouldRollbackTransactionOnFailure()
		{
			// Arrange
			var existingLang =
				Entities.Lang.TryCreate("CONFIG_ADD", "Dùng để thêm", "Thêm", "Add");
			var command = new UpdateLangCommand(existingLang.Id, "Dùng để cập nhật", "Cập nhật", "Update");
			langRepositoryMock.Setup(r => r.FindByIdAsync(command.Id, true, It.IsAny<CancellationToken>()))
				.ReturnsAsync(existingLang);
			var transactionMock = new Mock<IDbTransaction>();
			langRepositoryMock.Setup(r => r.BeginTransactionAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(transactionMock.Object);
			langRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
				.ThrowsAsync(new ValidationException("Validation failed"));

			// Act & Assert
			var exception =
				await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
			transactionMock.Verify(t => t.Rollback(), Times.Once);
		}

		/// <summary>
		/// Tests that the Handle method throws an exception when the Lang is not found.
		/// </summary>
		[Fact]
		public async Task Handle_ShouldThrowExceptionWhenLangNotFound()
		{
			// Arrange
			var command =
				new UpdateLangCommand("lang", "Dùng để cập nhật", "Cập nhật", "Update");
			langRepositoryMock.Setup(r => r.FindByIdAsync(command.Id, true, It.IsAny<CancellationToken>()))
				.ReturnsAsync((Entities.Lang)null);

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.False(result.IsSuccess);
			var expectedErrorMessage = $"Lang with Id = {command.Id} was not found";
			Assert.Contains(expectedErrorMessage, result.Error.Messages);
		}
	}
}

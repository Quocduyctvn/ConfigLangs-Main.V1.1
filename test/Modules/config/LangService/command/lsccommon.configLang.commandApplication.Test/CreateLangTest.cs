
using lsccommon.configLang.commandApplication.UserCases;
using lsccommon.configLang.commandDomain.Abstractions.Repositories;
using lsccommon.configLang.commandDomain.Exceptions;
using Moq;
using System.Data;
using Entities = lsccommon.configLang.commandDomain.Entities;

namespace obscommon.configLang.commandApplication.Test
{
	/// <summary>
	/// Test class for creating Lang entities.
	/// </summary>
	public class CreateLangTest
	{
		/// <summary>
		/// Class for asserting Lang equality ignoring the CreatedAt property.
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

		private readonly Mock<IUnitOfWork> unitOfWorkMock;
		private readonly CreateLangCommandHandler handler;

		/// <summary>
		/// Initializes a new instance of the <see cref="CreateLangTest"/> class.
		/// </summary>
		public CreateLangTest()
		{
			unitOfWorkMock = new Mock<IUnitOfWork>();
			handler = new CreateLangCommandHandler(unitOfWorkMock.Object);
		}

		/// <summary>
		/// Test to verify successful creation of a lang.
		/// </summary>
		[Fact]
		public async Task Handle_ShouldCreateLangSuccessfully()
		{
			// Arrange
			var command = new CreateLangCommand
			{
				Id = "CONFIG_ADD",
				description = "Dùng để thêm",
				vn = "Thêm",
				en = "Add"
			};
			var lang = Entities.Lang.TryCreate(command.Id, command.description, command.vn, command.en);
			var langRepositoryMock = new Mock<IGenericRepository<Entities.Lang, string>>();
			unitOfWorkMock.Setup(u => u.Repository<Entities.Lang, string>()).Returns(langRepositoryMock.Object);
			var transactionMock = new Mock<IDbTransaction>();
			unitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(transactionMock.Object);

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			LangAssertions.AssertEqualIgnoringCreatedAt(lang, result.Value);
			langRepositoryMock.Verify(
				r => r.Add(It.Is<Entities.Lang>(l =>
					l.Id == command.Id && l.description == command.description &&
					l.vn == command.vn && l.en == command.en)), Times.Once);
			langRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
			transactionMock.Verify(t => t.Commit(), Times.Once);
		}

		/// <summary>
		/// Test to verify that a transaction is rolled back on failure.
		/// </summary>
		[Fact]
		public async Task Handle_ShouldRollbackTransactionOnFailure()
		{
			// Arrange
			var command = new CreateLangCommand
			{
				Id = "CONFIG_ADD",
				description = "Dùng để thêm",
				vn = "Thêm",
				en = "Add"
			};
			var langRepositoryMock = new Mock<IGenericRepository<Entities.Lang, string>>();
			unitOfWorkMock.Setup(u => u.Repository<Entities.Lang, string>()).Returns(langRepositoryMock.Object);
			var transactionMock = new Mock<IDbTransaction>();
			unitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(transactionMock.Object);
			langRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
				.ThrowsAsync(new ValidationException("Validation failed"));

			// Act & Assert
			var exception =
				await Assert.ThrowsAsync<ValidationException>(() =>
					handler.Handle(command, CancellationToken.None));
			transactionMock.Verify(t => t.Rollback(), Times.Once);
		}

		/// <summary>
		/// Test to verify exception is thrown for invalid Id.
		/// </summary>
		[Fact]
		public async Task Handle_ShouldThrowExceptionForInvalidIdxKey()
		{
			// Arrange
			var command = new CreateLangCommand
			{
				Id = "lang",
				description = "Dùng để thêm",
				vn = "Thêm",
				en = "Add"
			};

			// Act & Assert
			await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
		}


		/// <summary>
		/// Test to verify validation exception for invalid Vietnamese translation.
		/// </summary>
		[Fact]
		public async Task Handle_ShouldThrowValidationExceptionForInvalidVn()
		{
			// Arrange
			var command = new CreateLangCommand
			{
				Id = "CONFIG_ADD",
				description = "Dùng để thêm",
				vn = "",
				en = "Add"
			};

			// Act & Assert
			await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
		}

		/// <summary>
		/// Test to verify validation exception when unit of work throws an exception.
		/// </summary>
		[Fact]
		public async Task Handle_ShouldThrowValidationExceptionWhenUnitOfWorkThrows()
		{
			// Arrange
			var command = new CreateLangCommand
			{
				Id = "CONFIG_ADD",
				description = "Dùng để thêm",
				vn = "Thêm",
				en = "Add"
			};
			unitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
				.ThrowsAsync(new ValidationException("Validation failed"));

			// Act & Assert
			await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
		}
	}
}

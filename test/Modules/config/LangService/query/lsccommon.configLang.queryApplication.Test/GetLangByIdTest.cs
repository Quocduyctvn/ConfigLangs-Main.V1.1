using lsccommon.configLang.queryApplication.UserCases;
using lsccommon.configLang.queryDomain.Abstractions.Repositories;
using Moq;
using Entities = lsccommon.configLang.queryDomain.Entities;

namespace lsccommon.configLang.queryApplication.Test
{
	/// <summary>
	/// Test class for GetLangByIdQueryHandler.
	/// </summary>
	public class GetLangByIdTest
	{
		private readonly Mock<IUnitOfWork> unitOfWorkMock;
		private readonly GetLangByIdQueryHandler handler;

		/// <summary>
		/// Initializes a new instance of the <see cref="GetLangByIdTest"/> class.
		/// </summary>
		public GetLangByIdTest()
		{
			unitOfWorkMock = new Mock<IUnitOfWork>();
			handler = new GetLangByIdQueryHandler(unitOfWorkMock.Object);
		}

		/// <summary>
		/// Test to verify that a lang is returned when it exists.
		/// </summary>
		[Fact]
		public async Task Handle_ShouldReturnLang_WhenLangExists()
		{
			// Arrange
			var lang = new Entities.Lang("KEY1", "Description 1", "VN 1", "EN 1");
			var langRepositoryMock = new Mock<IGenericRepository<Entities.Lang, string>>();
			langRepositoryMock
				.Setup(repo => repo.FindByIdAsync(It.IsAny<string>(), false, It.IsAny<CancellationToken>()))
				.ReturnsAsync(lang);
			unitOfWorkMock.Setup(u => u.Repository<Entities.Lang, string>()).Returns(langRepositoryMock.Object);
			var query = new GetLangByIdQuery("KEY1");

			// Act
			var result = await handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.Equal(lang, result.Value);
		}



		/// <summary>
		/// Test to verify that a NotFound result is returned when the lang does not exist.
		/// </summary>
		[Fact]
		public async Task Handle_ShouldReturnNotFound_WhenLangDoesNotExist()
		{
			// Arrange
			var langRepositoryMock = new Mock<IGenericRepository<Entities.Lang, string>>();
			langRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<string>(), true, It.IsAny<CancellationToken>()))
				.ReturnsAsync((Entities.Lang)null);
			unitOfWorkMock.Setup(u => u.Repository<Entities.Lang, string>()).Returns(langRepositoryMock.Object);
			var query = new GetLangByIdQuery("config_langs");

			// Act
			var result = await handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.NotNull(result.Error);
			Assert.Contains("Lang with Id = config_langs was not found", result.Error.Messages);
		}

		/// <summary>
		/// Test to verify that a failure result is returned when an exception is thrown.
		/// </summary>
		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenExceptionThrown()
		{
			// Arrange
			var langRepositoryMock = new Mock<IGenericRepository<Entities.Lang, string>>();
			langRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<string>(), true, It.IsAny<CancellationToken>()))
				.Throws(new Exception("Test Exception"));
			unitOfWorkMock.Setup(uow => uow.Repository<Entities.Lang, string>()).Returns(langRepositoryMock.Object);
			var query = new GetLangByIdQuery("CONFIG_ADD");

			// Act
			var result = await handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.NotNull(result.Error);
			Assert.Contains("Lang with Id = CONFIG_ADD was not found", result.Error.Messages);
		}


	}
}

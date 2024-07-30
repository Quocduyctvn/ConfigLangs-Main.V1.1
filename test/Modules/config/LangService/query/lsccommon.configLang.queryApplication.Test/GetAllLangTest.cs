using lsccommon.configLang.queryApplication.UserCases;
using lsccommon.configLang.queryDomain.Abstractions.Repositories;
using Moq;
using System.Linq.Expressions;
using Entities = lsccommon.configLang.queryDomain.Entities;

namespace lsccommon.configLang.queryApplication.Test
{
	/// <summary>
	/// Test class for GetAllLangQueryHandler.
	/// </summary>
	public class GetAllLangTest
	{
		private readonly Mock<IUnitOfWork> unitOfWorkMock;
		private readonly GetAllLangQueryHandler handler;

		/// <summary>
		/// Initializes a new instance of the <see cref="GetAllLangTest"/> class.
		/// </summary>
		public GetAllLangTest()
		{
			unitOfWorkMock = new Mock<IUnitOfWork>();
			handler = new GetAllLangQueryHandler(unitOfWorkMock.Object);
		}

		/// <summary>
		/// Test to verify that langs are returned when they exist.
		/// </summary>
		[Fact]
		public async Task Handle_ShouldReturnLangs_WhenLangsExist()
		{
			// Arrange
			var langs = new List<Entities.Lang>
			{
				new Entities.Lang("CONFIG_1", "config lang 1", "VN 1", "EN 1"),
				new Entities.Lang("CONFIG_2", "config lang 2", "VN 2", "EN 2")
			};
			var langRepositoryMock = new Mock<IGenericRepository<Entities.Lang, string>>();
			langRepositoryMock.Setup(repo =>
					repo.FindAll(false, null, It.IsAny<Expression<Func<Entities.Lang, object>>[]>()))
				.Returns(langs.AsQueryable());
			unitOfWorkMock.Setup(u => u.Repository<Entities.Lang, string>()).Returns(langRepositoryMock.Object);
			var query = new GetAllLangQuery();

			// Act
			var result = await handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.Equal(langs, result.Value);
		}

		/// <summary>
		/// Test to verify failure when an exception is thrown.
		/// </summary>
		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenExceptionThrown()
		{
			// Arrange
			var langRepositoryMock = new Mock<IGenericRepository<Entities.Lang, string>>();
			langRepositoryMock.Setup(repo =>
					repo.FindAll(false, null, It.IsAny<Expression<Func<Entities.Lang, object>>[]>()))
				.Throws(new Exception("Test Exception"));
			unitOfWorkMock.Setup(u => u.Repository<Entities.Lang, string>()).Returns(langRepositoryMock.Object);
			var query = new GetAllLangQuery();

			// Act
			var result = await handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.NotNull(result.Error);
			Assert.Contains("Test Exception", result.Error.Messages);
		}

		/// <summary>
		/// Test to verify that an empty list is returned when no langs exist.
		/// </summary>
		[Fact]
		public async Task Handle_ShouldReturnEmptyList_WhenNoLangsExist()
		{
			// Arrange
			var langs = new List<Entities.Lang>();
			var langRepositoryMock = new Mock<IGenericRepository<Entities.Lang, string>>();
			langRepositoryMock.Setup(repo =>
					repo.FindAll(false, null, It.IsAny<Expression<Func<Entities.Lang, object>>[]>()))
				.Returns(langs.AsQueryable());
			unitOfWorkMock.Setup(u => u.Repository<Entities.Lang, string>()).Returns(langRepositoryMock.Object);
			var query = new GetAllLangQuery();

			// Act
			var result = await handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.Empty(result.Value);
		}
	}
}

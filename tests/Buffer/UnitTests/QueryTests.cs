using Buffer.Infrastructure.Queries;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data;

namespace Buffer.UnitTests
{
    public class QueryTests
    {
        [Fact]
        public async void GetUrlFromBufferQueryHandler_Test()
        {
            // Arrange
            var mockDbConnection = new Mock<IDbConnection>();
            var mockLogger = new Mock<ILogger<GetUrlFromBufferQueryHandler>>();


            var handler = new GetUrlFromBufferQueryHandler(mockDbConnection.Object, mockLogger.Object);
            var query = new GetUrlFromBufferQuery(_url: "https://mock.docm");

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void GetAllUrlFromBufferQueryHandler_Test()
        {
            // Arrange
            var mockDbConnection = new Mock<IDbConnection>();
            var mockLogger = new Mock<ILogger<GetAllUrlsFromBufferQueryHandler>>();


            var handler = new GetAllUrlsFromBufferQueryHandler(mockDbConnection.Object, mockLogger.Object);
            var query = new GetAllUrlsFromBufferQuery();

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.Null(result.Urls);
        }

    }
}
using Microsoft.Extensions.Logging;
using Moq;
using Shortener.Infrastructure.Queries;
using System.Data;

namespace Shortener.UnitTests
{
    public class QueryTests
    {
        [Fact]
        public async void GetUrlQueryHandler_Returns_ValidResult()
        {
            var mockDbConnection = new Mock<IDbConnection>();
            var mockLogger = new Mock<ILogger<GetUrlQueryHandler>>();


            var handler = new GetUrlQueryHandler(mockDbConnection.Object, mockLogger.Object);
            var query = new GetUrlQuery("https://mock.docm");

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.NotNull(result);

        }
    }
}

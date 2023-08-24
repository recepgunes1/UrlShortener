using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Shortener.Infrastructure.Commands;
using Shortener.Infrastructure.Context;

namespace Shortener.UnitTests
{
    public class CommandTests
    {
        [Fact]
        public async void ShortUrlCommandHandler_ShoulTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseInMemoryDatabase(databaseName: "TestDatabase")
    .Options;

            var mockConfiguration = new Mock<IConfiguration>();
            var mockAppDbContext = new Mock<AppDbContext>(options);
            var mockPublishEndpoint = new Mock<IPublishEndpoint>();
            var mockLogger = new Mock<ILogger<ShortUrlCommandHandler>>();

            var mockHandler = new Mock<ShortUrlCommandHandler>(mockConfiguration.Object, mockAppDbContext.Object, mockPublishEndpoint.Object, mockLogger.Object);

            var result = await mockHandler.Object.Handle(new ShortUrlCommand("deneme.com", new(2023, 2, 1), true), default);

            Assert.Null(result);
        }
    }
}

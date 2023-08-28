using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.ConfigModels;
using Shortener.Infrastructure.Commands;
using Shortener.Infrastructure.Context;

namespace Shortener.UnitTests
{
    public class CommandTests
    {
        private readonly Mock<IConfiguration> mockConfiguration;
        private readonly AppDbContext appDbContext;
        private readonly Mock<ILogger<ShortUrlCommandHandler>> loggerMock;
        private readonly ShortUrlCommandHandler handler;

        public CommandTests()
        {
            var mockConfigurationSection = new Mock<IConfigurationSection>();
            mockConfigurationSection.SetupGet(m => m["CharSet"]).Returns("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
            mockConfigurationSection.SetupGet(m => m["Length"]).Returns("6");

            mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(a => a.GetSection("Shortener")).Returns(mockConfigurationSection.Object);

            appDbContext = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            appDbContext.Urls.Add(new Shared.Entities.Url { LongUrl = "https://example.com", IsPublic = true });
            appDbContext.SaveChanges();
            loggerMock = new Mock<ILogger<ShortUrlCommandHandler>>();
            handler = new ShortUrlCommandHandler(mockConfiguration.Object, appDbContext, loggerMock.Object);
        }


        [Fact]
        public async Task Handle_GivenValidRequest_ReturnsUrlDto()
        {
            var request = new ShortUrlCommand("https://example.com", DateTime.UtcNow.AddDays(1), true);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Handle_GivenInvalidRequest_ReturnsUrlDtoWithDefaultValues()
        {
            var request = new ShortUrlCommand("https://non-exist-example.com", DateTime.UtcNow.AddDays(1), true);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Null(result.LongUrl);
            Assert.Null(result.ShortPath);
        }
    }
}

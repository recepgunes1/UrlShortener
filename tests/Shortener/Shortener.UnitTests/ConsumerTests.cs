using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.DTOs;
using Shortener.Infrastructure.Commands;
using Shortener.Infrastructure.Consumers;

namespace Shortener.UnitTests
{
    public class ConsumerTests
    {
        [Fact]
        public async Task Consume_SendsExpectedCommandAndLogsCorrectly() //From ChatGPT
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<ShortedUrlConsumer>>();

            var consumer = new ShortedUrlConsumer(mediatorMock.Object, loggerMock.Object);

            var shortedUrlDto = new ShortedUrlDto
            {
                Url = "https://example.com",
                ExpireDateTime = DateTime.UtcNow.AddDays(1),
                IsPublic = true
            };

            var contextMock = new Mock<ConsumeContext<ShortedUrlDto>>();
            contextMock.Setup(c => c.Message).Returns(shortedUrlDto);

            // Act
            await consumer.Consume(contextMock.Object);

            // Assert
            loggerMock.Verify(
                log => log.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals($"{shortedUrlDto} was send to ShortUrlCommand", o.ToString())),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            mediatorMock.Verify(m => m.Send(It.Is<ShortUrlCommand>(
                cmd => cmd.Url == shortedUrlDto.Url && cmd.ExpireDateTime == shortedUrlDto.ExpireDateTime && cmd.IsPublic == shortedUrlDto.IsPublic),
                It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}

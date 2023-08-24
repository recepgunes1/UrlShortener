using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shortener.Infrastructure.Commands;

namespace Shortener.Infrastructure.Consumers
{
    public class ShortedUrlConsumer : IConsumer<ShortedUrlDto>
    {
        private readonly IMediator mediator;
        private readonly ILogger logger;

        public ShortedUrlConsumer(IMediator _mediator, ILogger<ShortedUrlConsumer> _logger)
        {
            mediator = _mediator;
            logger = _logger;
        }

        public async Task Consume(ConsumeContext<ShortedUrlDto> context)
        {
            var message = context.Message;
            logger.LogInformation($"{message} was send to {nameof(ShortUrlCommand)}");
            await mediator.Send(new ShortUrlCommand(message.Url, message.ExpireDateTime, message.IsPublic));
        }
    }
}

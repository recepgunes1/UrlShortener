using Infrastructure.Commands;
using MassTransit;
using MediatR;
using Shared.DTOs;

namespace Infrastructure.Consumers
{
    public class ShortedUrlConsumer : IConsumer<ShortedUrlDto>
    {
        private readonly IMediator mediator;

        public ShortedUrlConsumer(IMediator _mediator)
        {
            mediator = _mediator;
        }

        public async Task Consume(ConsumeContext<ShortedUrlDto> context)
        {
            var message = context.Message;
            await mediator.Send(new ShortUrlCommand(message.Url));
        }
    }
}

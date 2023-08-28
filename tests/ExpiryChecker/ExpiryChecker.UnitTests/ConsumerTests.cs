using ExpiryChecker.Infrastructure.Consumers;
using MassTransit;
using MassTransit.Testing;
using Shared.DTOs;

namespace ExpiryChecker.UnitTests
{
    public class ConsumerTests
    {
        [Fact]
        public async void ExpiredUrlConsumer_ConsumesMessageCorrectly()
        {
            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer<ExpiredUrlConsumer>();

            await harness.Start();

            try
            {
                var message = new ExpiredUrlDto { LongUrl = "http://hello.world/", ExpireDateTime = new DateTime(2023, 1, 1) };
                await harness.InputQueueSendEndpoint.Send(message);
                Assert.True(harness.Consumed.Select<ExpiredUrlDto>().Any());
                Assert.True(consumerHarness.Consumed.Select<ExpiredUrlDto>().Any());
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}

using ExpiryChecker.Infrastructure.Jobs;
using MassTransit;
using Microsoft.Extensions.Logging;
using Quartz;
using Shared.DTOs;

namespace ExpiryChecker.Infrastructure.Consumers
{
    public class ExpiredUrlConsumer : IConsumer<ExpiredUrlDto>
    {
        private readonly IScheduler scheduler;
        private readonly ILogger logger;

#pragma warning disable CS8618 
        public ExpiredUrlConsumer()
#pragma warning restore CS8618 
        {

        }

        public ExpiredUrlConsumer(IScheduler _scheduler, ILogger<ExpiredUrlConsumer> _logger)
        {
            scheduler = _scheduler;
            logger = _logger;
        }

        public async Task Consume(ConsumeContext<ExpiredUrlDto> context)
        {
            var message = context.Message;

            var job = JobBuilder.Create<ExpireUrlJob>()
                .UsingJobData("url", message.LongUrl)
                .Build();
            logger.LogInformation($"{nameof(ExpireUrlJob)} created for {message.LongUrl}");
            var offset = new DateTimeOffset(message.ExpireDateTime.ToUniversalTime());
            logger.LogInformation($"offset is {offset} for trigger");

            var trigger = TriggerBuilder.Create()
                .ForJob(job)
                .StartAt(offset)
                .Build();
            logger.LogInformation($"trigger created for {message.LongUrl}");

            await scheduler.ScheduleJob(job, trigger);
            logger.LogInformation("trigger and job was scheduled");

            await scheduler.Start();
            logger.LogInformation("scheduler started");
        }
    }
}

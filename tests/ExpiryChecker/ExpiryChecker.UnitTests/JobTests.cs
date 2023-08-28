using ExpiryChecker.Infrastructure.Context;
using ExpiryChecker.Infrastructure.Jobs;
using Microsoft.EntityFrameworkCore;
using Moq;
using Quartz;
using Quartz.Impl;
using Shared.Entities;

namespace ExpiryChecker.UnitTests
{
    public class JobTests
    {
        private readonly AppDbContext dbContext;
        private readonly List<Url> testData = new List<Url> { new Url { LongUrl = "http://test.com", ShortPath = "test123", ExpireDate = DateTime.Now } };

        public JobTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            dbContext = new AppDbContext(optionsBuilder.Options);
            dbContext.Urls.AddRange(testData);
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task Execute_WithExistingUrl_SetsPropertiesToNull()
        {
            // Arrange

            var jobDataMap = new JobDataMap { { "url", "http://test.com" } };
            var jobExecutionContextMock = new Mock<IJobExecutionContext>();
            jobExecutionContextMock.Setup(m => m.JobDetail).Returns(new JobDetailImpl { JobDataMap = jobDataMap });

            var job = new ExpireUrlJob(dbContext);

            // Act
            await job.Execute(jobExecutionContextMock.Object);

            // Assert
            Assert.Null(testData.First().ShortPath);
            Assert.Null(testData.First().ExpireDate);
        }

        [Fact]
        public async Task Execute_WithNonExistingUrl_NoChangesMade()
        {
            var jobDataMap = new JobDataMap { { "url", "http://nonexistent.com" } };
            var jobExecutionContextMock = new Mock<IJobExecutionContext>();
            jobExecutionContextMock.Setup(m => m.JobDetail).Returns(new JobDetailImpl { JobDataMap = jobDataMap });

            var job = new ExpireUrlJob(dbContext);

            // Act
            await job.Execute(jobExecutionContextMock.Object);

            // Assert
            var allUrls = dbContext.Urls.ToList();
            Assert.Equal(testData.Count, allUrls.Count);
        }
    }
}

using ExpiryChecker.Infrastructure.Context;
using ExpiryChecker.Infrastructure.Jobs;
using Microsoft.EntityFrameworkCore;
using Moq;
using Quartz;
using Quartz.Impl;

namespace ExpiryChecker.UnitTests
{
    public class JobTests
    {
        [Fact]
        public async void ExpireUrlJob_Test()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var entity = new Shared.Entities.Url()
            {
                Id = Guid.NewGuid(),
                LongUrl = "https://hello_world/",
                ShortPath = "dakaSAdsa",
                CreatedDate = DateTime.Now,
                LastRequestedDate = null,
                RequestCounter = 0,
                ExpireDate = DateTime.Now.AddYears(10),
                IsPublic = true,
            };

            List<Shared.Entities.Url> urlsData = new List<Shared.Entities.Url>();

            var mockSet = new Mock<DbSet<Shared.Entities.Url>>();
            var mockAppDbContext = new Mock<AppDbContext>(options);
            var mockJob = new Mock<ExpireUrlJob>(mockAppDbContext.Object);
            var mockJobExecutionContext = new Mock<IJobExecutionContext>();

            mockSet.As<IQueryable<Shared.Entities.Url>>().Setup(m => m.Provider).Returns(urlsData.AsQueryable().Provider);
            mockSet.As<IQueryable<Shared.Entities.Url>>().Setup(m => m.Expression).Returns(urlsData.AsQueryable().Expression);
            mockSet.As<IQueryable<Shared.Entities.Url>>().Setup(m => m.ElementType).Returns(urlsData.AsQueryable().ElementType);
            mockSet.As<IQueryable<Shared.Entities.Url>>().Setup(m => m.GetEnumerator()).Returns(urlsData.GetEnumerator());

            mockSet.Setup(d => d.Add(It.IsAny<Shared.Entities.Url>())).Callback<Shared.Entities.Url>(s => urlsData.Add(s));

            mockAppDbContext.Setup(c => c.Urls).Returns(mockSet.Object);

            mockJobExecutionContext.Setup(c => c.JobDetail).Returns(new JobDetailImpl());
            mockJobExecutionContext.Setup(j => j.JobDetail.JobDataMap).Returns(new JobDataMap { { "url", entity.LongUrl } });

            await mockJob.Object.Execute(mockJobExecutionContext.Object);

            Assert.Null(mockSet.Object.Find(entity.Id)!.ShortPath);
            Assert.Null(mockSet.Object.Find(entity.Id)!.ExpireDate);
        }
    }
}

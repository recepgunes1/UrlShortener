using ExpiryChecker.Infrastructure.Context;
using Quartz;

namespace ExpiryChecker.Infrastructure.Jobs
{
    public class ExpireUrlJob : IJob
    {
        private readonly AppDbContext appDbContext;

        public ExpireUrlJob(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            var url = dataMap.GetString("url");
            var entity = appDbContext.Urls.FirstOrDefault(p => p.LongUrl == url);
            if (entity == null)
            {
                return Task.CompletedTask;
            }
            entity.ShortPath = null;
            entity.ExpireDate = null;
            appDbContext.SaveChanges();
            return Task.CompletedTask;
        }
    }
}

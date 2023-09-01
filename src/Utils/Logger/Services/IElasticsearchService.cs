using Logger.Model;

namespace Logger.Services
{
    public interface IElasticsearchService
    {
        void InsertActionLog(ActionLogModel actionModel);
        void InsertErrorLog(ErrorLogModel errorModel);

    }
}

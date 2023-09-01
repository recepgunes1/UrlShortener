using Logger.Model;
using Logger.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Logger.Filters
{
    public class ErrorLogFilter : IExceptionFilter
    {
        private readonly IElasticsearchService elasticsearchService;
        private ErrorLogModel errorLogModel;

        public ErrorLogFilter(IElasticsearchService _elasticsearchService)
        {
            elasticsearchService = _elasticsearchService;
            errorLogModel = new ErrorLogModel();
        }

        public async void OnException(ExceptionContext context)
        {
            errorLogModel.Path = context.HttpContext.Request.Path;
            errorLogModel.Method = context.HttpContext.Request.Method;
            errorLogModel.QueryString = context.HttpContext.Request.QueryString.ToString();

            if (context.HttpContext.Request.Method != "GET")
            {
                context.HttpContext.Request.EnableBuffering();
                context.HttpContext.Request.Body.Position = 0;

                using (StreamReader reader = new StreamReader(context.HttpContext.Request.Body))
                {
                    errorLogModel.Payload = await reader.ReadToEndAsync();
                }
            }

            errorLogModel.ErrorStack = context.Exception.StackTrace;
            errorLogModel.ErrorMessage = context.Exception.Message;
            errorLogModel.ErrorAt = DateTime.Now;

            elasticsearchService.InsertErrorLog(errorLogModel);
        }

    }
}

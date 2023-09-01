using Logger.Model;
using Logger.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Text.Json;

namespace Logger.Filters
{
    public class ActionLogFilter : IActionFilter
    {
        private readonly IElasticsearchService elasticsearchService;
        private readonly Stopwatch stopwatch;
        private ActionLogModel actionLogModel;

        public ActionLogFilter(IElasticsearchService _elasticsearchService)
        {
            elasticsearchService = _elasticsearchService;
            stopwatch = new Stopwatch();
            actionLogModel = new();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed.ToString();

            string responseBody = "";

            if (context.Result != null)
            {
                responseBody = JsonSerializer.Serialize(context.Result);
            }

            actionLogModel.Response = responseBody;
            actionLogModel.ResponseCode = context.HttpContext.Response.StatusCode.ToString();
            actionLogModel.HttpStatusCode = context.HttpContext.Response.StatusCode;
            actionLogModel.RespondedAt = DateTime.Now;
            actionLogModel.ResponseTime = elapsed;

            elasticsearchService.InsertActionLog(actionLogModel);
        }

        public async void OnActionExecuting(ActionExecutingContext context)
        {
            stopwatch.Start();

            actionLogModel.Path = context.HttpContext.Request.Path;
            actionLogModel.Method = context.HttpContext.Request.Method;
            actionLogModel.QueryString = context.HttpContext.Request.QueryString.ToString();

            if (context.HttpContext.Request.Method != "GET")
            {
                context.HttpContext.Request.Body.Position = 0;

                using (StreamReader reader = new StreamReader(context.HttpContext.Request.Body))
                {
                    actionLogModel.Payload = await reader.ReadToEndAsync();
                }
            }

            actionLogModel.RequestedAt = DateTime.Now;
        }
    }
}

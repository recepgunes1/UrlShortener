using System.Collections;
using System.Text.Json;

namespace Shared.Extensions
{
    public static class SharedExtensions
    {
        public static string ToJsonString(this Exception ex)
        {
            var exceptionDetails = new
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Source = ex.Source,
                HelpLink = ex.HelpLink,
                Data = ex.Data.Cast<DictionaryEntry>().ToDictionary(entry => entry.Key?.ToString() ?? Guid.NewGuid().ToString(), entry => entry.Value?.ToString()),
                InnerException = ex.InnerException != null ? new
                {
                    Message = ex.InnerException.Message,
                    StackTrace = ex.InnerException.StackTrace,
                    Source = ex.InnerException.Source
                } : null
            };

            var jsonException = JsonSerializer.Serialize(exceptionDetails, new JsonSerializerOptions { WriteIndented = true });
            return jsonException;
        }
    }
}

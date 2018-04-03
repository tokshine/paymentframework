using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Workwiz.Common.Logging;

namespace Workwiz.PaymentFramework.Shared
{
    /// <remarks>
    ///     based on http://stackoverflow.com/questions/18924996/logging-request-response-messages-when-using-httpclient
    /// </remarks>
    public class LoggingHttpHandler : DelegatingHandler
    {
        private readonly IWorkwizLoggerFactory _logger;
        private readonly Type _loggerContext;
        private readonly LogLevel _logLevelRequest;
        private readonly LogLevel _logLevelResponse;

        public LoggingHttpHandler(
            IWorkwizLoggerFactory logger, Type loggerContext,
            HttpMessageHandler innerHandler,
            LogLevel logLevelRequest,
            LogLevel logLevelResponse)
            : base(innerHandler)
        {
            this._logger = logger;
            this._loggerContext = loggerContext;
            this._logLevelRequest = logLevelRequest;
            this._logLevelResponse = logLevelResponse;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            string contentDesc = "null";
            if (null != request.Content)
            {
                contentDesc = await request.Content.ReadAsStringAsync();
            }
            this._logger.CreateEntry(this._loggerContext, this._logLevelRequest,
                $"{request.RequestUri} {request.Method} : {contentDesc}");

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            contentDesc = "null";
            if (null != response.Content)
            {
                contentDesc = await response.Content.ReadAsStringAsync();
            }
            this._logger.CreateEntry(this._loggerContext, this._logLevelResponse,
                $"{request.RequestUri} Received {response.StatusCode:D} {response.ReasonPhrase} : {contentDesc}");

            return response;
        }

        public static HttpClient CreateLoggingClient(IWorkwizLoggerFactory logger, Type loggerContext,
            LogLevel logLevelRequest = LogLevel.Info,
            LogLevel logLevelResponse = LogLevel.Info)
        {
            LoggingHttpHandler handler = new LoggingHttpHandler(logger, loggerContext, new HttpClientHandler(),
                logLevelRequest, logLevelResponse);
            return new HttpClient(handler);
        }
    }
}
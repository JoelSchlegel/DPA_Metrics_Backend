using App.Metrics;

namespace ElasticFrontend.Metric
{
    public class MetricMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MetricMiddleware> _logger;
        private readonly IMetrics _metrics;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public MetricMiddleware(RequestDelegate next, ILogger<MetricMiddleware> logger, IMetrics metrics, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _next = next;
            _metrics = metrics;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var httpMethod = context.Request.Method.ToUpperInvariant();

            if (httpMethod == "POST" || httpMethod == "PUT")
            {
                if (context.Request.Headers != null && context.Request.Headers.ContainsKey("Content-Length"))
                {
                    _metrics.Measure.Histogram.Update(MetricsRegistry.PostAndPutRequestSize, long.Parse(context.Request.Headers["Content-Length"].First()));
                }
            }

            var contextpath = context.Request.Path;
            var userAgent = context.Request.Headers["User-Agent"].ToString();

            if (context.Request.Path.StartsWithSegments(new PathString("/metrics-text")))
            {
                try
                {
                    var metricsUpdater = new MetricsUpdater(_logger, _metrics, _serviceScopeFactory);
                    metricsUpdater.UpdateMetrics();
                }
                catch (Exception ex)
                {
                    string errorText = "Middleware Invoker failed";
                    _logger.LogError(nameof(InvokeAsync) + "; " + errorText + "; " + ex);
                }
            }

            await _next(context);

            if (userAgent.Contains("Elastic-Metricbeat"))
            {
                var metricsResetter = new MetricsResetter(_metrics);
                metricsResetter.ResetMetrics();
            }
        }
    }
}

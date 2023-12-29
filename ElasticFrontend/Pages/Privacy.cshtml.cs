using App.Metrics;
using ElasticFrontend.Metric;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElasticFrontend.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly IMetrics _metrics;

        public PrivacyModel(ILogger<PrivacyModel> logger, IMetrics metrics)
        {
            _metrics = metrics;
            _logger = logger;
        }

        public void OnGet()
        {
            _metrics.Measure.Counter.Increment(MetricsRegistry.TestCounter);
        }
    }
}
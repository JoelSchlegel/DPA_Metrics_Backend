using App.Metrics;

namespace ElasticFrontend.Metric
{
    public class MetricsResetter
    {
        private readonly IMetrics _metrics;
        public MetricsResetter(IMetrics metrics)
        {
            _metrics = metrics;
        }

        public void ResetMetrics()
        {
            _metrics.Provider.Counter.Instance(MetricsRegistry.RegisterCounter).Reset();
            _metrics.Provider.Counter.Instance(MetricsRegistry.LoginSuccessful).Reset();
            _metrics.Provider.Counter.Instance(MetricsRegistry.CreateSemesterSuccessful).Reset();
            _metrics.Provider.Counter.Instance(MetricsRegistry.DeleteSemester).Reset();
            _metrics.Provider.Counter.Instance(MetricsRegistry.TestCounter).Reset();
        }
    }
}

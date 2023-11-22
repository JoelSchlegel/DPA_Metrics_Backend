using App.Metrics;
using ElasticFrontend.Metric;

namespace ElasticFrontend.Interceptor
{
    public class TimingInterceptor
    {
        private readonly IMetrics _metrics;

        public TimingInterceptor(IMetrics metrics)
        {
            _metrics = metrics;
        }

        public void Intercept()
        {
            var timer = _metrics.Measure.Timer.Time(MetricsRegistry.DBTimer);
            try
            {
                // Ausführen der eigentlichen Methode
            }
            finally
            {
                timer.Dispose(); // Stoppt den Timer und registriert die Zeit
            }
        }
    }
}

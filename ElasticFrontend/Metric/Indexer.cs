using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Meter;

namespace ElasticFrontend.Metric
{
    public class Indexer
    {
        private readonly IMetrics _metrics;

        public Indexer(IMetrics metrics)
        {
            _metrics = metrics;
            ExecuteCounterOptionsIndexer();
            ExecuteMeterOptionsIndexer();
            ExecuteGaugeOptionsIndexer();
        }

        public void ExecuteCounterOptionsIndexer()
        {
            CounterOptionsIndexer(MetricsRegistry.RegisterCounter);
            CounterOptionsIndexer(MetricsRegistry.LoginUnsuccessful);
            CounterOptionsIndexer(MetricsRegistry.LoginSuccessful);
            CounterOptionsIndexer(MetricsRegistry.LogoutSuccessful);
            CounterOptionsIndexer(MetricsRegistry.CreateSemesterSuccessful);
            CounterOptionsIndexer(MetricsRegistry.CreateSemesterUnsuccessful);
        }
        public void CounterOptionsIndexer(CounterOptions counterOptions)
        {
            _metrics.Measure.Counter.Increment(counterOptions);
            _metrics.Measure.Counter.Decrement(counterOptions);
        }

        public void ExecuteGaugeOptionsIndexer()
        {
            GaugeOptionsIndexer(MetricsRegistry.RegistredUser);
            GaugeOptionsIndexer(MetricsRegistry.RegistredSemester);
            GaugeOptionsIndexer(MetricsRegistry.ActiveSemesters);
            GaugeOptionsIndexer(MetricsRegistry.ExpiredSemesters);
        }

        private void GaugeOptionsIndexer(GaugeOptions gaugeOptions)
        {
            _metrics.Measure.Gauge.SetValue(gaugeOptions, 0);
        }

        public void ExecuteMeterOptionsIndexer()
        {
            MeterOptionsIndexer(MetricsRegistry.LoginApiSuccessful);
        }

        public void MeterOptionsIndexer(MeterOptions meterOptions)
        {
            _metrics.Measure.Meter.Mark(meterOptions, 0);
        }
    }
}

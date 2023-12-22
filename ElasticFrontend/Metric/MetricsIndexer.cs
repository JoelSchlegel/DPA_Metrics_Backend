using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Meter;

namespace ElasticFrontend.Metric
{
    public class MetricsIndexer
    {
        private readonly IMetrics _metrics;

        public MetricsIndexer(IMetrics metrics)
        {
            _metrics = metrics;
            ExecuteCounterOptionsIndexer();
            ExecuteMeterOptionsIndexer();
            ExecuteGaugeOptionsIndexer();
        }

        #region Counter
        public void ExecuteCounterOptionsIndexer()
        {
            CounterOptionsIndexer(MetricsRegistry.TestCounter);
            CounterOptionsIndexer(MetricsRegistry.RegisterCounter);
            CounterOptionsIndexer(MetricsRegistry.LoginSuccessful);
            CounterOptionsIndexer(MetricsRegistry.CreateSemesterSuccessful);
            CounterOptionsIndexer(MetricsRegistry.DeleteSemester);
        }
        public void CounterOptionsIndexer(CounterOptions counterOptions)
        {
            _metrics.Measure.Counter.Increment(counterOptions);
            _metrics.Measure.Counter.Decrement(counterOptions);
        }
        #endregion

        #region Gauge
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
        #endregion

        #region Meter
        public void ExecuteMeterOptionsIndexer()
        {
            MeterOptionsIndexer(MetricsRegistry.LoginMeter);
        }

        public void MeterOptionsIndexer(MeterOptions meterOptions)
        {
            _metrics.Measure.Meter.Mark(meterOptions, 0);
        }
        #endregion
    }
}

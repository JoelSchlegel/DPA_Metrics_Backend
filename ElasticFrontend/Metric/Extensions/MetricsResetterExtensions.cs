using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Meter;

namespace ElasticFrontend.Metric.Extensions
{
    public static class MetricsResetterExtensions
    {
        public static CounterOptions ResetCounter(this CounterOptions counterOption, IMetrics metrics)
        {
            metrics.Provider.Counter.Instance(counterOption).Reset();

            return counterOption;
        }

        public static MeterOptions ResetMeter(this MeterOptions meterOptions, IMetrics metrics)
        {
            metrics.Provider.Meter.Instance(meterOptions).Reset();
            return meterOptions;
        }

        public static GaugeOptions ResetGauge(this GaugeOptions gaugeOptions, IMetrics metrics)
        {
            metrics.Provider.Gauge.Instance(gaugeOptions).Reset();
            return gaugeOptions;
        }
    }
}

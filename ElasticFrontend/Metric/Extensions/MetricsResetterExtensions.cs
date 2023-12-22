using App.Metrics;
using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

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

        public static ApdexOptions ResetApdex(this ApdexOptions apdexOptions, IMetrics metrics)
        {
            metrics.Provider.Apdex.Instance(apdexOptions).Reset();
            return apdexOptions;
        }

        public static HistogramOptions ResetHistogram(this HistogramOptions histogramOptions, IMetrics metrics)
        {
            metrics.Provider.Histogram.Instance(histogramOptions).Reset();
            return histogramOptions;
        }

        public static TimerOptions ResetTimer(this TimerOptions timerOptions, IMetrics metrics)
        {
            metrics.Provider.Timer.Instance(timerOptions).Reset();
            return timerOptions;
        }
    }
}

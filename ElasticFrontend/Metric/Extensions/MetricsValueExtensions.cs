using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Filtering;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace ElasticFrontend.Metric.Extensions
{
    public static class MetricsValueExtensions
    {
        public static long GetCounterValue(this CounterOptions counterOption, IMetrics metrics)
        {
            if (counterOption.Context == null)
                return 0;

            else
            {
                var filter = new MetricsFilter()
                    .WhereContext(counterOption.Context);

                var value = metrics.Snapshot
                    .Get(filter)
                    .Contexts.FirstOrDefault()?.Counters.FirstOrDefault()?.Value.Count ?? 0;

                return value;
            }
        }

        public static double GetGaugeValue(this GaugeOptions gaugeOptions, IMetrics metrics)
        {
            if (gaugeOptions.Context == null)
                return 0;

            else
            {
                var filter = new MetricsFilter()
                    .WhereContext(gaugeOptions.Context);

                var value = metrics.Snapshot
                    .Get(filter)
                    .Contexts.FirstOrDefault()?.Gauges.FirstOrDefault()?.Value ?? 0;

                return value;
            }
        }

        public static MeterValue GetMeterValue(this MeterOptions meterOtions, IMetrics metrics)
        {
            if (meterOtions.Context == null)
                throw new InvalidDataException("Context is null");

            else
            {
                var filter = new MetricsFilter()
                    .WhereContext(meterOtions.Context);

                var value = metrics.Snapshot
                    .Get(filter)
                    .Contexts.FirstOrDefault()?.Meters.FirstOrDefault()?.Value ?? throw new InvalidDataException("Meter not found");

                return value;
            }
        }

        public static HistogramValue GetTimerValue(this TimerOptions timerOptions, IMetrics metrics)
        {
            if (timerOptions.Context == null)
                throw new InvalidDataException("Context is null");

            else
            {
                var filter = new MetricsFilter()
                    .WhereContext(timerOptions.Context);

                var value = metrics.Snapshot
                    .Get(filter).Contexts.FirstOrDefault()?.Timers.FirstOrDefault()?.Value.Histogram ?? throw new InvalidDataException("Timer not found");

                return value;
            }
        }

        public static HistogramValue GetHistogramValue(this HistogramOptions timerOptions, IMetrics metrics)
        {
            if (timerOptions.Context == null)
                throw new InvalidDataException("Context is null");

            else
            {
                var filter = new MetricsFilter()
                    .WhereContext(timerOptions.Context);

                var value = metrics.Snapshot
                    .Get(filter).Contexts.FirstOrDefault()?.Histograms.FirstOrDefault()?.Value ?? throw new InvalidDataException("Histogram not found");

                return value;
            }
        }
    }
}

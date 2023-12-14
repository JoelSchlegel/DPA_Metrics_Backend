using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Filtering;
using App.Metrics.Gauge;
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

        public static long GetMeterValue(this MeterOptions counterOption, IMetrics metrics)
        {
            if (counterOption.Context == null)
                return 0;

            else
            {
                var filter = new MetricsFilter()
                    .WhereContext(counterOption.Context);

                var value = metrics.Snapshot
                    .Get(filter)
                    .Contexts.FirstOrDefault()?.Meters.FirstOrDefault()?.Value.Count ?? 0;

                return value;
            }
        }

        public static double GetGaugeValue(this GaugeOptions counterOption, IMetrics metrics)
        {
            if (counterOption.Context == null)
                return 0;

            else
            {
                var filter = new MetricsFilter()
                    .WhereContext(counterOption.Context);

                var value = metrics.Snapshot
                    .Get(filter)
                    .Contexts.FirstOrDefault()?.Gauges.FirstOrDefault()?.Value ?? 0;

                return value;
            }
        }

        public static double GetMeterValue(this TimerOptions counterOption, IMetrics metrics)
        {
            if (counterOption.Context == null)
                return 0;

            else
            {
                var filter = new MetricsFilter()
                    .WhereContext(counterOption.Context);

                var value = metrics.Snapshot
                    .Get(filter)
                    .Contexts.FirstOrDefault()?.Timers.FirstOrDefault()?.Value.Histogram.Mean ?? 0;  // Mean ist der durchschnitt 

                return value;
            }
        }
    }
}

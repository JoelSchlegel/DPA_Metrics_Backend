using App.Metrics;
using ElasticFrontend.Metric;
using ElasticFrontend.Metric.Extensions;
using NUnit.Framework;

namespace ElasticFrontend.Test
{
    [TestFixture]
    [Description("ElasticFrontend.Test")]
    [Category("ElasticFrontend.Test.Type")]
    public class Tests
    {
        [Test]
        [Description("ElasticFrontend.Test")]
        [Category("ElasticsFtontend.Test.Type")]
        public void LoginSuccessful_Counter_Test()
        {
            var metrics = new MetricsBuilder()
            .Configuration.Configure(options =>
            {
                options.DefaultContextLabel = "UnitTest";
                options.Enabled = true;
            }).Build();

            var loginSuccessfulCounter = MetricsRegistry.LoginSuccessful;

            metrics.Measure.Counter.Increment(loginSuccessfulCounter);

            var counterValue = loginSuccessfulCounter.GetCounterValue(metrics);
            Assert.AreEqual(1, counterValue);

            loginSuccessfulCounter.ResetCounter(metrics);
            var resetCounterValue = loginSuccessfulCounter.GetCounterValue(metrics);
            Assert.AreEqual(0, resetCounterValue);
        }

        [Test]
        [Description("ElasticFrontend.Test")]
        [Category("ElasticFrontend.Test.Type")]
        public void RegistredUsers_Gauge_Test()
        {
            var metrics = new MetricsBuilder()
            .Configuration.Configure(options =>
            {
                options.DefaultContextLabel = "UnitTest";
                options.Enabled = true;
            }).Build();

            var registredUserGauge = MetricsRegistry.RegistredUser;

            metrics.Measure.Gauge.SetValue(registredUserGauge, 5);

            var gaugeValue = registredUserGauge.GetGaugeValue(metrics);
            Assert.AreEqual(5, gaugeValue);

            registredUserGauge.ResetGauge(metrics);
            var resetGauge = registredUserGauge.GetGaugeValue(metrics);
            Assert.AreEqual(0, resetGauge);
        }

        [Test]
        [Description("ElasticFrontend.Test")]
        [Category("ElasticFrontend.Test.Type")]
        public void LoginMeter_Meter_Test()
        {
            var metrics = new MetricsBuilder()
            .Configuration.Configure(options =>
            {
                options.DefaultContextLabel = "UnitTest";
                options.Enabled = true;
            }).Build();

            var loginMeter = MetricsRegistry.LoginMeter;

            metrics.Measure.Meter.Mark(loginMeter, 111);

            var meterValue = loginMeter.GetMeterValue(metrics);
            Assert.AreEqual(111, meterValue.Count);

            loginMeter.ResetMeter(metrics);
            var resetMeter = loginMeter.GetMeterValue(metrics);
            Assert.AreEqual(0, resetMeter.Count);
        }

        [Test]
        [Description("ElasticFrontend.Test")]
        [Category("ElasticFrontend.Test.Type")]
        public void DBTimer_Timer_Test()
        {
            var metrics = new MetricsBuilder()
            .Configuration.Configure(options =>
            {
                options.DefaultContextLabel = "UnitTest";
                options.Enabled = true;
            }).Build();

            var dbTimer = MetricsRegistry.DBTimer;

            using (metrics.Measure.Timer.Time(dbTimer))
            {
                System.Threading.Thread.Sleep(1000);
            }

            var timerValue = dbTimer.GetTimerValue(metrics);
            Assert.IsTrue(1000 <= timerValue.Min, "Timer.Min Zeit ist nicht länger oder gleich als definierter Timeout");

            dbTimer.ResetTimer(metrics);
            var resetTimer = dbTimer.GetTimerValue(metrics);
            Assert.AreEqual(0, resetTimer.Count);
        }

        [Test]
        [Description("ElasticFrontend.Test")]
        [Category("ElasticFrontend.Test.Type")]
        public void PostAndPutRequestSize_Histogram_Test()
        {
            var metrics = new MetricsBuilder()
            .Configuration.Configure(options =>
            {
                options.DefaultContextLabel = "UnitTest";
                options.Enabled = true;
            }).Build();

            var postputHistogram = MetricsRegistry.PostAndPutRequestSize;

            metrics.Measure.Histogram.Update(postputHistogram, 1024);

            var histogramValue = postputHistogram.GetHistogramValue(metrics);
            Assert.AreEqual(1024, histogramValue.LastValue, "Histogram.LastValue ist nicht gleich");

            postputHistogram.ResetHistogram(metrics);
            var resetHistogram = postputHistogram.GetHistogramValue(metrics);
            Assert.AreEqual(0, resetHistogram.LastValue);
        }
    }
}
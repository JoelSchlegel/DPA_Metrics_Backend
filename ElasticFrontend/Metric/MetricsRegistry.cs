using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace ElasticFrontend.Metric
{
    public class MetricsRegistry
    {
        public static CounterOptions LoginSuccessful => new CounterOptions()
        {
            Context = "Login_Successful",
            Name = "Frontend",
            MeasurementUnit = Unit.Calls,
            ResetOnReporting = true,
        };

        public static GaugeOptions RegistredUser => new GaugeOptions()
        {
            Name = "Frontend",
            Context = "Registred_User",
            MeasurementUnit = Unit.Calls
        };

        public static MeterOptions LoginMeter => new MeterOptions()
        {
            Name = "Frontend",
            Context = "Login_Meter",
            MeasurementUnit = Unit.Calls,
            RateUnit = TimeUnit.Hours,
        };

        public static TimerOptions DBTimer => new TimerOptions()
        {
            Context = "DB_Timer",
            Name = "Frontend",
            MeasurementUnit = Unit.Requests,
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Minutes
        };

        public static HistogramOptions PostAndPutRequestSize => new HistogramOptions()
        {
            Context = "Web_Request_Post_Put_Size",
            Name = "Frontend",
            MeasurementUnit = Unit.Bytes
        };

        public static CounterOptions TestCounter => new CounterOptions()
        {
            Context = "Test_Counter1",
            Name = "Frontend",
            MeasurementUnit = Unit.Calls,
            ResetOnReporting = true,
        };

        public static CounterOptions RegisterCounter => new CounterOptions()
        {
            Context = "Register_Successful",
            Name = "Frontend",
            MeasurementUnit = Unit.Calls,
        };

        public static GaugeOptions InactiveUsers => new GaugeOptions()
        {
            Name = "Frontend",
            Context = "Inactive_Users",
            MeasurementUnit = Unit.Calls
        };

        public static GaugeOptions RegistredSemester => new GaugeOptions()
        {
            Name = "Frontend",
            Context = "Registred_Semester",
            MeasurementUnit = Unit.Calls
        };

        public static CounterOptions CreateSemesterSuccessful => new CounterOptions()
        {
            Name = "Frontend",
            Context = "Create_Semester_Successful",
            MeasurementUnit = Unit.Calls,
        };

        public static CounterOptions DeleteSemester => new CounterOptions()
        {
            Name = "Frontend",
            Context = "Delete_Semester",
            MeasurementUnit = Unit.Calls,
        };

        public static GaugeOptions ActiveSemesters => new GaugeOptions()
        {
            Name = "Frontend",
            Context = "Active_Semester",
            MeasurementUnit = Unit.Calls,
        };

        public static GaugeOptions ExpiredSemesters => new GaugeOptions()
        {
            Name = "Frontend",
            Context = "Expired_Semesters",
            MeasurementUnit = Unit.Calls,
        };

    }
}

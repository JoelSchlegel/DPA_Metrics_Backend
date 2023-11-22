using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace ElasticFrontend.Metric
{
    public class MetricsRegistry
    {
        public static CounterOptions RegisterCounter => new CounterOptions()
        {
            Context = "Frontend",
            Name = "Register_Successful",
            MeasurementUnit = Unit.Calls,
        };

        public static CounterOptions LoginSuccessful => new CounterOptions()
        {
            Context = "Frontend",
            Name = "Login_Successful",
            MeasurementUnit = Unit.Calls
        };

        public static CounterOptions LoginUnsuccessful => new CounterOptions()
        {
            Context = "Frontend",
            Name = "Login_Unsuccessful",
            MeasurementUnit = Unit.Calls
        };

        public static CounterOptions LogoutSuccessful => new CounterOptions()
        {
            Context = "Frontend",
            Name = "Logout_Successful",
            MeasurementUnit = Unit.Calls
        };

        public static CounterOptions LogoutUnsuccessful => new CounterOptions()
        {
            Context = "Frontend",
            Name = "Logout_Unsuccessful",
            MeasurementUnit = Unit.Calls
        };

        public static MeterOptions LoginApiSuccessful => new MeterOptions()
        {
            Context = "Frontend",
            Name = "Api_Login",
            MeasurementUnit = Unit.Calls,
            RateUnit = TimeUnit.Milliseconds,
            ResetOnReporting = true,
        };

        public static TimerOptions DBTimer => new TimerOptions()
        {
            Context = "Frontend",
            Name = "DB_Timer",
            MeasurementUnit = Unit.Requests,
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Minutes
        };

        public static GaugeOptions RegistredUser => new GaugeOptions()
        {
            Name = "Frontend",
            Context = "Registred_User",
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

        public static CounterOptions CreateSemesterUnsuccessful => new CounterOptions()
        {
            Name = "Frontend",
            Context = "Create_Semester_Unsuccessful",
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

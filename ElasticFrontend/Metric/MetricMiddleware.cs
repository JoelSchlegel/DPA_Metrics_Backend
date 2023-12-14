using App.Metrics;
using ElasticFrontend.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElasticFrontend.Metric
{
    public class MetricMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MetricMiddleware> _logger;
        private readonly IMetrics _metrics;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public MetricMiddleware(RequestDelegate next, ILogger<MetricMiddleware> logger, IMetrics metrics, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _next = next;
            _metrics = metrics;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var contextpath = context.Request.Path;
            var userAgent = context.Request.Headers["User-Agent"].ToString();

            if (context.Request.Path.StartsWithSegments(new PathString("/metrics-text"))) //metrics-text
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<SampleUser>>();
                        var usersInDB = await userManager.Users.ToListAsync();
                        _metrics.Measure.Gauge.SetValue(MetricsRegistry.RegistredUser, usersInDB.Count);

                        var inactiveUsers = await userManager.Users.Where(x => x.LastLogin == null || x.LastLogin < DateTime.Now.AddDays(-7)).ToListAsync();
                        _metrics.Measure.Gauge.SetValue(MetricsRegistry.InactiveUsers, inactiveUsers.Count);

                        var semesterManager = scope.ServiceProvider.GetService<IdentityContext>();
                        var allSemester = await semesterManager.Semester.ToListAsync();
                        _metrics.Measure.Gauge.SetValue(MetricsRegistry.RegistredSemester, allSemester.Count);

                        var allActiveSemesters = allSemester.FindAll(s => s.IsActive);
                        _metrics.Measure.Gauge.SetValue(MetricsRegistry.ActiveSemesters, allActiveSemesters.Count);

                        var allExpiredSemester = allSemester.FindAll(s => s.IsExpired);
                        _metrics.Measure.Gauge.SetValue(MetricsRegistry.ExpiredSemesters, allExpiredSemester.Count);

                        _logger.LogInformation("Registred Users in DB: {usersInDB.Count}", usersInDB.Count);


                        //metricsResetter.ResetMetrics();
                    }
                }
                catch
                {
                    string errorText = "Middleware Invoker failed";
                    _logger.LogError(nameof(InvokeAsync) + "; " + errorText);
                }
            }

            await _next(context);

            if (userAgent.Contains("Elastic-Metricbeat"))
            {
                var metricsResetter = new MetricsResetter(_metrics);
                metricsResetter.ResetMetrics();
            }
        }
    }
}

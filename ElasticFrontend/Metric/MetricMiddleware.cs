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
        //private readonly UserManager<SampleUser> _userManager;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public MetricMiddleware(RequestDelegate next, ILogger<MetricMiddleware> logger, IMetrics metrics, IServiceScopeFactory serviceScopeFactory/*, UserManager<SampleUser> userManager*/)
        {
            _logger = logger;
            _next = next;
            _metrics = metrics;
            //_userManager = userManager;
            _serviceScopeFactory = serviceScopeFactory;

        }

        public async Task InvokeAsync(HttpContext context)
        {

            var contextpath = context.Request.Path;
            if (context.Request.Path.StartsWithSegments(new PathString("/favicon.ico")))
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<SampleUser>>();
                        var allUser = await userManager.Users.ToListAsync();
                        _metrics.Measure.Gauge.SetValue(MetricsRegistry.RegistredUser, allUser.Count);

                        var semesterManager = scope.ServiceProvider.GetService<IdentityContext>();
                        var allSemester = await semesterManager.Semester.ToListAsync();
                        _metrics.Measure.Gauge.SetValue(MetricsRegistry.RegistredSemester, allSemester.Count);

                        var allActiveSemesters = allSemester.FindAll(s => s.IsActive);
                        _metrics.Measure.Gauge.SetValue(MetricsRegistry.ActiveSemesters, allActiveSemesters.Count);

                        var allExpiredSemester = allSemester.FindAll(s => s.IsExpired);
                        _metrics.Measure.Gauge.SetValue(MetricsRegistry.ExpiredSemesters, allExpiredSemester.Count);
                    }
                }
                catch
                {
                    string errorText = "Middleware Invoker failed";
                    _logger.LogError(nameof(InvokeAsync) + "; " + errorText);
                }
            }
            await _next(context);
        }
    }
}

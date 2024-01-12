using App.Metrics;
using ElasticFrontend.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElasticFrontend.Metric
{
    public class MetricsUpdater
    {
        private readonly ILogger _logger;
        private readonly IMetrics _metrics;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public MetricsUpdater(ILogger logger, IMetrics metrics, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _metrics = metrics;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async void UpdateMetrics()
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

            }
        }
    }
}

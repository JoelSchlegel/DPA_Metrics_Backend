using App.Metrics;
using ElasticFrontend.Manager;
using ElasticFrontend.Metric;
using ElasticFrontend.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElasticFrontend.Pages
{
    public class SemesterModel : PageModel
    {
        private readonly IdentityContext _context;
        private readonly ILogger _logger;
        private readonly IMetrics _metrics;

        private SemesterViewModel SemesterViewModel { get; set; }

        [BindProperty]
        public string CreatedBy { get { return SemesterViewModel.CreatedBy; } set { SemesterViewModel.CreatedBy = value; } }

        [BindProperty]
        public DateTime UploadDate { get { return SemesterViewModel.UploadDate; } set { SemesterViewModel.UploadDate = value; } }

        [BindProperty]
        public bool IsActive { get { return SemesterViewModel.IsActive; } set { SemesterViewModel.IsActive = value; } }

        [BindProperty]
        public bool IsExpired { get { return SemesterViewModel.IsExpired; } set { SemesterViewModel.IsExpired = value; } }



        public SemesterModel(IdentityContext context, ILogger<SemesterModel> logger, IMetrics metrics)
        {
            _context = context;
            _logger = logger;
            _metrics = metrics;

            if (SemesterViewModel == null)
                SemesterViewModel = new SemesterViewModel();
        }

        public IActionResult OnPost()
        {
            var validation = SemesterValidation();

            if (validation)
            {
                var saveResult = SaveSemesterData();

                if (saveResult)
                {
                    TempData["Message"] = "Save Successful";
                }
                else
                {
                    TempData["Message"] = "Save Unsuccessful";
                }
            }
            else
            {
                TempData["Message"] = "Semester not valid";
            }
            return RedirectToPage();
        }

        private bool SaveSemesterData()
        {
            try
            {
                var manager = new SemesterManager(_logger, _metrics, _context);
                var semester = manager.Add(SemesterViewModel);

                _metrics.Measure.Counter.Increment(MetricsRegistry.CreateSemesterSuccessful);
                _logger.LogInformation("Semester successful created by {semester_creator}", semester.CreatedBy);
                return true;
            }
            catch (Exception ex)
            {
                _metrics.Measure.Counter.Increment(MetricsRegistry.CreateSemesterUnsuccessful);
                _logger.LogError(ex, "Semester: {Semester} create unsuccessful", SemesterViewModel);
                return false;
            }
        }

        private bool SemesterValidation()
        {
            if (SemesterViewModel.UploadDate > DateTime.Now && SemesterViewModel.CreatedBy != null)
                return true;
            else
            {
                _logger.LogError(nameof(SemesterValidation), "Semester is not valid", SemesterViewModel);
            }
            return false;
        }
    }
}

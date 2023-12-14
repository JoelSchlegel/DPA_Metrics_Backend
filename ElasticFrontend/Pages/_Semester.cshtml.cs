using App.Metrics;
using ElasticFrontend.Manager;
using ElasticFrontend.Metric;
using ElasticFrontend.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElasticFrontend.Pages
{
    public class _SemesterModel : PageModel
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

        [BindProperty]
        public List<SemesterViewModelExtended> Semesters { get; set; }

        public _SemesterModel(IdentityContext context, ILogger<_SemesterModel> logger, IMetrics metrics)
        {
            _context = context;
            _logger = logger;
            _metrics = metrics;


            if (SemesterViewModel == null)
                SemesterViewModel = new SemesterViewModel();
        }

        public async Task<IActionResult> OnGet()
        {
            var manager = new SemesterManager(_logger, _metrics, _context);
            Semesters = manager.GetAll();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateSemesterAsync()
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

        public async Task<IActionResult> OnPostDeleteSemesterAsync(int id)
        {
            try
            {
                var manager = new SemesterManager(_logger, _metrics, _context);
                var result = manager.Delete(id);
                if (result)
                    _metrics.Measure.Counter.Increment(MetricsRegistry.DeleteSemester);
            }
            catch (Exception ex)
            {
                _logger.LogError(nameof(OnPostDeleteSemesterAsync), ex);
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
                _logger.LogInformation(nameof(SaveSemesterData) + " Semester successful created by {semester_creator}", semester.CreatedBy);

                if (SemesterViewModel.IsActive == false && SemesterViewModel.IsExpired == false)
                    _logger.LogWarning(nameof(SaveSemesterData) + " Semester is neither active nor expired", semester.CreatedBy);  //Log-Warning
                return true;
            }
            catch (Exception ex)
            {
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
                _logger.LogError(nameof(SemesterValidation) + "Semester is not valid", SemesterViewModel);
            }
            return false;
        }
    }
}

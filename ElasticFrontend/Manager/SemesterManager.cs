using App.Metrics;
using ElasticFrontend.Entity;
using ElasticFrontend.Mapper;
using ElasticFrontend.ViewModel;

namespace ElasticFrontend.Manager
{
    public class SemesterManager
    {
        private ILogger _logger;
        private readonly IMetrics _metrics;
        private readonly IdentityContext _identityContext;

        public SemesterManager(ILogger logger, IMetrics metrics, IdentityContext context)
        {
            _logger = logger;
            _identityContext = context;
            _metrics = metrics;

        }

        public SemesterViewModelExtended Add(SemesterViewModel semester)
        {
            var result = _identityContext.Semester.Add(semester.ToEntity()).Entity;
            _identityContext.SaveChanges();
            return result.ToViewModelExtended();
        }

        public SemesterViewModelExtended Update(SemesterViewModelExtended semester)
        {
            var result = _identityContext.Semester.Update(semester.ToEntityExtended()).Entity;
            _identityContext.SaveChanges();

            return result.ToViewModelExtended();
        }
        public bool Delete(int id)
        {
            var entity = _identityContext.Semester.Where(x => x.Id == id).FirstOrDefault();

            if (entity != null)
            {
                _identityContext.Semester.Remove(entity);
                _identityContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<SemesterViewModelExtended> GetAll()
        {
            var result = _identityContext.Semester.ToList();
            var semesterViewModels = new List<SemesterViewModelExtended>();

            semesterViewModels = result.Select(x => x.ToViewModelExtended()).ToList();

            return semesterViewModels;
        }

        public List<SemesterViewModelExtended> Get(Semester semester)
        {
            var result = _identityContext.Semester.ToList();
            var semesterViewModels = new List<SemesterViewModelExtended>();

            semesterViewModels = result.Select(x => x.ToViewModelExtended()).ToList();

            return semesterViewModels;
        }

        public SemesterViewModelExtended Detail(int id)
        {
            var result = _identityContext.Semester.Find(id);
            if (result != null)
                return result.ToViewModelExtended();
            else
            {
                throw new InvalidDataException("Entity not Found");
            }
        }
    }
}

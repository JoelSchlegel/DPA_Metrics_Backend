using ElasticFrontend.Entity;
using ElasticFrontend.ViewModel;

namespace ElasticFrontend.Mapper
{
    public static class SemesterMapper
    {
        public static SemesterViewModel ToViewModel(this Semester semester)
        {
            var semesterViewModel = new SemesterViewModel()
            {
                CreatedBy = semester.CreatedBy,
                UploadDate = semester.UploadDate,
                IsActive = semester.IsActive,
                IsExpired = semester.IsExpired
            };
            return semesterViewModel;
        }

        public static SemesterViewModelExtended ToViewModelExtended(this Semester semester)
        {
            var semesterViewModel = new SemesterViewModelExtended()
            {
                Id = semester.Id,
                CreatedBy = semester.CreatedBy,
                UploadDate = semester.UploadDate,
                IsActive = semester.IsActive,
                IsExpired = semester.IsExpired
            };
            return semesterViewModel;
        }

        public static Semester ToEntity(this SemesterViewModel semesterViewModel)
        {
            var semester = new Semester()
            {
                CreatedBy = semesterViewModel.CreatedBy,
                UploadDate = semesterViewModel.UploadDate,
                IsActive = semesterViewModel.IsActive,
                IsExpired = semesterViewModel.IsExpired,
            };
            return semester;
        }

        public static Semester ToEntityExtended(this SemesterViewModelExtended semesterViewModelExtended)
        {
            var semester = new Semester()
            {
                Id = semesterViewModelExtended.Id,
                CreatedBy = semesterViewModelExtended.CreatedBy,
                UploadDate = semesterViewModelExtended.UploadDate,
                IsActive = semesterViewModelExtended.IsActive,
                IsExpired = semesterViewModelExtended.IsExpired,
            };
            return semester;
        }

    }
}

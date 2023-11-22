namespace ElasticFrontend.Entity
{
    public class Semester
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }
    }
}

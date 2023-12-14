using Microsoft.AspNetCore.Identity;

namespace ElasticFrontend.Areas.Identity.Data
{
    public class SampleUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime LastLogin { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElasticFrontend.Pages
{
    public class _SimulateHttpDurationModel : PageModel
    {
        public void OnGet()
        {
            //throw new InvalidProgramException();
            System.Threading.Thread.Sleep(10000);
        }
    }
}

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElasticFrontend.Pages
{
    public class _SimulateUnhandledExceptionModel : PageModel
    {
        public void OnGet()
        {
            throw new InvalidOperationException("Simulate unhandled Exception");
        }
    }
}

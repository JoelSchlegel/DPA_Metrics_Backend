using App.Metrics;
using ElasticFrontend.Response;
using ElasticFrontend.ViewModel;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElasticFrontend.Pages
{
    public class _ExternalInterfaceModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IMetrics _metrics;

        public List<WeatherViewModel> _weatherData;

        public _ExternalInterfaceModel(HttpClient httpClient, ILogger<_ExternalInterfaceModel> logger, IMetrics metrics)
        {
            _httpClient = httpClient;
            _logger = logger;
            _metrics = metrics;

            _weatherData = new List<WeatherViewModel>();
        }

        public async void OnGet()
        {
            await GetResultFromApi();
        }

        private async Task GetResultFromApi()
        {
            string apiKey = "69f715ef0cd64679b2395025231112";
            string cityName = "St. Gallen";
            string url = $"http://api.weatherapi.com/v1/current.json?key={apiKey}&q={cityName}";

            try
            {

                var response = _httpClient.GetFromJsonAsync<WeatherResponse>(url).Result;

                if (response != null)
                {
                    _weatherData.Add(new WeatherViewModel
                    {
                        City = response.Location.name,
                        Temperature = response.Current.temp_c,
                        Humidity = response.Current.humidity,
                    });

                    _logger.LogInformation(nameof(GetResultFromApi) + "; " + "successful Api Call");
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, nameof(GetResultFromApi));
            }

        }
    }
}

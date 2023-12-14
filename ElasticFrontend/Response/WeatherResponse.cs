namespace ElasticFrontend.Response
{
    public class WeatherResponse
    {
        public Location Location { get; set; }
        public Current Current { get; set; }
    }

    public class Location
    {
        public string name { get; set; }
        public string region { get; set; }
        public string country { get; set; }

        //    "name": "Saint Gallen",
        //    "region": "",
        //    "country": "Switzerland",
        //    "lat": 47.47,
        //    "lon": 9.4,
        //    "tz_id": "Europe/Zurich",
        //    "localtime_epoch": 1702288345,
        //    "localtime": "2023-12-11 10:52"
    }
    public class Current
    {
        public double temp_c { get; set; }
        public double humidity { get; set; }

        //"last_updated_epoch": 1702287900,
        //"last_updated": "2023-12-11 10:45",
        //"temp_c": 11.0,
        //"temp_f": 51.8,
        //"is_day": 1,
        //"condition": {
        //    "text": "Moderate rain",
        //    "icon": "//cdn.weatherapi.com/weather/64x64/day/302.png",
        //    "code": 1189
        //},
        //"wind_mph": 11.9,
        //"wind_kph": 19.1,
        //"wind_degree": 230,
        //"wind_dir": "SW",
        //"pressure_mb": 1009.0,
        //"pressure_in": 29.8,
        //"precip_mm": 1.56,
        //"precip_in": 0.06,
        //"humidity": 87,
        //"cloud": 75,
        //"feelslike_c": 9.0,
        //"feelslike_f": 48.1,
        //"vis_km": 7.0,
        //"vis_miles": 4.0,
        //"uv": 2.0,
        //"gust_mph": 18.1,
        //"gust_kph": 29.1
    }
}

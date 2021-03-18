using Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WpfClient
{
    public class WeatherService
    {
        public async Task<List<WeatherForecast>> GetWeather(string town,CancellationToken token)
        {
            var client = new HttpClient();

            var response = await client.GetAsync($"http://localhost:9876/WeatherForecast/{town}", token);
            var responseString = await response.Content.ReadAsStringAsync();
            var forecasts = JsonConvert.DeserializeObject<List<WeatherForecast>>(responseString);

            return forecasts;
        }


        public  List<WeatherForecast> GetWeatherSync(string town)
        {
            var client = new WebClient();

            var response =  client.DownloadString($"http://localhost:9876/WeatherForecast/{town}");
            var forecasts = JsonConvert.DeserializeObject<List<WeatherForecast>>(response);

            return forecasts;
        }
    }
}

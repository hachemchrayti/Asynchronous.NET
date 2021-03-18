using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WpfClient
{
    public class WeatherService
    {
        public async Task GetWeather(string town, CancellationToken token, IProgress<(string, List<WeatherForecast>)> p)
        {
            var client = new HttpClient();

            var response = await client.GetAsync($"http://localhost:9876/WeatherForecast/{town}", token);
            var responseString = await response.Content.ReadAsStringAsync();
            var forecasts = JsonConvert.DeserializeObject<List<WeatherForecast>>(responseString);
            p?.Report((town, forecasts));
        }

        public Task<List<WeatherForecast>> GetWeatherAsync(string town)
        {
            var tcs = new TaskCompletionSource<List<WeatherForecast>>();
            var client = new WebClient();

            client.DownloadStringAsync(new Uri($"http://localhost:9876/WeatherForecast/{town}"));

            client.DownloadStringCompleted += (s, e) =>
            {
                var forecasts = JsonConvert.DeserializeObject<List<WeatherForecast>>(e.Result);
                tcs.SetResult(forecasts);
            };

            return tcs.Task;
        }

        public List<WeatherForecast> GetWeatherSync(string town)
        {
            var client = new WebClient();

            var response = client.DownloadString(new Uri($"http://localhost:9876/WeatherForecast/{town}"));

            var forecasts = JsonConvert.DeserializeObject<List<WeatherForecast>>(response);
            return forecasts;
        }
    }
}

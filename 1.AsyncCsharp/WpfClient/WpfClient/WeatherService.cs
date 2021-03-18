using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Common;
using Newtonsoft.Json;

namespace WpfClient
{
   public  class WeatherService
    {
       

        public  async Task<string> getWeatherResult()
        {
            var client = new WebClient();
            var response = await client.DownloadStringTaskAsync("http://localhost:9876/WeatherForecast");
            var forecasts = JsonConvert.DeserializeObject<List<WeatherForecast>>(response);
            return string.Join(Environment.NewLine, forecasts.Select(f => $"Le {f.Date:dd/MM/yyyy}, il fera {f.TemperatureC}°"));
          

        }
    }
}

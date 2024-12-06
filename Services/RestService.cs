using APIOpenWeather.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json;

namespace APIOpenWeather.Services
{
    public class RestService : IRestService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _serializerOptions;

        public RestService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<WeatherData> GetWeatherData(string url)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(url);
                    Debug.WriteLine($"Status Code: {response.StatusCode}");

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Response JSON: {json}");
                        return JsonConvert.DeserializeObject<WeatherData>(json);
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Error: {response.StatusCode}, Content: {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error fetching weather data: {ex.Message}");
                }

                return null;
            }
        }
    }
}

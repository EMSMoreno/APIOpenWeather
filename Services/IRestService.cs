using APIOpenWeather.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIOpenWeather.Services
{
    public interface IRestService
    {
        Task<WeatherData> GetWeatherData(string cityName);
    }
}

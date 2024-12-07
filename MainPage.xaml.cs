using APIOpenWeather.Models;
using APIOpenWeather.Pages;
using APIOpenWeather.Services;
using APIOpenWeather.Validators;
using Newtonsoft.Json;
using System.Diagnostics;

namespace APIOpenWeather
{
    public partial class MainPage : ContentPage
    {
        private readonly IRestService _restService;

        private readonly ApiService _apiService;
        private readonly IValidator _validator;
        private bool _loginPageDisplayed = false;

        public MainPage(IRestService restService, ApiService apiService, IValidator validator)
        {
            InitializeComponent();
            _restService = restService;
            _apiService = apiService;
            _validator = validator;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_cidade.Text))
            {
                await DisplayAlert("Error", "Please enter a valid city name.", "OK");
                return;
            }

            string url = GenerateRequestURL(Constants.OpenWeatherMapEndpoint);
            Debug.WriteLine($"Generated URL: {url}");

            WeatherData weatherData = await _restService.GetWeatherData(url);

            if (weatherData != null)
            {
                Debug.WriteLine($"Weather Data: {JsonConvert.SerializeObject(weatherData)}");
                BindingContext = weatherData;
            }
            else
            {
                await DisplayAlert("Error", "Failed to fetch weather data. Please try again.", "OK");
            }
        }

        private string GenerateRequestURL(string endPoint)
        {
            string requestUri = endPoint;
            requestUri += $"?q={_cidade.Text}";
            requestUri += "&units=metric";
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            requestUri += "&lang=en-US";

            Debug.WriteLine($"Request URL: {requestUri}");
            return requestUri;
        }

        // Handle the Profile button click
        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage(_apiService, _validator));
        }

        // Handle the About Me button click
        private async void OnAboutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }

        // Handle the About API button click
        private async void OnAboutAPIClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutApi());
        }

        // Handle the FAQ button click
        private async void OnFaqClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FaqPage());
        }

    }

}

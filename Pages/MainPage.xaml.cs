using APIOpenWeather.Models;
using APIOpenWeather.Services;
using APIOpenWeather.Validators;

namespace APIOpenWeather.Pages;

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
        if (!string.IsNullOrWhiteSpace(_cidade.Text))
        {
            try
            {
                WeatherData weatherData = await _restService.GetWeatherData(_cidade.Text);
                BindingContext = weatherData;
            }
            catch (Exception ex)
            {
                // Tratar erros adequadamente
                await DisplayAlert("Error", $"Failed to fetch weather data: {ex.Message}", "OK");
            }
        }
        else
        {
            await DisplayAlert("Validation", "Please enter a city name.", "OK");
        }
    }


    private string GenerateRequestURL(string endPoint)
    {
        string requestUri = endPoint;
        requestUri += $"?q={_cidade.Text}";
        requestUri += "&units=metric";
        requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
        requestUri += $"&lang=en-US";
        return requestUri;
    }
}
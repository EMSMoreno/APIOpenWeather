using APIOpenWeather.Pages;
using APIOpenWeather.Services;
using APIOpenWeather.Validators;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace APIOpenWeather
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddHttpClient<ApiService>(client =>
            {
                client.BaseAddress = new Uri("https://fj507lsz-44324.uks1.devtunnels.ms/");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            });

            builder.Services.AddHttpClient<IRestService, RestService>(client =>
            {
                client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/weather/");
            });

            builder.Services.AddSingleton<IRestService, RestService>();
            builder.Services.AddSingleton<ApiService>();
            builder.Services.AddSingleton<IValidator, Validator>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ProfilePage>();
            return builder.Build();
        }
    }
}
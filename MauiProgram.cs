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
            // Register HttpClient for RestService
            builder.Services.AddHttpClient<RestService>(client =>
            {
                client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            // Register RestService as IRestService
            builder.Services.AddSingleton<IRestService, RestService>();

            // Register HttpClient for ApiService
            builder.Services.AddHttpClient<ApiService>(client =>
            {
                client.BaseAddress = new Uri("https://fj507lsz-44324.uks1.devtunnels.ms/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            // Register Validator service
            builder.Services.AddSingleton<IValidator, Validator>();

            // Register pages
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ProfilePage>();

            return builder.Build();
        }
    }
}
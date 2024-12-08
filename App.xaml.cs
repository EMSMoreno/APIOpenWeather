using APIOpenWeather.Pages;
using APIOpenWeather.Services;
using APIOpenWeather.Validators;

namespace APIOpenWeather
{
    public partial class App : Application
    {
        private readonly ApiService _apiService;
        private readonly IValidator _validator;
        private readonly IRestService _restService;

        public App(ApiService apiService, IValidator validator, IRestService restService)
        {
            InitializeComponent();
            _apiService = apiService;
            _validator = validator;
            _restService = restService;

            SetMainPage();
        }

        private void SetMainPage()
        {
            var accessToken = Preferences.Get("accesstoken", string.Empty);

            if (string.IsNullOrEmpty(accessToken))
            {
                MainPage = new NavigationPage(new LoginPage(_apiService, _validator, _restService));
                return;
                //MainPage = new NavigationPage(new MainPage(_restService, _apiService, _validator));
                //return;
            }

            MainPage = new AppShell(_restService, _apiService, _validator);
        }
    }
}
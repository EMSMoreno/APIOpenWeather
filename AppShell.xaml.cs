using APIOpenWeather.Pages;
using APIOpenWeather.Services;
using APIOpenWeather.Validators;

namespace APIOpenWeather
{
    public partial class AppShell : Shell
    {
        private readonly ApiService _apiService;
        private readonly IValidator _validator;
        private readonly IRestService _restService;

        public AppShell(IRestService restService, ApiService apiService, IValidator validator)
        {
            InitializeComponent();

            _restService = restService ?? throw new ArgumentNullException(nameof(restService));
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));


            Routing.RegisterRoute("ProfilePage", typeof(ProfilePage));
            Routing.RegisterRoute("AboutPage", typeof(AboutPage));
            Routing.RegisterRoute("FaqPage", typeof(FaqPage));
        }

        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//ProfilePage");
        }

        private async void OnAboutClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//AboutPage");
        }

        private async void OnFaqClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//FaqPage");
        }
    }
}

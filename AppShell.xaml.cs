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
            var homePage = new MainPage(restService, apiService, validator);

            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            ConfigureShell();
        }

        private void ConfigureShell()
        {
            var homePage = new MainPage(_restService, _apiService, _validator);
            var profilePage = new ProfilePage(_apiService, _validator);
            var aboutPage = new AboutPage(_apiService, _validator, _restService);
            var faqPage = new FaqPage();

            Items.Add(new TabBar
            {
                Items =
        {
            new ShellContent { Title = "Home", Icon = "home.png", Content = homePage },
            new ShellContent { Title = "Profile", Icon = "profile.png", Content = profilePage },
            new ShellContent { Title = "About Me", Icon = "about.png", Content = aboutPage },
            new ShellContent { Title = "FAQ", Icon = "faq.png", Content = faqPage }
        }
            });
        }
    }
}

using APIOpenWeather.Services;
using APIOpenWeather.Validators;

namespace APIOpenWeather.Pages;

public partial class LoginPage : ContentPage
{
    private readonly IRestService _restService;
    private readonly ApiService _apiService;
    private readonly IValidator _validator;

    public LoginPage(ApiService apiService, IValidator validator, IRestService restService)
	{
		InitializeComponent();
        _apiService = apiService;
        _validator = validator;
        _restService = restService ?? throw new ArgumentNullException(nameof(restService));
    }

    private async void BtnSignIn_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(EntEmail.Text))
        {
            await DisplayAlert("Error", "Provide an Email", "Cancel");
            return;
        }

        if (string.IsNullOrEmpty(EntPassword.Text))
        {
            await DisplayAlert("Error", "Provide a Password", "Cancel");
            return;
        }

        var response = await _apiService.Login(EntEmail.Text, EntPassword.Text);

        if (!response.HasError)
        {
            Application.Current!.MainPage = new AppShell(_restService, _apiService, _validator);
        }
        else
        {
            await DisplayAlert("Error", $"Something went wrong: {response.ErrorMessage}", "Cancel");
        }
    }

    private async void TapRegister_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage(_apiService, _validator, _restService));
    }
}
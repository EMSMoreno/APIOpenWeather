using APIOpenWeather.Services;
using APIOpenWeather.Validators;

namespace APIOpenWeather.Pages;

public partial class RegisterPage : ContentPage
{
    private readonly IRestService _restService;
    private readonly ApiService _apiService;
    private readonly IValidator _validator;

    public RegisterPage(ApiService apiService, IValidator validator, IRestService restService)
	{
		InitializeComponent();
        _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _restService = restService;
    }

    private async void BtnSignup_Clicked(object sender, EventArgs e)
    {
        if (await _validator.Validate(EntName.Text, EntEmail.Text, EntPhone.Text, EntPassword.Text))
        {
            var response = await _apiService.RegisterUser(EntName.Text, EntEmail.Text, EntPhone.Text, EntPassword.Text);

            if (!response.HasError)
            {
                await DisplayAlert("Notice", "Your account has been created successfully!", "OK");
                await Navigation.PushAsync(new LoginPage(_apiService, _validator, _restService));
            }
            else
            {
                await DisplayAlert("Error", "Something went wrong!", "Cancel");
            }
        }
        else
        {
            string errorMessage = "";

            errorMessage += _validator.NameError != null ? $"\n- {_validator.NameError}" : "";
            errorMessage += _validator.EmailError != null ? $"\n- {_validator.EmailError}" : "";
            errorMessage += _validator.PhoneError != null ? $"\n- {_validator.PhoneError}" : "";
            errorMessage += _validator.PasswordError != null ? $"\n- {_validator.PasswordError}" : "";

            await DisplayAlert("Error", errorMessage, "OK");
        }
    }

    private async void TapLogin_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new LoginPage(_apiService, _validator, _restService));
    }
}
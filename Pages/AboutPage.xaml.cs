using APIOpenWeather.Services;
using APIOpenWeather.Validators;

namespace APIOpenWeather.Pages;

public partial class AboutPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private readonly IRestService _restService;
    private bool _loginPageDisplayed = false;

    public AboutPage(ApiService apiService, IValidator validator, IRestService restService)
	{
		InitializeComponent();
        LblUserName.Text = Preferences.Get("username", string.Empty);
        _apiService = apiService;
        _validator = validator;
        _restService = restService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        BtnProfileImg.Source = await GetProfileImage();
    }

    private async Task<string?> GetProfileImage()
    {
        // Get the AppConfig default image
        string defaultImage = AppConfig.DefaultProfileImage;

        var (response, errorMessage) = await _apiService.GetUserProfileImage();

        // Handle error cases
        if (errorMessage != null)
        {
            switch (errorMessage)
            {
                case "Unauthorized":
                    if (!_loginPageDisplayed)
                    {
                        await DisplayLoginPage();
                        return null;
                    }
                    break;
                default:
                    await DisplayAlert("Error", errorMessage ?? "Unable to retrieve image.", "OK");
                    return defaultImage;
            }
        }

        if (response?.UrlImage != null)
        {
            return response.UrlImage;
        }

        return defaultImage;
    }

    private async Task DisplayLoginPage()
    {
        _loginPageDisplayed = true;
        await Navigation.PushAsync(new LoginPage(_apiService, _validator, _restService));
    }

    private async void BtnProfileImg_Clicked(object sender, EventArgs e)
    {
        try
        {
            var arrayImage = await SelectImageAsync();
            if (arrayImage == null)
            {
                await DisplayAlert("Error", "Unable to load image", "Ok");
                return;
            }
            BtnProfileImg.Source = ImageSource.FromStream(() => new MemoryStream(arrayImage));

            var response = await _apiService.UploadUserImage(arrayImage);
            if (response.Data)
            {
                await DisplayAlert("", "Image sent successfully", "Ok");
            }
            else
            {
                await DisplayAlert("Error", response.ErrorMessage ?? "An unknown error occurred", "Cancel");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "Ok");
        }
    }

    private async Task<byte[]?> SelectImageAsync()
    {
        try
        {
            var arquive = await MediaPicker.PickPhotoAsync();

            if (arquive is null) return null;

            using (var stream = await arquive.OpenReadAsync())
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        catch (FeatureNotSupportedException)
        {
            await DisplayAlert("Error", "Feature is not supported by the device", "Ok");
        }
        catch (PermissionException)
        {
            await DisplayAlert("Error", "Permissions not granted to access camera or gallery", "Ok");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error selecting image: {ex.Message}", "Ok");
        }
        return null;
    }

    private void BtnLogout_Clicked(object sender, EventArgs e)
    {
        Preferences.Set("accesstoken", string.Empty);
        Application.Current!.MainPage = new NavigationPage(new LoginPage(_apiService, _validator, _restService));
    }
}
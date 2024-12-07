using APIOpenWeather.Services;
using APIOpenWeather.Validators;

namespace APIOpenWeather.Pages;

public partial class ProfilePage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;

    public ProfilePage(ApiService apiService, IValidator validator)
	{
		InitializeComponent();
        LblUserName.Text = Preferences.Get("username", string.Empty);
        _apiService = apiService;
        _validator = validator;

        // LinkedIn Gesture
        var linkedInTap = new TapGestureRecognizer
        {
            Command = new Command(() => OpenUrl("https://www.linkedin.com/in/eduardo-moreno-1bb8b71b3/"))
        };
        LblLinkedIn.GestureRecognizers.Add(linkedInTap);

        // GitHub Gesture
        var gitHubTap = new TapGestureRecognizer
        {
            Command = new Command(() => OpenUrl("https://github.com/EMSMoreno/"))
        };
        LblGitHub.GestureRecognizers.Add(gitHubTap);
    }

    private void OpenUrl(string url)
    {
        try
        {
            Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to open URL: {ex.Message}");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        BtnProfileImg.Source = await GetProfileImage();
    }

    private async Task<string?> GetProfileImage()
    {
        string defaultImage = "user1.png";

        var (response, errorMessage) = await _apiService.GetUserProfileImage();

        if (errorMessage != null)
        {
            await DisplayAlert("Error", errorMessage ?? "Unable to retrieve image.", "OK");
            return defaultImage;
        }

        return response?.UrlImage ?? defaultImage;
    }

    private async void BtnProfileImg_Clicked(object sender, EventArgs e)
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
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error selecting image: {ex.Message}", "Ok");
            return null;
        }
    }

    private void BtnLogout_Clicked(object sender, EventArgs e)
    {
        Preferences.Set("accesstoken", string.Empty);

        var restService = DependencyService.Resolve<IRestService>();

        Application.Current!.MainPage = new NavigationPage(new LoginPage(_apiService, _validator, restService));
    }


}
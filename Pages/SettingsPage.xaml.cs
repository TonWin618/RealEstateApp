namespace RealEstateApp.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}

    private void TapLogout_Tapped(object sender, TappedEventArgs e)
    {
		Preferences.Set("accesstoken", string.Empty);
		Application.Current.MainPage = new LoginPage();
    }
}
using RealEstateApp.Pages;

namespace RealEstateApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		var aceessToken = Preferences.Get("accesstoken",string.Empty);
		if(string.IsNullOrEmpty(aceessToken))
		{
			MainPage = new LoginPage();
		}
		else
		{
			MainPage= new CustomTabbedPage();
		}
	}
}

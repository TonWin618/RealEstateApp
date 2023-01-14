using RealEstateApp.Models;
using RealEstateApp.Services;

namespace RealEstateApp.Pages;

public partial class PropertiesListPage : ContentPage
{
	public PropertiesListPage(int categoryId, string categoryName)
	{
		InitializeComponent();
		Title= categoryName;
		GetPropertiesList(categoryId);
	}

    private async void GetPropertiesList(int categoryId)
    {
		var properties = await ApiService.GetPropertyByCategory(categoryId);
		CvProperties.ItemsSource = properties;
    }

    private void ImgBackBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private void CvProperties_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as PropertyByCategory;
        if (currentSelection == null) return;
        Navigation.PushModalAsync(new PropertyDetailPage(currentSelection.Id));
        ((CollectionView)sender).SelectedItem = null;
    }
}
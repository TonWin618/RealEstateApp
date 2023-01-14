using RealEstateApp.Models;
using RealEstateApp.Services;

namespace RealEstateApp.Pages;

public partial class SearchPage : ContentPage
{
	public SearchPage()
	{
		InitializeComponent();
	}

    private void ImgBackBtn_Clicked(object sender, EventArgs e)
    {
		Navigation.PopModalAsync();
    }

    private async void SbProperty_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue == null) return;
        var propertiesResult = await ApiService.FindProperties(e.NewTextValue);
        CvSearch.ItemsSource = propertiesResult;
    }

    private void CvSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as SearchProperty;
        if (currentSelection == null) return;
        Navigation.PushModalAsync(new PropertyDetailPage(currentSelection.Id));
        ((CollectionView)sender).SelectedItem = null;
    }
}
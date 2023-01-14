using RealEstateApp.Models;
using RealEstateApp.Services;

namespace RealEstateApp.Pages;

public partial class PropertyDetailPage : ContentPage
{
    private string phoneNumber;
    private bool IsBookmarkEnabled;
    private int propertyId;
    private int bookmarkId;
	public PropertyDetailPage(int propertyId)
	{
		InitializeComponent();
		GetPropertyDetail(propertyId);
        this.propertyId = propertyId;

    }

    private async void GetPropertyDetail(int propertyId)
    {
        var property = await ApiService.GetPropertyDetail(propertyId);
		LblPrice.Text = "$" + property.Price;
		LblDescription.Text = property.Detail;
		LblAddress.Text = property.Address;
		ImgProperty.Source = property.FullImageUrl;
        phoneNumber = property.Phone;
        if(property.Bookmark == null)
        {
            ImgBookmarkBtn.Source = "bookmark_empty_icon";
            IsBookmarkEnabled= false;
        }
        else
        {
            ImgBookmarkBtn.Source = property.Bookmark.Status ? "bookmark_fill_icon" : "bookmark_empty_icon";
            bookmarkId = property.Bookmark.Id;
            IsBookmarkEnabled = true;
        }

    }

    private  void TapMessage_Tapped(object sender, TappedEventArgs e)
    {
        var message = new SmsMessage("Hi! I am interested in your property", phoneNumber);
        Sms.ComposeAsync(message);
    }

    private void TapCall_Tapped(object sender, TappedEventArgs e)
    {
        PhoneDialer.Default.Open(phoneNumber);
    }

    private void ImgBackBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private async void ImgBookmarkBtn_Clicked(object sender, EventArgs e)
    {
        if(IsBookmarkEnabled == false)
        {
            var addBookmark = new AddBookmark()
            {
                User_Id = Preferences.Get("userid", 0),
                PropertyId = this.propertyId
            };
            var response = await ApiService.AddBookmark(addBookmark);
            if (response)
            {
                ImgBookmarkBtn.Source = "bookmark_fill_icon";
                IsBookmarkEnabled = true;
            }
        }
        else
        {
            var response = await ApiService.DeleteBookmark(bookmarkId);
            if(response == true)
            {
                ImgBookmarkBtn.Source = "bookmark_empty_icon";
                IsBookmarkEnabled = false;
            }
        }
    }
}

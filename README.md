This is a project I made following the asfend's course. But some of the features didn't work well on Android, so I made a few changes to make it work as I expect. Below are my study notes.  Forgive me there are some areas that read unsmoothly, my English is not that good.

## 01.Intruduction
Course Links:[Build Real World App with .NET MAUI | Udemy](https://www.udemy.com/course/build-real-world-app-with-net-maui/?persist_locale=&locale=en_US)
Description: Learn to make Real Estate App with .NET MAUI From Beginning to End
Instructor: Asfend

## 02.Config Application Backend
How to make a ASP.Net API is not the point in this course, so for us, it's enough to know how to publish it.
I didn't find the file named `realestateapp01.PublishSettings` in given assets.However, after trying many times, it finally works by checking the option named `delete other files on the target`.

## 03.Test Rest Api Endpotints
It works very well, but there is one area for improvement: the tedious process that changing the domain name in the request url.

## 04.Getting Started With .NET MAUI Project
create a .Net MAUI project and add assets the instructor have published.
### File directory:
Properties:
  launchSettings
Dependencies:
  net7.0-android
  net7.0-ios
  net7.0-maccatalyst
  net7.0-windows10.0.19041.0
Platforms:
  Android
  iOS
  MacCatalyst
  Tizen
  WIndows
Resources:
  AppIcon
  Fonts
  Images
  Raw
  Splash
  Styles
App.xaml
  App.xaml.cs
MainPage.xaml
  MainPage.xaml.cs
MauiProgram.cs
## 05.Create Model Classes
### Steps to create a Model Class:
1.Search `Newtonsoft.Json` in NuGet Software Package Manager and install it.
2.Import `Newtonsoft.Json` in the model class file.
3.Copy and paste the response's body into the text box on the left side of the following website.
[Convert JSON to C# Classes Online - Json2CSharp Toolkit](https://json2csharp.com/)
4.Check `Use Pascal Case` option and `Add JsonProperty Attributes` option.
5.Click the `Convert` button and paste the converted result into the inside of the target model class as fields.

This section is boring, just doing a same thing over and over again. So I highly advice you paste the rest of models after create two or three models.

## 06.Create Service Layer
Add a new folder called `Services` in the root directory. Then, add a new `cs` file under this directory, named it `ApiService.cs`. All the API interfaces we are going to used will be written here. Most of them are asynchronous function, because waiting for a response from the server always takes some time. There are some examples:
```csharp
public static async Task<bool> RegsiterUser(string name, string email, string password, string phone)
{
	var register = new Register()
	{
		Name = name,
		Email = email,
		Password = password,
		Phone = phone
	};
	var httpClient = new HttpClient();
	var json = JsonConvert.SerializeObject(register);
	var content = new StringContent(json, Encoding.UTF8, "application/json");
	var response = await httpClient.PostAsync(AppSettings.ApiUrl +  "api/users/register", content);
	if (!response.IsSuccessStatusCode) return false;
	return true;
}

public static async Task<List<Category>> GetCategories()
{
	var httpClient = new HttpClient();
	httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("aceesstoken",string.Empty));
	var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/categories");
	return JsonConvert.DeserializeObject<List<Category>>(response);
}
```
Steps to create a API request function:
1.Create an instance of `HttpClient` class. We usually name it `httpClient`.
2.If it is an authorization-required request, such as access token, you may need to set the Authorization field in the header of it.
3.send request through some functions of the `HttpClient` class, such as `GetStringAsync`, `PostAsync`, `PutAsync` and `DeleteAsync`. Don't forget to add `await` keyword before calling these functions.
4.If this function you defined contain the handling json format data part, you need to use `DeserializeObject`  or `SerializeObject` function to deserialize or serialize it and then you can use it like an instance of a related model class.

## 07.Register Page
A maui page consists of XAML file and CS file. XAML is a markup language, similar to HTML.
MAUI is designed based on `MVVM` mode, XAML is the `View`  layer, and XAML.CS is the `ViewModel` layer. The `Model` layer we already defined in the section 5. 
There I list a few tags and their attributes we are going to use in this section:
```html
<ContentPage Title=""></ContentPage>

<VerticalStackLayout HorizentalOptions="" VerticalOptions="" ></VerticalStackLayout>

<HorizentalStackLayout Margin="" Spacing="">
	<HorizontalStackLayout.GestureRecognizers>
		<TapGestureRecognizer x:Name="TapLogin" Tapped="TapLogin_Tapped"/>
	</HorizontalStackLayout.GestureRecognizers>
</HorizentalStackLayout>

<Label Text="" TextColor="" FontSize=""/>

<Image source="" HeightRequest="" WidthRequest=""/>

<Entry x:Name="" Placeholder="" IsPassword=""/>

<Button x:Name="" Text="" FontSize="" Clicked=""/>
```
Corresponding viewModel code:
```csharp
async void BtnLogin_Clicked(object sender, EventArgs e)
{
	var response = await ApiService.Login(EntEmail.Text, EntPassword.Text);
	async void BtnRegister_Clicked(object sender, EventArgs e)
	{
		var response = await ApiService.RegsiterUser(EntFullName.Text, EntEmail.Text, EntPassword.Text, EntPhone.Text);
		if (response)
		{
			await DisplayAlert("", "Your account has been created", "Alright");
			await Navigation.PushModalAsync(new LoginPage());
		}
		else
		{
			await DisplayAlert("", "Oops something went wrong", "Cancel");
		}
	}
}

private async void TapJoinNow_Tapped(object sender, TappedEventArgs e)
{
	await Navigation.PushModalAsync(new RegisterPage());
}
```
If we name a attribute in xaml, we can operate it in viewModel code. 
In above code, we realize page-jumping function by two different ways. One is assigning 
 `Application.Current.MainPage` , and the another is calling the `Navigation.PushModelAsync` function. However, there are some differences between them. If we assigning  `Application.Current.MainPage` , the user can't come back to previous page. If we call `Navigation.PushModelAsync` function, the user can come back to previous page by swiping screen left.
 `DisplayAlert` will display a pop-up dialog, the first argument is dialog's title, the second argument is dialog's content, and the third argument is the text of dialog's button.
## 08.Login page
Much the same as in the previous section.
## 09.Tabbed Page and Pages Flow
As before, I list a few tags and their attributes that haven't been mentioned before but used in this section.
```html
<TabbedPage xmlns:local="clr-namespace:RealEstateApp.Pages"
			xmlns:android="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;assembly=Microsoft.Maui.Controls"
            android:TabbedPage.ToolbarPlacement="Bottom"
            SelectedTabColor="#1d94ab"
            UnselectedTabColor="#78909c">
            <local:Page1 Title="page1" IconImageSource=""/>
            <local:Page2 Title="page2" IconImageSource=""/>
</TabbedPage>
```
I don't know why the app released will stop running in both my android phone and android simulator when we jump from `HomePage` to `TabbedPage` if I use `NavigationPage` tag in `TabbedPage` tag as he said. So I deleted the `NavigationPage` tag and added a navigation bar on the `propertyListPage` to realize the page-jumping function.

## 10.Home Page
As before, I list a few tags and their attributes that haven't been mentioned before but we used in this section.
```html
<Grid RowSpacing="">
	<Grid.RowDefinitions>
		<RowDefinition Height=""/>
	</Grid.RowDefinitions>
	<Border Grid.Row="0">
		<Border.StrokeShape>
			<RoundRectangle ConorRadius=""/>
		</Border.StrokeShape>
		<Border.GestureRecognizers>
			<TapGestureRecognize/>
		</Border.GestureRecognizers>
	</Border>
</Grid>

<CollectionView SelectionMode=""
				SelectionChanged="">
	<CollectionView.ItemsLayout>
		<LinearItemsLayout ItemSpacing="" Orientation=""/>
	</CollectionView.ItemsLayout>
	
	<CollectionView.ItemTemplate>
		<DataTamplate>
		
		</DataTamplate>
	</CollectionView.ItemTemplate>
<\CollectionView>

<Label Text="{Binding text}"/>
```
Corresponding viewModel code:
```csharp
private async void GetCategories()
{
	var categories = await ApiService.GetCategories();
	CvCategories.ItemsSource = categories;
}
```
If we want to show information about a list of a model we defined on our webpage,  the wise choice is using the `CollectionView` tag. In collectionView, we can describe a template and bind the list data, then data will be assigned automaticlly to the binding attributes.

## 11.Properties List Page
As before, I list a few tags and their attributes that haven't been mentioned before but we used in this section.
```html
<ImageButton x:Name="ImgBackBtn"
			 Source="back_icon"
			 Clicked="ImgBackBtn_Clicked"/>
```

```csharp
private void ImgBackBtn_Clicked(object sender, EventArgs e)
{
	Navigation.PopModalAsync();
}
```
`Navigation.PopModalAsync` function is calling to turn back to previous page.

## 12.Property  Detail Page
Much the same as in the previous section.
## 13.Search Page
Much the same as in the previous section.
```html
<SearchBar />
```
## 14.Bookmark Page
Much the same as in the previous section.
## 15.Settings Page
Much the same as in the previous section.

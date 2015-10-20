using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;
using bluemart.Models.Local;
using bluemart.MainViews;

namespace bluemart
{
	public partial class ReceiptView : ContentPage
	{
		private UserClass mUserModel = new UserClass();
		private CartPage mCartPage;

		public ReceiptView (Page parent)
		{			
			InitializeComponent ();
			mCartPage = parent as CartPage;
			mUserModel = mUserModel.GetUser ();
			NavigationPage.SetHasNavigationBar (this, false);
			SetMainGridDefinitions ();
			SetTopGridDefinitions ();
			SetMiddleGridDefinitions ();
			PopulateTopGrid ();
			PopulateMiddleGrid ();
			SetButtonSize ();
		}

		private void SetMainGridDefinitions()
		{
			MainGrid.RowDefinitions [0].Height = MyDevice.ScreenHeight * 3 / 10;
			//MainGrid.RowDefinitions [1].Height = MyDevice.ScreenHeight * 6 / 10;
			MainGrid.RowDefinitions [2].Height = MyDevice.ScreenHeight  / 10;
			MainGrid.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
			MainGrid.BackgroundColor = Color.White;
			ScrollViewGrid.BackgroundColor = Color.White;
		}

		private void SetTopGridDefinitions()
		{
			/*TopGrid.RowDefinitions [0].Height = MyDevice.ScreenHeight * 1 / 10;
			TopGrid.RowDefinitions [1].Height = MyDevice.ScreenHeight * 1 / 10;*/
			TopGrid.ColumnDefinitions [0].Width = MyDevice.ScreenWidth / 2;
			TopGrid.ColumnDefinitions [1].Width = MyDevice.ScreenWidth / 2;


			DateLabel.FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label));
			DateText.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
			NameLabel.FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label));
			NameText.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
			AddressLabel.FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label));
			AddressText.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
			PhoneLabel.FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label));
			PhoneText.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
		}

		private void SetMiddleGridDefinitions()
		{	

			QuantityLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
			ProductNameLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
			DescriptionLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
			CostLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));

			AgreeButton.BackgroundColor = Color.White;
			AgreeButton.TextColor = Color.Green;
			AgreeButton.BorderWidth = 2;
			AgreeButton.BorderColor = MyDevice.BlueColor;

			DisagreeButton.BackgroundColor = Color.White;
			DisagreeButton.TextColor = MyDevice.RedColor;
			DisagreeButton.BorderWidth = 2;
			DisagreeButton.BorderColor = MyDevice.BlueColor;
		}

		private void PopulateTopGrid()
		{
			DateText.Text = DateTime.Now.ToString ();
			NameText.Text = mUserModel.Name;
			AddressText.Text = mUserModel.Address;
			PhoneText.Text = mUserModel.PhoneNumber;
		}

		private void PopulateMiddleGrid()
		{
			for (int row = 0; row < Cart.ProductsInCart.Count; row++) {
				ScrollViewGrid.RowDefinitions.Add (new RowDefinition ());

				string quantity = Cart.ProductsInCart[row].ProductNumberInCart.ToString();
				string name = Cart.ProductsInCart [row].Name;
				string description = Cart.ProductsInCart [row].Quantity;
				string cost = (Cart.ProductsInCart [row].ProductNumberInCart * Cart.ProductsInCart [row].Price).ToString ();

				Label quantityLabel = new Label () {
					Text = quantity,
					FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label)),
					HorizontalOptions = LayoutOptions.Start,
					TextColor = Color.Black
				};

				Label nameLabel = new Label () {
					Text = name,
					FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label)),
					HorizontalOptions = LayoutOptions.StartAndExpand,
					TextColor = Color.Black
				};

				Label descriptionLabel = new Label () {
					Text = description,
					FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label)),
					HorizontalOptions = LayoutOptions.StartAndExpand,
					TextColor = Color.Black
				};

				Label costLabel = new Label () {
					Text = "DH " + cost,
					FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label)),
					HorizontalOptions = LayoutOptions.Center,
					TextColor = Color.Black
				};

				ScrollViewGrid.Children.Add (quantityLabel, 0, row);
				ScrollViewGrid.Children.Add (nameLabel, 1, row);
				ScrollViewGrid.Children.Add (descriptionLabel, 2, row);
				ScrollViewGrid.Children.Add (costLabel, 3, row);
			}

			//MiddleGrid.RowDefinitions.Add (new RowDefinition ());

			Label subSumLabel = new Label () {
				Text = "---------------",
				FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label)),
				HorizontalOptions = LayoutOptions.End,
				TextColor = Color.Black
			};

			MiddleGrid.Children.Add (subSumLabel, 3, 2);


			Label totalPriceLabel = new Label () {
				Text = "Total Price:",
				FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label)),
				HorizontalOptions = LayoutOptions.Center,
				TextColor = Color.Black
			};

			MiddleGrid.Children.Add (totalPriceLabel,2,3 );

			Label totalPriceText = new Label () {
				Text = "DH " + Cart.ProductTotalPrice.ToString(),
				FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label)),
				HorizontalOptions = LayoutOptions.Center,
				TextColor = Color.Black
			};

			MiddleGrid.Children.Add (totalPriceText,3,3 );

			MiddleGrid.RowDefinitions[1].Height =  MyDevice.ScreenHeight / 4;

			/*MiddleGrid.RowDefinitions[0].Height =  MyDevice.ScreenHeight / 10;
			MiddleGrid.RowDefinitions[1].Height =  MyDevice.ScreenHeight / 2;
			MiddleGrid.RowDefinitions [2].Height =  MyDevice.ScreenHeight / 10;
			MiddleGrid.RowDefinitions [3].Height =  MyDevice.ScreenHeight / 10;*/
		}

		private async void AgreeClicked( Object sender, EventArgs e )
		{
			await DisplayAlert ("Order Accepted", "Your Order Has Been Received!", "OK");
			mCartPage.ClearCart ();
			await Navigation.PopAsync ();
		}

		private async void DisagreeClicked( Object sender, EventArgs e )
		{
			await Navigation.PopAsync ();
		}

		private void SetButtonSize()
		{
			AgreeButton.HeightRequest = MyDevice.ScreenHeight / 10; 
			DisagreeButton.HeightRequest = MyDevice.ScreenHeight / 10;
		}
			
	}
}


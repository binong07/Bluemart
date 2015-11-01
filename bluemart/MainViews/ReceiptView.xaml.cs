using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;
using bluemart.Models.Local;
using bluemart.MainViews;
using bluemart.Models.Remote;

namespace bluemart
{
	public partial class ReceiptView : ContentPage
	{
		private UserClass mUserModel = new UserClass();
		private RootPage mParent;
		private HistoryClass mHistory = null;

		public ReceiptView (RootPage parent)
		{			
			InitializeComponent ();
			mParent = parent;
			mUserModel = mUserModel.GetUser ();
			NavigationPage.SetHasNavigationBar (this, false);
			SetMainGridDefinitions ();
			SetTopGridDefinitions ();
			SetMiddleGridDefinitions ();
			PopulateTopGrid ();
			PopulateMiddleGrid ();
			SetButtonSize ();
		}

		public ReceiptView (RootPage parent, HistoryClass history)
		{			
			InitializeComponent ();
			mParent = parent;
			mHistory = history;
			mUserModel = mUserModel.GetUser ();
			NavigationPage.SetHasNavigationBar (this, false);
			SetMainGridDefinitions ();
			SetTopGridDefinitions ();
			SetMiddleGridDefinitions ();
			PopulateTopGrid (history);
			PopulateMiddleGrid (history);
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

		private void PopulateTopGrid(HistoryClass history = null)
		{
			if (history == null) {
				DateText.Text = DateTime.Now.ToString ();
				NameText.Text = mUserModel.Name;
				//change
				//AddressText.Text = mUserModel.Address;
				PhoneText.Text = mUserModel.PhoneNumber;
			} else {
				DateText.Text = history.Date;
				NameText.Text = history.Name + " " + history.Surname;
				AddressText.Text = history.Address;
				PhoneText.Text = history.Phone;
			}
		}

		private void PopulateMiddleGrid(HistoryClass history = null)
		{
			int productCount = 0;
			if (history == null)
				productCount = Cart.ProductsInCart.Count;
			else
				productCount = history.ProductOrderList.Count;
			
			for (int row = 0; row < productCount; row++) {
				ScrollViewGrid.RowDefinitions.Add (new RowDefinition ());

				string quantity;
				string name ;
				string description;
				string cost;

				if (history == null) {
					quantity = Cart.ProductsInCart [row].ProductNumberInCart.ToString ();
					name = Cart.ProductsInCart [row].Name;
					description = Cart.ProductsInCart [row].Quantity;
					cost = (Cart.ProductsInCart [row].ProductNumberInCart * Cart.ProductsInCart [row].Price).ToString ();
				} else {
					var firstSplitArray = history.ProductOrderList [row].Split (',');
					quantity = firstSplitArray [0].Split (':') [1];
					name = firstSplitArray [1].Split (':') [1];
					description = firstSplitArray [2].Split (':') [1];
					cost = firstSplitArray [3].Split (':') [1];
				}

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

			string totalPrice = "DH ";

			if (history == null)
				totalPrice += Cart.ProductTotalPrice.ToString ();
			else
				totalPrice += history.TotalPrice;

			Label totalPriceText = new Label () {
				Text = totalPrice,
				FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label)),
				HorizontalOptions = LayoutOptions.Center,
				TextColor = Color.Black
			};

			MiddleGrid.Children.Add (totalPriceText,3,3 );

			MiddleGrid.RowDefinitions[1].Height =  MyDevice.ScreenHeight / 4;

			if (mHistory != null) {
				AgreeButton.Text = "OK";
				DisagreeButton.IsVisible = false;
			} else {
				AgreeButton.Text = "Agree";
				DisagreeButton.Text = "Disagree";
			}

			/*MiddleGrid.RowDefinitions[0].Height =  MyDevice.ScreenHeight / 10;
			MiddleGrid.RowDefinitions[1].Height =  MyDevice.ScreenHeight / 2;
			MiddleGrid.RowDefinitions [2].Height =  MyDevice.ScreenHeight / 10;
			MiddleGrid.RowDefinitions [3].Height =  MyDevice.ScreenHeight / 10;*/
		}

		private async void AgreeClicked( Object sender, EventArgs e )
		{
			if (mHistory == null) {
				if (MyDevice.GetNetworkStatus () != "NotReachable") {

					bool OrderSucceeded = OrderModel.SendOrderToRemote (mUserModel).Result;

					if (OrderSucceeded)
						await DisplayAlert ("Order Accepted", "Your order has been received!", "OK");
					else
						await DisplayAlert ("Connection Error", "Your order couldn't be delivered. Check your internet connection and try again.", "OK");
					
					mParent.mCartPage.ClearCart ();
					mParent.mFooter.ChangeColorOfLabel (mParent.mFooter.mCategoriesLabel);
					mParent.SwitchTab ("BrowseCategories");
				} else {
					await DisplayAlert ("Connection Error", "Your order couldn't be delivered. Check your internet connection and try again.", "OK");
				}
			} else {
				mParent.mFooter.ChangeColorOfLabel (mParent.mFooter.mHistoryLabel);
				mParent.SwitchTab ("History");
			}
		}

		private void DisagreeClicked( Object sender, EventArgs e )
		{
			if (mHistory == null) {
				mParent.LoadCartPage ();
			}

		}

		private void SetButtonSize()
		{
			AgreeButton.HeightRequest = MyDevice.ScreenHeight / 10; 
			DisagreeButton.HeightRequest = MyDevice.ScreenHeight / 10;
		}
			
	}
}


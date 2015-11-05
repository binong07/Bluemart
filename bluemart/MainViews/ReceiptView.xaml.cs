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
		private AddressClass mAddressModel = new AddressClass();
		private RootPage mParent;
		private Object mObject = null;

		public ReceiptView (RootPage parent)
		{			
			InitializeComponent ();
			mParent = parent;
			mUserModel = mUserModel.GetUser ();
			NavigationPage.SetHasNavigationBar (this, false);
			SetMainGridDefinitions ();
			SetTopGridDefinitions ();
			SetMiddleGridDefinitions ();
			SetBottomGridDefinitions ();
			PopulateTopGrid ();
			PopulateMiddleGrid ();
			SetButtonSize ();
		}

		public ReceiptView (RootPage parent, Object obj)
		{		
			

			InitializeComponent ();
			mParent = parent;
			mObject = obj;
			mUserModel = mUserModel.GetUser ();
			NavigationPage.SetHasNavigationBar (this, false);
			SetMainGridDefinitions ();
			SetTopGridDefinitions ();
			SetMiddleGridDefinitions ();
			SetBottomGridDefinitions ();
			PopulateTopGrid (obj);
			PopulateMiddleGrid (obj);
			SetButtonSize ();
		}

		private void SetMainGridDefinitions()
		{
			MainGrid.RowDefinitions [0].Height = MyDevice.ScreenHeight * 3 / 10;
			//MainGrid.RowDefinitions [1].Height = MyDevice.ScreenHeight * 6 / 10;
			MainGrid.RowDefinitions [2].Height = Device.GetNamedSize (NamedSize.Large, typeof(Label)) * 3;
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
			AgreeButton.WidthRequest = MyDevice.ScreenWidth / 2.5f;

			DisagreeButton.BackgroundColor = Color.White;
			DisagreeButton.TextColor = MyDevice.RedColor;
			DisagreeButton.BorderWidth = 2;
			DisagreeButton.BorderColor = MyDevice.BlueColor;
			DisagreeButton.WidthRequest = MyDevice.ScreenWidth / 2.5f;
		}

		private void SetBottomGridDefinitions()
		{
			BottomGrid.ColumnDefinitions [0].Width = MyDevice.ViewPadding;
			BottomGrid.ColumnDefinitions [1].Width = (MyDevice.ScreenWidth - 2*MyDevice.ViewPadding) / 2;
			BottomGrid.ColumnDefinitions [2].Width = (MyDevice.ScreenWidth - 2*MyDevice.ViewPadding) / 2;
			BottomGrid.ColumnDefinitions [3].Width = MyDevice.ViewPadding;
			//BottomGrid.RowDefinitions [0].Height = Device.GetNamedSize (NamedSize.Large, typeof(Label)) * 3;
		}

		private void PopulateTopGrid(Object obj = null)
		{
			if (obj == null) {
				DateText.Text = DateTime.Now.ToString ();
				AddressClass address = mAddressModel.GetActiveAddress (mUserModel.ActiveRegion);
				NameText.Text = address.Name;
				//change
				//AddressText.Text = mUserModel.Address;
				PhoneText.Text = address.PhoneNumber;
			} else {
				if (obj is HistoryClass) {
					HistoryClass history = obj as HistoryClass;
					DateText.Text = history.Date;
					NameText.Text = history.Name + " " + history.Surname;
					AddressText.Text = history.Address;
					PhoneText.Text = history.Phone;
				} else if (obj is StatusClass) {
					StatusClass status = obj as StatusClass;
					DateText.Text = status.Date;
					NameText.Text = status.Name + " " + status.Surname;
					AddressText.Text = status.Address;
					PhoneText.Text = status.Phone;
				}
			}
		}

		private void PopulateMiddleGrid(Object obj = null)
		{
			int productCount = 0;
			if (obj == null)
				productCount = Cart.ProductsInCart.Count;
			else {
				if( obj is HistoryClass )
					productCount = (obj as HistoryClass).ProductOrderList.Count;
				else if( obj is StatusClass )
					productCount = (obj as StatusClass).ProductOrderList.Count;
			}

			
			for (int row = 0; row < productCount; row++) {
				ScrollViewGrid.RowDefinitions.Add (new RowDefinition ());

				string quantity = "";
				string name = "";
				string description = "";
				string cost = "";

				if (obj == null) {
					quantity = Cart.ProductsInCart [row].ProductNumberInCart.ToString ();
					name = Cart.ProductsInCart [row].Name;
					description = Cart.ProductsInCart [row].Quantity;
					cost = (Cart.ProductsInCart [row].ProductNumberInCart * Cart.ProductsInCart [row].Price).ToString ();
				} else {
					if (obj is HistoryClass) {
						var firstSplitArray = (obj as HistoryClass).ProductOrderList [row].Split (',');
						quantity = firstSplitArray [0].Split (':') [1];
						name = firstSplitArray [1].Split (':') [1];
						description = firstSplitArray [2].Split (':') [1];
						cost = firstSplitArray [3].Split (':') [1];
					}
					else if (obj is StatusClass) {
						var firstSplitArray = (obj as StatusClass).ProductOrderList [row].Split (',');
						quantity = firstSplitArray [0].Split (':') [1];
						name = firstSplitArray [1].Split (':') [1];
						description = firstSplitArray [2].Split (':') [1];
						cost = firstSplitArray [3].Split (':') [1];
					}

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

			if (obj == null)
				totalPrice += Cart.ProductTotalPrice.ToString ();
			else {
				if (obj is HistoryClass) {
					totalPrice += (obj as HistoryClass).TotalPrice;
				} else if (obj is StatusClass) {
					totalPrice += (obj as StatusClass).TotalPrice;
				}
			}
				

			Label totalPriceText = new Label () {
				Text = totalPrice,
				FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label)),
				HorizontalOptions = LayoutOptions.Center,
				TextColor = Color.Black
			};

			MiddleGrid.Children.Add (totalPriceText,3,3 );

			MiddleGrid.RowDefinitions[1].Height =  MyDevice.ScreenHeight / 4;

			if (mObject != null) {
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
			if (mObject == null) {
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
				if (mObject is HistoryClass) {
					mParent.mFooter.ChangeColorOfLabel (mParent.mFooter.mHistoryLabel);
					mParent.SwitchTab ("History");
				} else if (mObject is StatusClass) {
					mParent.mFooter.ChangeColorOfLabel (mParent.mFooter.mTrackLabel);
					mParent.SwitchTab ("Track");
				}
			}
		}

		private void DisagreeClicked( Object sender, EventArgs e )
		{
			if (mObject == null) {
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


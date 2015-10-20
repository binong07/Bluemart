using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Objects;
using bluemart.Common.Utilities;
using System.Threading.Tasks;
using bluemart.Common.ViewCells;
using bluemart.Models.Local;

namespace bluemart.MainViews
{
	public partial class CartPage : ContentPage
	{
		UserClass mUserModel = new UserClass();
		private Label mTotalPriceLabel;
		private StackLayout mStackLayout;

		public CartPage ()
		{
			InitializeComponent ();
			mStackLayout = StackLayout1;
			NavigationPage.SetHasNavigationBar (this, false);

			SetGrid1Properties ();
			SetGrid2Properties ();
			AddTapRecognizers ();
			PrintDictionaryContents ();
		}

		private void SetGrid1Properties()
		{
			Grid1.RowDefinitions [0].Height = MyDevice.ScreenHeight / 15;
			Grid1.RowDefinitions [1].Height = GridLength.Auto;
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
		}

		private void SetGrid2Properties()
		{
			//Grid2.RowDefinitions [0].Height = MyDevice.ScreenHeight / 15;
			Grid2.ColumnDefinitions [0].Width = MyDevice.ScreenWidth / 5;
			Grid2.ColumnDefinitions [1].Width = MyDevice.ScreenWidth * 2 / 5;
			Grid2.ColumnDefinitions [2].Width = MyDevice.ScreenWidth * 2 / 5;
			//Grid2.ColumnDefinitions [2].Width = MyDevice.ScreenWidth - MyDevice.ScreenHeight / 15 - MyDevice.ScreenWidth * 2 / 5;

			//CloseButton.HeightRequest = MyDevice.ScreenHeight / 15;
			CloseButton.Aspect = Aspect.AspectFit;
			LocationLabel.TextColor = MyDevice.RedColor;
			LocationLabel.FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label));
			RemoveButton.TextColor = MyDevice.RedColor;
			RemoveButton.BorderColor = MyDevice.BlueColor;
		}

		private void AddTapRecognizers()
		{
			var closeButtonTapGestureRecognizer = new TapGestureRecognizer ();
			closeButtonTapGestureRecognizer.Tapped += async (sender, e) => {

				CloseButton.Opacity = 0.5f;
				await Task.Delay(200);
				await Navigation.PopAsync();

				//if ( Navigation.NavigationStack/. = BrowseProductsPage )
				CloseButton.Opacity = 1f;
			};
			CloseButton.GestureRecognizers.Add (closeButtonTapGestureRecognizer);
		}

		private void PrintDictionaryContents()
		{
			Cart.ProductTotalPrice = 0.0f;

			foreach (Product p in Cart.ProductsInCart) {
				mStackLayout.Children.Add( new CartCell( p,this ).View );
			}

			Grid orderGrid = new Grid (){ HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill, BackgroundColor = Color.White };
			orderGrid.ColumnDefinitions.Add (new ColumnDefinition(){Width = MyDevice.ScreenWidth/2});
			orderGrid.ColumnDefinitions.Add (new ColumnDefinition(){Width = MyDevice.ScreenWidth/2}); 
			orderGrid.RowDefinitions.Add (new RowDefinition(){Height = GridLength.Auto}); 


			mTotalPriceLabel = new Label () {
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Center,
				FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label)),
				TextColor = MyDevice.BlueColor
			};
			UpdateTotalPriceLabel ();
			orderGrid.Children.Add (mTotalPriceLabel, 0, 0);


			Button OrderButton = new Button (){HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center,Text = "ORDER NOW", TextColor = MyDevice.RedColor, BorderWidth = 2, BorderColor = MyDevice.BlueColor, BackgroundColor = Color.White};
			OrderButton.Clicked += (sender, e) => {
				if( Cart.ProductTotalPrice == 0 )
				{
					DisplayAlert("Failed","You don't have any product on cart","OK");
				}
				else if( String.IsNullOrWhiteSpace(mUserModel.GetUser().Address)  )
				{
					DisplayAlert("Failed","Please Enter Your Address On Settings Page","OK");
				}
				else
				{
					Navigation.PushAsync( new ReceiptView(this));
				}
			};
			orderGrid.Children.Add (OrderButton, 1, 0);
			mStackLayout.Children.Add (orderGrid);
		}

		public void ClearCart()
		{
			foreach (Product p in Cart.ProductsInCart) {
				p.ProductNumberInCart = 0;
			}

			Cart.ProductsInCart.Clear ();
			mStackLayout.Children.Clear ();

			foreach (Page p in Navigation.NavigationStack) {
				if (p is BrowseProductsPage) {
					BrowseProductsPage b = p as BrowseProductsPage;
					foreach (ProductCell pc in b.mProductCellList) {
						pc.UpdateNumberLabel ();
					}
				} else if (p is SearchPage) {
					SearchPage s = p as SearchPage;
					foreach (ProductCell pc in s.mProductCellList) {
						pc.UpdateNumberLabel ();
					}
				}
			}
		}

		async void OnClickedRemoveButton( Object sender, EventArgs e )
		{
			var answer =await DisplayAlert ("Remove Products", "Do you really want to remove products?", "Yes", "No");
			if (answer) {
				ClearCart ();			
			}
		}

		public void UpdateTotalPriceLabel()
		{
			mTotalPriceLabel.Text = "Total Price: " + Cart.ProductTotalPrice.ToString ();
		}

		public void RemoveProductFromCart(View CartCellView)
		{
			mStackLayout.Children.Remove (CartCellView);
		}
	}
}


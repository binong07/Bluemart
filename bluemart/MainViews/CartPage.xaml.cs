using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Objects;
using bluemart.Common.Utilities;
using System.Threading.Tasks;
using bluemart.Common.ViewCells;
using bluemart.Models.Local;
using System.Linq;

namespace bluemart.MainViews
{
	public partial class CartPage : ContentPage
	{
		UserClass mUserModel = new UserClass();
		private List<CartCell> mCartCellList = new List<CartCell> ();
		private Label mTotalPriceLabel;
		private StackLayout mStackLayout;

		public RootPage mParent;

		public CartPage (RootPage parent)
		{
			InitializeComponent ();
			mParent = parent;
			mStackLayout = StackLayout1;
			NavigationPage.SetHasNavigationBar (this, false);

			SetGrid1Properties ();
			SetGrid2Properties ();
		}

		private void SetGrid1Properties()
		{
			Grid1.RowDefinitions [0].Height = Device.GetNamedSize(NamedSize.Large,typeof(Label))*3;
			Grid1.RowDefinitions [1].Height = GridLength.Auto;
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
		}

		private void SetGrid2Properties()
		{			
			Grid2.ColumnDefinitions [0].Width = MyDevice.ViewPadding;
			Grid2.ColumnDefinitions [1].Width = (MyDevice.ScreenWidth - 2*MyDevice.ViewPadding) / 2;
			Grid2.ColumnDefinitions [2].Width = (MyDevice.ScreenWidth - 2*MyDevice.ViewPadding) / 2;
			Grid2.ColumnDefinitions [3].Width = MyDevice.ViewPadding;

			//CloseButton.HeightRequest = MyDevice.ScreenHeight / 15;
			//CloseButton.Aspect = Aspect.AspectFit;
			LocationLabel.TextColor = MyDevice.RedColor;
			LocationLabel.FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label));
			RemoveButton.FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label));
			RemoveButton.TextColor = MyDevice.RedColor;
			RemoveButton.BorderColor = MyDevice.BlueColor;
			RemoveButton.WidthRequest = MyDevice.ScreenWidth / 2.5f;
		}

		public void PrintDictionaryContents()
		{			
			mStackLayout.Children.Clear ();

			UserClass user = mUserModel.GetUser ();
			if (user.Name.Length > 0) 
				LocationLabel.Text = user.Name.Split (' ') [0].ToUpper () + "'S BASKET";
			else
				LocationLabel.Text = "BASKET";

			Cart.ProductTotalPrice = new Decimal(0.0);

			foreach (Product p in Cart.ProductsInCart) {
				var cartCell = new CartCell (p, this);
				mCartCellList.Add (cartCell);
				mStackLayout.Children.Add( cartCell.View );
			}

			Grid orderGrid = new Grid (){ HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Fill, BackgroundColor = Color.White, ColumnSpacing = 0 };
			orderGrid.ColumnDefinitions.Add(new ColumnDefinition(){Width = MyDevice.ViewPadding});
			orderGrid.ColumnDefinitions.Add(new ColumnDefinition(){Width = (MyDevice.ScreenWidth - 2*MyDevice.ViewPadding) / 2});
			orderGrid.ColumnDefinitions.Add(new ColumnDefinition(){Width = (MyDevice.ScreenWidth - 2*MyDevice.ViewPadding) / 2});
			orderGrid.ColumnDefinitions.Add(new ColumnDefinition(){Width = MyDevice.ViewPadding});
			orderGrid.RowDefinitions.Add (new RowDefinition(){Height = Device.GetNamedSize(NamedSize.Large,typeof(Label))*3}); 


			mTotalPriceLabel = new Label () {
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Center,
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
				TextColor = MyDevice.BlueColor
			};
			UpdateTotalPriceLabel ();
			orderGrid.Children.Add (mTotalPriceLabel, 1, 0);


			Button OrderButton = new Button (){HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center,Text = "ORDER NOW", TextColor = MyDevice.RedColor, BorderWidth = 2, BorderColor = MyDevice.BlueColor, BackgroundColor = Color.White};
			OrderButton.WidthRequest = RemoveButton.Width;
			OrderButton.HeightRequest = RemoveButton.Height;
			OrderButton.Clicked += (sender, e) => {
				if( Cart.ProductTotalPrice == 0 )
				{
					DisplayAlert("Failed","You don't have any product on cart","OK");
				}
				//change
				/*else if( String.IsNullOrWhiteSpace(mUserModel.GetUser().Address)  )
				{
					DisplayAlert("Failed","Please Enter Your Address On Settings Page","OK");
				}*/
				else if( Cart.ProductTotalPrice < 50 )
				{
					DisplayAlert("Failed","Please order AED 50, as this is the minimum order.","OK");
				}
				else
				{
					mParent.LoadReceiptPage();
				}
			};
			orderGrid.Children.Add (OrderButton, 2, 0);
			mStackLayout.Children.Add (orderGrid);
		}

		public void ClearCart()
		{
			foreach (Product p in Cart.ProductsInCart) {
				p.ProductNumberInCart = 0;
			}

			Cart.ProductsInCart.Clear ();
			Cart.ProductTotalPrice = new decimal(0.0);

			var stackLayoutChildren = mStackLayout.Children;

			foreach (var cartCell in mCartCellList) {
				mStackLayout.Children.Remove (cartCell.View);
			}


			UpdateTotalPriceLabel ();
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
			mParent.mRootHeader.mPriceLabel.Text = "DH " + Cart.ProductTotalPrice.ToString ();
			mParent.mTopNavigationBar.mPriceLabel.Text = "DH " + Cart.ProductTotalPrice.ToString ();
		}

		public void RemoveProductFromCart(View CartCellView)
		{
			mStackLayout.Children.Remove (CartCellView);
		}
	}
}


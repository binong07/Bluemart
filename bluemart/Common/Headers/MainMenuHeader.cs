using System;
using System.Collections.Generic;
using bluemart.Common.Utilities;
using bluemart.MainViews;
using Xamarin.Forms;
using bluemart.Common.Objects;

namespace bluemart.Common.Headers
{
	public partial class MainMenuHeader: Grid
	{
		public Label mPriceLabel;
		public RootPage mParent;
		public MainMenuHeader ()
		{
			
			InitializeComponent ();
			mPriceLabel = PriceLabel;
			SetGridDefinitions ();
			SetImageSize ();

			AddTapRecognizers ();
		}


		private void SetGridDefinitions()
		{
			this.RowDefinitions [0].Height = MyDevice.ScreenHeight / 10;
			//this.ColumnDefinitions [0].Width = MyDevice.ScreenHeight / 10;
			this.ColumnDefinitions [0].Width = MyDevice.ScreenWidth - MyDevice.ScreenHeight * 2 / 10;
			this.ColumnDefinitions [1].Width = MyDevice.ScreenHeight / 10;
			this.ColumnDefinitions [2].Width = MyDevice.ScreenHeight / 10;
			CartGrid.RowDefinitions [0].Height = MyDevice.ScreenHeight / 15;
			CartGrid.RowDefinitions [1].Height = MyDevice.ScreenHeight / 30;

			PriceLabel.TextColor = MyDevice.BlueColor;
			PriceLabel.FontSize = Device.GetNamedSize (NamedSize.Micro, typeof(Label));
		}

		private void SetImageSize()
		{
			//MenuButton.HeightRequest = MyDevice.ScreenHeight / 15;
			SearchButton.HeightRequest = MyDevice.ScreenHeight / 15;
			CartButton.Aspect = Aspect.AspectFill;
			LogoImage.HeightRequest = MyDevice.ScreenHeight / 15;
			//Lbl1.FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label));
		}

		private void AddTapRecognizers()
		{
			var cartButtonGestureRecognizer = new TapGestureRecognizer ();
			cartButtonGestureRecognizer.Tapped += (sender, e) => {

				CartButton.Opacity = 0.5f;
				mParent.LoadCartPage();
				//Navigation.PushAsync( new CartPage());
				CartButton.Opacity = 1f;
			};
			CartButton.GestureRecognizers.Add (cartButtonGestureRecognizer);

			var searchButtonGestureRecognizer = new TapGestureRecognizer ();
			searchButtonGestureRecognizer.Tapped += (sender, e) => {

				SearchButton.Opacity = 0.5f;
				Navigation.PushAsync( new SearchPage());
				SearchButton.Opacity = 1f;
			};
			SearchButton.GestureRecognizers.Add (searchButtonGestureRecognizer);
		}
	}
}

 
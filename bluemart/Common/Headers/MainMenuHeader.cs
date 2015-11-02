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
			this.RowDefinitions [0].Height = MyDevice.ScreenHeight / 12;
			this.ColumnDefinitions [0].Width = MyDevice.MenuPadding;
			this.ColumnDefinitions [1].Width = MyDevice.ScreenWidth - MyDevice.ScreenHeight  / 7 - MyDevice.MenuPadding*2;
			this.ColumnDefinitions [2].Width = MyDevice.ScreenHeight / 7;
			this.ColumnDefinitions [3].Width = MyDevice.MenuPadding;
	
			CartGrid.RowDefinitions [0].Height = MyDevice.ScreenHeight / 18;
			CartGrid.RowDefinitions [1].Height = MyDevice.ScreenHeight / 36;

			PriceLabel.TextColor = MyDevice.BlueColor;
			PriceLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
		}

		private void SetImageSize()
		{
			CartButton.Aspect = Aspect.AspectFill;
			LogoImage.HeightRequest = MyDevice.ScreenHeight / 15;
		}

		private void AddTapRecognizers()
		{
			var CartGridGestureRecognizer = new TapGestureRecognizer ();
			CartGridGestureRecognizer.Tapped += (sender, e) => {

				CartGrid.Opacity = 0.5f;
				mParent.LoadCartPage();
				CartGrid.Opacity = 1f;
			};
			CartGrid.GestureRecognizers.Add (CartGridGestureRecognizer);
		}
	}
}

 
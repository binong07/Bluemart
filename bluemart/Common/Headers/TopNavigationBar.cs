using System;
using System.Collections.Generic;
using bluemart.Common.Utilities;
using bluemart.MainViews;
using Xamarin.Forms;
using System.Threading.Tasks;
using PCLStorage;
using System.Linq;

namespace bluemart.Common.Headers
{
	public partial class TopNavigationBar : Grid
	{
		public Label NavigationText;
		public Label mPriceLabel;
		public RootPage mParent;
		public TopNavigationBar ()
		{
			InitializeComponent ();
			NavigationText = NavigationTitle;
			mPriceLabel = PriceLabel;
			NavigationTitle.TextColor = MyDevice.RedColor;
			SetGridDefinitions ();
			SetImageSize ();
			AddTapRecognizers ();

		}


		private void SetGridDefinitions()
		{
			this.RowDefinitions [0].Height = MyDevice.ScreenHeight / 10;
			this.ColumnDefinitions [0].Width = MyDevice.ScreenWidth - MyDevice.ScreenHeight / 10;
			this.ColumnDefinitions [1].Width = MyDevice.ScreenHeight / 10;
			CartGrid.RowDefinitions [0].Height = MyDevice.ScreenHeight / 15;
			CartGrid.RowDefinitions [1].Height = MyDevice.ScreenHeight / 30;

			PriceLabel.TextColor = MyDevice.BlueColor;
			PriceLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
		}

		private void SetImageSize()
		{
			CartButton.HeightRequest = MyDevice.ScreenHeight / 15;
			NavigationTitle.FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label));
		}

		private void AddTapRecognizers()
		{
			/*var backButtonTapGestureRecognizer = new TapGestureRecognizer ();
			backButtonTapGestureRecognizer.Tapped += async (sender, e) => {

				BackButton.Opacity = 0.5f;
				await Task.Delay(200);
				if( Navigation.NavigationStack.Last() is RootPage )
					(Navigation.NavigationStack.Last() as RootPage).SwitchTab("BrowseCategories");
				//await Navigation.PopAsync();
				BackButton.Opacity = 1f;
			};
			BackButton.GestureRecognizers.Add (backButtonTapGestureRecognizer);*/

			var cartButtonTapGestureRecognizer = new TapGestureRecognizer ();
			cartButtonTapGestureRecognizer.Tapped += async (sender, e) => {

				CartButton.Opacity = 0.5f;
				await Task.Delay(200);
				//await Navigation.PushAsync( new CartPage());
				mParent.LoadCartPage();
				CartButton.Opacity = 1f;
			};
			CartButton.GestureRecognizers.Add (cartButtonTapGestureRecognizer);
		}
	}
}

 
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
			this.RowDefinitions [0].Height = MyDevice.ScreenHeight / 12;
			this.ColumnDefinitions [0].Width = MyDevice.ScreenHeight   / 7;
			this.ColumnDefinitions [1].Width = MyDevice.ScreenWidth - MyDevice.ScreenHeight * 2 / 7;
			this.ColumnDefinitions [2].Width = MyDevice.ScreenHeight   / 7;
			CartGrid.RowDefinitions [0].Height = MyDevice.ScreenHeight / 18;
			CartGrid.RowDefinitions [1].Height = MyDevice.ScreenHeight / 36;

			PriceLabel.TextColor = MyDevice.BlueColor;
			PriceLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
		}

		private void SetImageSize()
		{
			BackButton.HeightRequest = MyDevice.ScreenHeight / 20;
			CartButton.HeightRequest = MyDevice.ScreenHeight / 18;
			NavigationTitle.FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label));
		}

		private void AddTapRecognizers()
		{		

			var cartButtonTapGestureRecognizer = new TapGestureRecognizer ();
			cartButtonTapGestureRecognizer.Tapped += async (sender, e) => {

				CartButton.Opacity = 0.5f;
				await Task.Delay(MyDevice.DelayTime);
				mParent.mFooter.ChangeColorOfLabel (mParent.mFooter.mCategoriesLabel);
				mParent.LoadCartPage();
				CartButton.Opacity = 1f;
			};
			CartButton.GestureRecognizers.Add (cartButtonTapGestureRecognizer);

			var backButtonTapGestureRecognizer = new TapGestureRecognizer ();
			backButtonTapGestureRecognizer.Tapped += async (sender, e) => {

				BackButton.Opacity = 0.5f;
				await Task.Delay(MyDevice.DelayTime);			
				mParent.SwitchTab("BrowseCategories");
				BackButton.Opacity = 1f;
			};
			BackButton.GestureRecognizers.Add (backButtonTapGestureRecognizer);
		}
	}
}

 
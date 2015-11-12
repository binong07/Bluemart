﻿using System;
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
			this.ColumnDefinitions [0].Width = MyDevice.MenuPadding;
			this.ColumnDefinitions [1].Width = MyDevice.ScreenHeight   / 7;
			this.ColumnDefinitions [2].Width = MyDevice.ScreenWidth - MyDevice.ScreenHeight * 2 / 7 - MyDevice.MenuPadding*2;
			this.ColumnDefinitions [3].Width = MyDevice.ScreenHeight   / 7;
			this.ColumnDefinitions [4].Width = MyDevice.MenuPadding;
			CartGrid.RowDefinitions [0].Height = MyDevice.ScreenHeight / 18;
			CartGrid.RowDefinitions [1].Height = MyDevice.ScreenHeight / 36;

			PriceLabel.TextColor = MyDevice.BlueColor;
			PriceLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
		}

		private void SetImageSize()
		{
			BackButton.HeightRequest = MyDevice.ScreenHeight / 20;
			CartButton.HeightRequest = MyDevice.ScreenHeight / 18;
			NavigationTitle.FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label));
		}

		private void AddTapRecognizers()
		{		

			var CartGridTapGestureRecognizer = new TapGestureRecognizer ();
			CartGridTapGestureRecognizer.Tapped += async (sender, e) => {
				if( mParent.mBrowseProductPage.mSearchBar.mSearchEntry.IsFocused )
					return;
				CartGrid.Opacity = 0.5f;
				await Task.Delay(MyDevice.DelayTime);
				mParent.mFooter.ChangeColorOfLabel (mParent.mFooter.mCategoriesLabel);
				mParent.LoadCartPage();
				CartGrid.Opacity = 1f;
			};
			CartGrid.GestureRecognizers.Add (CartGridTapGestureRecognizer);

			var backButtonTapGestureRecognizer = new TapGestureRecognizer ();
			backButtonTapGestureRecognizer.Tapped += async (sender, e) => {
				if( mParent.mCurrentPageParent == "BrowseCategories" && mParent.mBrowseProductPage.mSearchBar.mSearchEntry.IsFocused )
					return;
				BackButton.Opacity = 0.5f;
				await Task.Delay(MyDevice.DelayTime);			
				mParent.SwitchTab (mParent.mCurrentPageParent);
				BackButton.Opacity = 1f;
			};
			BackButton.GestureRecognizers.Add (backButtonTapGestureRecognizer);
		}
	}
}

 
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;
using bluemart.Common;
using bluemart.Common.Headers;

namespace bluemart.MainViews
{
	public partial class RootPage : ContentPage
	{
		BrowseCategoriesPage mBrowseCategoriesPage;
		SettingsPage mSettingsPage;
		FavoritesPage mFavoritesPage;
		public CartPage mCartPage;
		private string mCurrentPage = "";
		public Footer mFooter;
		public Grid mGrid;
		public TopNavigationBar mTopNavigationBar;
		public MainMenuHeader mRootHeader;

		private View mContentGrid;

		public RootPage ()
		{
			InitializeComponent ();
			SwitchHeaderVisibility (true);

			mFooter = Footer;
			mGrid = Grid1;
			mBrowseCategoriesPage = new BrowseCategoriesPage (this);
			mSettingsPage = new SettingsPage(this);
			mFavoritesPage = new FavoritesPage (this);
			mCartPage = new CartPage (this);
			RootHeader.mParent = this;
			ProductHeader.mParent = this;
			mTopNavigationBar = ProductHeader;
			mRootHeader = RootHeader;

			NavigationPage.SetHasNavigationBar (this, false);
			mCurrentPage = "BrowseCategories";
			SetGrid1Definitions ();

			mContentGrid = mBrowseCategoriesPage.Content;
			Grid1.Children.Add(mContentGrid,0,1);
		}

		private void SetGrid1Definitions()
		{
			Grid1.BackgroundColor = MyDevice.BlueColor;
			Grid1.RowDefinitions [0].Height = MyDevice.ScreenHeight / 12;
			Grid1.RowDefinitions [2].Height = MyDevice.ScreenHeight / 12;
		}

		private void SwitchContentGrid(View content)
		{
			Grid1.Children.Remove(mContentGrid);
			mContentGrid = content;
			Grid1.Children.Add(mContentGrid,0,1);
		}

		public void SwitchTab( string pageName )
		{
			if (pageName == mCurrentPage)
				return;
			
			switch (pageName) {
			case "BrowseCategories":
				SwitchHeaderVisibility (true);
				RootHeader.mPriceLabel.Text = "DH:" + Cart.ProductTotalPrice.ToString ();
				mBrowseCategoriesPage.RefreshSearchText ();
				SwitchContentGrid (mBrowseCategoriesPage.Content);
				mCurrentPage = pageName;
				break;
			case "Settings":
				SwitchHeaderVisibility (true);
				SwitchContentGrid (mSettingsPage.Content);
				mCurrentPage = pageName;
				break;
			case "Favorites":
				SwitchHeaderVisibility (true);
				mFavoritesPage.RefreshFavoritesGrid ();
				SwitchContentGrid (mFavoritesPage.Content);
				mCurrentPage = pageName;
				break;
			default:
				break;
			}
		}

		private void SwitchHeaderVisibility(bool bRootHeaderIsVisible)
		{
			RootHeader.IsVisible = bRootHeaderIsVisible;
			ProductHeader.IsVisible = !bRootHeaderIsVisible;
		}

		public void LoadProductsPage( Dictionary<string, List<Product>> productDictionary, Category category )
		{
			mCurrentPage = "";
			//Footer.SetLabelProperties ();
			SwitchHeaderVisibility (false);
			SwitchContentGrid ((new BrowseProductsPage(productDictionary,category,this)).Content);
		}

		public void LoadCartPage()
		{
			mCurrentPage = "";
			SwitchHeaderVisibility (true);
			Footer.SetLabelProperties ();
			SwitchContentGrid (mCartPage.Content);
			mCartPage.PrintDictionaryContents ();
		}

		public void LoadReceiptPage()
		{
			mCurrentPage = "";
			SwitchHeaderVisibility (true);
			Footer.SetLabelProperties ();
			SwitchContentGrid ((new ReceiptView (this)).Content);
		}

		public void LoadSearchPage(string searchString)
		{
			mCurrentPage = "";
			Footer.SetLabelProperties ();
			SwitchHeaderVisibility (true);
			SwitchContentGrid ((new SearchPage (searchString,this)).Content);
		}

		public void RemoveFooter()
		{
			Grid1.RowDefinitions.RemoveAt (2);
			Grid1.Children.Remove (mFooter);
		}
		public void AddFooter()
		{
			Grid1.RowDefinitions.Add (new RowDefinition (){ Height = MyDevice.ScreenHeight / 10 });
			Grid1.Children.Add (mFooter,0,2);
		}
	}
}


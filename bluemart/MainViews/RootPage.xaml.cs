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

			Grid1.Children.Add(mBrowseCategoriesPage.Content,0,1);
		}

		private void SetGrid1Definitions()
		{
			Grid1.RowDefinitions [0].Height = MyDevice.ScreenHeight / 10;
			Grid1.RowDefinitions [2].Height = MyDevice.ScreenHeight / 10;
		}

		public void SwitchTab( string pageName )
		{
			if (pageName == mCurrentPage)
				return;
			
			switch (pageName) {
			case "BrowseCategories":
				SwitchHeaderVisibility (true);
				Grid1.Children.RemoveAt (3);
				RootHeader.mPriceLabel.Text = "DH: " + Cart.ProductTotalPrice.ToString();
				mBrowseCategoriesPage.RefreshSearchText();
				Grid1.Children.Add(mBrowseCategoriesPage.Content,0,1);
				mCurrentPage = pageName;
				break;
			case "Settings":
				SwitchHeaderVisibility (true);
				Grid1.Children.RemoveAt (3);
				mSettingsPage.RefreshPriceInCart ();
				Grid1.Children.Add(mSettingsPage.Content,0,1);
				mCurrentPage = pageName;
				break;
			case "Favorites":
				SwitchHeaderVisibility (true);
				Grid1.Children.RemoveAt (3);
				mFavoritesPage.RefreshFavoritesGrid ();
				Grid1.Children.Add(mFavoritesPage.Content,0,1);
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
			Footer.SetLabelProperties ();
			SwitchHeaderVisibility (false);
			Grid1.Children.RemoveAt (3);
			Grid1.Children.Add((new BrowseProductsPage(productDictionary,category,this)).Content,0,1);
		}

		public void LoadCartPage()
		{
			mCurrentPage = "";
			SwitchHeaderVisibility (true);
			Footer.SetLabelProperties ();
			Grid1.Children.RemoveAt (3);
			Grid1.Children.Add(mCartPage.Content,0,1);
			mCartPage.PrintDictionaryContents ();
		}

		public void LoadReceiptPage()
		{
			mCurrentPage = "";
			SwitchHeaderVisibility (true);
			Footer.SetLabelProperties ();
			Grid1.Children.RemoveAt (3);
			Grid1.Children.Add((new ReceiptView(this)).Content,0,1);
		}

		public void LoadSearchPage(string searchString)
		{
			mCurrentPage = "";
			Footer.SetLabelProperties ();
			SwitchHeaderVisibility (true);
			Grid1.Children.RemoveAt (3);
			Grid1.Children.Add ((new SearchPage (searchString)).Content, 0, 1);
		}
	}
}


using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;

namespace bluemart.MainViews
{
	public partial class RootPage : ContentPage
	{
		BrowseCategoriesPage mBrowseCategoriesPage;
		SettingsPage mSettingsPage;
		FavoritesPage mFavoritesPage;
		public CartPage mCartPage;
		private string mCurrentPage = "";

		public RootPage ()
		{
			InitializeComponent ();
			mBrowseCategoriesPage = new BrowseCategoriesPage (this);
			mSettingsPage = new SettingsPage(this);
			mFavoritesPage = new FavoritesPage (this);
			mCartPage = new CartPage (this);
			NavigationPage.SetHasNavigationBar (this, false);
			mCurrentPage = "BrowseCategories";
			SetGrid1Definitions ();

			Grid1.Children.Add(mBrowseCategoriesPage.Content,0,0);
		}

		private void SetGrid1Definitions()
		{
			Grid1.RowDefinitions [1].Height = MyDevice.ScreenHeight / 10;
		}

		public void SwitchTab( string pageName )
		{
			if (pageName == mCurrentPage)
				return;
			
			switch (pageName) {
			case "BrowseCategories":
				Grid1.Children.RemoveAt (1);
				mBrowseCategoriesPage.RefreshPriceInCart ();
				Grid1.Children.Add(mBrowseCategoriesPage.Content,0,0);
				mCurrentPage = pageName;
				break;
			case "Settings":
				Grid1.Children.RemoveAt (1);
				mSettingsPage.RefreshPriceInCart ();
				Grid1.Children.Add(mSettingsPage.Content,0,0);
				mCurrentPage = pageName;
				break;
			case "Favorites":
				Grid1.Children.RemoveAt (1);
				mFavoritesPage.RefreshFavoritesGrid ();
				Grid1.Children.Add(mFavoritesPage.Content,0,0);
				mCurrentPage = pageName;
				break;
			default:
				break;
			}
		}

		public void LoadProductsPage( Dictionary<string, List<Product>> productDictionary, Category category )
		{
			mCurrentPage = "";
			Footer.SetLabelProperties ();
			Grid1.Children.RemoveAt (1);
			Grid1.Children.Add((new BrowseProductsPage(productDictionary,category,this)).Content,0,0);
		}

		public void LoadCartPage()
		{
			mCurrentPage = "";
			Footer.SetLabelProperties ();
			Grid1.Children.RemoveAt (1);
			Grid1.Children.Add(mCartPage.Content,0,0);
			mCartPage.PrintDictionaryContents ();
		}

		public void LoadReceiptPage()
		{
			mCurrentPage = "";
			Footer.SetLabelProperties ();
			Grid1.Children.RemoveAt (1);
			Grid1.Children.Add((new ReceiptView(this)).Content,0,0);
		}
	}
}


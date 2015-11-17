using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;
using bluemart.Common;
using bluemart.Common.Headers;
using bluemart.Models.Local;
using MR.Gestures;
using System.IO;
using System.Reflection;

namespace bluemart.MainViews
{
	public partial class RootPage : Xamarin.Forms.ContentPage
	{
		public BrowseCategoriesPage mBrowseCategoriesPage;
		public SettingsPage mSettingsPage;
		FavoritesPage mFavoritesPage;
		HistoryPage mHistoryPage;
		TrackPage mTrackPage;
		public BrowseProductsPage mBrowseProductPage;
		public AddAddressPage mAddAddressPage;
		public CartPage mCartPage;
		public string mCurrentPage = "";
		public string mCurrentPageParent = "";
		public Footer mFooter;
		public Xamarin.Forms.Grid mGrid;
		public TopNavigationBar mTopNavigationBar;
		public MainMenuHeader mRootHeader;
		private List<string> mPageList;

		public Stream mAddFavoritesImage;
		public Stream mRemoveFavoritesImage;
		public Stream mRemoveProductImage;
		public Stream mAddProductImage;
		//private int mActivePageIndex = 2;

		public View mContentGrid;

		public RootPage ()
		{
			InitializeComponent ();
			SwitchHeaderVisibility (true);

			var assembly = typeof(RootPage).GetTypeInfo().Assembly;
			mAddFavoritesImage = assembly.GetManifestResourceStream("bluemart.SavedImages.bookmark_add.png");
			mRemoveFavoritesImage = assembly.GetManifestResourceStream("bluemart.SavedImages.bookmark_remove.png");
			mRemoveProductImage = assembly.GetManifestResourceStream("bluemart.SavedImages.minus.png");
			mAddProductImage = assembly.GetManifestResourceStream("bluemart.SavedImages.plus.png");

			mFooter = Footer;
			mGrid = Grid1;
			mBrowseCategoriesPage = new BrowseCategoriesPage (this);
			mBrowseProductPage = new BrowseProductsPage (this);
			mSettingsPage = new SettingsPage(this);
			mFavoritesPage = new FavoritesPage (this);
			mHistoryPage = new HistoryPage (this);
			mTrackPage = new TrackPage (this);
			mCartPage = new CartPage (this);
			mPageList = new List<string> (){ "Track","Settings","BrowseCategories","Favorites","Cart" };

			RootHeader.mParent = this;
			ProductHeader.mParent = this;
			mTopNavigationBar = ProductHeader;
			mRootHeader = RootHeader;

			NavigationPage.SetHasNavigationBar (this, false);
			mCurrentPage = "BrowseCategories";
			SetGrid1Definitions ();

			mContentGrid = mBrowseCategoriesPage.Content;
			Grid1.Children.Add(mContentGrid,0,1);
			Grid1.Swiped += (sender, e) => {
				int indexOfCurrentPage = mPageList.IndexOf(mCurrentPage);
				indexOfCurrentPage = ( indexOfCurrentPage + 1 ) % mPageList.Count;
				//SwitchTab( mPageList[indexOfCurrentPage] );
			};
		}			

		private void SetGrid1Definitions()
		{
			Grid1.BackgroundColor = MyDevice.RedColor;
			Grid1.RowDefinitions [0].Height = MyDevice.ScreenHeight / 12;
			Grid1.RowDefinitions [2].Height = MyDevice.ScreenHeight / 14;
		}

		private void SwitchContentGrid(View content)
		{
			Grid1.Children.Remove(mContentGrid);
			mContentGrid = content;
			Grid1.Children.Add(mContentGrid,0,1);
		}	
		protected override bool OnBackButtonPressed ()
		{
			if( mCurrentPage != "BrowseCategories" )
				SwitchTab ("BrowseCategories");		
			else
				Navigation.PopAsync ();
			//Check if product page is active
			/*if( mTopNavigationBar.IsVisible == true ){					
				SwitchTab (mCurrentPageParent);
			}*/					
								
			return true;
		}

		public void SwitchTab( string pageName )
		{
			if (pageName == mCurrentPage)
				return;
			
			switch (pageName) {
			case "BrowseCategories":
				SwitchHeaderVisibility (true);

				if (mBrowseProductPage != null) {
					mBrowseProductPage.ClearContainers ();

				//	mBrowseProductPage = null;
				}

				mBrowseCategoriesPage.RefreshSearchText ();
				mFooter.ChangeColorOfLabel (mFooter.mCategoriesLabel);
				SwitchContentGrid (mBrowseCategoriesPage.Content);
				mCurrentPage = pageName;
				break;
			case "Settings":
				SwitchHeaderVisibility (true);
				mFooter.ChangeColorOfLabel (mFooter.mSettingsLabel);			
				mSettingsPage.PopulateListView();
				SwitchContentGrid (mSettingsPage.Content);
				mCurrentPage = pageName;
				break;
			case "Favorites":
				SwitchHeaderVisibility (true);
				mFavoritesPage.RefreshFavoritesGrid ();
				mFooter.ChangeColorOfLabel (mFooter.mFavoritesLabel);
				SwitchContentGrid (mFavoritesPage.Content);
				mCurrentPage = pageName;
				break;
			case "History":
				SwitchHeaderVisibility (true);
				mHistoryPage.PopulateListView ();
				mFooter.ChangeColorOfLabel (mFooter.mCartLabel);
				SwitchContentGrid (mHistoryPage.Content);
				mCurrentPage = pageName;
				break;
			case "Track":
				SwitchHeaderVisibility (true);
				mTrackPage.PopulateListView ();
				mFooter.ChangeColorOfLabel (mFooter.mTrackLabel);
				SwitchContentGrid (mTrackPage.Content);
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
			mCurrentPageParent = "BrowseCategories";
			SwitchContentGrid (mBrowseProductPage.Content);
			mBrowseProductPage.PopulationOfNewProductPage (productDictionary, category);
			//mBrowseProductPage = (new BrowseProductsPage (productDictionary, category, this)); 

		}

		public void LoadAddAddress(AddressClass address = null)
		{
			mCurrentPage = "";
			Footer.SetLabelProperties ();
			SwitchHeaderVisibility (false);
			mCurrentPageParent = "Settings";
			mAddAddressPage = (new AddAddressPage (address,this)); 
			SwitchContentGrid (mAddAddressPage.Content);
		}

		public void LoadCartPage()
		{
			mCurrentPage = "";
			SwitchHeaderVisibility (true);
			Footer.SetLabelProperties ();
			mFooter.ChangeColorOfLabel (mFooter.mCartLabel);
			mBrowseCategoriesPage.mSearchBar.mSearchEntry.Unfocus ();
			SwitchContentGrid (mCartPage.Content);
			mCartPage.PrintDictionaryContents ();
		}

		public void LoadReceiptPage(Object obj = null)
		{
			mCurrentPage = "";
			SwitchHeaderVisibility (true);
			Footer.SetLabelProperties ();
			if (obj == null)
				SwitchContentGrid ((new ReceiptView (this)).Content);
			else {
				SwitchContentGrid ((new ReceiptView (this,obj)).Content);
			}
				
		}

		public void LoadSearchPage(string searchString,string categoryId = "")
		{
			mCurrentPage = "";
			Footer.SetLabelProperties ();
			SwitchHeaderVisibility (true);
			SwitchContentGrid ((new SearchPage (searchString,categoryId,this)).Content);
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
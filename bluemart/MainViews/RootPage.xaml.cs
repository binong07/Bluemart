using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;
using bluemart.Common;
using bluemart.Common.Headers;
using bluemart.Models.Local;
using MR.Gestures;

namespace bluemart.MainViews
{
	public partial class RootPage : Xamarin.Forms.ContentPage
	{
		BrowseCategoriesPage mBrowseCategoriesPage;
		public SettingsPage mSettingsPage;
		FavoritesPage mFavoritesPage;
		HistoryPage mHistoryPage;
		TrackPage mTrackPage;
		public BrowseProductsPage mBrowseProductPage;
		public CartPage mCartPage;
		private string mCurrentPage = "";
		public Footer mFooter;
		public Xamarin.Forms.Grid mGrid;
		public TopNavigationBar mTopNavigationBar;
		public MainMenuHeader mRootHeader;
		private List<string> mPageList;
		//private int mActivePageIndex = 2;

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
			mHistoryPage = new HistoryPage (this);
			mTrackPage = new TrackPage (this);
			mCartPage = new CartPage (this);
			mPageList = new List<string> (){ "History","Track","BrowseCategories","Favorites","Settings" };

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
				SwitchTab( mPageList[indexOfCurrentPage] );
			};
		}

		/*void OnleftSwipe(object sender, MR.Gestures.SwipeEventArgs e)
		{
			//DisplayAlert ("as", e.Direction.ToString(), "a");
			System.Diagnostics.Debug.WriteLine("BoxViewXaml.Red_Swiped method called, swiped " + e.Direction);
		}*/

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

		private string GetPageName( Page page )
		{
			string pageName = "";
			if (page is BrowseCategoriesPage)
				pageName = "BrowseCategories";
			else if (page is SettingsPage)
				pageName = "Settings";
			else if (page is FavoritesPage)
				pageName = "Favorites";
			else if (page is HistoryPage)
				pageName = "History";
			else if (page is TrackPage)
				pageName = "Track";

			return pageName;
		}

		public void SwitchTab( string pageName )
		{
			if (pageName == mCurrentPage)
				return;
			
			switch (pageName) {
			case "BrowseCategories":
				SwitchHeaderVisibility (true);
				mBrowseProductPage = null;
				mBrowseCategoriesPage.RefreshSearchText ();
				mFooter.ChangeColorOfLabel (mFooter.mCategoriesLabel);
				SwitchContentGrid (mBrowseCategoriesPage.Content);
				mCurrentPage = pageName;
				break;
			case "Settings":
				SwitchHeaderVisibility (true);
				mSettingsPage.SetInitialTexts ();
				mFooter.ChangeColorOfLabel (mFooter.mSettingsLabel);
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
				mFooter.ChangeColorOfLabel (mFooter.mHistoryLabel);
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
			mBrowseProductPage = (new BrowseProductsPage (productDictionary, category, this)); 
			SwitchContentGrid (mBrowseProductPage.Content);
		}

		public void LoadCartPage()
		{
			mCurrentPage = "";
			SwitchHeaderVisibility (true);
			Footer.SetLabelProperties ();
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
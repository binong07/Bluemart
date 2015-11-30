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
using PCLStorage;
using System.Linq;

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
		public SearchPage mSearchPage;
		public AddAddressPage mAddAddressPage;
		public CartPage mCartPage;
		public string mCurrentPage = "";
		public string mCurrentPageParent = "";
		public Footer mFooter;
		public Xamarin.Forms.Grid mGrid;
		public TopNavigationBar mTopNavigationBar;
		public MainMenuHeader mRootHeader;
		private List<string> mPageList;

		//public Stream mAddFavoritesImage;
		public Stream mRemoveFavoritesImage;
		public Stream mBorderImage;
		public Stream mCategoryBorderImage;
		//private int mActivePageIndex = 2;
		private static IFolder mRootFolder =  FileSystem.Current.LocalStorage;
		private static string mRootFolderPath = mRootFolder.Path;
		public IFolder mFolder;
		public View mContentGrid;
		private double mGrid1Height;
		private Xamarin.Forms.Image mCartBackgroundImage;
		public Xamarin.Forms.Label mPriceLabel;
		public Xamarin.Forms.ActivityIndicator mActivityIndicator;

		public RootPage ()
		{


			InitializeComponent ();
			mActivityIndicator = new Xamarin.Forms.ActivityIndicator();
			mCartBackgroundImage = new Xamarin.Forms.Image () {
				Source = "CartBackground",
				Aspect = Aspect.Fill
			};
					

			mCartBackgroundImage.GestureRecognizers.Add (new TapGestureRecognizer());

			RelativeLayout1.Children.Add (mCartBackgroundImage, 
				Constraint.RelativeToView (Grid1, (parent, sibling) => {
					return MyDevice.ScreenWidth - MyDevice.ScreenWidth*0.394444444f+MyDevice.ScreenWidth*0.002222f;
				}),
				Constraint.RelativeToView (Grid1, (parent, sibling) => {
					return sibling.Bounds.Bottom  - MyDevice.ScreenWidth * 0.2096296296f - MyDevice.ScreenWidth*0.065740741f+MyDevice.ScreenWidth*0.002222f;
				}),
				Constraint.Constant(MyDevice.ScreenWidth*0.394444444f),
				Constraint.Constant(MyDevice.ScreenWidth*0.065740741f)
			);

			mPriceLabel = new Xamarin.Forms.Label () {
				FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Xamarin.Forms.Label)),
				Text = "0",
				TextColor = Color.White,
				BackgroundColor = Color.Transparent,
				HorizontalTextAlignment = TextAlignment.Start,
				HorizontalOptions = LayoutOptions.Start
			};

			RelativeLayout1.Children.Add (mPriceLabel, 
				Constraint.RelativeToView (mCartBackgroundImage, (parent, sibling) => {
					return sibling.Bounds.Center.X + MyDevice.ScreenWidth*0.032f;
				}),
				Constraint.RelativeToView (mCartBackgroundImage, (parent, sibling) => {
					return sibling.Bounds.Top + MyDevice.ScreenWidth*0.01f;
				})
			);
			SwitchHeaderVisibility (true);

			ReloadStreams ();


			mFooter = Footer;
			mGrid = Grid1;
			mBrowseCategoriesPage = new BrowseCategoriesPage (this);
			//mBrowseProductPage = new BrowseProductsPage (this);
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
			Grid1.Children.Add(mContentGrid,0,2);
			Grid1.Swiped += (sender, e) => {
				int indexOfCurrentPage = mPageList.IndexOf(mCurrentPage);
				indexOfCurrentPage = ( indexOfCurrentPage + 1 ) % mPageList.Count;
				//SwitchTab( mPageList[indexOfCurrentPage] );
			};
			RelativeLayout1.Children.Add (mActivityIndicator,
				Constraint.RelativeToParent (parent => {
					return parent.Width/2-MyDevice.ViewPadding*3;		
				}),
				Constraint.RelativeToParent (parent => {
					return parent.Height/2-MyDevice.ViewPadding*2;		
				})
			);
		}	

		public void ReloadStreams()
		{
			var assembly = typeof(RootPage).GetTypeInfo().Assembly;
			mCategoryBorderImage = assembly.GetManifestResourceStream("bluemart.SavedImages.categoryBorder.png");
			mRemoveFavoritesImage = assembly.GetManifestResourceStream("bluemart.SavedImages.bookmark_remove.png");
			mBorderImage = assembly.GetManifestResourceStream("bluemart.SavedImages.border.png");
			mFolder = mRootFolder.GetFolderAsync(ParseConstants.IMAGE_FOLDER_NAME).Result;
		}

		private void SetGrid1Definitions()
		{	
			RelativeLayout1.HeightRequest = MyDevice.ScreenHeight;
			Grid1.RowSpacing = 0;
			//Grid1.RowSpacing = MyDevice.ScreenWidth * 0.0055f;
			Grid1.HeightRequest = MyDevice.ScreenHeight;// - 2*Grid1.RowSpacing;
			Grid1.RowDefinitions [0].Height = MyDevice.ScreenWidth * 0.148148148f;
			Grid1.RowDefinitions[1].Height = MyDevice.ViewPadding;
			Grid1.RowDefinitions [3].Height = MyDevice.ScreenWidth * 0.2096296296f;

		}

		private void SwitchContentGrid(View content)
		{			
			mGrid1Height = Grid1.Height;
			Grid1.Children.Remove(mContentGrid);
			mContentGrid = content;

			Grid1.Children.Add(mContentGrid,0,2);
			Grid1.HeightRequest = mGrid1Height;
			Grid1.RowDefinitions [0].Height = MyDevice.ScreenWidth * 0.148148148f;
			Grid1.RowDefinitions[1].Height = MyDevice.ViewPadding;
			Grid1.RowDefinitions [3].Height = MyDevice.ScreenWidth * 0.2096296296f;
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
				mBrowseCategoriesPage.RefreshBorderStream ();
				if (mBrowseProductPage != null) {
					mBrowseProductPage.ClearContainers ();
					mBrowseProductPage.Content = null;
					mBrowseProductPage = null;
					GC.Collect ();
				}

				mBrowseCategoriesPage.RefreshSearchText ();
				mFooter.ChangeColorOfLabel (mFooter.mCategoriesLabel);
				mFooter.ChangeImageOfButton (0);
				SwitchContentGrid (mBrowseCategoriesPage.Content);
				mCurrentPage = pageName;
				break;
			case "Settings":
				SwitchHeaderVisibility (true);
				mFooter.ChangeColorOfLabel (mFooter.mSettingsLabel);
				mFooter.ChangeImageOfButton (1);
				mSettingsPage.PopulateListView();
				SwitchContentGrid (mSettingsPage.Content);
				mCurrentPage = pageName;
				break;
			case "Favorites":
				SwitchHeaderVisibility (true);
				mFavoritesPage.RefreshFavoritesGrid ();
				mFooter.ChangeColorOfLabel (mFooter.mFavoritesLabel);
				mFooter.ChangeImageOfButton (2);
				SwitchContentGrid (mFavoritesPage.Content);
				mCurrentPage = pageName;
				break;
			case "History":
				SwitchHeaderVisibility (true);
				mHistoryPage.PopulateListView ();
				mFooter.ChangeColorOfLabel (mFooter.mCartLabel);
				mFooter.ChangeImageOfButton (3);
				SwitchContentGrid (mHistoryPage.Content);
				mCurrentPage = pageName;
				break;
			case "Track":
				SwitchHeaderVisibility (true);
				mTrackPage.PopulateListView ();
				mFooter.ChangeColorOfLabel (mFooter.mTrackLabel);
				mFooter.ChangeImageOfButton (4);
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

			//mBrowseProductPage.PopulationOfNewProductPage (productDictionary, category);

			mBrowseProductPage = (new BrowseProductsPage (productDictionary, category, this)); 
			SwitchContentGrid (mBrowseProductPage.Content);

		}

		public void LoadSearchPage(string searchString,string categoryId = "")
		{
			mCurrentPage = "";
			Footer.SetLabelProperties ();
			SwitchHeaderVisibility (true);
			mSearchPage = new SearchPage (searchString, categoryId, this);
			SwitchContentGrid (mSearchPage.Content);
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
			mFooter.ChangeImageOfButton (3);
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



		public void RemoveFooter()
		{			
			mFooter.IsVisible=false;
			mCartBackgroundImage.IsVisible = false;
			/*Grid1.RowDefinitions.RemoveAt (3);
			Grid1.Children.Remove (mFooter);*/
		}
		public void AddFooter()
		{		
			mFooter.IsVisible=true;
			mCartBackgroundImage.IsVisible = true;	
			/*
			mCartBackgroundImage.IsVisible = true;
			Grid1.RowDefinitions.Add (new RowDefinition (){ Height = MyDevice.ScreenWidth * 0.179f });
			Grid1.Children.Add (mFooter,0,3);
*/

		}
	}
}
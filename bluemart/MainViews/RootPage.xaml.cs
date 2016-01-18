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
using System.Threading.Tasks;

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
		//public TopNavigationBar mTopNavigationBar;
		//public MainMenuHeader mRootHeader;
		private List<string> mPageList;

		public Stream mAddFavoritesImage;
		public Stream mRemoveFavoritesImage;
		public Stream mBorderImage;
		public Stream mProductCellForeground;
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
			//ReloadStreams ();
			mActivityIndicator = new Xamarin.Forms.ActivityIndicator();
			NavigationPage.SetHasNavigationBar (this, false);

			mBrowseCategoriesPage = new BrowseCategoriesPage (this);
			//mFavoritesPage = new FavoritesPage (this);

			SwitchTab ("BrowseCategories");
			//SwitchContent (mBrowseCategoriesPage.Content);


			/*mActivityIndicator = new Xamarin.Forms.ActivityIndicator();
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
			);*/
		}	

		/*public void ReloadStreams()
		{
			var assembly = typeof(RootPage).GetTypeInfo().Assembly;

			//mCategoryBorderImage = assembly.GetManifestResourceStream("bluemart.SavedImages.categoryBorder.png");
			//mRemoveFavoritesImage = assembly.GetManifestResourceStream("bluemart.SavedImages.ProductsPage_RemoveFavorites.png");
			//mAddFavoritesImage = assembly.GetManifestResourceStream("bluemart.SavedImages.ProductsPage_AddFavorites.png");
			mBorderImage = assembly.GetManifestResourceStream("bluemart.SavedImages.ProductsPage_ProductCell.png");
			mProductCellForeground = assembly.GetManifestResourceStream("bluemart.SavedImages.ProductsPage_ProductForeground.png");
			mFolder = mRootFolder.CreateFolderAsync (ParseConstants.IMAGE_FOLDER_NAME, CreationCollisionOption.OpenIfExists).Result;             
			//mFolder = mRootFolder.CreateFolderAsync (ParseConstants.IMAGE_FOLDER_NAME, CreationCollisionOption.OpenIfExists).Result;			 
		}*/

		private void SetGrid1Definitions()
		{	
		/*	RelativeLayout1.HeightRequest = MyDevice.ScreenHeight;
			Grid1.RowSpacing = 0;
			//Grid1.RowSpacing = MyDevice.ScreenWidth * 0.0055f;
			Grid1.HeightRequest = MyDevice.ScreenHeight;// - 2*Grid1.RowSpacing;
			Grid1.RowDefinitions [0].Height = MyDevice.ScreenWidth * 0.148148148f;
			Grid1.RowDefinitions[1].Height = MyDevice.ViewPadding;
			Grid1.RowDefinitions [3].Height = MyDevice.ScreenWidth * 0.2096296296f;*/

		}

		private void SwitchContent(View content)
		{	
			if (this.Content == null)
				System.Diagnostics.Debug.WriteLine ("aq1");
			if (content == null)
				System.Diagnostics.Debug.WriteLine ("aq2");
			//this.Content = null;
			else this.Content = content;
			/*mGrid1Height = Grid1.Height;
			Grid1.Children.Remove(mContentGrid);
			mContentGrid = content;

			Grid1.Children.Add(mContentGrid,0,2);
			Grid1.HeightRequest = mGrid1Height;
			Grid1.RowDefinitions [0].Height = MyDevice.ScreenWidth * 0.148148148f;
			Grid1.RowDefinitions[1].Height = MyDevice.ViewPadding;
			Grid1.RowDefinitions [3].Height = MyDevice.ScreenWidth * 0.2096296296f;*/
		}	
		protected override bool OnBackButtonPressed ()
		{
			switch (mCurrentPageParent) {
			case "BrowseProducts":
				LoadProductsPage (mBrowseProductPage.mProductDictionary, mBrowseProductPage.mCategory);
				break;
			case "Settings":
				LoadSettingsPage ();
				break;
			case "BrowseCategories":
				SwitchTab ("BrowseCategories");
				break;
			case "TrackPage":
				LoadTrackPage ();
				break;
			case "MainPage":
				Navigation.PopAsync ();
				break;
			default:
				SwitchTab ("BrowseCategories");
				break;
			}
								
			return true;
		}

		public void SwitchTab( string pageName )
		{			
			switch (pageName) {
			case "BrowseCategories":				
				if (mBrowseProductPage != null) {
					mBrowseProductPage.ClearContainers ();
					mBrowseProductPage.Content = null;
					mBrowseProductPage = null;
					GC.Collect ();
				}

				mBrowseCategoriesPage.RefreshViews ();
				mBrowseCategoriesPage.UpdatePriceLabel ();
				mBrowseCategoriesPage.UpdateProductCountLabel ();

				if (mBrowseCategoriesPage.IsCartOpen)
					mBrowseCategoriesPage.ActivateOrDeactivateCart ();
				if (mBrowseCategoriesPage.IsMenuOpen)
					mBrowseCategoriesPage.ActivateOrDeactivateMenu ();
				//mFooter.ChangeColorOfLabel (mFooter.mCategoriesLabel);
				//mFooter.ChangeImageOfButton (0);
				SwitchContent (mBrowseCategoriesPage.Content);

				mCurrentPageParent = "MainPage";
				mBrowseCategoriesPage.SetScrollPos ();
				break;			
			case "History":
				mHistoryPage.PopulateListView ();
				mFooter.ChangeColorOfLabel (mFooter.mCartLabel);
				mFooter.ChangeImageOfButton (3);
				SwitchContent (mHistoryPage.Content);
				mCurrentPage = pageName;
				break;
			case "Track":
				SwitchContent (mTrackPage.Content);
				mCurrentPage = pageName;
				break;
			default:
				break;
			}
		}

		public async void LoadTrackPage ()
		{
			if (mBrowseProductPage != null) {
				mBrowseProductPage.ClearContainers ();
				mBrowseProductPage.Content = null;
				mBrowseProductPage = null;
				GC.Collect ();
			}

			mCurrentPageParent = "BrowseCategories";
			mTrackPage = new TrackPage (this);
			SwitchContent (mTrackPage.Content);
			await Task.Factory.StartNew (() => mTrackPage.PopulateTrackPage ()
				, TaskCreationOptions.LongRunning
			);

		}

		public void LoadSettingsPage ()
		{
			if (mBrowseProductPage != null) {
				mBrowseProductPage.ClearContainers ();
				mBrowseProductPage.Content = null;
				mBrowseProductPage = null;
				GC.Collect ();
			}
			mCurrentPageParent = "BrowseCategories";

			mSettingsPage = new SettingsPage (this);
			SwitchContent (mSettingsPage.Content);
		}

		public void LoadFavoritesPage()
		{
			if (mBrowseProductPage != null) {
				mBrowseProductPage.ClearContainers ();
				mBrowseProductPage.Content = null;
				mBrowseProductPage = null;
				GC.Collect ();
			}

			mCurrentPageParent = "BrowseCategories";
			mFavoritesPage = new FavoritesPage (this);
			SwitchContent (mFavoritesPage.Content);
		}
			
		public void LoadProductsPage( Dictionary<string, List<Product>> productDictionary, Category category )
		{
			mCurrentPageParent = "BrowseCategories";
			mBrowseProductPage = (new BrowseProductsPage (productDictionary, category, this)); 
			SwitchContent (mBrowseProductPage.Content);

		}

		public void LoadSearchPage(string searchString,string categoryId = "")
		{
			mCurrentPageParent = (mBrowseProductPage != null) ? "BrowseProducts" : "BrowseCategories";
			mSearchPage = new SearchPage (searchString, categoryId, this);
			SwitchContent (mSearchPage.Content);
		}

		public void LoadAddAddress(AddressClass address = null)
		{
			mCurrentPageParent = "Settings";
			mAddAddressPage = (new AddAddressPage (address,this)); 
			SwitchContent (mAddAddressPage.Content);
		}

		public void LoadCartPage()
		{
			mCurrentPage = "";

			mCurrentPageParent = "BrowseCategories";

			mFooter.ChangeColorOfLabel (mFooter.mCartLabel);
			mFooter.ChangeImageOfButton (3);
			SwitchContent (mCartPage.Content);
			mCartPage.PrintDictionaryContents ();
		}

		public void LoadReceiptPage(Object obj = null)
		{			
			

			if (obj == null) {
				SwitchContent ((new ReceiptView (this)).Content);
				mCurrentPageParent = "BrowseCategories";
			}
			else {
				SwitchContent ((new ReceiptView (this,obj)).Content);
				mCurrentPageParent = "TrackPage";
			}
				
		}
	}
}
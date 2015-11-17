using System;
using System.Collections.Generic;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;
using bluemart.Common.ViewCells;
using Xamarin.Forms;
using System.Linq;

namespace bluemart.MainViews
{
	public partial class BrowseProductsPage : ContentPage
	{
		private Dictionary<string,List<Product>> mProductDictionary;
		private int mRowCount;
		private List<BoxView> mBoxViewList;
		private List<Label> mButtonList;
		private BoxView mEnabledBoxView;
		private List<int> mCategoryIndexList;
		private double mPreviousScrollPositionY = 0;
		private int mActiveButtonIndex = 0;
		public RootPage mParent;
		public string mCategoryID;
		public Common.SearchBar mSearchBar;
		private List<Product> mProductList = new List<Product> ();
		private int mLoadSize = 10;
		private int mLastLoadedIndex = 0;
		private double mTopOfElement = 0.0f;
		private List<ProductCell> mProductCellList = new List<ProductCell> ();

		public BrowseProductsPage (RootPage parent)
		{					
			InitializeComponent ();
			mParent = parent;
			CreationInitialization ();
		}

		public void CreationInitialization()
		{
			NavigationPage.SetHasNavigationBar (this, false);
			mSearchBar = SearchBar;
			SearchBar.mParent = mParent;
			mBoxViewList = new List<BoxView> ();
			mButtonList = new List<Label> ();
			mCategoryIndexList = new List<int> ();

			SetGrid1Definitions ();
		}


		public void PopulationOfNewProductPage(Dictionary<string,List<Product>> productDictionary,Category category)
		{	
			mParent.mTopNavigationBar.NavigationText.Text = category.Name;
			mCategoryID = category.CategoryID;
			mProductDictionary = productDictionary;

			if (mProductDictionary.Count <= 1) {
				ScrollView1.IsEnabled = false;
				Grid1.RowDefinitions [1].Height = 0;
				ScrollView1.IsVisible = false;
			}


			int count = 0;
			foreach (var product in productDictionary) {
				count += product.Value.Count;
			}
			mRowCount = Convert.ToInt32(Math.Ceiling(count / 2.0f));


			PopulateGrid ();
			UpdatePriceLabel ();
		}

		public void ClearContainers()
		{			
			//Clear Product Grid
			Grid2.Children.Clear ();
			Grid2.RowDefinitions.Clear ();
			Grid2.ColumnDefinitions.Clear ();
			SubCategoryStackLayout.Children.Clear ();
			mProductDictionary.Clear ();
			mBoxViewList.Clear ();
			mButtonList.Clear ();
			mCategoryIndexList.Clear ();
		}

		public void  UpdatePriceLabel()
		{
			mParent.mRootHeader.mPriceLabel.Text = "AED:" + Cart.ProductTotalPrice.ToString();
			mParent.mTopNavigationBar.mPriceLabel.Text = "AED:" + Cart.ProductTotalPrice.ToString();
		}

		protected override void OnAppearing()
		{			
			//UpdatePriceLabel ();
			//mTopOfElement = Grid2.Children.ElementAt (mLastLoadedIndex-5).Bounds.Top;
		}

		private void SetGrid1Definitions()
		{
			Grid1.RowDefinitions [0].Height = GridLength.Auto;
			Grid1.RowDefinitions [1].Height = GridLength.Auto;//MyDevice.ScreenHeight / 25;
			Grid1.RowDefinitions [2].Height = GridLength.Auto;
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
			Grid1.BackgroundColor = MyDevice.BlueColor;
		}

		private void PopulateSubCategoryButtons()
		{
			mBoxViewList.Clear ();
			foreach (var productPair in mProductDictionary) {

				var relativeLayout = new RelativeLayout(){					
					VerticalOptions = LayoutOptions.Fill,
					BackgroundColor = Color.Blue,
					Padding = 0
				};

				Label label = new Label () {
					VerticalOptions = LayoutOptions.FillAndExpand,
					BackgroundColor = Color.White,
					Text = productPair.Key,
					TextColor = MyDevice.RedColor,
					FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label))
				};

				var tapRecognizer = new TapGestureRecognizer ();
				tapRecognizer.Tapped += (sender, e) => {
					if( mSearchBar.mSearchEntry.IsFocused )
						return;
					FocusSelectedButton(sender as Label);
				};

				label.GestureRecognizers.Add (tapRecognizer);

				mButtonList.Add (label);
				BoxView boxView = new BoxView (){
					HeightRequest = 3,
					Color = MyDevice.RedColor,
					IsVisible = false
				};
				mBoxViewList.Add (boxView);

				relativeLayout.Children.Add(label, Constraint.RelativeToParent(parent => {
					return 0;	
				}));

				relativeLayout.Children.Add (boxView, 
					Constraint.RelativeToView (label, (parent, sibling) => {
						return sibling.Bounds.Left + 5;
					}),
					Constraint.RelativeToView (label, (parent, sibling) => {
						return sibling.Bounds.Bottom - 3;
					}),
					Constraint.RelativeToView (label, (parent, sibling) => {
						return sibling.Width - 10;
					}));

				//relativeLayout.WidthRequest = my

				SubCategoryStackLayout.Children.Add (relativeLayout);
			}

			mEnabledBoxView = mBoxViewList [mActiveButtonIndex];
			mBoxViewList [mActiveButtonIndex].IsVisible = true;
		}

		private void OnScrolled( Object sender, ScrolledEventArgs e)
		{
			
			if (DecideIfIsUpOrDown (sender as ScrollView) == "Down") {
				if (mActiveButtonIndex + 1 != mCategoryIndexList.Count) {
					int productCellIndex = mCategoryIndexList [mActiveButtonIndex + 1];
					try{
					double top = Grid2.Children.ElementAt (productCellIndex).Bounds.Top;					
					if (ProductScrollView.ScrollY > top) {
						mActiveButtonIndex += 1;
						mEnabledBoxView.IsVisible = false;
						mEnabledBoxView = mBoxViewList [mActiveButtonIndex];
						mEnabledBoxView.IsVisible = true;
						}
					}
					catch
					{
						System.Diagnostics.Debug.WriteLine ("Something is wrong with Product Number in Grid");
					}
				}

				if (ProductScrollView.ScrollY >= mTopOfElement) {
					LoadLimitedNumberOfProductCells ();
					ClearBackwards ();
				}

			}else {
				if (mActiveButtonIndex != 0) {					
					int productCellIndex = mCategoryIndexList [mActiveButtonIndex];
					try{
						double top = Grid2.Children.ElementAt (productCellIndex).Bounds.Top;
						if (ProductScrollView.ScrollY < top) {
							mActiveButtonIndex -= 1;
							mEnabledBoxView.IsVisible = false;
							mEnabledBoxView = mBoxViewList [mActiveButtonIndex];
							mEnabledBoxView.IsVisible = true;
						}
					}
					catch{
						System.Diagnostics.Debug.WriteLine ("Something is wrong with Product Number in Grid");	
					}
				
			}}		
		}

		private string DecideIfIsUpOrDown(ScrollView scrollView)
		{
			string rotation = "";
			if (scrollView.ScrollY >= mPreviousScrollPositionY)
				rotation = "Down";
			else
				rotation = "Up";

			mPreviousScrollPositionY = scrollView.ScrollY;

			return rotation;
		}

		private void FocusSelectedButton(Label selectedButton)
		{
			mEnabledBoxView.IsVisible = false;
			mActiveButtonIndex = mButtonList.IndexOf (selectedButton);
			int productCellIndex = mCategoryIndexList [mActiveButtonIndex];
			try
			{
				ProductScrollView.ScrollToAsync (Grid2.Children.ElementAt (productCellIndex), ScrollToPosition.Start, true);
			}
			catch{
				System.Diagnostics.Debug.WriteLine ("Something is wrong with Product Number in Grid");
			}
				mEnabledBoxView = mBoxViewList [mActiveButtonIndex];
				mEnabledBoxView.IsVisible = true;
		}

		private void PopulateGrid()
		{
			SetGrid2Definitions ();
			PopulateSubCategoryButtons ();

			//Populate a list with all products 
			//To be able to define product index
			var valueList = mProductDictionary.Values.Cast<List<Product>> ().ToList();
			//var tempProductList = new List<Product> ();

			foreach (var products in valueList) {
				mCategoryIndexList.Add (mProductList.Count);
				foreach (var tempProduct in products) {
					mProductList.Add (tempProduct);	
				}

			}

			LoadLimitedNumberOfProductCells ();

			LoadLimitedNumberOfProductCells ();
		}

		private void ClearBackwards()
		{
			int startIndex = mLastLoadedIndex - 3*mLoadSize;
			int endIndex = mLastLoadedIndex - 2*mLoadSize;

			foreach (var productCell in mProductCellList.Skip(startIndex) ) {
				if (mProductCellList.IndexOf (productCell) == endIndex)
					break;
				
				productCell.mProductImage.Source = "";
				productCell.mAddProductStream.Dispose ();
				productCell.mRemoveProductStream.Dispose ();
				productCell.mFavoriteStream.Dispose ();
				/*productCell.mAddImage.Source = "";
				productCell.mRemoveImage.Source = "";
				productCell.mFavoriteImage.Source = "";*/
			}
		}

		private void LoadLimitedNumberOfProductCells()
		{
			
			foreach (var productPair in mProductDictionary) {
				var productList = productPair.Value;
				foreach (var product in productList.Skip(mLastLoadedIndex)) {
					
					ProductCell productCell = new ProductCell (Grid2, product, this);
					mProductCellList.Add (productCell);
					int productIndex = mProductList.IndexOf(product);
					if (productIndex == mLastLoadedIndex+mLoadSize)
						break;
					Grid2.Children.Add (productCell.View, productIndex % 2, productIndex / 2);
				}
			}

			mLastLoadedIndex += mLoadSize;
			mTopOfElement = Grid2.Children.ElementAt (mLastLoadedIndex-5).Bounds.Top;
		}

		private void SetGrid2Definitions()
		{
			SubCategoryStackLayout.Spacing = MyDevice.ViewPadding*3;
			ScrollView1.Padding = MyDevice.ViewPadding/2;
			/*for (int i = 0; i < mRowCount; i++) 
			{
				Grid2.RowDefinitions.Add (new RowDefinition ());
			}*/
			Grid2.Padding = new Thickness (MyDevice.ViewPadding / 2, 0, 0, 0);
			Grid2.ColumnDefinitions.Add (new ColumnDefinition(){Width = (MyDevice.ScreenWidth-Grid2.ColumnSpacing-MyDevice.ViewPadding)/2});
			Grid2.ColumnDefinitions.Add (new ColumnDefinition(){Width = (MyDevice.ScreenWidth-Grid2.ColumnSpacing-MyDevice.ViewPadding)/2}); 
		}


		/*private void SetGrid2DefinitionsTest(int rowCount)
		{
			SubCategoryStackLayout.Spacing = MyDevice.ViewPadding*3;
			ScrollView1.Padding = MyDevice.ViewPadding/2;
			for (int i = 0; i < rowCount; i++) 
			{
				Grid2.RowDefinitions.Add (new RowDefinition ());
			}
			Grid2.Padding = new Thickness (MyDevice.ViewPadding / 2, 0, 0, 0);
			Grid2.ColumnDefinitions.Add (new ColumnDefinition(){Width = (MyDevice.ScreenWidth-Grid2.ColumnSpacing-MyDevice.ViewPadding)/2});
			Grid2.ColumnDefinitions.Add (new ColumnDefinition(){Width = (MyDevice.ScreenWidth-Grid2.ColumnSpacing-MyDevice.ViewPadding)/2}); 
		}
		public void PopulationOfNewProductPageTest(Dictionary<string,List<Product>> productDictionary,Category category)
		{	
			mParent.mTopNavigationBar.NavigationText.Text = category.Name;
			mCategoryID = category.CategoryID;
			mProductDictionary = productDictionary;

			if (mProductDictionary.Count <= 1) {
				ScrollView1.IsEnabled = false;
				Grid1.RowDefinitions [1].Height = 0;
				ScrollView1.IsVisible = false;
			}


			int count = 0;
			foreach (var product in productDictionary) {
				count += product.Value.Count;
			}
			mRowCount = Convert.ToInt32(Math.Ceiling(count / 2.0f));


			PopulateGrid ();
			UpdatePriceLabel ();
		}*/
	}
}


using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;
using bluemart.Models.Remote;
using bluemart.Models.Local;
using bluemart.Common.ViewCells;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace bluemart.MainViews
{
	public partial class FavoritesPage : ContentPage
	{
		private FavoritesClass mFavoritesModel = new FavoritesClass();
		public RootPage mParent;
		private double mPreviousScrollPositionY = 0;
		private int mActiveButtonIndex = 0;
		private int mLastLoadedIndex = 0;
		private int mInitialLoadSize = 4;
		private int mLastScrollIndex = 0;
		private int mLoadSize = 30;
		private bool bIsImagesProduced = false;
		private Label mEnabledButtonView;
		//Containers
		private List<string> mCategoryNames = new List<string> ();
		private List<Product> mProductList = new List<Product>();
		private List<Label> mButtonList = new List<Label> ();
		private Dictionary<string,List<Product>> mProductDictionary = new Dictionary<string, List<Product>>();
		private List<int> mCategoryIndexList = new List<int>();
		//Queues
		private List<ProductCell> mProductCellList = new List<ProductCell> ();
		private Queue<ProductCell> mTrashProductCellQueue = new Queue<ProductCell>();
		private Queue<ProductCell> mPopulaterProductCellQueue = new Queue<ProductCell> ();
		private Queue<ProductCell> mManagerProductCellQueue = new Queue<ProductCell> ();

		CancellationTokenSource mLoadProductsToken = new CancellationTokenSource();
		static readonly Object _ListLock = new Object();

		//Grid Grid2;
		public FavoritesPage (RootPage parent)
		{
			InitializeComponent ();
			mParent = parent;
			//Header.mParent = parent;
			NavigationPage.SetHasNavigationBar (this, false);
			ScrollView1.BackgroundColor = MyDevice.BackgroundColor;
			SetGrid1Definitions ();
			//SetGrid2Definitions ();

		}
		public void UpdatePriceLabel()
		{
			mParent.mPriceLabel.Text = Cart.ProductTotalPrice.ToString();
		}

		protected override void OnAppearing()
		{
			UpdatePriceLabel ();
			RefreshFavoritesGrid ();
		}			

	

		private void PopulateGrid()
		{
			SetGrid1Definitions ();

			var valueList = mProductDictionary.Values.Cast<List<Product>> ().ToList();

			foreach (var products in valueList) {
				if (products.Count > 0) {
					mCategoryIndexList.Add (mProductList.Count);
					foreach (var tempProduct in products) {					
						mProductList.Add (tempProduct);	
					}
				}
			}

			LoadAllProducts();
			ManageQueuesInBackground ();
			PopulateProductCellInBackground ();
			EraseProductCellInBackground ();
			CheckIfLastIndexChanged ();
		}

		private async void CheckIfLastIndexChanged(){

			while (true) {
				if (mLastLoadedIndex != mLastScrollIndex) {
					lock (_ListLock) {
						mManagerProductCellQueue.Clear ();
					}

					mLastScrollIndex = mLastLoadedIndex;
					bIsImagesProduced = false;
				} else {
					if (!bIsImagesProduced) {
						bIsImagesProduced = true;
						for (int i = 0; i < 8; i++) {
							int next = mLastLoadedIndex + i;
							int prev = mLastLoadedIndex - i;
							if (next < mProductCellList.Count /*&& !mTrashProductCellQueue.Contains (mProductCellList [mLastLoadedIndex + i])*/) {							
								lock (_ListLock) {									
									mManagerProductCellQueue.Enqueue (mProductCellList [next]);
								}
							}
							if (prev > 0 /*&& !mTrashProductCellQueue.Contains (mProductCellList [prev])*/) {							
								lock (_ListLock) {									
									mManagerProductCellQueue.Enqueue (mProductCellList [prev]);
								}
							}
						}
					}
				}
				await Task.Delay (100);
			}
		}

		private async void ManageQueuesInBackground()
		{
			while (true) {
				while (mManagerProductCellQueue.Count > mLoadSize) {
					lock (_ListLock) {
						mManagerProductCellQueue.Dequeue ();
					}
				}

				if( mManagerProductCellQueue.Count > 0 ){
					ProductCell productCell;

					lock (_ListLock) {
						productCell = mManagerProductCellQueue.Dequeue ();
					}

					if (!mPopulaterProductCellQueue.Contains (productCell)) {
						mPopulaterProductCellQueue.Enqueue (productCell);	
					}
				}

				await Task.Delay (100);	
			}
		}

		private async void EraseProductCellInBackground()
		{
			while (true) {
				if (mTrashProductCellQueue.Count > mLoadSize) {
					var	productCell = mTrashProductCellQueue.Dequeue ();

					if (productCell.bIsImageSet) {
						productCell.bIsImageSet = false;
						productCell.ClearStreamsAndImages ();
						productCell.mProductImage.Source = null;
					}

				}
				await Task.Delay (100);	
			}
		}

		private async void PopulateProductCellInBackground()
		{
			while (true) {				

				if (mPopulaterProductCellQueue.Count > 0) {
					var productCell = mPopulaterProductCellQueue.Dequeue ();	
					if (!productCell.bIsImageSet) {	
						productCell.bIsImageSet = true;
						mTrashProductCellQueue.Enqueue (productCell);
						productCell.ProduceProductImages ();
					}
				}

				await Task.Delay (100);				

			}
		}

		private async void LoadLimitedNumberOfProducts(int count)
		{	
			foreach (var product in mProductList) {
				int productIndex = mProductList.IndexOf (product);
				if (productIndex == count)
					break;

				ProductCell productCell = new ProductCell (Grid2, product, this);

				mProductCellList.Add (productCell);					

				Grid2.Children.Add (productCell.View, productIndex % 2, productIndex / 2);			
				productCell.ProduceStreamsAndImages ();

				await Task.Delay (100);
			}
		}

		private async void LoadAllProducts()
		{	
			foreach (var product in mProductList) {
				int productIndex = mProductList.IndexOf (product);
				ProductCell productCell = new ProductCell (Grid2, product, this);

				mProductCellList.Add (productCell);					

				productCell.ProduceStreamsAndImages ();	
				Grid2.Children.Add (productCell.View, productIndex % 2, productIndex / 2);			

				if( productIndex < mInitialLoadSize )
					mManagerProductCellQueue.Enqueue (productCell);

				try{
					await Task.Delay (50,mLoadProductsToken.Token);
				}
				catch {
					mProductList.Clear ();
					break;
				}
			}
		}

		private void SetGrid1Definitions()
		{
			ParentCategoryStackLayout.Spacing = MyDevice.ViewPadding;
			ScrollView1.Padding = MyDevice.ViewPadding/2;
			Grid1.RowDefinitions [0].Height = HeightRequest = MyDevice.ScreenWidth * 0.15f;
			Grid1.RowDefinitions [1].Height = GridLength.Auto;
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
			Grid1.BackgroundColor = MyDevice.BackgroundColor;
		}

		private void PopulateProducts( )
		{
			foreach (string productID in mFavoritesModel.GetProductIDs()) {
				string ImagePath = ProductModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + ProductModel.mProductImageNameDictionary[productID] + ".jpg";
				string ProductName = ProductModel.mProductNameDictionary [productID];
				decimal price = ProductModel.mProductPriceDictionary [productID];
				string quantity = ProductModel.mProductQuantityDictionary [productID];
				string parentCategoryID = ProductModel.mProductParentCategoryIDsDictionary [productID];

			
				if (mProductDictionary.ContainsKey (parentCategoryID)) {
					mProductDictionary [parentCategoryID].Add (new Product (productID, ProductName, ImagePath, price, parentCategoryID, quantity));
				} else {
					mProductDictionary.Add (parentCategoryID, new List<Product> ());
					mProductDictionary [parentCategoryID].Add (new Product (productID, ProductName, ImagePath, price, parentCategoryID, quantity));
				}
				
			}	
		}

		public void RefreshFavoritesGrid()
		{			
			Grid2.Children.Clear ();
			Grid2.ColumnDefinitions.Clear ();
			Grid2.RowDefinitions.Clear ();
			ParentCategoryStackLayout.Children.Clear ();
			mProductDictionary.Clear ();
			mProductList.Clear ();
			mCategoryNames.Clear ();
			mButtonList.Clear ();
			mCategoryIndexList.Clear ();

			mProductCellList.Clear ();

			mProductCellList.Clear();
			mTrashProductCellQueue.Clear();
			mPopulaterProductCellQueue.Clear();
			mManagerProductCellQueue.Clear();

			PopulateProducts();			
			PopulateGrid ();
			PopulateParentCategoryButtons ();
			ProductScrollView.Content = Grid2;
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

		private void PopulateParentCategoryButtons()
		{
			mButtonList.Clear ();
			foreach (var productPair in mProductDictionary) {
				if (productPair.Value.Count > 0) {
					var relativeLayout = new RelativeLayout () {					
						VerticalOptions = LayoutOptions.Fill,
						BackgroundColor = MyDevice.BlueColor,
						Padding = 0
					};

					Label label = new Label () {
						VerticalOptions = LayoutOptions.End,
						BackgroundColor = Color.White,
						Text = " " + CategoryModel.mCategoryNameDictionary[productPair.Key] + " ",
						TextColor = MyDevice.BlueColor,
						FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
						HeightRequest = MyDevice.ScreenWidth * 0.11f,
						HorizontalTextAlignment = TextAlignment.Center,
						VerticalTextAlignment = TextAlignment.Center
					};

					var tapRecognizer = new TapGestureRecognizer ();
					tapRecognizer.Tapped += (sender, e) => {
						if (mParent.mTopNavigationBar.mSearchEntry.IsFocused)
							return;
						if (mParent.mActivityIndicator.IsRunning)
							return;
						FocusSelectedButton (sender as Label);
					};

					label.GestureRecognizers.Add (tapRecognizer);



					relativeLayout.Children.Add (label, Constraint.RelativeToParent (parent => {
						return 0;	
					}));

					var frame = new Frame { 				
						Padding = MyDevice.ScreenWidth * 0.0055f,
						OutlineColor = MyDevice.BlueColor,
						BackgroundColor = MyDevice.BlueColor,
						VerticalOptions = LayoutOptions.Start,
						Content = relativeLayout
					};


					mButtonList.Add (label);

					ParentCategoryStackLayout.Children.Add (frame);
				}
			}

			if (mButtonList.Count > 0) {
				mEnabledButtonView = mButtonList [mActiveButtonIndex];
				mEnabledButtonView.BackgroundColor = MyDevice.BlueColor;
				mEnabledButtonView.TextColor = Color.White;
			}
		}

		private void  OnScrolled( Object sender, ScrolledEventArgs e)
		{
			if (DecideIfIsUpOrDown (sender as ScrollView) == "Down") {
				if (mActiveButtonIndex + 1 != mCategoryIndexList.Count) {
					int productCellIndex = mCategoryIndexList [mActiveButtonIndex + 1];
					try {
						double top = Grid2.Children.ElementAt (productCellIndex).Bounds.Top;					
						if (ProductScrollView.ScrollY > top) {
							mActiveButtonIndex += 1;
							ChangeSelectedButton();
						}
					} catch {
						System.Diagnostics.Debug.WriteLine ("Something is wrong with Product Number in Grid");
					}
				}


				if ( ProductScrollView.ScrollY >= Grid2.Children.ElementAt (mLastLoadedIndex).Bounds.Bottom-50 ) {
					int endIndex = (int)Math.Ceiling (ProductScrollView.ScrollY / (int)Math.Floor(Grid2.Children.ElementAt(0).Height-MyDevice.ViewPadding/2)) * 2 - 1;

					if (endIndex >= Grid2.Children.Count) {
						endIndex = Grid2.Children.Count - 1;
					} 

					mLastLoadedIndex = endIndex;

				}
			}else {				

				if (mActiveButtonIndex != 0) {					
					int productCellIndex = mCategoryIndexList [mActiveButtonIndex];
					try{
						double top = Grid2.Children.ElementAt (productCellIndex).Bounds.Top;
						if (ProductScrollView.ScrollY < top) {
							mActiveButtonIndex -= 1;
							ChangeSelectedButton();
						}
					}
					catch{
						System.Diagnostics.Debug.WriteLine ("Something is wrong with Product Number in Grid");	
					}

				}

				if (mLastLoadedIndex >= Grid2.Children.Count)
					mLastLoadedIndex = Grid2.Children.Count - 1;

				if (ProductScrollView.ScrollY <= Grid2.Children.ElementAt (mLastLoadedIndex).Bounds.Top) {
					int endIndex = (int)Math.Floor (ProductScrollView.ScrollY / (int)Math.Floor(Grid2.Children.ElementAt(0).Height-MyDevice.ViewPadding/2)) * 2 - 1;

					if (endIndex < 0) {
						endIndex = 0;
					}
					else if( endIndex >= Grid2.Children.Count )
						endIndex =	Grid2.Children.Count - 1;

					mLastLoadedIndex = endIndex;
				}
			}	


		}

		private async void FocusSelectedButton(Label selectedButton)
		{			
			mActiveButtonIndex = mButtonList.IndexOf (selectedButton);			
			int productCellIndex = mCategoryIndexList [mActiveButtonIndex];
			ChangeSelectedButton ();

			lock (_ListLock) {
				mManagerProductCellQueue.Clear ();
			}

			try
			{
				if( productCellIndex < Grid2.Children.Count )
					await ProductScrollView.ScrollToAsync (Grid2.Children.ElementAt (productCellIndex), ScrollToPosition.Start, true);
				else
				{
					await WaitUntilCorrespondingSubCategoryLoaded(productCellIndex);
					await ProductScrollView.ScrollToAsync (Grid2.Children.ElementAt (productCellIndex), ScrollToPosition.Start, true);
				}
			}
			catch{
				System.Diagnostics.Debug.WriteLine ("Something is wrong with Product Number in Grid");
			}				
		}

		private async Task WaitUntilCorrespondingSubCategoryLoaded(int productCellIndex)
		{
			mParent.mActivityIndicator.IsRunning = true;
			ProductScrollView.IsVisible = false;
			while (Grid2.Children.Count-2 < productCellIndex) {
				await Task.Delay (100);
			}
			mParent.mActivityIndicator.IsRunning = false;
			ProductScrollView.IsVisible = true;
		}

		private void ChangeSelectedButton()
		{
			mEnabledButtonView.BackgroundColor = Color.White;
			mEnabledButtonView.TextColor = MyDevice.BlueColor;

			mEnabledButtonView = mButtonList [mActiveButtonIndex];

			mEnabledButtonView.BackgroundColor = MyDevice.BlueColor;
			mEnabledButtonView.TextColor = Color.White;
		}
	}
}
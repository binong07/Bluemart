using System;
using System.Collections.Generic;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;
using bluemart.Common.ViewCells;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;

namespace bluemart.MainViews
{
	public partial class BrowseProductsPage : ContentPage
	{
		private Dictionary<string,List<Product>> mProductDictionary;
		//private List<BoxView> mBoxViewList;
		private List<Label> mButtonList;
		private BoxView mEnabledBoxView;
		private List<int> mCategoryIndexList;
		private double mPreviousScrollPositionY = 0;
		private int mActiveButtonIndex = 0;
		public RootPage mParent;
		public string mCategoryID;
		private List<Product> mProductList = new List<Product> ();
		private int mLoadSize = 30;
		private int mLastLoadedIndex = 0;
		private int mLastScrollIndex = 0;
		private bool bIsImagesProduced = false;
		private int mInitialLoadSize = 8;

		private List<ProductCell> mProductCellList = new List<ProductCell> ();
		private Queue<ProductCell> mTrashProductCellQueue = new Queue<ProductCell>();
		private Queue<ProductCell> mPopulaterProductCellQueue = new Queue<ProductCell> ();
		private Queue<ProductCell> mManagerProductCellQueue = new Queue<ProductCell> ();


		static readonly Object _ListLock = new Object();

		public BrowseProductsPage (Dictionary<string, List<Product>> productDictionary, Category category,RootPage parent)
		{					
			InitializeComponent ();
			mParent = parent;
			CreationInitialization ();
			PopulationOfNewProductPage (productDictionary, category);
			//WaitBeforeInit ();
		}

		public void CreationInitialization()
		{
			NavigationPage.SetHasNavigationBar (this, false);
			//mBoxViewList = new List<BoxView> ();
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
				Grid1.RowDefinitions [0].Height = 0;
				ScrollView1.IsVisible = false;
			}


			int count = 0;
			foreach (var product in productDictionary) {
				count += product.Value.Count;
			}


			PopulateGrid ();
			UpdatePriceLabel ();


		}

		public void ClearContainers()
		{			
			
			SubCategoryStackLayout.Children.Clear ();
			mProductDictionary.Clear ();
			//mBoxViewList.Clear ();
			mButtonList.Clear ();
			mCategoryIndexList.Clear ();
			foreach (var productCell in mProductCellList) {
				productCell.ClearStreamsAndImages ();
			}	


			mProductCellList.Clear ();
			mProductList.Clear ();
			mProductCellList.Clear();
			mTrashProductCellQueue.Clear();
			mPopulaterProductCellQueue.Clear();
			mManagerProductCellQueue.Clear();
		}

		public void  UpdatePriceLabel()
		{
			mParent.mPriceLabel.Text = Cart.ProductTotalPrice.ToString();
		}

		protected override void OnAppearing()
		{			
			UpdatePriceLabel ();
		}

		private void SetGrid1Definitions()
		{
			Grid1.RowDefinitions [0].Height = GridLength.Auto;
			Grid1.RowDefinitions [1].Height = GridLength.Auto;
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
			Grid1.BackgroundColor = MyDevice.BackgroundColor;
		}

		private async void WaitBeforeInit()
		{
			await Task.Delay (200);
			//LoadLimitedNumberOfProducts (50);
			/*LoadAllProducts ();
			LoadInitialImages ();*/
		}

		private void PopulateSubCategoryButtons()
		{
			//mBoxViewList.Clear ();
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
					TextColor = MyDevice.BlueColor,
					FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label))
				};

				var tapRecognizer = new TapGestureRecognizer ();
				tapRecognizer.Tapped += (sender, e) => {
					if( mParent.mTopNavigationBar.mSearchEntry.IsFocused )
						return;
					FocusSelectedButton(sender as Label);
				};

				label.GestureRecognizers.Add (tapRecognizer);

				mButtonList.Add (label);
				/*BoxView boxView = new BoxView (){
					HeightRequest = 3,
					Color = MyDevice.RedColor,
					IsVisible = false
				};
				mBoxViewList.Add (boxView);*/

				relativeLayout.Children.Add(label, Constraint.RelativeToParent(parent => {
					return 0;	
				}));

				/*relativeLayout.Children.Add (boxView, 
					Constraint.RelativeToView (label, (parent, sibling) => {
						return sibling.Bounds.Left + 5;
					}),
					Constraint.RelativeToView (label, (parent, sibling) => {
						return sibling.Bounds.Bottom - 3;
					}),
					Constraint.RelativeToView (label, (parent, sibling) => {
						return sibling.Width - 10;
					}));*/

				//relativeLayout.WidthRequest = my

				var frame = new Frame { 				
					Padding = 2,
					OutlineColor = Color.Blue,
					BackgroundColor = Color.Blue,
					VerticalOptions = LayoutOptions.Start,
					Content = relativeLayout
				};

				SubCategoryStackLayout.Children.Add (frame);
			}

			//mEnabledBoxView = mBoxViewList [mActiveButtonIndex];
			//mBoxViewList [mActiveButtonIndex].IsVisible = true;
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
							mEnabledBoxView.IsVisible = false;
							//mEnabledBoxView = mBoxViewList [mActiveButtonIndex];
							mEnabledBoxView.IsVisible = true;
						}
					} catch {
						System.Diagnostics.Debug.WriteLine ("Something is wrong with Product Number in Grid");
					}
				}
					

				if ( ProductScrollView.ScrollY >= Grid2.Children.ElementAt (mLastLoadedIndex).Bounds.Bottom-50 ) {
					int endIndex = (int)Math.Ceiling (ProductScrollView.ScrollY / (int)Math.Floor(Grid2.Children.ElementAt(0).Height-MyDevice.ViewPadding/2)) * 2 +1 ;

					if (endIndex >= Grid2.Children.Count) {
						endIndex = Grid2.Children.Count - 1;
					} 

					mLastLoadedIndex = endIndex;

					/*for (int i = startIndex; i <= endIndex; i++) {						
						if (!mTrashProductCellQueue.Contains (mProductCellList [i])) {							
							lock (_ListLock) {									
								mManagerProductCellQueue.Enqueue (mProductCellList [i]);
							}
						}
					}*/

			}
			}else {				

				if (mActiveButtonIndex != 0) {					
					int productCellIndex = mCategoryIndexList [mActiveButtonIndex];
					try{
						double top = Grid2.Children.ElementAt (productCellIndex).Bounds.Top;
						if (ProductScrollView.ScrollY < top) {
							mActiveButtonIndex -= 1;
							mEnabledBoxView.IsVisible = false;
							//mEnabledBoxView = mBoxViewList [mActiveButtonIndex];
							mEnabledBoxView.IsVisible = true;
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

					/*for (int i = startIndex; i >= endIndex; i--) {
						
						if (!mTrashProductCellQueue.Contains (mProductCellList [i])) {							
							lock (_ListLock) {									
								mManagerProductCellQueue.Enqueue (mProductCellList [i]);
							}
						}
					}*/
				}
			}	


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
				//mEnabledBoxView = mBoxViewList [mActiveButtonIndex];
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

			//LoadLimitedNumberOfProducts (1000);
			LoadAllProducts();
			ManageQueuesInBackground ();
			PopulateProductCellInBackground ();
			EraseProductCellInBackground ();
			CheckIfLastIndexChanged ();

			//LoadInitialImages ();
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
							if (next < mProductCellList.Count && !mTrashProductCellQueue.Contains (mProductCellList [mLastLoadedIndex + i])) {							
								lock (_ListLock) {									
									mManagerProductCellQueue.Enqueue (mProductCellList [next]);
								}
							}
							if (prev > 0 && !mTrashProductCellQueue.Contains (mProductCellList [prev])) {							
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


				Grid2.Children.Add (productCell.View, productIndex % 2, productIndex / 2);			
				productCell.ProduceStreamsAndImages ();	

				await Task.Delay (100);
			}
		}

		private void LoadInitialImages()
		{
			foreach (var productCell in mProductCellList) {
				int productIndex = mProductCellList.IndexOf (productCell);
				if (productIndex < mInitialLoadSize) {
					//lock (_ListLock) {						
						mManagerProductCellQueue.Enqueue (productCell);
					//}
				}
				productCell.ProduceStreamsAndImages ();
			}

			ManageQueuesInBackground ();
			PopulateProductCellInBackground ();
			EraseProductCellInBackground ();
			CheckIfLastIndexChanged ();
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
	}
}


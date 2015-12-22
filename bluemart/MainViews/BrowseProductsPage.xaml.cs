using System;
using System.Collections.Generic;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;
using bluemart.Common.ViewCells;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using XLabs.Forms.Controls;

namespace bluemart.MainViews
{
	public partial class BrowseProductsPage : ContentPage
	{
		
		//private List<BoxView> mBoxViewList;



		private double mPreviousScrollPositionY = 0;
		private int mActiveButtonIndex = 0;
		public RootPage mParent;
		public string mCategoryID;

		private int mLoadSize = 30;
		private int mLastLoadedIndex = 0;
		private int mLastScrollIndex = 0;
		private bool bIsImagesProduced = false;
		private int mInitialLoadSize = 4;
		//Containers
		private Dictionary<string,List<Product>> mProductDictionary;
		private List<Label> mButtonList;
		private List<Product> mProductList = new List<Product> ();
		private List<int> mCategoryIndexList;
		//Queues
		private List<ProductCell> mProductCellList = new List<ProductCell> ();
		private Queue<ProductCell> mTrashProductCellQueue = new Queue<ProductCell>();
		private Queue<ProductCell> mPopulaterProductCellQueue = new Queue<ProductCell> ();
		private Queue<ProductCell> mManagerProductCellQueue = new Queue<ProductCell> ();
		CancellationTokenSource mLoadProductsToken = new CancellationTokenSource();

		static readonly Object _ListLock = new Object();

		List<Product> mTopSellingProductList = new List<Product>();
		List<ProductCell> mTopSellingProductCellList = new List<ProductCell> ();

		Grid Grid1;

		ScrollView ProductScrollView;
		Grid ProductGrid;

		private RelativeLayout SubcategoryLayout;
		private ScrollView SubcategoryScrollView;
		private StackLayout SubCategoryStackLayout;
		private List<BoxView> mBoxViewList;
		private BoxView mEnabledBoxView;

		private RelativeLayout InputBlocker;
		private Category mCategory;
		private RelativeLayout mTopLayout;
		private RelativeLayout mSearchLayout;
		private ExtendedEntry SearchEntry;
		private Label SearchLabel;

		public BrowseProductsPage (Dictionary<string, List<Product>> productDictionary, Category category,RootPage parent)
		{					
			InitializeComponent ();
			mParent = parent;
			mCategory = category;
			PopulationOfNewProductPage (productDictionary, category);
			CreationInitialization ();

			//WaitBeforeInit ();
		}

		public void CreationInitialization()
		{
			NavigationPage.SetHasNavigationBar (this, false);
			mBoxViewList = new List<BoxView> ();
			mButtonList = new List<Label> ();
			mCategoryIndexList = new List<int> ();
			InitializeLayout ();
			//SetGrid1Definitions ();
		}

		private void InitializeLayout()
		{	
			mainRelativeLayout.BackgroundColor = Color.FromRgb (236, 240, 241);

			InputBlocker = new RelativeLayout () {
				WidthRequest = MyDevice.ScreenWidth,
				HeightRequest = MyDevice.ScreenHeight
			};

			var inputBlockerTapRecogniser = new TapGestureRecognizer ();
			inputBlockerTapRecogniser.Tapped += (sender, e) => {				
				SearchEntry.Unfocus();
			};
			InputBlocker.GestureRecognizers.Add(inputBlockerTapRecogniser);


			InitializeHeaderLayout ();
			InitializeSearchLayout ();
			InitializeSubCategoriesLayout ();
			InitializeBottomLayout ();
			EventHandlers ();
		}

		private void InitializeHeaderLayout ()
		{
			mTopLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.GetScaledSize(87),
				BackgroundColor = Color.FromRgb(27,184,105)
			};

			var menuIcon = new Image () {
				WidthRequest = MyDevice.GetScaledSize(36),
				HeightRequest = MyDevice.GetScaledSize(37),
				Source = "CategoriesPage_MenuIcon"
			};

			var categoryLabel = new Label (){ 
				Text = mCategory.Name,
				TextColor = Color.White,
				FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label))
			};

			var priceLabel = new Label () {
				Text = "0\nAED",	
				TextColor = Color.White,
				FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
				HorizontalTextAlignment = TextAlignment.Center
			};

			var verticalLine = new Image () {
				WidthRequest = MyDevice.GetScaledSize(1),
				HeightRequest = MyDevice.GetScaledSize(63),
				Aspect = Aspect.Fill,
				Source = "CategoriesPage_VerticalLine"
			};

			var cartImage = new Image () {
				WidthRequest = MyDevice.GetScaledSize(71),
				HeightRequest = MyDevice.GetScaledSize(57),
				Aspect = Aspect.Fill,
				Source = "ProductsPage_BasketIcon"
			};

			var productCount = new Label () {
				Text = "15",	
				TextColor = Color.White,
				FontSize = Device.GetNamedSize(NamedSize.Micro,typeof(Label)),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				WidthRequest = MyDevice.GetScaledSize(37),
				HeightRequest = MyDevice.GetScaledSize(27)
			};

			mainRelativeLayout.Children.Add (mTopLayout,
				Constraint.Constant (0),
				Constraint.Constant (0)
			);

			mainRelativeLayout.Children.Add (menuIcon, 
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Left +  MyDevice.GetScaledSize(20);
				}),
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Top + MyDevice.GetScaledSize(27);
				})
			);

			mainRelativeLayout.Children.Add (categoryLabel,
				Constraint.RelativeToView (menuIcon, (parent, sibling) => {
					return sibling.Bounds.Right + MyDevice.GetScaledSize (22);
				}),
				Constraint.RelativeToView (menuIcon, (parent, sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize (3);
				})
			);

			mainRelativeLayout.Children.Add (cartImage, 
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Right -  MyDevice.GetScaledSize(79);
				}),
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Top + MyDevice.GetScaledSize(16);
				})
			);

			mainRelativeLayout.Children.Add (verticalLine,
				Constraint.RelativeToView (cartImage, (parent, sibling) => {
					return sibling.Bounds.Left - MyDevice.GetScaledSize (14);
				}),
				Constraint.RelativeToView (cartImage, (parent, sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize (5);
				})
			);

			mainRelativeLayout.Children.Add (priceLabel,
				Constraint.RelativeToView (verticalLine, (parent, sibling) => {
					return sibling.Bounds.Left - MyDevice.GetScaledSize (75);
				}),
				Constraint.RelativeToView (cartImage, (parent, sibling) => {
					return sibling.Bounds.Top;
				})
			);

			mainRelativeLayout.Children.Add (productCount,
				Constraint.RelativeToView (cartImage, (parent, sibling) => {
					return sibling.Bounds.Right - MyDevice.GetScaledSize (37);
				}),
				Constraint.RelativeToView (cartImage, (parent, sibling) => {
					return sibling.Bounds.Bottom - MyDevice.GetScaledSize (27);
				})
			);	
		}

		private void InitializeSearchLayout()
		{
			mSearchLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.GetScaledSize(73),
				BackgroundColor = Color.FromRgb(27,184,105)
			};

			SearchEntry = new ExtendedEntry () {
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.GetScaledSize(73),
				Text = "Search",
				MaxLength = 15
			};

			var searchImage = new Image () {
				WidthRequest = MyDevice.GetScaledSize(583),
				HeightRequest = MyDevice.GetScaledSize(52),
				Source = "ProductsPage_SearchBar"	
			};

			var searchButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(444),
				HeightRequest = MyDevice.GetScaledSize(51)
			};

			SearchLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize(444),
				HeightRequest = MyDevice.GetScaledSize(51),
				TextColor = Color.White,
				FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label)),
				Text = "Search",
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center
			};

			var deleteButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(69),
				HeightRequest = MyDevice.GetScaledSize(51)
			};

			var backButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(65),
				HeightRequest = MyDevice.GetScaledSize(72)
			};					


			var searchEntryTapRecognizer= new TapGestureRecognizer ();
			searchEntryTapRecognizer.Tapped += (sender, e) => {				
				SearchEntry.Focus();
			};
			searchButton.GestureRecognizers.Add(searchEntryTapRecognizer);

			var deleteButtonTapRecognizer= new TapGestureRecognizer ();
			deleteButtonTapRecognizer.Tapped += (sender, e) => {				
				if( SearchEntry.Text.Length > 0 )
					SearchEntry.Text = SearchEntry.Text.Remove(SearchEntry.Text.Length - 1);
			};
			deleteButton.GestureRecognizers.Add(deleteButtonTapRecognizer);

			var backButtonTapRecognizer= new TapGestureRecognizer ();
			backButtonTapRecognizer.Tapped += (sender, e) => {				
				
			};
			backButton.GestureRecognizers.Add(backButtonTapRecognizer);

			mainRelativeLayout.Children.Add (SearchEntry,
				Constraint.Constant(0),
				Constraint.RelativeToView (mTopLayout, (parent, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (1);
				})
			);

			mainRelativeLayout.Children.Add (mSearchLayout,
				Constraint.Constant(0),
				Constraint.RelativeToView (mTopLayout, (parent, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (1);
				})
			);

			mainRelativeLayout.Children.Add (searchImage,
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize (28);
				}),
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize (10);
				})
			);

			mainRelativeLayout.Children.Add (searchButton,
				Constraint.RelativeToView (searchImage, (parent, sibling) => {
					return sibling.Bounds.Left;
				}),
				Constraint.RelativeToView (searchImage, (parent, sibling) => {
					return sibling.Bounds.Top;
				})
			);

			mainRelativeLayout.Children.Add (SearchLabel,
				Constraint.RelativeToView (searchButton, (parent, sibling) => {
					return sibling.Bounds.Left+ MyDevice.GetScaledSize(118);
				}),
				Constraint.RelativeToView (searchButton, (parent, sibling) => {
					return sibling.Bounds.Top;
				})
			);

			mainRelativeLayout.Children.Add (deleteButton,
				Constraint.RelativeToView (searchImage, (parent, sibling) => {
					return sibling.Bounds.Right - MyDevice.GetScaledSize (67);
				}),
				Constraint.RelativeToView (searchImage, (parent, sibling) => {
					return sibling.Bounds.Top;
				})
			);

			mainRelativeLayout.Children.Add (backButton,
				Constraint.Constant (0),
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Top;
				})
			);
		}

		private void InitializeSubCategoriesLayout()
		{
			SubcategoryLayout = new RelativeLayout (){ 
				HeightRequest = MyDevice.GetScaledSize(66),
				BackgroundColor = Color.Red/*,
				BackgroundColor = Color.FromRgb(27,184,105)*/
			};

			SubCategoryStackLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Padding = new Thickness(MyDevice.GetScaledSize(15),0,0,0),
				Spacing = 0
			};

			PopulateSubCategoryButtons ();

			SubcategoryScrollView = new ScrollView {
				Orientation = ScrollOrientation.Horizontal,
				Content = SubCategoryStackLayout
			};

			mainRelativeLayout.Children.Add (SubcategoryScrollView,
				Constraint.Constant(0),
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Bottom;
				}),
				Constraint.Constant(MyDevice.ScreenWidth)
			);
		}

		private void PopulateSubCategoryButtons()
		{
			mButtonList.Clear ();
			mBoxViewList.Clear ();
			mTopSellingProductList.Clear ();
			mTopSellingProductCellList.Clear ();

			foreach (var productPair in mProductDictionary) {
				if (productPair.Value.Count > 0) {

					if (productPair.Key == "Top Selling") {
						mTopSellingProductList = productPair.Value;
					}

					var buttonLayout = new RelativeLayout () {
						BackgroundColor = Color.Transparent,
					};

					Label label = new Label () {
						VerticalOptions = LayoutOptions.Center,
						BackgroundColor = Color.Transparent,
						Text = " "+productPair.Key+" ",
						TextColor = Color.FromRgb(136,147,161),
						FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
						HeightRequest = MyDevice.GetScaledSize(60),
						HorizontalTextAlignment = TextAlignment.Center,
						VerticalTextAlignment = TextAlignment.Center
					};

					BoxView boxView = new BoxView (){
						HeightRequest = 1,
						Color = MyDevice.RedColor,
						IsVisible = false
					};

					mBoxViewList.Add (boxView);
					mButtonList.Add (label);

					buttonLayout.Children.Add (label,
						Constraint.Constant (0),
						Constraint.Constant (0)
					);

					buttonLayout.Children.Add (boxView,
						Constraint.RelativeToView( label, (parent,sibling) =>{
							return sibling.Bounds.Left + MyDevice.GetScaledSize(0);	
						}),
						Constraint.RelativeToView( label, (parent,sibling) =>{
							return sibling.Bounds.Bottom - MyDevice.GetScaledSize(14);	
						}),
						Constraint.RelativeToView( label, (parent,sibling) =>{
							return sibling.Bounds.Width - MyDevice.GetScaledSize(5);	
						})
					);

					var tapRecognizer = new TapGestureRecognizer ();
					tapRecognizer.Tapped += (sender, e) => {

						if (mParent.mActivityIndicator.IsRunning)
							return;
						FocusSelectedButton (sender as Label);
					};

					label.GestureRecognizers.Add (tapRecognizer);

					SubCategoryStackLayout.Children.Add (buttonLayout);
				}
			}

			if (mButtonList.Count > 0) {
				mEnabledBoxView = mBoxViewList [0];
				mEnabledBoxView.IsVisible = true;
			}
		}


		private void InitializeBottomLayout()
		{
			ProductGrid = new Grid () {
				Padding = 0
			};

			PopulateGrid ();

			ProductScrollView = new ScrollView () {
				Orientation = ScrollOrientation.Vertical,
				Content = ProductGrid
			};



			mainRelativeLayout.Children.Add (ProductScrollView,
				Constraint.Constant(0),
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(64);
				}),
				Constraint.Constant(MyDevice.GetScaledSize(630)),
				Constraint.Constant(MyDevice.ScreenHeight-MyDevice.GetScaledSize(87)-MyDevice.GetScaledSize(73)-MyDevice.GetScaledSize(1)-MyDevice.GetScaledSize(51))
			);
		}

		private void EventHandlers()
		{
			SearchEntry.PropertyChanged += (sender, e) => {
				SearchLabel.Text = SearchEntry.Text;
			};

			SearchEntry.Focused += (sender, e) => {
				SearchEntry.Text = "";
				mainRelativeLayout.Children.Add( InputBlocker,
					Constraint.Constant(0),
					Constraint.Constant(0)
				);
			};

			SearchEntry.Unfocused += (sender, e) => {
				if( SearchEntry.Text == "" )
					SearchEntry.Text = "Search";
				mainRelativeLayout.Children.Remove(InputBlocker);
			};

			SearchEntry.Completed += (sender, e) => {
				if (SearchEntry.Text.Length >= 3) {				
					mParent.LoadSearchPage (SearchEntry.Text);
				} else {				
					SearchEntry.Text = "Must be longer than 2 characters!";
				}
				mainRelativeLayout.Children.Remove(InputBlocker);
			};

			ProductScrollView.Scrolled += OnScrolled;
		}


		public void PopulationOfNewProductPage(Dictionary<string,List<Product>> productDictionary,Category category)
		{	
			//mParent.mTopNavigationBar.NavigationText.Text = category.Name;
			mCategoryID = category.CategoryID;
			mProductDictionary = productDictionary;

			if (mProductDictionary.Count <= 1) {
				SubcategoryScrollView.IsEnabled = false;
				Grid1.RowDefinitions [0].Height = 0;
				SubcategoryScrollView.IsVisible = false;
			}

			//PopulateGrid ();
			UpdatePriceLabel ();
		}

		public void ClearContainers()
		{			
			mLoadProductsToken.Cancel ();
			mParent.mActivityIndicator.IsRunning = false;

			SubCategoryStackLayout.Children.Clear ();
			mProductDictionary.Clear ();
			mButtonList.Clear ();
			mButtonList.Clear ();
			mCategoryIndexList.Clear ();
			foreach (var productCell in mProductCellList) {
				productCell.ClearStreamsAndImages ();
			}	


			mProductCellList.Clear ();

			mProductCellList.Clear();
			mTrashProductCellQueue.Clear();
			mPopulaterProductCellQueue.Clear();
			mManagerProductCellQueue.Clear();
		}

		public void  UpdatePriceLabel()
		{
			//mParent.mPriceLabel.Text = Cart.ProductTotalPrice.ToString();
		}

		protected override void OnAppearing()
		{			
			UpdatePriceLabel ();
		}

		private void SetGrid1Definitions()
		{
			/*Grid1.RowDefinitions [0].Height = HeightRequest = MyDevice.ScreenWidth * 0.15f;
			Grid1.RowDefinitions [1].Height = GridLength.Auto;
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
			Grid1.BackgroundColor = MyDevice.BackgroundColor;*/
		}

		private async Task WaitUntilCorrespondingSubCategoryLoaded(int productCellIndex)
		{
			mParent.mActivityIndicator.IsRunning = true;
			ProductScrollView.IsVisible = false;
			while (ProductGrid.Children.Count-2 < productCellIndex) {
				await Task.Delay (100);
			}
			mParent.mActivityIndicator.IsRunning = false;
			ProductScrollView.IsVisible = true;
		}

		private void PopulateTopSelling()
		{
			
		}			

		private void  OnScrolled( Object sender, ScrolledEventArgs e)
		{
			if (DecideIfIsUpOrDown (sender as ScrollView) == "Down") {
				if (mActiveButtonIndex + 1 != mCategoryIndexList.Count) {
					int productCellIndex = mCategoryIndexList [mActiveButtonIndex + 1];
					try {
						double top = ProductGrid.Children.ElementAt (productCellIndex).Bounds.Top;					
						if (ProductScrollView.ScrollY > top) {
							mActiveButtonIndex += 1;
							ChangeSelectedButton();
						}
					} catch {
						System.Diagnostics.Debug.WriteLine ("Something is wrong with Product Number in Grid");
					}
				}
					

				if ( ProductScrollView.ScrollY >= ProductGrid.Children.ElementAt (mLastLoadedIndex).Bounds.Bottom-50 ) {
					int endIndex = (int)Math.Ceiling (ProductScrollView.ScrollY / (int)Math.Floor(ProductGrid.Children.ElementAt(0).Height-MyDevice.ViewPadding/2)) * 2 - 1;

					if (endIndex >= ProductGrid.Children.Count) {
						endIndex = ProductGrid.Children.Count - 1;
					} 

					mLastLoadedIndex = endIndex;

				}
			}else {				

				if (mActiveButtonIndex != 0) {					
					int productCellIndex = mCategoryIndexList [mActiveButtonIndex];
					try{
						double top = ProductGrid.Children.ElementAt (productCellIndex).Bounds.Top;
						if (ProductScrollView.ScrollY < top) {
							mActiveButtonIndex -= 1;
							ChangeSelectedButton();
						}
					}
					catch{
						System.Diagnostics.Debug.WriteLine ("Something is wrong with Product Number in Grid");	
					}

				}

				if (mLastLoadedIndex >= ProductGrid.Children.Count)
					mLastLoadedIndex = ProductGrid.Children.Count - 1;

				if (ProductScrollView.ScrollY <= ProductGrid.Children.ElementAt (mLastLoadedIndex).Bounds.Top) {
					int endIndex = (int)Math.Floor (ProductScrollView.ScrollY / (int)Math.Floor(ProductGrid.Children.ElementAt(0).Height-MyDevice.ViewPadding/2)) * 2 - 1;

					if (endIndex < 0) {
						endIndex = 0;
					}
					else if( endIndex >= ProductGrid.Children.Count )
						endIndex =	ProductGrid.Children.Count - 1;

					mLastLoadedIndex = endIndex;
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
				if( productCellIndex < ProductGrid.Children.Count )
					await ProductScrollView.ScrollToAsync (ProductGrid.Children.ElementAt (productCellIndex), ScrollToPosition.Start, true);
				else
				{
					await WaitUntilCorrespondingSubCategoryLoaded(productCellIndex);
					await ProductScrollView.ScrollToAsync (ProductGrid.Children.ElementAt (productCellIndex), ScrollToPosition.Start, true);
				}
			}
			catch{
				System.Diagnostics.Debug.WriteLine ("Something is wrong with Product Number in Grid");
			}				
		}

		private void ChangeSelectedButton()
		{
			mEnabledBoxView.IsVisible = false;
			mEnabledBoxView = mBoxViewList [mActiveButtonIndex];
			mEnabledBoxView.IsVisible = true;
		}

		private void PopulateGrid()
		{
			SetGrid2Definitions ();

	
			//Populate a list with all products 
			//To be able to define product index
			var valueList = mProductDictionary.Values.Cast<List<Product>> ().ToList();
			//var tempProductList = new List<Product> ();
			foreach (var products in valueList) {
				if (products.Count > 0) {
					mCategoryIndexList.Add (mProductList.Count);
					foreach (var tempProduct in products) {					
						mProductList.Add (tempProduct);	
					}
				}
			}
			//PopulateSubCategoryButtons ();
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

				ProductCell productCell = new ProductCell (ProductGrid, product, this);

				mProductCellList.Add (productCell);					
			
				ProductGrid.Children.Add (productCell.View, productIndex % 2, productIndex / 2);			
				productCell.ProduceStreamsAndImages ();
				 
				await Task.Delay (100);
			}
		}

		private int CheckIfProductIsInTopSellingListAndReturnIndex(Product p)
		{			
			int counter = 0;
			foreach (var product in mTopSellingProductList) {				
				if (p.ProductID == product.ProductID)
					return counter;
				counter++;
			}

			return counter;
		}

	 	private async void LoadAllProducts()
		{	
			foreach (var product in mProductList) {
				int productIndex = mProductList.IndexOf (product);
				ProductCell productCell;


					
				if (productIndex < mTopSellingProductList.Count) {
					productCell = new ProductCell (ProductGrid, product, this);
					mTopSellingProductCellList.Add (productCell);
				} else {
					int index = CheckIfProductIsInTopSellingListAndReturnIndex (product);

					if (index != mTopSellingProductList.Count) {
						productCell = new ProductCell (ProductGrid, mTopSellingProductCellList [index].mProduct, this);
						productCell.mPairCell = mTopSellingProductCellList [index];
						mTopSellingProductCellList [index].mPairCell = productCell;
					}
					else
						productCell = new ProductCell (ProductGrid, product, this);					
				}

				mProductCellList.Add (productCell);					

				productCell.ProduceStreamsAndImages ();	
				ProductGrid.Children.Add (productCell.View, productIndex % 2, productIndex / 2);			



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

		private void SetGrid2Definitions()
		{
			//SubCategoryStackLayout.Spacing = MyDevice.ViewPadding;
			//SubcategoryScrollView.Padding = MyDevice.ViewPadding/2;
			/*for (int i = 0; i < mRowCount; i++) 
			{
				Grid2.RowDefinitions.Add (new RowDefinition ());
			}*/
			ProductGrid.Padding = new Thickness (MyDevice.ViewPadding / 2, 0, 0, 0); 
			ProductGrid.ColumnDefinitions.Add (new ColumnDefinition(){Width = (MyDevice.ScreenWidth-ProductGrid.ColumnSpacing-MyDevice.ViewPadding)/2});
			ProductGrid.ColumnDefinitions.Add (new ColumnDefinition(){Width = (MyDevice.ScreenWidth-ProductGrid.ColumnSpacing-MyDevice.ViewPadding)/2}); 
		}			
	}
}


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
		public List<ProductCell> mProductCellList;
		private List<BoxView> mBoxViewList;
		private List<Label> mButtonList;
		private BoxView mEnabledBoxView;
		private List<int> mCategoryIndexList;
		private double mPreviousScrollPositionY = 0;
		private int mActiveButtonIndex = 0;
		RootPage mParent;
		public string mCategoryID;
		public Common.SearchBar mSearchBar;

		public BrowseProductsPage (Dictionary<string,List<Product>> productDictionary,Category category, RootPage parent)
		{					
			InitializeComponent ();
			mParent = parent;
			mSearchBar = SearchBar;
			SearchBar.mParent = parent;
			//NavigationBar.mParent = parent;
			mProductCellList = new List<ProductCell> ();
			mBoxViewList = new List<BoxView> ();
			mButtonList = new List<Label> ();
			mCategoryIndexList = new List<int> ();
			//NavigationBar.NavigationText.Text = category.Name;
			parent.mTopNavigationBar.NavigationText.Text = category.Name;
			mCategoryID = category.CategoryID;
			mProductDictionary = productDictionary;
			int count = 0;
			foreach (var product in productDictionary) {
				count += product.Value.Count;
			}
			mRowCount = Convert.ToInt32(Math.Ceiling(count / 2.0f));

			NavigationPage.SetHasNavigationBar (this, false);
			SetGrid1Definitions ();
			PopulateGrid ();
			UpdatePriceLabel ();
		}

		public void  UpdatePriceLabel()
		{
			mParent.mRootHeader.mPriceLabel.Text = "AED:" + Cart.ProductTotalPrice.ToString();
			mParent.mTopNavigationBar.mPriceLabel.Text = "AED:" + Cart.ProductTotalPrice.ToString();
		}

		protected override void OnAppearing()
		{			
			//UpdatePriceLabel ();
		}

		private void SetGrid1Definitions()
		{
			Grid1.RowDefinitions [0].Height = GridLength.Auto;
			Grid1.RowDefinitions [1].Height = GridLength.Auto;//MyDevice.ScreenHeight / 25;
			Grid1.RowDefinitions [2].Height = GridLength.Auto;
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
			Grid1.BackgroundColor = MyDevice.BlueColor;
			if (mProductDictionary.Count <= 1) {
				ScrollView1.IsEnabled = false;
				Grid1.RowDefinitions [1].Height = 0;
				ScrollView1.IsVisible = false;
			}
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

		private void OnScrolled( Object sender, EventArgs e)
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
			var tempProductList = new List<Product> ();

			foreach (var products in valueList) {
				mCategoryIndexList.Add (tempProductList.Count);
				foreach (var tempProduct in products) {
					tempProductList.Add (tempProduct);	
				}

			}

			foreach (var productPair in mProductDictionary) {
				var productList = productPair.Value;
				foreach (var product in productList) {
					ProductCell productCell = new ProductCell (Grid2, product, this);
					int productIndex = tempProductList.IndexOf(product);
					mProductCellList.Add (productCell);				
					Grid2.Children.Add (productCell.View, productIndex % 2, productIndex / 2);
				}					
			}
		}

		private void SetGrid2Definitions()
		{
			SubCategoryStackLayout.Spacing = MyDevice.ViewPadding*3;
			ScrollView1.Padding = MyDevice.ViewPadding/2;
			for (int i = 0; i < mRowCount; i++) 
			{
				Grid2.RowDefinitions.Add (new RowDefinition ());
			}
			Grid2.Padding = new Thickness (MyDevice.ViewPadding / 2, 0, 0, 0);
			Grid2.ColumnDefinitions.Add (new ColumnDefinition(){Width = (MyDevice.ScreenWidth-Grid2.ColumnSpacing-MyDevice.ViewPadding)/2});
			Grid2.ColumnDefinitions.Add (new ColumnDefinition(){Width = (MyDevice.ScreenWidth-Grid2.ColumnSpacing-MyDevice.ViewPadding)/2}); 
		}

	}
}


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

		public BrowseProductsPage (Dictionary<string,List<Product>> productDictionary,Category category)
		{						
			InitializeComponent ();
			mProductCellList = new List<ProductCell> ();
			mBoxViewList = new List<BoxView> ();
			NavigationBar.NavigationText.Text = category.Name;
			mProductDictionary = productDictionary;
			int count = 0;
			foreach (var product in productDictionary) {
				count += product.Value.Count;
			}
			mRowCount = Convert.ToInt32(Math.Ceiling(count / 2.0f));

			NavigationPage.SetHasNavigationBar (this, false);
			SetGrid1Definitions ();
			PopulateGrid ();

		}

		public void  UpdatePriceLabel()
		{
			NavigationBar.mPriceLabel.Text = "DH: " + Cart.ProductTotalPrice.ToString();
		}

		protected override void OnAppearing()
		{
			UpdatePriceLabel ();
		}

		private void SetGrid1Definitions()
		{
			Grid1.RowDefinitions [0].Height = GridLength.Auto;
			Grid1.RowDefinitions [1].Height = MyDevice.ScreenHeight / 15;
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
					BackgroundColor = Color.White,
					Padding = 0
				};

				Button button = new Button (){
					VerticalOptions = LayoutOptions.Fill,
					BackgroundColor = Color.White,
					Text = productPair.Key,
					BorderWidth = 0,
					BorderRadius = 0,
					TextColor = MyDevice.RedColor,
					FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label))
				};

				BoxView boxView = new BoxView (){
					HeightRequest = 3,
					Color = Color.Olive,
					IsVisible = false
				};
				mBoxViewList.Add (boxView);
				relativeLayout.Children.Add (button,Constraint.RelativeToParent( (parent) =>{
					return 0;
				}));
				relativeLayout.Children.Add(boxView, 
					Constraint.RelativeToView(button, (parent, sibling) =>
						{
							return sibling.Bounds.Left + 5;
						}),
					Constraint.RelativeToView(button, (parent, sibling) =>
						{
							return sibling.Bounds.Bottom - 3;//sibling.Bounds.Bottom + 5;
						}),
					Constraint.RelativeToView(button, (parent, sibling) =>
						{
							return sibling.Width - 10;//sibling.Bounds.Bottom + 5;
						}));

				SubCategoryStackLayout.Children.Add (relativeLayout);
			}

			mBoxViewList [0].IsVisible = true;
			//XLabs.Forms.Controls.ExtendedButton.
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
				foreach (var tempProduct in products) {
					tempProductList.Add (tempProduct);	
				}

			}

			foreach (var productPair in mProductDictionary) {
				var productList = productPair.Value;
				foreach (var product in productList) {
					ProductCell productCell = new ProductCell (Grid2, product, this);
					int productIndex = tempProductList.IndexOf(product);

					Grid2.Children.Add (productCell.View, productIndex % 2, productIndex / 2);
				}					
			}

			/*for (int row = 0; row < mRowCount; row++) 
			{
				for (int col = 0; col < 2; col++) 
				{
					ProductCell productCell = new ProductCell (Grid2,[counter++],this );	
					mProductCellList.Add (productCell);
					Grid2.Children.Add (productCell.View, col, row);

					if ( counter == mProducts.Count)
						break;
				}
			}*/
		}

		private void SetGrid2Definitions()
		{
			for (int i = 0; i < mRowCount; i++) 
			{
				Grid2.RowDefinitions.Add (new RowDefinition ());
			}

			Grid2.ColumnDefinitions.Add (new ColumnDefinition(){Width = (MyDevice.ScreenWidth-Grid2.ColumnSpacing)/2});
			Grid2.ColumnDefinitions.Add (new ColumnDefinition(){Width = (MyDevice.ScreenWidth-Grid2.ColumnSpacing)/2}); 
		}

	}
}


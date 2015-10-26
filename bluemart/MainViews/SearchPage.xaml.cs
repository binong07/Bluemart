using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Remote;
using bluemart.Common.ViewCells;
using bluemart.Common.Objects;

namespace bluemart.MainViews
{
	public partial class SearchPage : ContentPage
	{
		private int mRowCount {
			get;
			set;
		}

		private List<Product> mProductList = new List<Product>();
		public List<ProductCell> mProductCellList = new List<ProductCell>();
		private string mSearchString ;

		RootPage mParent;

		public SearchPage (string searchString,RootPage parent)
		{
			mParent = parent;
			InitializeComponent ();
			mSearchString = searchString;
			NavigationPage.SetHasNavigationBar (this, false);
			SetGrid1Definitions ();
			PopulateSearch ();
		}

		private void SetGrid1Definitions()
		{
			Grid1.RowDefinitions [0].Height = GridLength.Auto;
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
		}

		public void UpdatePriceLabel()
		{
			mParent.mRootHeader.mPriceLabel.Text = "DH:"+Cart.ProductTotalPrice;
		}


		private void PopulateSearch()
		{
			//if (SearchEntry.Text.Length >= 3) {
			ProductModel.PopulateSearchProductList (mSearchString);
			mRowCount = Convert.ToInt32 (Math.Ceiling (ProductModel.mSearchProductIDList.Count / 2.0f));
			PopulateProducts ();
			PopulateGrid ();
		//	} else {
			//	SearchEntry.Text = "Must be longer than 2 characters!";
			//	SearchEntry.TextColor = Color.Red;
		//	}
		}

		private void PopulateGrid()
		{
			SetGrid2Definitions ();
			int counter = 0;
			for (int row = 0; row < mRowCount; row++) 
			{
				for (int col = 0; col < 2; col++) 
				{
					ProductCell productCell = new ProductCell (Grid2,mProductList[counter++],this );	
					mProductCellList.Add (productCell);
					Grid2.Children.Add (productCell.View, col, row);

					if ( counter == ProductModel.mSearchProductIDList.Count)
						break;
				}
			}
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

		void PopulateProducts()
		{
			mProductCellList.Clear ();
			mProductList.Clear ();
			Grid2.Children.Clear ();
			foreach (string productID in ProductModel.mSearchProductIDList ) {
				string ImagePath = ProductModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + ProductModel.mProductImageNameDictionary [productID] + ".jpg";
				string ProductName = ProductModel.mProductNameDictionary [productID];
				decimal price = ProductModel.mProductPriceDictionary [productID];
				string quantity = ProductModel.mProductQuantityDictionary [productID];
				mProductList.Add (new Product (productID, ProductName, ImagePath, price, quantity)); 
			}
		}
	}
}


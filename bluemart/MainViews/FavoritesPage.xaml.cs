using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;
using bluemart.Models.Remote;
using bluemart.Models.Local;
using bluemart.Common.ViewCells;

namespace bluemart.MainViews
{
	public partial class FavoritesPage : ContentPage
	{
		private FavoritesClass mFavoritesModel = new FavoritesClass();
		private int mRowCount;
		private List<Product> mProductList = new List<Product>();
		RootPage mParent;
		public FavoritesPage (RootPage parent)
		{
			InitializeComponent ();
			mParent = parent;
			Header.mParent = parent;
			NavigationPage.SetHasNavigationBar (this, false);
			SetGrid1Definitions ();
			SetGrid2Definitions ();

		}
		public void UpdatePriceLabel()
		{
			Header.mPriceLabel.Text = "DH: "+Cart.ProductTotalPrice;
		}

		protected override void OnAppearing()
		{
			UpdatePriceLabel ();
			RefreshFavoritesGrid ();
		}

		private void SetGrid1Definitions()
		{
			Grid1.RowDefinitions [0].Height = GridLength.Auto;
			Grid1.RowDefinitions [1].Height = GridLength.Auto;
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
			Grid1.BackgroundColor = MyDevice.BlueColor;
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
					Grid2.Children.Add (productCell.View, col, row);

					if ( counter == mProductList.Count)
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

		private void PopulateProducts( )
		{
			mProductList.Clear ();
			foreach (string productID in mFavoritesModel.GetProductIDs()) {
				string ImagePath = ProductModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + ProductModel.mProductImageNameDictionary[productID] + ".jpg";
				string ProductName = ProductModel.mProductNameDictionary [productID];
				double price = ProductModel.mProductPriceDictionary [productID];
				string quantity = ProductModel.mProductQuantityDictionary [productID];
				mProductList.Add (new Product (productID,ProductName, ImagePath, price, quantity)); 
			}
			mRowCount = Convert.ToInt32(Math.Ceiling(mProductList.Count / 2.0f));	
		}

		public void RefreshFavoritesGrid()
		{
			Grid2.Children.Clear ();
			mProductList.Clear ();
			PopulateProducts();
			PopulateGrid ();
			RefreshPriceInCart ();
		}

		public void RefreshPriceInCart()
		{
			Header.mPriceLabel.Text = "DH: " + Cart.ProductTotalPrice.ToString();
		}
	}
}
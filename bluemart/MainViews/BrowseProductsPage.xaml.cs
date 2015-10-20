using System;
using System.Collections.Generic;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;
using bluemart.Common.ViewCells;
using Xamarin.Forms;

namespace bluemart.MainViews
{
	public partial class BrowseProductsPage : ContentPage
	{
		private List<Product> mProducts;
		private int mRowCount;
		public List<ProductCell> mProductCellList;

		public BrowseProductsPage (List<Product> product,Category category)
		{						
			InitializeComponent ();
			mProductCellList = new List<ProductCell> ();
			NavigationBar.NavigationText.Text = category.Name;
			mProducts = product;
			mRowCount = Convert.ToInt32(Math.Ceiling(mProducts.Count / 2.0f));

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
					ProductCell productCell = new ProductCell (Grid2,mProducts[counter++],this );	
					mProductCellList.Add (productCell);
					Grid2.Children.Add (productCell.View, col, row);

					if ( counter == mProducts.Count)
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

	}
}


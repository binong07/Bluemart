using System;
using System.Collections.Generic;
using bluemart.Common.Objects;
using bluemart.Common.Utilities;
using bluemart.Common.ViewCells;
using Xamarin.Forms;
using bluemart.Models.Remote;

namespace bluemart.MainViews
{
	public partial class BrowseCategoriesPage : ContentPage
	{		
		List<Category> mCategories;
		RootPage mParent;

		public BrowseCategoriesPage (RootPage parent)
		{						
			InitializeComponent ();
			mParent = parent;
			Header.mParent = parent;
			CategoryModel.PopulateCategories ();
			mCategories = CategoryModel.CategoryList;
			NavigationPage.SetHasNavigationBar (this, false);
			SetGrid1Definitions ();
			PopulateGrid ();
		}

		public void RefreshPriceInCart()
		{
			Header.mPriceLabel.Text = "DH: " + Cart.ProductTotalPrice.ToString();
		}

		protected override void OnAppearing()
		{
			//Header.mPriceLabel.Text = "DH: " + Cart.ProductTotalPrice.ToString();
		}

		private void SetGrid1Definitions()
		{
			Grid1.RowDefinitions [0].Height = GridLength.Auto;
			//Grid1.RowDefinitions [2].Height = MyDevice.ScreenHeight / 10;
			Grid1.BackgroundColor = MyDevice.BlueColor;
			ScrollView1.BackgroundColor = MyDevice.BlueColor;
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
		}
			
		private void PopulateGrid()
		{
			for (int i = 0; i < mCategories.Count; i++) {
				if (!mCategories [i].IsSubCategory) {
					CategoryCell categoryCell = new CategoryCell (StackLayout1,
						mCategories [i],
						mParent);
					StackLayout1.Children.Add (categoryCell.View);	
				}
			}

		}
	}
}


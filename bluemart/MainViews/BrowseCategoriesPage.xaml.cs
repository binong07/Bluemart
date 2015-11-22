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
			CategoryModel.PopulateCategories ();
			mCategories = CategoryModel.CategoryList;
			NavigationPage.SetHasNavigationBar (this, false);
			PopulateGrid ();
		}

		/*protected override void OnAppearing()
		{
			mParent.mRootHeader.mPriceLabel.Text = "DH:"+Cart.ProductTotalPrice;
		}*/

		public void RefreshSearchText()
		{
			mParent.mRootHeader.mSearchEntry.Text = "Search Products";
		}

		private void SetGrid1Definitions()
		{
			ScrollView1.BackgroundColor = MyDevice.BlueColor;
			Content.BackgroundColor = MyDevice.BlueColor;

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


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
		private List<CategoryCell> mCategoryCellList = new List<CategoryCell>();

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

		public void RefreshBorderStream()
		{
			foreach (var categoryCell in mCategoryCellList) {
				categoryCell.SetBorderStream ();
			}
		}

		private void SetGrid1Definitions()
		{
			ScrollView1.BackgroundColor = MyDevice.BlueColor;
			ScrollView1.Padding = new Thickness (0, MyDevice.ScreenWidth * 0.0138f, 0, 0);
			Content.BackgroundColor = MyDevice.BlueColor;

		}
			
		private void PopulateGrid()
		{			
			//ScrollView1.Padding = MyDevice.ViewPadding;
			ScrollView1.BackgroundColor = MyDevice.BackgroundColor;
			StackLayout1.Spacing = MyDevice.ScreenWidth * 0.0139f;
			for (int i = 0; i < mCategories.Count; i++) {
				if (!mCategories [i].IsSubCategory) {
					CategoryCell categoryCell = new CategoryCell (StackLayout1,
						mCategories [i],
						mParent);
					mCategoryCellList.Add (categoryCell);
					StackLayout1.Children.Add (categoryCell.View);	
				}
			}
		}
	}
}


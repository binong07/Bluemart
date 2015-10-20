using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Objects;
using bluemart.Common.Utilities;
using bluemart.Common.ViewCells;

namespace bluemart.MainViews
{
	public partial class BrowseSubCategoriesPage : ContentPage
	{
		List<Category> mCategories;

		public BrowseSubCategoriesPage (List<Category> category, Category parentCategory)
		{						
			InitializeComponent ();
			mCategories = category;
			NavigationBar.NavigationText.Text = parentCategory.Name;
			NavigationPage.SetHasNavigationBar (this, false);
			SetGrid1Definitions ();
			PopulateGrid ();
		}

		protected override void OnAppearing()
		{
			NavigationBar.mPriceLabel.Text = "DH: " + Cart.ProductTotalPrice.ToString();
		}

		private void SetGrid1Definitions()
		{
			Grid1.RowDefinitions [0].Height = GridLength.Auto;
			Grid1.BackgroundColor = MyDevice.BlueColor;
			ScrollView1.BackgroundColor = MyDevice.BlueColor;
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
		}

		private void PopulateGrid()
		{
			for (int i = 0; i < mCategories.Count; i++) {
				CategoryCell categoryCell = new CategoryCell (  StackLayout1,
					mCategories[i],
					null,
					this);
					StackLayout1.Children.Add (categoryCell.View);						
			}
		}
	}
}


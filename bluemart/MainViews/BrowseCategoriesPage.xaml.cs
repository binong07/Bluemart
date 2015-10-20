using System;
using System.Collections.Generic;
using bluemart.Common.Objects;
using bluemart.Common.Utilities;
using bluemart.Common.ViewCells;
using Xamarin.Forms;

namespace bluemart.MainViews
{
	public partial class BrowseCategoriesPage : ContentPage
	{		
		List<Category> mCategories;

		public BrowseCategoriesPage (List<Category> category, RootPage rootPage)
		{						
			InitializeComponent ();
			mCategories = category;
			NavigationPage.SetHasNavigationBar (this, false);
			SetGrid1Definitions ();
			PopulateGrid ();

			Header.mMenuButton.GestureRecognizers.Add (new TapGestureRecognizer{ 
				Command = new Command( (o) =>
				{
					rootPage.IsPresented = true;
				})
			});
		}

		protected override void OnAppearing()
		{
			Header.mPriceLabel.Text = "DH: " + Cart.ProductTotalPrice.ToString();
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
				if (!mCategories [i].IsSubCategory) {
					CategoryCell categoryCell = new CategoryCell (StackLayout1,
						mCategories [i],
						this);
					StackLayout1.Children.Add (categoryCell.View);	
					//Grid2.Children.Add (categoryCell.View, col, row);
				}
			}

		}
	}
}


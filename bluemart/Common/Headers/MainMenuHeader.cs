using System;
using System.Collections.Generic;
using bluemart.Common.Utilities;
using bluemart.MainViews;
using Xamarin.Forms;
using bluemart.Common.Objects;
using XLabs.Forms.Controls;

namespace bluemart.Common.Headers
{
	public partial class MainMenuHeader: Grid
	{
		public Label mPriceLabel;
		public RootPage mParent;
		public ExtendedEntry mSearchEntry;

		public MainMenuHeader ()
		{			
			InitializeComponent ();
			mSearchEntry = SearchEntry;
			SearchEntry.FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label));
			SearchEntry.TextColor = MyDevice.BlueColor;
			SetGridDefinitions ();
			SetImageSize ();

			AddTapRecognizers ();
		}



		private void SetGridDefinitions()
		{
			this.RowDefinitions [0].Height = MyDevice.ScreenHeight / 12;
			this.ColumnDefinitions [0].Width = MyDevice.MenuPadding;
			this.ColumnDefinitions [1].Width = MyDevice.ScreenWidth - MyDevice.ScreenHeight  / 7 - MyDevice.MenuPadding*2;
			this.ColumnDefinitions [2].Width = MyDevice.ScreenHeight / 7;
			this.ColumnDefinitions [3].Width = MyDevice.MenuPadding;
		}

		private void SetImageSize()
		{
			SearchButton.Aspect = Aspect.Fill;
			SearchButton.WidthRequest = MyDevice.ScreenWidth*0.087f;
			SearchButton.HeightRequest = MyDevice.ScreenWidth*0.087f;
			LogoImage.Aspect = Aspect.Fill;
			LogoImage.HeightRequest = MyDevice.ScreenWidth * 0.109f;
			LogoImage.WidthRequest = MyDevice.ScreenWidth * 0.568f;
		}

		private void AddTapRecognizers()
		{
			var searchGridGestureRecognizer = new TapGestureRecognizer ();
			searchGridGestureRecognizer.Tapped += (sender, e) => {
				SearchEntry.IsVisible = !SearchEntry.IsVisible;
				LogoImage.IsVisible = !LogoImage.IsVisible;
				if( SearchEntry.IsFocused )
					SearchEntry.Unfocus();
				else
					SearchEntry.Focus();
			};
			this.Children[2].GestureRecognizers.Add (searchGridGestureRecognizer);
		}

		private void SearchEntryCompleted(Object sender,EventArgs e)
		{
			if (SearchEntry.Text.Length >= 3) {				
				mParent.LoadSearchPage (SearchEntry.Text);
			} else {				
				SearchEntry.Text = "Must be longer than 2 characters!";
			}
		}

		private void SearchEntryFocused(Object sender,EventArgs e)
		{
			SearchEntry.Text = "";
			mParent.RemoveFooter ();
		}

		private void SearchEntryUnfocused(Object sender,EventArgs e)
		{
			if (SearchEntry.Text == "")
				SearchEntry.Text = "Search Products";

			SearchEntry.IsVisible = !SearchEntry.IsVisible;
			LogoImage.IsVisible = !LogoImage.IsVisible;

			mParent.AddFooter ();
		}
	}
}

 
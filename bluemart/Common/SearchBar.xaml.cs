using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.MainViews;
using bluemart.Models.Remote;

namespace bluemart.Common
{
	public partial class SearchBar : Grid
	{
		public RootPage mParent;
		public Entry mSearchEntry;
		public SearchBar ()
		{
			InitializeComponent ();
			mSearchEntry = SearchEntry;
			SetGridDefinitions ();
			AddTapRecognizers ();
		}

		private void AddTapRecognizers()
		{
			var searchButtonGestureRecognizer = new TapGestureRecognizer ();
			searchButtonGestureRecognizer.Tapped += (sender, e) =>  {
				SearchButton.Opacity = 0.5f;
				FocusOnEntry();
				SearchButton.Opacity = 1f;
			};
			SearchButton.GestureRecognizers.Add (searchButtonGestureRecognizer);
		}

		private void FocusOnEntry()
		{			
			SearchEntry.Focus ();
		}

		private void SetGridDefinitions()
		{
			this.WidthRequest = MyDevice.ScreenWidth * 4 / 5;
			this.ColumnDefinitions [0].Width = MyDevice.ScreenWidth / 5;
			this.ColumnDefinitions [1].Width = MyDevice.ScreenWidth *3 / 5;
			this.RowDefinitions [0].Height = MyDevice.ScreenHeight / 20;
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
			mParent.mFooter.IsVisible = false;
		}

		private void SearchEntryUnfocused(Object sender,EventArgs e)
		{
			if (SearchEntry.Text == "")
				SearchEntry.Text = "Search Products";
			mParent.mFooter.IsVisible = true;
		}
	}
}


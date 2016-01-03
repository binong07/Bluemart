using System;
using System.Collections.Generic;
using bluemart.Common.Utilities;
using bluemart.MainViews;
using Xamarin.Forms;
using bluemart.Common.Objects;
using XLabs.Forms.Controls;
using System.Linq;
using FFImageLoading.Forms;

namespace bluemart.Common.Headers
{
	public partial class MainMenuHeader: Grid
	{
		public Label mPriceLabel;
		public RootPage mParent;
		public ExtendedEntry mSearchEntry;
		public RelativeLayout mSearchButtonLayout;

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
			this.RowDefinitions [0].Height = MyDevice.ScreenWidth * 0.148f;
			this.ColumnDefinitions [0].Width = MyDevice.MenuPadding;
			this.ColumnDefinitions [1].Width = MyDevice.ScreenWidth - MyDevice.ScreenWidth*0.174f - MyDevice.MenuPadding*2;
			this.ColumnDefinitions [2].Width = MyDevice.ScreenWidth*0.174f;
			this.ColumnDefinitions [3].Width = MyDevice.MenuPadding;

			mSearchButtonLayout = new RelativeLayout(){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = 0
			};

			var searchImage = new CachedImage () {
				Source = "search",
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false
			};

			mSearchButtonLayout.Children.Add(searchImage,
				Constraint.RelativeToParent( parent =>
					{
						return (MyDevice.ScreenWidth*0.174f-MyDevice.ScreenWidth*0.087f)/2;
					}),
				Constraint.RelativeToParent( parent =>
					{
						return (MyDevice.ScreenWidth*0.148f-MyDevice.ScreenWidth*0.087f)/2;					
					}),
				Constraint.Constant(MyDevice.ScreenWidth*0.087f),
				Constraint.Constant(MyDevice.ScreenWidth*0.087f)
				);

			this.Children.Add (mSearchButtonLayout, 2, 0);
		}

		private void SetImageSize()
		{
			//SearchButton.Aspect = Aspect.Fill;
			//SearchButton.WidthRequest = MyDevice.ScreenWidth*0.087f;
			//SearchButton.HeightRequest = MyDevice.ScreenWidth*0.087f;
		
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

			//this.Children[2].GestureRecognizers.Add (searchGridGestureRecognizer);
			mSearchButtonLayout.GestureRecognizers.Add (searchGridGestureRecognizer);

			/*var emptyGestureRecognizer = new TapGestureRecognizer ();
			//this.ColumnDefinitions[0]
			emptyGestureRecognizer.Tapped += (sender, e) => {};
			this.Children[0].GestureRecognizers.Add (emptyGestureRecognizer);
			this.Children[1].GestureRecognizers.Add (emptyGestureRecognizer);*/
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

		}

		private void SearchEntryUnfocused(Object sender,EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine ("SearchEntryUnfocused");
			if (SearchEntry.Text == "")
				SearchEntry.Text = "Search Products";

			SearchEntry.IsVisible = !SearchEntry.IsVisible;
			LogoImage.IsVisible = !LogoImage.IsVisible;
		}
	}
}

 
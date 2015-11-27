using System;
using System.Collections.Generic;
using bluemart.Common.Utilities;
using bluemart.MainViews;
using Xamarin.Forms;
using System.Threading.Tasks;
using PCLStorage;
using System.Linq;
using XLabs.Forms.Controls;


namespace bluemart.Common.Headers
{
	public partial class TopNavigationBar : Grid
	{
		public Label NavigationText;
		public Label mPriceLabel;
		public RootPage mParent;
		public ExtendedEntry mSearchEntry;
		private RelativeLayout mSearchButtonLayout;
		private RelativeLayout mBackButtonLayout;

		public TopNavigationBar ()
		{
			InitializeComponent ();
			NavigationText = NavigationTitle;
			mSearchEntry = SearchEntry;
			SearchEntry.FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label));
			SearchEntry.TextColor = MyDevice.BlueColor;
			//mPriceLabel = PriceLabel;
			NavigationTitle.TextColor = MyDevice.RedColor;
			SetGridDefinitions ();
			SetImageSize ();
			AddTapRecognizers ();
		}


		private void SetGridDefinitions()
		{
			this.RowDefinitions [0].Height = MyDevice.ScreenWidth * 0.148f;;
			this.ColumnDefinitions [0].Width = MyDevice.MenuPadding;
			this.ColumnDefinitions [1].Width = MyDevice.ScreenWidth*0.174f;
			this.ColumnDefinitions [2].Width = MyDevice.ScreenWidth - MyDevice.ScreenWidth*0.174f*2 - MyDevice.MenuPadding*2;
			this.ColumnDefinitions [3].Width = MyDevice.ScreenWidth*0.174f;
			this.ColumnDefinitions [4].Width = MyDevice.MenuPadding;

			mBackButtonLayout = new RelativeLayout(){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = 0
			};

			var backImage = new Image () {
				Source = "back",
				Aspect = Aspect.Fill
			};

			mBackButtonLayout.Children.Add(backImage,
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

			this.Children.Add (mBackButtonLayout, 1, 0);

			mSearchButtonLayout = new RelativeLayout(){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = 0
			};

			var searchImage = new Image () {
				Source = "search",
				Aspect = Aspect.Fill
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

			this.Children.Add (mSearchButtonLayout, 3, 0);

			/*CartGrid.RowDefinitions [0].Height = MyDevice.ScreenHeight / 18;
			CartGrid.RowDefinitions [1].Height = MyDevice.ScreenHeight / 36;

			PriceLabel.TextColor = MyDevice.BlueColor;
			PriceLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));*/
		}

		private void SetImageSize()
		{
			/*SearchButton.Aspect = Aspect.Fill;
			SearchButton.WidthRequest = MyDevice.ScreenWidth*0.087f;
			SearchButton.HeightRequest = MyDevice.ScreenWidth*0.087f;*/
			//BackButton.HeightRequest = MyDevice.ScreenWidth*0.087f;
			//CartButton.HeightRequest = MyDevice.ScreenHeight / 18;
			NavigationTitle.FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label));
		}

		private void AddTapRecognizers()
		{		

			var searchGridGestureRecognizer = new TapGestureRecognizer ();
			searchGridGestureRecognizer.Tapped += (sender, e) => {
				SearchEntry.IsVisible = !SearchEntry.IsVisible;
				NavigationText.IsVisible = !NavigationText.IsVisible;
				if( SearchEntry.IsFocused )
					SearchEntry.Unfocus();
				else
					SearchEntry.Focus();
			};
			mSearchButtonLayout.GestureRecognizers.Add (searchGridGestureRecognizer);

			var backButtonTapGestureRecognizer = new TapGestureRecognizer ();
			backButtonTapGestureRecognizer.Tapped += async (sender, e) => {
				if( mParent.mCurrentPageParent == "BrowseCategories" && mParent.mTopNavigationBar.mSearchEntry.IsFocused )
					return;				
				await Task.Delay(MyDevice.DelayTime);			
				mParent.SwitchTab (mParent.mCurrentPageParent);

			};
			mBackButtonLayout.GestureRecognizers.Add (backButtonTapGestureRecognizer);
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
			NavigationText.IsVisible = !NavigationText.IsVisible;

			mParent.AddFooter ();
		}
	}
}

 
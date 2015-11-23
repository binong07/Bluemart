using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Utilities;
using System.Linq;
using bluemart.MainViews;


namespace bluemart.Common
{
	public partial class Footer : StackLayout
	{
		private Label mActiveLabel;
		public Label mCategoriesLabel;
		public Label mSettingsLabel;
		public Label mFavoritesLabel;
		public Label mCartLabel;
		public Label mTrackLabel;

		public Footer ()
		{
			InitializeComponent ();

			BackgroundColor = MyDevice.BlueColor;

			mCategoriesLabel = CategoryLabel;
			mSettingsLabel = SettingsLabel;
			mFavoritesLabel = FavoritesLabel;
			mCartLabel = CartLabel;
			mTrackLabel = TrackLabel;

			SetImageSize ();
			SetLabelProperties ();
			AddTapRecognizers ();
			mActiveLabel = CategoryLabel;
			mActiveLabel.TextColor = MyDevice.RedColor;
		}

		public void ChangeColorOfLabel(Label newActiveLabel)
		{
			mActiveLabel.TextColor = Color.White;
			mActiveLabel = newActiveLabel;
			mActiveLabel.TextColor = MyDevice.RedColor;
		}

		public void SetLabelProperties()
		{
			CategoryLabel.TextColor = Color.White;
			SettingsLabel.TextColor = Color.White;
			FavoritesLabel.TextColor = Color.White;
			CartLabel.TextColor = Color.White;
			TrackLabel.TextColor = Color.White;


			CategoryLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))-1;
			SettingsLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))-1;
			FavoritesLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))-1;
			CartLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))-1;
			TrackLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))-1;
		}

		private void SetImageSize()
		{
			CartGrid.RowDefinitions [0].Height = MyDevice.ViewPadding/3;
			TrackGrid.RowDefinitions [0].Height = MyDevice.ViewPadding/3;
			CategoryGrid.RowDefinitions [0].Height = MyDevice.ViewPadding/3;
			FavoritesGrid.RowDefinitions [0].Height = MyDevice.ViewPadding/3;
			SettingsGrid.RowDefinitions [0].Height = MyDevice.ViewPadding/3;
			CategoryButton.WidthRequest = MyDevice.ScreenWidth / 5;
			SettingsButton.WidthRequest = MyDevice.ScreenWidth / 5;
			FavoritesButton.WidthRequest = MyDevice.ScreenWidth / 5;
			CartButton.WidthRequest = MyDevice.ScreenWidth / 5;
			TrackButton.WidthRequest = MyDevice.ScreenWidth / 5;
		}			

		private void AddTapRecognizers()
		{
			
			var categoryButtonTapRecognizer = new TapGestureRecognizer ();
			categoryButtonTapRecognizer.Tapped += (sender, e) => {

				CategoryGrid.Opacity = 0.5f;
				if( Navigation.NavigationStack.Last() is RootPage )
					(Navigation.NavigationStack.Last() as RootPage).SwitchTab("BrowseCategories");
				
				CategoryGrid.Opacity = 1f;
			};
			CategoryGrid.GestureRecognizers.Add (categoryButtonTapRecognizer);

			var SettingsGridTapRecognizer = new TapGestureRecognizer ();
			SettingsGridTapRecognizer.Tapped += (sender, e) => {

				SettingsGrid.Opacity = 0.5f;
				if( Navigation.NavigationStack.Last() is RootPage )
					(Navigation.NavigationStack.Last() as RootPage).SwitchTab("Settings");
				
				SettingsGrid.Opacity = 1f;
			};
			SettingsGrid.GestureRecognizers.Add (SettingsGridTapRecognizer);

			var FavoritesGridTapRecognizer = new TapGestureRecognizer ();
			FavoritesGridTapRecognizer.Tapped += (sender, e) => {

				FavoritesGrid.Opacity = 0.5f;
				if( Navigation.NavigationStack.Last() is RootPage )
					(Navigation.NavigationStack.Last() as RootPage).SwitchTab("Favorites");

				FavoritesGrid.Opacity = 1f;
			};
			FavoritesGrid.GestureRecognizers.Add (FavoritesGridTapRecognizer);

			var CartGridTapRecognizer = new TapGestureRecognizer ();
			CartGridTapRecognizer.Tapped += (sender, e) => {

				CartGrid.Opacity = 0.5f;
				if( Navigation.NavigationStack.Last() is RootPage )
					(Navigation.NavigationStack.Last() as RootPage).LoadCartPage();

				CartGrid.Opacity = 1f;
			};
			CartGrid.GestureRecognizers.Add (CartGridTapRecognizer);

			var TrackGridTapRecognizer = new TapGestureRecognizer ();
			TrackGridTapRecognizer.Tapped += (sender, e) => {

				TrackGrid.Opacity = 0.5f;
				if( Navigation.NavigationStack.Last() is RootPage )
					(Navigation.NavigationStack.Last() as RootPage).SwitchTab("Track");

				TrackGrid.Opacity = 1f;
			};
			TrackGrid.GestureRecognizers.Add (TrackGridTapRecognizer);

		}
	}
}


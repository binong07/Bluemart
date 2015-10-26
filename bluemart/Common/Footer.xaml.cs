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
		public Label mHistoryLabel;
		public Label mTrackLabel;

		public Footer ()
		{
			InitializeComponent ();

			mCategoriesLabel = CategoryLabel;
			mSettingsLabel = SettingsLabel;
			mFavoritesLabel = FavoritesLabel;
			mHistoryLabel = HistoryLabel;
			mTrackLabel = TrackLabel;

			SetImageSize ();
			SetLabelProperties ();
			AddTapRecognizers ();
			mActiveLabel = CategoryLabel;
			mActiveLabel.TextColor = MyDevice.BlueColor;
		}

		public void ChangeColorOfLabel(Label newActiveLabel)
		{
			mActiveLabel.TextColor = MyDevice.RedColor;
			mActiveLabel = newActiveLabel;
			mActiveLabel.TextColor = MyDevice.BlueColor;
		}

		public void SetLabelProperties()
		{
			CategoryLabel.TextColor = MyDevice.RedColor;
			SettingsLabel.TextColor = MyDevice.RedColor;
			FavoritesLabel.TextColor = MyDevice.RedColor;
			HistoryLabel.TextColor = MyDevice.RedColor;
			TrackLabel.TextColor = MyDevice.RedColor;
		}

		private void SetImageSize()
		{
			CategoryButton.WidthRequest = MyDevice.ScreenWidth / 5;
			SettingsButton.WidthRequest = MyDevice.ScreenWidth / 5;
			FavoritesButton.WidthRequest = MyDevice.ScreenWidth / 5;
			HistoryButton.WidthRequest = MyDevice.ScreenWidth / 5;
			TrackButton.WidthRequest = MyDevice.ScreenWidth / 5;				
		}			

		private void AddTapRecognizers()
		{
			
			var categoryButtonTapRecognizer = new TapGestureRecognizer ();
			categoryButtonTapRecognizer.Tapped += (sender, e) => {

				CategoryButton.Opacity = 0.5f;

				ChangeColorOfLabel(CategoryLabel);

				if( Navigation.NavigationStack.Last() is RootPage )
					(Navigation.NavigationStack.Last() as RootPage).SwitchTab("BrowseCategories");
				
				CategoryButton.Opacity = 1f;
			};
			CategoryButton.GestureRecognizers.Add (categoryButtonTapRecognizer);

			var settingsButtonTapRecognizer = new TapGestureRecognizer ();
			settingsButtonTapRecognizer.Tapped += (sender, e) => {

				SettingsButton.Opacity = 0.5f;
				ChangeColorOfLabel(SettingsLabel);
				if( Navigation.NavigationStack.Last() is RootPage )
					(Navigation.NavigationStack.Last() as RootPage).SwitchTab("Settings");
				
				SettingsButton.Opacity = 1f;
			};
			SettingsButton.GestureRecognizers.Add (settingsButtonTapRecognizer);

			var favoritesButtonTapRecognizer = new TapGestureRecognizer ();
			favoritesButtonTapRecognizer.Tapped += (sender, e) => {

				FavoritesButton.Opacity = 0.5f;
				ChangeColorOfLabel(FavoritesLabel);
				if( Navigation.NavigationStack.Last() is RootPage )
					(Navigation.NavigationStack.Last() as RootPage).SwitchTab("Favorites");

				FavoritesButton.Opacity = 1f;
			};
			FavoritesButton.GestureRecognizers.Add (favoritesButtonTapRecognizer);

		}
	}
}


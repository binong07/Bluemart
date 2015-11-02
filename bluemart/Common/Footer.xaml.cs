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

			CategoryLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
			SettingsLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
			FavoritesLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
			HistoryLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
			TrackLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
		}

		private void SetImageSize()
		{
			HistoryGrid.RowDefinitions [0].Height = MyDevice.ViewPadding/3;
			TrackGrid.RowDefinitions [0].Height = MyDevice.ViewPadding/3;
			CategoryGrid.RowDefinitions [0].Height = MyDevice.ViewPadding/3;
			FavoritesGrid.RowDefinitions [0].Height = MyDevice.ViewPadding/3;
			SettingsGrid.RowDefinitions [0].Height = MyDevice.ViewPadding/3;
			//HistoryButton.Aspect = Aspect.Fill;
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

				CategoryGrid.Opacity = 0.5f;
				ChangeColorOfLabel(CategoryLabel);

				if( Navigation.NavigationStack.Last() is RootPage )
					(Navigation.NavigationStack.Last() as RootPage).SwitchTab("BrowseCategories");
				
				CategoryGrid.Opacity = 1f;
			};
			CategoryGrid.GestureRecognizers.Add (categoryButtonTapRecognizer);

			var SettingsGridTapRecognizer = new TapGestureRecognizer ();
			SettingsGridTapRecognizer.Tapped += (sender, e) => {

				SettingsGrid.Opacity = 0.5f;
				ChangeColorOfLabel(SettingsLabel);
				if( Navigation.NavigationStack.Last() is RootPage )
					(Navigation.NavigationStack.Last() as RootPage).SwitchTab("Settings");
				
				SettingsGrid.Opacity = 1f;
			};
			SettingsGrid.GestureRecognizers.Add (SettingsGridTapRecognizer);

			var FavoritesGridTapRecognizer = new TapGestureRecognizer ();
			FavoritesGridTapRecognizer.Tapped += (sender, e) => {

				FavoritesGrid.Opacity = 0.5f;
				ChangeColorOfLabel(FavoritesLabel);
				if( Navigation.NavigationStack.Last() is RootPage )
					(Navigation.NavigationStack.Last() as RootPage).SwitchTab("Favorites");

				FavoritesGrid.Opacity = 1f;
			};
			FavoritesGrid.GestureRecognizers.Add (FavoritesGridTapRecognizer);

			var HistoryGridTapRecognizer = new TapGestureRecognizer ();
			HistoryGridTapRecognizer.Tapped += (sender, e) => {

				HistoryGrid.Opacity = 0.5f;
				ChangeColorOfLabel(HistoryLabel);
				if( Navigation.NavigationStack.Last() is RootPage )
					(Navigation.NavigationStack.Last() as RootPage).SwitchTab("History");

				HistoryGrid.Opacity = 1f;
			};
			HistoryGrid.GestureRecognizers.Add (HistoryGridTapRecognizer);

			var TrackGridTapRecognizer = new TapGestureRecognizer ();
			TrackGridTapRecognizer.Tapped += (sender, e) => {

				TrackGrid.Opacity = 0.5f;
				ChangeColorOfLabel(TrackLabel);
				if( Navigation.NavigationStack.Last() is RootPage )
					(Navigation.NavigationStack.Last() as RootPage).SwitchTab("Track");

				TrackGrid.Opacity = 1f;
			};
			TrackGrid.GestureRecognizers.Add (TrackGridTapRecognizer);

		}
	}
}


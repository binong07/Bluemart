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
		private int mActiveButtonIndex;
		public Label mCategoriesLabel;
		public Label mSettingsLabel;
		public Label mFavoritesLabel;
		public Label mCartLabel;
		public Label mTrackLabel;

		public Footer ()
		{
			InitializeComponent ();

			BackgroundColor = MyDevice.BlueColor;

			this.Padding = new Thickness (MyDevice.ViewPadding*2, 0, 0, 0);
			this.Spacing = MyDevice.ViewPadding*3/2;
			//this.HeightRequest = MyDevice.ScreenWidth * 0.1796296296f;
			this.HeightRequest = MyDevice.ScreenWidth * 0.2096296296f;

			mCategoriesLabel = CategoryLabel;
			mSettingsLabel = SettingsLabel;
			mFavoritesLabel = FavoritesLabel;
			mCartLabel = CartLabel;
			mTrackLabel = TrackLabel;

			SetImageSize ();
			SetLabelProperties ();
			AddTapRecognizers ();
			mActiveLabel = CategoryLabel;
			mActiveButtonIndex = 0;
			mActiveLabel.TextColor = MyDevice.SelectedColor;
		}

		public void ChangeColorOfLabel(Label newActiveLabel)
		{
			mActiveLabel.TextColor = Color.White;
			mActiveLabel = newActiveLabel;
			mActiveLabel.TextColor = MyDevice.SelectedColor;
		}
		public void ChangeImageOfButton(int i)
		{
			switch (mActiveButtonIndex) {
			case 0://"BrowseCategories":
				CategoryButton.Source = (FileImageSource) ImageSource.FromFile("Categories.png");
				break;
			case 1://"Settings":
				SettingsButton.Source = (FileImageSource) ImageSource.FromFile("Settings.png");
				break;
			case 2://"Favorites":
				FavoritesButton.Source = (FileImageSource) ImageSource.FromFile("Favorites.png");
				break;
			case 3://"History":
				CartButton.Source = (FileImageSource) ImageSource.FromFile("cart2.png");
				break;
			case 4://"Track":
				TrackButton.Source = (FileImageSource) ImageSource.FromFile("Track.png");
				break;
			default:
				break;
			}
			mActiveButtonIndex = i;
			switch (mActiveButtonIndex) {
			case 0://"BrowseCategories":
				CategoryButton.Source = (FileImageSource) ImageSource.FromFile("Categories_S.png");
				break;
			case 1://"Settings":
				SettingsButton.Source = (FileImageSource) ImageSource.FromFile("Settings_S.png");
				break;
			case 2://"Favorites":
				FavoritesButton.Source = (FileImageSource) ImageSource.FromFile("Favorites_S.png");
				break;
			case 3://"History":
				CartButton.Source = (FileImageSource) ImageSource.FromFile("cart2_S.png");
				break;
			case 4://"Track":
				TrackButton.Source = (FileImageSource) ImageSource.FromFile("Track_S.png");
				break;
			default:
				break;
			}

		}
		public void SetLabelProperties()
		{
			CategoryLabel.TextColor = Color.White;
			SettingsLabel.TextColor = Color.White;
			FavoritesLabel.TextColor = Color.White;
			CartLabel.TextColor = Color.White;
			TrackLabel.TextColor = Color.White;


			CategoryLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
			SettingsLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
			FavoritesLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
			CartLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
			TrackLabel.FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));
		}

		private void SetImageSize()
		{			
			TrackButton.Aspect = Aspect.Fill;
			TrackButton.WidthRequest = MyDevice.ScreenWidth * 0.063888889f;
			TrackGrid.RowDefinitions [0].Height = MyDevice.ScreenWidth * 0.09f;
			TrackButton.HeightRequest = MyDevice.ScreenWidth * 0.064814815f;

			SettingsButton.Aspect = Aspect.Fill;
			SettingsButton.WidthRequest = MyDevice.ScreenWidth * 0.075185185f;
			SettingsGrid.RowDefinitions [0].Height = MyDevice.ScreenWidth * 0.09f;
			SettingsButton.HeightRequest = MyDevice.ScreenWidth * 0.075185185f;

			CategoryButton.Aspect = Aspect.Fill;
			CategoryButton.WidthRequest = MyDevice.ScreenWidth * 0.0866264f;
			CategoryGrid.RowDefinitions [0].Height = MyDevice.ScreenWidth * 0.09f;
			CategoryButton.HeightRequest = MyDevice.ScreenWidth * 0.075185185f;

			FavoritesButton.Aspect = Aspect.Fill;
			FavoritesButton.WidthRequest = MyDevice.ScreenWidth * 0.0784541f;
			FavoritesGrid.RowDefinitions [0].Height = MyDevice.ScreenWidth * 0.09f;
			FavoritesButton.HeightRequest = MyDevice.ScreenWidth * 0.075185185f;

			CartButton.Aspect = Aspect.Fill;
			CartButton.WidthRequest = MyDevice.ScreenWidth * 0.08254025;
			CartGrid.RowDefinitions [0].Height = MyDevice.ScreenWidth * 0.09f;
			CartButton.HeightRequest = MyDevice.ScreenWidth * 0.075185185f;
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


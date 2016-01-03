using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Local;
using bluemart.Common.Objects;
using bluemart.Common.ViewCells;
using FFImageLoading.Forms;

namespace bluemart.MainViews
{
	public partial class SettingsPage : ContentPage
	{
		UserClass mUserModel = new UserClass ();
		AddressClass mAddressModel = new AddressClass ();
		public AddressCell mActiveAddressCell;
		public RootPage mParent;

		private RelativeLayout mTopLayout;
		private RelativeLayout mMenuLayout;
		private RelativeLayout mMidLayout;
		private CachedImage mAddressInfoLayout;
		private Label categoriesLabel;
		private CachedImage menuIcon;
		private bool IsMenuOpen = false;
		private double mMenuWidth = 517.0;

		private RelativeLayout InputBlockerForSwipeMenu;

		public SettingsPage (RootPage parent)
		{
			InitializeComponent ();
			mParent = parent;
			NavigationPage.SetHasNavigationBar (this, false);
			InitializeLayout ();
		}

		private void InitializeLayout()
		{	
			mainRelativeLayout.BackgroundColor = Color.FromRgb (236, 240, 241);
			mMidLayout = new RelativeLayout ();

			Point totalDistance = new Point(0,0);

			mainRelativeLayout.Panning += (object sender, MR.Gestures.PanEventArgs e) => {
				totalDistance = e.TotalDistance;
			};

			mainRelativeLayout.Swiped += (object sender, MR.Gestures.SwipeEventArgs e) => {
				if(e.Direction == MR.Gestures.Direction.Left)
				{
					if(IsMenuOpen)
						ActivateOrDeactivateMenu();					
				}
				else if( e.Direction == MR.Gestures.Direction.Right)
				{
					if(!IsMenuOpen)
						ActivateOrDeactivateMenu();
				}
				else if( totalDistance.X != 0 && e.Direction == MR.Gestures.Direction.NotClear)
				{
					if( totalDistance.X < - MyDevice.SwipeDistance )
					{
						if(IsMenuOpen)
							ActivateOrDeactivateMenu();
					}
					else if( totalDistance.X > MyDevice.SwipeDistance )
					{
						if(!IsMenuOpen)
							ActivateOrDeactivateMenu();
					}
				}
			};

			mainRelativeLayout.Children.Add (mMidLayout,
				Constraint.Constant (0),
				Constraint.Constant (0)
			);

			InputBlockerForSwipeMenu = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(123),
				HeightRequest = MyDevice.ScreenHeight,
				Padding = 0
			};

			InitializeHeaderLayout ();
			InitializeMenuLayout ();
			InitializeAddressInfoLayout ();
		}

		private void InitializeHeaderLayout ()
		{
			mTopLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.GetScaledSize(87),
				BackgroundColor = Color.White
			};

			menuIcon = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(36),
				HeightRequest = MyDevice.GetScaledSize(37),
				Source = "ReceiptPage_MenuIcon",
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false
			};

			var logo = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(217),
				HeightRequest = MyDevice.GetScaledSize(39),
				Source = "ReceiptPage_Logo",
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false
			};



			var menuButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(72),
				HeightRequest = MyDevice.GetScaledSize(86)
			};

			var menuTapRecognizer= new TapGestureRecognizer ();
			menuTapRecognizer.Tapped += (sender, e) => {				
				ActivateOrDeactivateMenu();
			};
			menuButton.GestureRecognizers.Add(menuTapRecognizer);



			mMidLayout.Children.Add (mTopLayout,
				Constraint.Constant (0),
				Constraint.Constant (0)
			);

			mMidLayout.Children.Add (menuIcon, 
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Left +  MyDevice.GetScaledSize(20);
				}),
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Top + MyDevice.GetScaledSize(27);
				})
			);	

			mMidLayout.Children.Add (logo,
				Constraint.RelativeToView (menuIcon, (p, sibling) => {
					return sibling.Bounds.Right + MyDevice.GetScaledSize (150);	
				}),
				Constraint.RelativeToView (menuIcon, (p, sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize(5);	
				})
			);

			mMidLayout.Children.Add (menuButton,
				Constraint.Constant (0),
				Constraint.Constant (0));

		}

		private void InitializeMenuLayout()
		{
			mMenuLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(mMenuWidth),
				HeightRequest = MyDevice.ScreenHeight,
				BackgroundColor = Color.FromRgb(51,51,51)
			};

			var openImage = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(54),
				HeightRequest = MyDevice.GetScaledSize(44),
				Source = "MenuPage_Open",
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false
			};

			categoriesLabel = new Label () {
				Text = "Categories",
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
				FontSize = MyDevice.FontSizeMedium,
				WidthRequest = MyDevice.GetScaledSize(400),
				HeightRequest = MyDevice.GetScaledSize(44)
			};

			var categoriesButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(512),
				HeightRequest = MyDevice.GetScaledSize(50)
			};

			var firstLine = new BoxView (){
				HeightRequest = 1,
				WidthRequest = MyDevice.GetScaledSize(mMenuWidth),
				Color = Color.FromRgb(129,129,129)
			};

			var settingsImage = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(40),
				HeightRequest = MyDevice.GetScaledSize(35),
				Source = "MenuPage_Settings",
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false
			};

			var settingsLabel = new Label () {
				Text = "My Settings",
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
				FontSize = MyDevice.FontSizeMedium,
				WidthRequest = MyDevice.GetScaledSize(400),
				HeightRequest = MyDevice.GetScaledSize(44)
			};

			var settingsButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(512),
				HeightRequest = MyDevice.GetScaledSize(50)
			};

			var favoritesImage = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(40),
				HeightRequest = MyDevice.GetScaledSize(35),
				Source = "MenuPage_Favorites",
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false
			};

			var favoritesLabel = new Label () {
				Text = "Favorites",
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
				FontSize = MyDevice.FontSizeMedium,
				WidthRequest = MyDevice.GetScaledSize(400),
				HeightRequest = MyDevice.GetScaledSize(44)
			};

			var favoritesButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(512),
				HeightRequest = MyDevice.GetScaledSize(50)
			};

			var trackImage = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(40),
				HeightRequest = MyDevice.GetScaledSize(35),
				Source = "MenuPage_Track",
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false
			};

			var trackLabel = new Label () {
				Text = "Track Your Order",
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
				FontSize = MyDevice.FontSizeMedium,
				WidthRequest = MyDevice.GetScaledSize(400),
				HeightRequest = MyDevice.GetScaledSize(44)
			};

			var trackButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(512),
				HeightRequest = MyDevice.GetScaledSize(50),
				Padding = 0
			};

			var trackTapRecognizer = new TapGestureRecognizer ();
			trackTapRecognizer.Tapped += (sender, e) => {
				mParent.LoadTrackPage();
			};
			trackButton.GestureRecognizers.Add (trackTapRecognizer);

			mMenuLayout.Children.Add (trackButton,
				Constraint.Constant(0),
				Constraint.RelativeToView (trackImage, (parent,sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize(3);
				})
			);

			var secondLine = new BoxView (){
				HeightRequest = 1,
				WidthRequest = MyDevice.GetScaledSize(mMenuWidth),
				Color = Color.FromRgb(129,129,129)
			};

			var categoryNameStackLayout = new StackLayout {
				Orientation = StackOrientation.Vertical,
				Padding = 0,
				Spacing = 0
			};

			for (int i = 0; i < mParent.mBrowseCategoriesPage.mCategories.Count; i++) {
				if (!mParent.mBrowseCategoriesPage.mCategories [i].IsSubCategory) {
					Label label = new Label () {
						WidthRequest = MyDevice.GetScaledSize(442),	
						HeightRequest = MyDevice.GetScaledSize(78),
						TextColor = Color.White,
						HorizontalTextAlignment = TextAlignment.Start,
						VerticalTextAlignment = TextAlignment.Center,
						Text = mParent.mBrowseCategoriesPage.mCategories [i].Name,
						FontSize = MyDevice.FontSizeMedium
					};

					var tapRecog = new TapGestureRecognizer ();
					tapRecog.Tapped += (sender, e) => {
						string categoryName = (sender as Label).Text;
						Category category = null;
						foreach(var tempCategory in mParent.mBrowseCategoriesPage.mCategories)
						{
							if(tempCategory.Name == categoryName)
							{
								category = tempCategory;
							}
						}

						foreach(var categoryCell in mParent.mBrowseCategoriesPage.mCategoryCellList)
						{
							if( category != null && categoryCell.mCategory == category )
							{
								IsMenuOpen = false;
								categoryCell.LoadProductsPage(category.CategoryID,mParent);
							}
						}

					};

					label.GestureRecognizers.Add (tapRecog);
					categoryNameStackLayout.Children.Add (label);	
				}
			}

			var categoryNameScrollView = new ScrollView {
				Orientation = ScrollOrientation.Vertical,
				Content = categoryNameStackLayout
			};

			var categoriesTapRecognizer = new TapGestureRecognizer ();
			categoriesTapRecognizer.Tapped += (sender, e) => {
				mParent.SwitchTab("BrowseCategories");
			};
			categoriesButton.GestureRecognizers.Add (categoriesTapRecognizer);

			var favoritesTapRecognizer = new TapGestureRecognizer ();
			favoritesTapRecognizer.Tapped += (sender, e) => {
				mParent.LoadFavoritesPage();
			};
			favoritesButton.GestureRecognizers.Add (favoritesTapRecognizer);

			var settingsTapRecognizer = new TapGestureRecognizer ();
			settingsTapRecognizer.Tapped += (sender, e) => {
				mParent.LoadSettingsPage();
			};
			settingsButton.GestureRecognizers.Add (settingsTapRecognizer);

			mainRelativeLayout.Children.Add (mMenuLayout,
				Constraint.Constant (MyDevice.GetScaledSize (mMenuWidth) * -1),
				Constraint.Constant (0)
			);

			mMenuLayout.Children.Add (openImage,				
				Constraint.Constant(MyDevice.GetScaledSize(16)),
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Top + MyDevice.GetScaledSize (20.59f);
				})
			);

			mMenuLayout.Children.Add (categoriesLabel,
				Constraint.RelativeToView (openImage, (parent,sibling) => {
					return sibling.Bounds.Right + MyDevice.GetScaledSize (10);
				}),
				Constraint.RelativeToView (openImage, (parent,sibling) => {
					return sibling.Bounds.Top;
				})
			);

			mMenuLayout.Children.Add (firstLine,
				Constraint.Constant(0),
				Constraint.RelativeToView (openImage, (parent,sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(22);
				})
			);

			mMenuLayout.Children.Add (settingsImage,
				Constraint.RelativeToView (openImage, (parent,sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize (9);
				}),
				Constraint.RelativeToView (firstLine, (parent,sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (25);
				})
			);

			mMenuLayout.Children.Add (settingsLabel,
				Constraint.RelativeToView (settingsImage, (parent,sibling) => {
					return sibling.Bounds.Right + MyDevice.GetScaledSize (15);
				}),
				Constraint.RelativeToView (settingsImage, (parent,sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize(4);
				})
			);

			mMenuLayout.Children.Add (favoritesImage,
				Constraint.RelativeToView (openImage, (parent,sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize (9);
				}),
				Constraint.RelativeToView (settingsImage, (parent,sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (46);
				})
			);

			mMenuLayout.Children.Add (favoritesLabel,
				Constraint.RelativeToView (favoritesImage, (parent,sibling) => {
					return sibling.Bounds.Right + MyDevice.GetScaledSize (15);
				}),
				Constraint.RelativeToView (favoritesImage, (parent,sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize(4);
				})
			);

			mMenuLayout.Children.Add (trackImage,
				Constraint.RelativeToView (openImage, (parent,sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize (9);
				}),
				Constraint.RelativeToView (favoritesImage, (parent,sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (46);
				})
			);

			mMenuLayout.Children.Add (trackLabel,
				Constraint.RelativeToView (trackImage, (parent,sibling) => {
					return sibling.Bounds.Right + MyDevice.GetScaledSize (15);
				}),
				Constraint.RelativeToView (trackImage, (parent,sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize(4);
				})
			);

			mMenuLayout.Children.Add (secondLine,
				Constraint.Constant(MyDevice.GetScaledSize(0)),
				Constraint.RelativeToView (trackImage, (parent,sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(22);
				})
			);

			mMenuLayout.Children.Add (categoryNameScrollView,
				Constraint.Constant(MyDevice.GetScaledSize(76)),
				Constraint.RelativeToView (secondLine, (parent,sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(22);
				}),
				Constraint.Constant(MyDevice.GetScaledSize(440)),
				Constraint.Constant(MyDevice.ScreenHeight - MyDevice.GetScaledSize(445))
			);

			mMenuLayout.Children.Add (categoriesButton,
				Constraint.Constant(0),
				Constraint.RelativeToView (openImage, (parent,sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize(3);
				})
			);

			mMenuLayout.Children.Add (settingsButton,
				Constraint.Constant(0),
				Constraint.RelativeToView (settingsImage, (parent,sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize(3);
				})
			);

			mMenuLayout.Children.Add (favoritesButton,
				Constraint.Constant(0),
				Constraint.RelativeToView (favoritesImage, (parent,sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize(3);
				})
			);
		}

		private void ActivateOrDeactivateMenu()
		{
			Rectangle menuRectangle;
			Rectangle midRectangle;

			if (!IsMenuOpen) {
				menuRectangle = new Rectangle (new Point (MyDevice.GetScaledSize(mMenuWidth), 0), new Size (mMenuLayout.Bounds.Width, mMenuLayout.Bounds.Height));
				midRectangle = new Rectangle (new Point (MyDevice.GetScaledSize (mMenuWidth), 0), new Size (mMidLayout.Bounds.Width, mMidLayout.Bounds.Height));
				mainRelativeLayout.Children.Add (InputBlockerForSwipeMenu,
					Constraint.Constant (MyDevice.GetScaledSize (mMenuWidth)),
					Constraint.Constant (0)
				);

				var tapRecognizer = new TapGestureRecognizer ();
				if (InputBlockerForSwipeMenu.GestureRecognizers.Count == 0) {
					tapRecognizer.Tapped += (sender, e) => {				 				
						ActivateOrDeactivateMenu();				
					};
				}
				InputBlockerForSwipeMenu.GestureRecognizers.Add(tapRecognizer);
			} else {
				menuRectangle = new Rectangle (new Point (MyDevice.GetScaledSize (0), 0), new Size (mMenuLayout.Bounds.Width, mMenuLayout.Bounds.Height));
				midRectangle = new Rectangle (new Point (0, 0), new Size (mMidLayout.Bounds.Width, mMidLayout.Bounds.Height));
				mainRelativeLayout.Children.Remove (InputBlockerForSwipeMenu);
			}

			mMenuLayout.TranslateTo (menuRectangle.X,menuRectangle.Y, MyDevice.AnimationTimer, Easing.Linear);
			mMidLayout.TranslateTo (midRectangle.X,midRectangle.Y, MyDevice.AnimationTimer, Easing.Linear);

			IsMenuOpen = !IsMenuOpen;
		}

		private void InitializeAddressInfoLayout(){
			mAddressInfoLayout = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(600),
				HeightRequest = MyDevice.GetScaledSize(214),
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false,
				Source = "SettingsPage_AddressInfoBackground"
			};

			var informationLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (400),
				HeightRequest = MyDevice.GetScaledSize(52),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(189,82,121),
				Text = "You have selected the following:",
				FontSize = MyDevice.FontSizeSmall
			};

			var regionLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (509),
				HeightRequest = MyDevice.GetScaledSize(30),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = "Dubai Marina / Serviced by Bluemart",
				FontSize = MyDevice.FontSizeSmall
			};

			var locationLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (509),
				HeightRequest = MyDevice.GetScaledSize(25),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = "Location: Dubai Marina",
				FontSize = MyDevice.FontSizeSmall
			};

			var changeLocationHint = new Label () {
				WidthRequest = MyDevice.GetScaledSize (600),
				HeightRequest = MyDevice.GetScaledSize(70),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = "< TAP TO CHANGE LOCATION >",
				FontSize = MyDevice.FontSizeMedium
			};

			var addAddressButton = new Label () {
				WidthRequest = MyDevice.GetScaledSize (215),
				HeightRequest = MyDevice.GetScaledSize(62),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				BackgroundColor = Color.FromRgb(132,178,98),
				TextColor = Color.White,
				Text = "ADD NEW\nDELIVERY ADDRESS",
				FontSize = MyDevice.FontSizeMicro
			};
					

			var browseAddressInfoLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (500),
				HeightRequest = MyDevice.GetScaledSize(62),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = "Your Bluemart /\nDubai Marina Addresses:",
				FontSize = MyDevice.FontSizeSmall
			};

			var addAddressTapRecognizer = new TapGestureRecognizer ();
			addAddressTapRecognizer.Tapped += (sender, e) => {
				mParent.LoadAddAddress();
			};
			addAddressButton.GestureRecognizers.Add (addAddressTapRecognizer);

			var addressStackLayout = new StackLayout {
				Orientation = StackOrientation.Vertical,
				Padding = 0,
				Spacing = 0
			};
					

			string region = mUserModel.GetUser().ActiveRegion;
			int shopNumber = RegionHelper.DecideShopNumber (region);
			string shopName = RegionHelper.DecideShopName (shopNumber);


			regionLabel.Text = region + " / Serviced by Bluemart";
			locationLabel.Text = "Location: "+shopName;
			browseAddressInfoLabel.Text = "Your Bluemart /\n" + shopName;

			var addressList = mAddressModel.GetAddressList (shopNumber);

			foreach (var address in addressList) {
				if (address != null) {
					if (address.IsActive) {
						mActiveAddressCell = new AddressCell (address, this);
						addressStackLayout.Children.Add (mActiveAddressCell.View);
					}	
					else
						addressStackLayout.Children.Add (new AddressCell (address, this).View);
				}
			}

			var changeAddressTapRecognizer = new TapGestureRecognizer ();
			changeAddressTapRecognizer.Tapped += (sender, e) => {
				mParent.Navigation.PopAsync();
			};
			changeLocationHint.GestureRecognizers.Add (changeAddressTapRecognizer);

			var addressScrollView = new ScrollView {
				Orientation = ScrollOrientation.Vertical,
				Content = addressStackLayout
			};
					

			mMidLayout.Children.Add (mAddressInfoLayout,
				Constraint.RelativeToView (menuIcon, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (mTopLayout, (p, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(25);	
				})
			);

			mMidLayout.Children.Add (informationLabel,
				Constraint.RelativeToView (mAddressInfoLayout, (p, sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize(67);	
				}),
				Constraint.RelativeToView (mAddressInfoLayout, (p, sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize(12);	
				})
			);

			mMidLayout.Children.Add (regionLabel,
				Constraint.RelativeToView (informationLabel, (p, sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize(5);	
				}),
				Constraint.RelativeToView (informationLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(10);	
				})
			);

			mMidLayout.Children.Add (locationLabel,
				Constraint.RelativeToView (regionLabel, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (regionLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(4);	
				})
			);

			mMidLayout.Children.Add (changeLocationHint,
				Constraint.RelativeToView (mAddressInfoLayout, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (mAddressInfoLayout, (p, sibling) => {
					return sibling.Bounds.Bottom - MyDevice.GetScaledSize(70);	
				})
			);

			mMidLayout.Children.Add (addAddressButton,
				Constraint.RelativeToView (mAddressInfoLayout, (p, sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize(192);	
				}),
				Constraint.RelativeToView (mAddressInfoLayout, (p, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(20);	
				})
			);

			mMidLayout.Children.Add (browseAddressInfoLabel,
				Constraint.RelativeToView (mAddressInfoLayout, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (addAddressButton, (p, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(20);	
				})
			);

			double addressHeight = MyDevice.GetScaledSize (117)*(addressStackLayout.Children.Count);

			if (addressHeight > MyDevice.GetScaledSize (400))
				addressHeight = MyDevice.GetScaledSize (400);

			mMidLayout.Children.Add (addressScrollView,
				Constraint.RelativeToView (browseAddressInfoLabel, (parent,sibling) => {
					return sibling.Bounds.Left;
				}),
				Constraint.RelativeToView (browseAddressInfoLabel, (parent,sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(16);
				}),
				Constraint.Constant(MyDevice.GetScaledSize(600)),
				Constraint.Constant(addressHeight)
			);

		}


		public void SwitchActiveAddress(AddressCell address)
		{
			mActiveAddressCell.mActiveAddressImage.IsVisible = true;
			mActiveAddressCell = address;
			mActiveAddressCell.mActiveAddressImage.IsVisible = false;
		}



		/*public void PopulateListView()
		{
			StackLayout2.Children.Clear ();
			string region = mUserModel.GetUser().ActiveRegion;
			int shopNumber = RegionHelper.DecideShopNumber (region);
			string shopName = RegionHelper.DecideShopName (shopNumber);
			ChangeLocationButton.Text = "You selected " + region + "\nServiced by Bluemart - " + shopName + " Location.\nTap To Change Location";
			AddressExplanationLabel.Text = "Your Bluemart - " + shopName + " Adresses :";
			var addressList = mAddressModel.GetAddressList (shopNumber);

			foreach (var address in addressList) {
				if (address != null) {
					if (address.IsActive) {
						mActiveAddressCell = new AddressCell (address, this);
						StackLayout2.Children.Add (mActiveAddressCell.View);	
					}
					else
						StackLayout2.Children.Add (new AddressCell (address, this).View);	
				}
			}
			/*for (int i = 0; i < 10; i++) {
				StackLayout2.Children.Add (new AddressCell ().View);
			}*/
			/*var orderHistoryList = OrderModel.GetOrdersForHistory ();
			foreach (var history in orderHistoryList) {
				MainStackLayout.Children.Add( new HistoryCell(history,this ).View );
			}*/
		/*}
			

		private void SetGrid1Definitions()
		{
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
			Grid1.RowDefinitions [0].Height = GridLength.Auto;
			Grid1.RowDefinitions [1].Height = GridLength.Auto;
			Grid1.BackgroundColor = MyDevice.BackgroundColor;
			AddressExplanationLabel.FontSize = MyDevice.FontSizeMedium;
			ChangeLocationButton.WidthRequest = MyDevice.ScreenWidth - MyDevice.ViewPadding * 2;
			AddAddressButton.WidthRequest = MyDevice.ScreenWidth - MyDevice.ViewPadding * 2;
		}
			
		private void OnChangeLocationButtonClicked( Object sender, EventArgs e )
		{
			mParent.Navigation.PopAsync ();
		}

		private void OnAddAddressButtonClicked( Object sender, EventArgs e )
		{
			mParent.LoadAddAddress ();
		}

		public void SwitchActiveAddress(AddressCell address)
		{
			mActiveAddressCell.mActiveAddressImage.IsVisible = false;
			mActiveAddressCell = address;
			mActiveAddressCell.mActiveAddressImage.IsVisible = true;
		}*/
	}
}



using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Models.Local;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;
using FFImageLoading.Forms;
using XLabs.Forms.Controls;

namespace bluemart.MainViews
{
	public partial class AddAddressPage : ContentPage
	{
		UserClass mUserModel = new UserClass ();
		AddressClass mAddressModel;
		bool bNameTextChanged = false;
		bool bSurNameTextChanged = false;
		bool bAddressTextChanged = false;
		bool bRegionTextChanged = false;
		bool bAddressDescriptionTextChanged = false;
		bool bAdressLine3TextChanged = false;
		bool bTelephoneTextChanged = false;
		RootPage mParent;

		private RelativeLayout mTopLayout;
		private RelativeLayout mMenuLayout;
		private RelativeLayout mMidLayout;
		private CachedImage mAddressInfoLayout;
		private Label categoriesLabel;
		private CachedImage menuIcon;
		private bool IsMenuOpen = false;
		private double mMenuWidth = 517.0;

		Entry NameEntry;
		Entry SurNameEntry;
		Entry AddressEntry;
		Entry AddressDescriptionEntry;
		Entry AddressLine3Entry;
		Entry PhoneEntry;
		Entry mFocusedEntry;
		Label RegionEntryLabel;
		Label SubmitButton;
		TapGestureRecognizer SubmitTapRecognizer = new TapGestureRecognizer();
		ScrollView scroll;
		//double mScrollHeight;
		private RelativeLayout InputBlockerForSwipeMenu;

		public AddAddressPage (AddressClass address, RootPage parent)
		{
			InitializeComponent ();
			mParent = parent;

			mAddressModel = address;

			
			NavigationPage.SetHasNavigationBar (this, false);
			InitializeLayout ();

			if (address != null) {
				SetInitialTexts ();
				bNameTextChanged = true;
				bSurNameTextChanged = true;
				bAddressTextChanged = true;
				bRegionTextChanged = true;
				bAddressDescriptionTextChanged = true;
				bAdressLine3TextChanged = true;
				bTelephoneTextChanged = true;
				SetSubmitButton ();
			}

			EventHandlers ();
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
			InitializeAddAddressLayout ();

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
				Text = "My Profile",
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
		public bool isMenuAnimationWorking = false;
		private void ActivateOrDeactivateMenu()
		{
			if (isMenuAnimationWorking)
				return;
			else
				isMenuAnimationWorking = true;
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

			mMenuLayout.TranslateTo (menuRectangle.X,menuRectangle.Y, MyDevice.AnimationTimer, Easing.Linear).ContinueWith(antecendent => isMenuAnimationWorking=false);
			mMidLayout.TranslateTo (midRectangle.X,midRectangle.Y, MyDevice.AnimationTimer, Easing.Linear);

			IsMenuOpen = !IsMenuOpen;
		}

		private void InitializeAddAddressLayout(){

			double labelGap = MyDevice.GetScaledSize (8);
			double entryGap = MyDevice.GetScaledSize (14);
			var addressLayout = new RelativeLayout () {
				Padding = 0
			};
			var infoLabel = new Label () {
				Text = "Please Enter Your Delivery Address",
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				FontSize = MyDevice.FontSizeMedium,
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.GetScaledSize(40)
			};

			addressLayout.Children.Add (infoLabel,
				Constraint.Constant (0),
				Constraint.Constant (0)
			);

			#region FirstName
			var firstNameLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (222),
				HeightRequest = MyDevice.GetScaledSize(26),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = "First Name",
				FontSize = MyDevice.FontSizeSmall
			};

			NameEntry = new Entry () {
				WidthRequest = MyDevice.GetScaledSize(600),
				HeightRequest = MyDevice.GetScaledSize(61),
				BackgroundColor = Color.White,
				Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeSentence)
			};					

			addressLayout.Children.Add (firstNameLabel,
				Constraint.RelativeToView (infoLabel, (p, sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize (28);	
				}),
				Constraint.RelativeToView (infoLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (43);	
				})
			);

			addressLayout.Children.Add (NameEntry,
				Constraint.RelativeToView (firstNameLabel, (p, sibling) => {
					return sibling.Bounds.Left - MyDevice.GetScaledSize (7);	
				}),
				Constraint.RelativeToView (firstNameLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + labelGap;	
				})
			);				
			#endregion

			#region SurName
			var surNameLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (222),
				HeightRequest = MyDevice.GetScaledSize(26),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = "Last Name",
				FontSize = MyDevice.FontSizeSmall
			};

			SurNameEntry = new Entry () {
				WidthRequest = MyDevice.GetScaledSize(600),
				HeightRequest = MyDevice.GetScaledSize(61),
				BackgroundColor = Color.White,
				Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeSentence)
			};					

			addressLayout.Children.Add (surNameLabel,
				Constraint.RelativeToView (firstNameLabel, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (NameEntry, (p, sibling) => {
					return sibling.Bounds.Bottom + entryGap;	
				})
			);

			addressLayout.Children.Add (SurNameEntry,
				Constraint.RelativeToView (surNameLabel, (p, sibling) => {
					return sibling.Bounds.Left - MyDevice.GetScaledSize (7);	
				}),
				Constraint.RelativeToView (surNameLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + labelGap;	
				})
			);
				
			#endregion

			#region Region
			var regionLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (222),
				HeightRequest = MyDevice.GetScaledSize(28),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = "Nearest Bluemart Location",
				FontSize = MyDevice.FontSizeSmall
			};

			var regionImageBackground = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(600),
				HeightRequest = MyDevice.GetScaledSize(61),
				Source = "AddressPage_EntryBackground",
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false
			};

			RegionEntryLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (581),
				HeightRequest = MyDevice.GetScaledSize(61),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(230,88,91),
				FontSize = MyDevice.FontSizeSmall
			};

			addressLayout.Children.Add (regionLabel,
				Constraint.RelativeToView (firstNameLabel, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (SurNameEntry, (p, sibling) => {
					return sibling.Bounds.Bottom + entryGap;	
				})
			);

			addressLayout.Children.Add (regionImageBackground,
				Constraint.RelativeToView (regionLabel, (p, sibling) => {
					return sibling.Bounds.Left - MyDevice.GetScaledSize (7);	
				}),
				Constraint.RelativeToView (regionLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + labelGap;	
				})
			);


			addressLayout.Children.Add (RegionEntryLabel,
				Constraint.RelativeToView (regionImageBackground, (p, sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize (17);	
				}),
				Constraint.RelativeToView (regionImageBackground, (p, sibling) => {
					return sibling.Bounds.Top;	
				})
			);

			UserClass user = mUserModel.GetUser ();
			RegionEntryLabel.Text = user.ActiveRegion;
			#endregion

			#region Address
			var addressLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (222),
				HeightRequest = MyDevice.GetScaledSize(26),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = "Area / Cluster",
				FontSize = MyDevice.FontSizeSmall
			};

			AddressEntry = new Entry () {
				WidthRequest = MyDevice.GetScaledSize(600),
				HeightRequest = MyDevice.GetScaledSize(61),
				BackgroundColor = Color.White
			};
					
			addressLayout.Children.Add (addressLabel,
				Constraint.RelativeToView (firstNameLabel, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (regionImageBackground, (p, sibling) => {
					return sibling.Bounds.Bottom + entryGap;	
				})
			);

			addressLayout.Children.Add (AddressEntry,
				Constraint.RelativeToView (addressLabel, (p, sibling) => {
					return sibling.Bounds.Left - MyDevice.GetScaledSize (7);	
				}),
				Constraint.RelativeToView (addressLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + labelGap;	
				})
			);				
			#endregion

			#region AddressDescription
			var addressDescriptionLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (422),
				HeightRequest = MyDevice.GetScaledSize(28),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = "Community / Building Name",
				FontSize = MyDevice.FontSizeSmall
			};

			AddressDescriptionEntry = new Entry () {
				WidthRequest = MyDevice.GetScaledSize(600),
				HeightRequest = MyDevice.GetScaledSize(61),
				BackgroundColor = Color.White
			};					

			addressLayout.Children.Add (addressDescriptionLabel,
				Constraint.RelativeToView (firstNameLabel, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (AddressEntry, (p, sibling) => {
					return sibling.Bounds.Bottom + entryGap;	
				})
			);

			addressLayout.Children.Add (AddressDescriptionEntry,
				Constraint.RelativeToView (addressDescriptionLabel, (p, sibling) => {
					return sibling.Bounds.Left - MyDevice.GetScaledSize (7);	
				}),
				Constraint.RelativeToView (addressDescriptionLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + labelGap;	
				})
			);				
			#endregion

			#region AddressLine3
			var addressLine3 = new Label () {
				WidthRequest = MyDevice.GetScaledSize (422),
				HeightRequest = MyDevice.GetScaledSize(28),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = "Apartment / House No",
				FontSize = MyDevice.FontSizeSmall
			};

			AddressLine3Entry = new Entry () {
				WidthRequest = MyDevice.GetScaledSize(600),
				HeightRequest = MyDevice.GetScaledSize(61),
				BackgroundColor = Color.White
			};					

			addressLayout.Children.Add (addressLine3,
				Constraint.RelativeToView (firstNameLabel, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (AddressDescriptionEntry, (p, sibling) => {
					return sibling.Bounds.Bottom + entryGap;	
				})
			);

			addressLayout.Children.Add (AddressLine3Entry,
				Constraint.RelativeToView (addressLine3, (p, sibling) => {
					return sibling.Bounds.Left - MyDevice.GetScaledSize (7);	
				}),
				Constraint.RelativeToView (addressLine3, (p, sibling) => {
					return sibling.Bounds.Bottom + labelGap;	
				})
			);				
			#endregion

			#region Phone
			var phoneLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (222),
				HeightRequest = MyDevice.GetScaledSize(26),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = "Telephone",
				FontSize = MyDevice.FontSizeSmall
			};

			var initialText = new Label () {
				WidthRequest = MyDevice.GetScaledSize (54),
				HeightRequest = MyDevice.GetScaledSize(62),
				HorizontalTextAlignment = TextAlignment.End,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.Black,
				Text = "05",
				FontSize = MyDevice.FontSizeMedium + MyDevice.GetScaledSize(3),
				BackgroundColor = Color.White
			};

			PhoneEntry = new Entry () {
				WidthRequest = MyDevice.GetScaledSize(565),
				HeightRequest = MyDevice.GetScaledSize(61),
				BackgroundColor = Color.White,
				Keyboard = Keyboard.Numeric,
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalOptions = LayoutOptions.Center
			};					

			addressLayout.Children.Add (phoneLabel,
				Constraint.RelativeToView (firstNameLabel, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (AddressLine3Entry, (p, sibling) => {
					return sibling.Bounds.Bottom + entryGap;	
				})
			);

			addressLayout.Children.Add (PhoneEntry,
				Constraint.RelativeToView (phoneLabel, (p, sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize (30);		
				}),
				Constraint.RelativeToView (phoneLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + labelGap + MyDevice.GetScaledSize(5);	
				})
			);

			addressLayout.Children.Add (initialText,
				Constraint.RelativeToView (phoneLabel, (p, sibling) => {
					return sibling.Bounds.Left - MyDevice.GetScaledSize (4);	
				}),
				Constraint.RelativeToView (phoneLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + labelGap;	
				})
			);	

						

			#endregion

		
			var line = new BoxView () {
				WidthRequest = MyDevice.GetScaledSize(600),
				HeightRequest = MyDevice.GetScaledSize(1),
				Color = Color.FromRgb(181,185,187)
			};

			addressLayout.Children.Add (line,
				Constraint.RelativeToView (PhoneEntry, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (PhoneEntry, (p, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(40);	
				})
			);

			SubmitButton = new Label () {
				WidthRequest = MyDevice.GetScaledSize (197),
				HeightRequest = MyDevice.GetScaledSize(56),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				BackgroundColor = Color.FromRgb(98,98,98),
				TextColor = Color.White,
				Text = "SUBMIT",
				FontSize = MyDevice.FontSizeMicro
			};

			addressLayout.Children.Add (SubmitButton,
				Constraint.RelativeToView (line, (p, sibling) => {
					return sibling.Bounds.Right - MyDevice.GetScaledSize(197);	
				}),
				Constraint.RelativeToView (line, (p, sibling) => {
					return sibling.Bounds.Bottom;	
				})
			);



			scroll = new ScrollView () {
				Orientation = ScrollOrientation.Vertical,
				Content = addressLayout,
				HeightRequest = MyDevice.ScreenHeight - MyDevice.GetScaledSize(200)
			};

			mMidLayout.Children.Add (scroll,
				Constraint.Constant (0),
				Constraint.Constant (MyDevice.GetScaledSize (120)),
				Constraint.Constant (MyDevice.ScreenWidth)/*,
				Constraint.Constant (MyDevice.ScreenHeight - MyDevice.GetScaledSize(200))*/
			);
					
		}

		private void ActivateScroll()
		{
			//mScrollHeight = scroll.Height;
			scroll.HeightRequest = MyDevice.ScreenHeight * 4  / 10;
			//mainRelativeLayout.Children.Remove (mMidLayout);
			/*mainRelativeLayout.Children.Add (scroll,
				Constraint.Constant (0),
				Constraint.Constant (0),
				Constraint.Constant (MyDevice.ScreenWidth),
				Constraint.Constant (MyDevice.ScreenHeight / 3)
			);*/
		}

		private void DeactivateScroll()
		{
			scroll.HeightRequest = MyDevice.ScreenHeight - MyDevice.GetScaledSize(200);
		}

		private async void Submit()
		{
			if (mFocusedEntry != null)
				mFocusedEntry.Unfocus ();

			string address = AddressEntry.Text.ToString ();
			string activeRegion = RegionEntryLabel.Text.ToString ();
			string addressDescription = AddressDescriptionEntry.Text.ToString ();
			string addressLine3 = AddressLine3Entry.Text.ToString ();
			string name = NameEntry.Text.ToString () + " " + SurNameEntry.Text.ToString ();
			string phoneNumber = PhoneEntry.Text.ToString ();

			AddressClass addressClass = new AddressClass();
			addressClass.Name = name;
			addressClass.PhoneNumber = phoneNumber;
			addressClass.Address = address;
			addressClass.AddressDescription = addressDescription;
			addressClass.AddressLine3 = addressLine3;
			addressClass.ShopNumber = RegionHelper.DecideShopNumber (activeRegion);
			addressClass.IsActive = false;


			if (mAddressModel == null) {
				mAddressModel = new AddressClass ();
				mAddressModel.AddAddress (addressClass);
			} else {
				addressClass.Id = mAddressModel.Id;
				mAddressModel.UpdateAddress (addressClass);
			}

			mAddressModel.MakeActive (addressClass);

			await DisplayAlert ("User Information Submitted", "You have successfully submitted your information", "OK");

			mParent.LoadSettingsPage ();
		}

		private void EventHandlers(){
			#region NameEntry
			NameEntry.TextChanged += (sender, e) => {				
				bNameTextChanged = CheckIfTextChanged (NameEntry);
				SetSubmitButton ();
			};
			NameEntry.Completed += (sender, e) => {
				SurNameEntry.Focus ();
			};
			NameEntry.Focused += (sender, e) => {
				mFocusedEntry = (sender as Entry);
				ActivateScroll();
			};
			NameEntry.Unfocused += (sender, e) => {
				DeactivateScroll();
			};
			#endregion
			#region SurNameEntry
			SurNameEntry.TextChanged += (sender, e) => {				
				bSurNameTextChanged = CheckIfTextChanged (SurNameEntry);
				SetSubmitButton ();
			};
			SurNameEntry.Completed += (sender, e) => {
				AddressEntry.Focus ();
			};
			SurNameEntry.Focused += (sender, e) => {
				mFocusedEntry = (sender as Entry);
				ActivateScroll();
			};
			SurNameEntry.Unfocused += (sender, e) => {
				DeactivateScroll();
			};
			#endregion
			#region AddressEntry
			AddressEntry.TextChanged += (sender, e) => {				
				bAddressTextChanged = CheckIfTextChanged (AddressEntry);
				SetSubmitButton ();
			};
			AddressEntry.Completed += (sender, e) => {
				AddressDescriptionEntry.Focus ();
			};
			AddressEntry.Focused += (sender, e) => {
				mFocusedEntry = (sender as Entry);
				ActivateScroll();
			};
			AddressEntry.Unfocused += (sender, e) => {
				DeactivateScroll();
			};
			#endregion
			#region AddressDescriptionEntry
			AddressDescriptionEntry.TextChanged += (sender, e) => {				
				bAddressDescriptionTextChanged = CheckIfTextChanged (AddressDescriptionEntry);
				SetSubmitButton ();
			};
			AddressDescriptionEntry.Completed += (sender, e) => {
				AddressLine3Entry.Focus ();
			};
			AddressDescriptionEntry.Focused += (sender, e) => {
				mFocusedEntry = (sender as Entry);
				ActivateScroll();
			};
			AddressDescriptionEntry.Unfocused += (sender, e) => {
				DeactivateScroll();
			};
			#endregion
			#region AddressDescriptionEntry
			AddressLine3Entry.TextChanged += (sender, e) => {				
				bAdressLine3TextChanged = CheckIfTextChanged (AddressLine3Entry);
				SetSubmitButton ();
			};
			AddressLine3Entry.Completed += (sender, e) => {
				PhoneEntry.Focus ();
			};
			AddressLine3Entry.Focused += (sender, e) => {
				mFocusedEntry = (sender as Entry);
				ActivateScroll();
			};
			AddressLine3Entry.Unfocused += (sender, e) => {
				DeactivateScroll();
			};
			#endregion
			#region PhoneEntry
			PhoneEntry.TextChanged += (sender, e) => {					
				string phoneNumber = PhoneEntry.Text.Trim ();

				if (phoneNumber.Length == 9)
					phoneNumber = phoneNumber.Remove (8);		

				PhoneEntry.Text = phoneNumber;
				if (PhoneEntry.Text.Length == 8)
					bTelephoneTextChanged = true;
				else
					bTelephoneTextChanged = false;
				SetSubmitButton ();
			};
			PhoneEntry.Focused += (sender, e) => {
				mFocusedEntry = (sender as Entry);
				ActivateScroll();
			};
			PhoneEntry.Completed += (sender, e) => {
				//Translate(0);
				PhoneEntry.Unfocus();
			};
			PhoneEntry.Unfocused += (sender, e) => {
				DeactivateScroll();
			};
			#endregion
			bRegionTextChanged = true;
		}

		private bool CheckIfTextChanged (Entry entry)
		{
			bool bTextChanged = false;

			if (!String.IsNullOrWhiteSpace (entry.Text.ToString ()) && entry.Text.Length > 0)
				bTextChanged = true;

			return bTextChanged;
		}

		private void SetSubmitButton() 
		{
			if (bNameTextChanged && bSurNameTextChanged && bAddressTextChanged && bTelephoneTextChanged && bRegionTextChanged && bAddressDescriptionTextChanged && bAdressLine3TextChanged) {
				SubmitButton.TextColor = Color.White;
				SubmitButton.BackgroundColor = Color.FromRgb(132,178,98);

				if (!SubmitButton.GestureRecognizers.Contains (SubmitTapRecognizer)) {
					SubmitTapRecognizer.Tapped += (sender, e) => {
						Submit ();
					};						
					SubmitButton.GestureRecognizers.Add (SubmitTapRecognizer);
				}

			} else {
				SubmitButton.TextColor = Color.White;
				SubmitButton.BackgroundColor = Color.FromRgb(98,98,98);
			}
		}

		public void SetInitialTexts()
		{
			NameEntry.Text = mAddressModel.Name;
			AddressEntry.Text = mAddressModel.Address;
			AddressDescriptionEntry.Text = mAddressModel.AddressDescription;
			AddressLine3Entry.Text = mAddressModel.AddressLine3;

			if (mAddressModel.Name.Length > 0) {
				NameEntry.Text = mAddressModel.Name.Split (' ') [0];
				SurNameEntry.Text = mAddressModel.Name.Split (' ') [1];
			}
			if (mAddressModel.PhoneNumber.Length > 0) {
				PhoneEntry.Text = mAddressModel.PhoneNumber;
			}
		}			
	}
}


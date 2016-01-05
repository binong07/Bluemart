using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Remote;
using bluemart.Common.ViewCells;
using bluemart.Common.Objects;
using XLabs.Forms.Controls;
using bluemart.Models.Local;
using FFImageLoading.Forms;

namespace bluemart.MainViews
{
	public partial class SearchPage : ContentPage
	{
		private int mRowCount {
			get;
			set;
		}

		private List<Product> mProductList = new List<Product>();
		public List<ProductCell> mProductCellList = new List<ProductCell>();
		private string mSearchString ;

		public RootPage mParent;

		ScrollView ProductScrollView;
		Grid ProductGrid;

		private RelativeLayout InputBlocker;
		private Category mCategory;
		private RelativeLayout mTopLayout;
		private RelativeLayout mMenuLayout;
		private RelativeLayout mMidLayout;
		private RelativeLayout mSearchLayout;
		private ExtendedEntry SearchEntry;
		private Label SearchLabel;
		private Label ProductCountLabel;
		private Label PriceLabel;
		private bool IsMenuOpen = false;
		private double mMenuWidth = 517.0;
		private Label categoriesLabel;

		public bool IsCartOpen = false;
		UserClass mUserModel = new UserClass();
		AddressClass mAddressModel = new AddressClass();
		private RelativeLayout mCartLayout;
		private double mCartWidth = 552.0;
		public StackLayout CartStackLayout;
		public Label subtotalPriceLabel;
		public Label checkoutPriceLabel;

		private RelativeLayout InputBlockerForSwipeMenu;
		private RelativeLayout InputBlockerForSwipeCart;

		public SearchPage (string searchString, string categoryId,RootPage parent)
		{
			mParent = parent;
			InitializeComponent ();
			mSearchString = searchString;
			NavigationPage.SetHasNavigationBar (this, false);
			InitializeLayout ();
			//SetGrid1Definitions ();
			PopulateSearch (mParent.mBrowseProductPage);
		}

		private void InitializeLayout()
		{				
			mMidLayout = new RelativeLayout ();

			Point totalDistance = new Point(0,0);

			mainRelativeLayout.Panning += (object sender, MR.Gestures.PanEventArgs e) => {
				totalDistance = e.TotalDistance;
			};

			mainRelativeLayout.Swiped += (object sender, MR.Gestures.SwipeEventArgs e) => {
				if(e.Direction == MR.Gestures.Direction.Left)
				{
					if(!IsCartOpen&&!IsMenuOpen)
						ActivateOrDeactivateCart();
					else if(IsMenuOpen&&!IsCartOpen)
						ActivateOrDeactivateMenu();					
				}
				else if( e.Direction == MR.Gestures.Direction.Right)
				{
					if(IsCartOpen&&!IsMenuOpen)
						ActivateOrDeactivateCart();
					else if(!IsMenuOpen&&!IsCartOpen)
						ActivateOrDeactivateMenu();
				}
				else if( totalDistance.X != 0 && e.Direction == MR.Gestures.Direction.NotClear)
				{
					if( totalDistance.X < - MyDevice.SwipeDistance )
					{
						if(!IsCartOpen&&!IsMenuOpen)
							ActivateOrDeactivateCart();
						else if(IsMenuOpen&&!IsCartOpen)
							ActivateOrDeactivateMenu();
					}
					else if( totalDistance.X > MyDevice.SwipeDistance )
					{
						if(IsCartOpen&&!IsMenuOpen)
							ActivateOrDeactivateCart();
						else if(!IsMenuOpen&&!IsCartOpen)
							ActivateOrDeactivateMenu();
					}
				}
			};

			mainRelativeLayout.BackgroundColor = Color.FromRgb (236, 240, 241);
			mainRelativeLayout.Children.Add (mMidLayout,
				Constraint.Constant (0),
				Constraint.Constant (0)
			);

			InputBlocker = new RelativeLayout () {
				WidthRequest = MyDevice.ScreenWidth,
				HeightRequest = MyDevice.ScreenHeight
			};

			var inputBlockerTapRecogniser = new TapGestureRecognizer ();
			inputBlockerTapRecogniser.Tapped += (sender, e) => {				
				SearchEntry.Unfocus();
			};
			InputBlocker.GestureRecognizers.Add(inputBlockerTapRecogniser);

			InputBlockerForSwipeMenu = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(123),
				HeightRequest = MyDevice.ScreenHeight,
				Padding = 0
			};

			InputBlockerForSwipeCart = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(86),
				HeightRequest = MyDevice.ScreenHeight,
				Padding = 0
			};

			InitializeHeaderLayout ();
			InitializeSearchLayout ();
			InitializeCartLayout ();
			InitializeBottomLayout ();
			InitializeMenuLayout ();
			EventHandlers ();
		}

		public void ActivateOrDeactivateCart()
		{
			Rectangle cartRectangle;
			Rectangle midRectangle;

			if (!IsCartOpen) {
				cartRectangle = new Rectangle (new Point (MyDevice.GetScaledSize (mCartWidth*-1), 0), new Size (mCartLayout.Bounds.Width, mCartLayout.Bounds.Height));
				midRectangle = new Rectangle (new Point (MyDevice.GetScaledSize (mCartWidth*-1), 0), new Size (mMidLayout.Bounds.Width, mMidLayout.Bounds.Height));

				mainRelativeLayout.Children.Add (InputBlockerForSwipeCart,
					Constraint.Constant (0),
					Constraint.Constant (0)
				);

				var tapRecognizer = new TapGestureRecognizer ();
				if (InputBlockerForSwipeCart.GestureRecognizers.Count == 0) {
					tapRecognizer.Tapped += (sender, e) => {				 				
						ActivateOrDeactivateCart();				
					};
				}
				InputBlockerForSwipeCart.GestureRecognizers.Add(tapRecognizer);

				subtotalPriceLabel.Text = Cart.ProductTotalPrice.ToString();
				checkoutPriceLabel.Text = "AED " + Cart.ProductTotalPrice.ToString ();

				CartStackLayout.Children.Clear ();

				foreach (Product p in Cart.ProductsInCart) {
					var cartCell = new CartCell (p, this);
					//mCartCellList.Add (cartCell);
					CartStackLayout.Children.Add( cartCell.View );
				}
			} else {
				cartRectangle = new Rectangle (new Point (0, 0), new Size (mCartLayout.Bounds.Width, mCartLayout.Bounds.Height));
				midRectangle = new Rectangle (new Point (0, 0), new Size (mMidLayout.Bounds.Width, mMidLayout.Bounds.Height));
				mainRelativeLayout.Children.Remove (InputBlockerForSwipeCart);
			}

			mCartLayout.TranslateTo (cartRectangle.X,cartRectangle.Y, MyDevice.AnimationTimer, Easing.Linear);
			mMidLayout.TranslateTo (midRectangle.X,midRectangle.Y, MyDevice.AnimationTimer, Easing.Linear);

			IsCartOpen = !IsCartOpen;
		}

		private void InitializeCartLayout()
		{
			mCartLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(mCartWidth),
				HeightRequest = MyDevice.ScreenHeight,
				BackgroundColor = Color.FromRgb(51,51,51),
				Padding = 0
			};

			UserClass user = mUserModel.GetUser ();
			AddressClass activeAdress = mAddressModel.GetActiveAddress (user.ActiveRegion);
			string userName = "";
			if (activeAdress != null)
				userName = activeAdress.Name;

			var userNameLabel = new Label () {				
				Text = userName.Split(' ')[0],
				TextColor = Color.White,
				HorizontalTextAlignment = TextAlignment.End,
				VerticalTextAlignment = TextAlignment.Center,
				FontSize = MyDevice.FontSizeMedium,
				WidthRequest = MyDevice.GetScaledSize(190),
				HeightRequest = MyDevice.GetScaledSize(85)
			};

			var titleLabel = new Label () {
				Text = "'s Basket (AED)",
				TextColor = Color.FromRgb(152,152,152),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				FontSize = MyDevice.FontSizeMedium,
				WidthRequest = MyDevice.GetScaledSize(250),
				HeightRequest = MyDevice.GetScaledSize(85)
			};

			var profilePic = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(33),
				HeightRequest = MyDevice.GetScaledSize(37),
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false,
				Source = "CartPage_ProfilePic"
			};

			var firstLine = new BoxView (){
				HeightRequest = 1,
				WidthRequest = MyDevice.GetScaledSize(mCartWidth),
				Color = Color.FromRgb(129,129,129)
			};

			CartStackLayout = new StackLayout {
				Orientation = StackOrientation.Vertical,
				Padding = 0,
				Spacing = 0
			};					

			var cartScrollView = new ScrollView {
				Orientation = ScrollOrientation.Vertical,
				Content = CartStackLayout
			};

			var bottomLayout = new RelativeLayout () {
				BackgroundColor = Color.Black,
				WidthRequest = MyDevice.GetScaledSize(mCartWidth),
				HeightRequest = MyDevice.GetScaledSize(239)
			};

			var subtotalLabel = new Label () {
				Text = "Subtotal",
				TextColor = Color.White,
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				FontSize = MyDevice.FontSizeMedium,
				WidthRequest = MyDevice.GetScaledSize(140),
				HeightRequest = MyDevice.GetScaledSize(51)
			};

			subtotalPriceLabel = new Label () {
				Text = "1055,85",
				TextColor = Color.White,
				HorizontalTextAlignment = TextAlignment.End,
				VerticalTextAlignment = TextAlignment.Center,
				FontSize = MyDevice.FontSizeMedium,
				WidthRequest = MyDevice.GetScaledSize(130),
				HeightRequest = MyDevice.GetScaledSize(51)
			};

			var deliveryFeeLabel = new Label () {
				Text = "Delivery Fee",
				TextColor = Color.White,
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				FontSize = MyDevice.FontSizeMedium,
				WidthRequest = MyDevice.GetScaledSize(213),
				HeightRequest = MyDevice.GetScaledSize(51)
			};

			var deliveryFeePriceLabel = new Label () {
				Text = "FREE",
				TextColor = Color.White,
				HorizontalTextAlignment = TextAlignment.End,
				VerticalTextAlignment = TextAlignment.Center,
				FontSize = MyDevice.FontSizeMedium,
				WidthRequest = MyDevice.GetScaledSize(130),
				HeightRequest = MyDevice.GetScaledSize(51)
			};

			var checkoutLabel = new Label () {
				Text = "Checkout Now",
				TextColor = Color.White,
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				FontSize = MyDevice.FontSizeMedium,
				WidthRequest = MyDevice.GetScaledSize(251),
				HeightRequest = MyDevice.GetScaledSize(60)
			};

			checkoutPriceLabel = new Label () {
				Text = "1055,85",
				TextColor = Color.White,
				HorizontalTextAlignment = TextAlignment.End,
				VerticalTextAlignment = TextAlignment.Center,
				FontSize = MyDevice.FontSizeSmall,
				WidthRequest = MyDevice.GetScaledSize(200),
				HeightRequest = MyDevice.GetScaledSize(60)
			};

			var checkoutButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(493),
				HeightRequest = MyDevice.GetScaledSize(60),
				BackgroundColor = Color.FromRgb(253,59,47)
			};

			var checkoutTapRecogniser = new TapGestureRecognizer ();
			checkoutTapRecogniser.Tapped += async (sender, e) => {
				if( Cart.ProductTotalPrice == 0 )
					await DisplayAlert("Warning","Please add products in your basket","OK");
				else if( mAddressModel.GetActiveAddress(mUserModel.GetUser().ActiveRegion) == null )
				{					
					await DisplayAlert("Sorry","Please Enter Your Address On Settings Page","OK");
					mParent.SwitchTab("Settings");
				}
				else if( Cart.ProductTotalPrice < 50 )
				{
					await DisplayAlert("Sorry","Your orders must exceed 50 AED, as it is the minimum amount.","OK");
				}
				else
					mParent.LoadReceiptPage();
			};
			checkoutButton.GestureRecognizers.Add (checkoutTapRecogniser);

			mainRelativeLayout.Children.Add (mCartLayout,
				Constraint.Constant (MyDevice.ScreenWidth),
				Constraint.Constant (0)
			);

			mCartLayout.Children.Add (profilePic,
				Constraint.Constant (MyDevice.GetScaledSize(mCartWidth-43.5f)),
				Constraint.Constant (MyDevice.GetScaledSize(25f))
			);

			mCartLayout.Children.Add (titleLabel,
				Constraint.RelativeToView (profilePic, (parent,sibling) => {
					return sibling.Bounds.Left - MyDevice.GetScaledSize(268);	
				} ),
				Constraint.Constant (0)
			);

			mCartLayout.Children.Add (userNameLabel,
				Constraint.RelativeToView (titleLabel, (parent,sibling) => {
					return sibling.Bounds.Left - MyDevice.GetScaledSize(190);	
				} ),
				Constraint.Constant (0)
			);

			mCartLayout.Children.Add (firstLine,
				Constraint.Constant(0),
				Constraint.RelativeToView (userNameLabel, (parent,sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(2);
				})
			);

			mCartLayout.Children.Add (cartScrollView,
				Constraint.Constant(MyDevice.GetScaledSize(0)),
				Constraint.RelativeToView (firstLine, (parent,sibling) => {
					return sibling.Bounds.Bottom;
				}),
				Constraint.Constant(MyDevice.GetScaledSize(mCartWidth)),
				Constraint.Constant(MyDevice.ScreenHeight - MyDevice.GetScaledSize(324))
			);

			mCartLayout.Children.Add (bottomLayout,
				Constraint.Constant(0),
				Constraint.RelativeToView( mCartLayout, (parent,sibling) =>
					{
						return sibling.Bounds.Bottom - MyDevice.GetScaledSize(239);
					})
			);

			bottomLayout.Children.Add (subtotalLabel,
				Constraint.Constant(MyDevice.GetScaledSize(55)),	
				Constraint.Constant(MyDevice.GetScaledSize(25))
			);

			bottomLayout.Children.Add (subtotalPriceLabel,
				Constraint.Constant(MyDevice.GetScaledSize(mCartWidth-179)),	
				Constraint.Constant(MyDevice.GetScaledSize(25))
			);

			bottomLayout.Children.Add (deliveryFeeLabel,
				Constraint.RelativeToView( subtotalLabel, (parent,sibling) => {
					return sibling.Bounds.Left;	
				}),	
				Constraint.RelativeToView( subtotalLabel, (parent,sibling) => {
					return sibling.Bounds.Bottom;	
				})
			);

			bottomLayout.Children.Add (deliveryFeePriceLabel,
				Constraint.RelativeToView( subtotalPriceLabel, (parent,sibling) => {
					return sibling.Bounds.Left;	
				}),	
				Constraint.RelativeToView( subtotalPriceLabel, (parent,sibling) => {
					return sibling.Bounds.Bottom;	
				})
			);

			bottomLayout.Children.Add (checkoutButton,
				Constraint.RelativeToView (deliveryFeeLabel, (parent, sibling) => {
					return sibling.Bounds.Left - MyDevice.GetScaledSize(22);	
				}),	
				Constraint.RelativeToView (deliveryFeeLabel, (parent, sibling) => {
					return sibling.Bounds.Bottom;	
				})
			);

			bottomLayout.Children.Add (checkoutLabel,
				Constraint.RelativeToView( deliveryFeeLabel, (parent,sibling) => {
					return sibling.Bounds.Left;	
				}),	
				Constraint.RelativeToView( deliveryFeeLabel, (parent,sibling) => {
					return sibling.Bounds.Bottom;	
				})
			);

			bottomLayout.Children.Add (checkoutPriceLabel,
				Constraint.RelativeToView( checkoutLabel, (parent,sibling) => {
					return sibling.Bounds.Right;	
				}),	
				Constraint.RelativeToView( deliveryFeePriceLabel, (parent,sibling) => {
					return sibling.Bounds.Bottom;	
				})
			);
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

		private void InitializeHeaderLayout ()
		{
			mTopLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.GetScaledSize(87),
				BackgroundColor = Color.FromRgb(27,184,105)
			};

			var menuIcon = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(36),
				HeightRequest = MyDevice.GetScaledSize(37),
				Source = "CategoriesPage_MenuIcon",
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false
			};

			var categoryLabel = new Label (){ 
				Text = "Search",
				TextColor = Color.White,
				FontSize = MyDevice.FontSizeLarge
			};

			PriceLabel = new Label () {
				Text = "0\nAED",	
				TextColor = Color.White,
				FontSize = MyDevice.FontSizeSmall,
				HorizontalTextAlignment = TextAlignment.Center
			};

			var verticalLine = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(1),
				HeightRequest = MyDevice.GetScaledSize(63),
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false,
				Source = "CategoriesPage_VerticalLine"
			};

			var cartImage = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(71),
				HeightRequest = MyDevice.GetScaledSize(57),
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false,
				Source = "ProductsPage_BasketIcon"
			};

			ProductCountLabel = new Label () {					
				TextColor = Color.White,
				FontSize = MyDevice.FontSizeMicro,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				WidthRequest = MyDevice.GetScaledSize(37),
				HeightRequest = MyDevice.GetScaledSize(27)
			};

			var menuButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(72),
				HeightRequest = MyDevice.GetScaledSize(86)
			};

			var cartButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(90),
				HeightRequest = MyDevice.GetScaledSize(90),
				Padding = 0
			};

			var menuTapRecognizer= new TapGestureRecognizer ();
			menuTapRecognizer.Tapped += (sender, e) => {				
				ActivateOrDeactivateMenu();
			};
			menuButton.GestureRecognizers.Add(menuTapRecognizer);

			var cartTapRecognizer= new TapGestureRecognizer ();
			cartTapRecognizer.Tapped += (sender, e) => {				
				ActivateOrDeactivateCart();
			};
			cartButton.GestureRecognizers.Add(cartTapRecognizer);

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

			mMidLayout.Children.Add (categoryLabel,
				Constraint.RelativeToView (menuIcon, (parent, sibling) => {
					return sibling.Bounds.Right + MyDevice.GetScaledSize (22);
				}),
				Constraint.RelativeToView (menuIcon, (parent, sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize (3);
				})
			);

			mMidLayout.Children.Add (cartImage, 
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Right -  MyDevice.GetScaledSize(79);
				}),
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Top + MyDevice.GetScaledSize(16);
				})
			);

			mMidLayout.Children.Add (verticalLine,
				Constraint.RelativeToView (cartImage, (parent, sibling) => {
					return sibling.Bounds.Left - MyDevice.GetScaledSize (14);
				}),
				Constraint.RelativeToView (cartImage, (parent, sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize (5);
				})
			);

			mMidLayout.Children.Add (PriceLabel,
				Constraint.RelativeToView (verticalLine, (parent, sibling) => {
					return sibling.Bounds.Left - MyDevice.GetScaledSize (75);
				}),
				Constraint.RelativeToView (cartImage, (parent, sibling) => {
					return sibling.Bounds.Top;
				})
			);

			mMidLayout.Children.Add (ProductCountLabel,
				Constraint.RelativeToView (cartImage, (parent, sibling) => {
					return sibling.Bounds.Right - MyDevice.GetScaledSize (37);
				}),
				Constraint.RelativeToView (cartImage, (parent, sibling) => {
					return sibling.Bounds.Bottom - MyDevice.GetScaledSize (27);
				})
			);	

			mMidLayout.Children.Add (menuButton,
				Constraint.Constant (0),
				Constraint.Constant (0));

			mMidLayout.Children.Add (cartButton,
				Constraint.Constant(MyDevice.GetScaledSize(550)),
				Constraint.Constant (0)
			);

			UpdateProductCountLabel ();
			UpdatePriceLabel ();
		}

		private void ActivateOrDeactivateMenu()
		{
			Rectangle menuRectangle;
			Rectangle midRectangle;

			if (!IsMenuOpen) {
				menuRectangle = new Rectangle (new Point (0, 0), new Size (mMenuLayout.Bounds.Width, mMenuLayout.Bounds.Height));
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
				menuRectangle = new Rectangle (new Point (MyDevice.GetScaledSize (mMenuWidth*-1), 0), new Size (mMenuLayout.Bounds.Width, mMenuLayout.Bounds.Height));
				midRectangle = new Rectangle (new Point (0, 0), new Size (mMidLayout.Bounds.Width, mMidLayout.Bounds.Height));
				mainRelativeLayout.Children.Remove (InputBlockerForSwipeMenu);
			}

			mMenuLayout.LayoutTo (menuRectangle, 1000, Easing.CubicIn);
			mMidLayout.LayoutTo (midRectangle, 1000, Easing.CubicIn);

			IsMenuOpen = !IsMenuOpen;
		}

		private void InitializeSearchLayout()
		{
			mSearchLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.GetScaledSize(73),
				BackgroundColor = Color.FromRgb(27,184,105)
			};

			SearchEntry = new ExtendedEntry () {
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.GetScaledSize(73),
				Text = "Search",
				MaxLength = 15
			};

			var searchImage = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(583),
				HeightRequest = MyDevice.GetScaledSize(52),
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false,
				Source = "ProductsPage_SearchBar"	
			};

			var searchButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(444),
				HeightRequest = MyDevice.GetScaledSize(51)
			};

			SearchLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize(444),
				HeightRequest = MyDevice.GetScaledSize(51),
				TextColor = Color.White,
				FontSize = MyDevice.FontSizeMedium,
				Text = "Search",
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center
			};

			var deleteButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(69),
				HeightRequest = MyDevice.GetScaledSize(51)
			};

			var backButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(65),
				HeightRequest = MyDevice.GetScaledSize(72)
			};					


			var searchEntryTapRecognizer= new TapGestureRecognizer ();
			searchEntryTapRecognizer.Tapped += (sender, e) => {				
				SearchEntry.Focus();
			};
			searchButton.GestureRecognizers.Add(searchEntryTapRecognizer);

			var deleteButtonTapRecognizer= new TapGestureRecognizer ();
			deleteButtonTapRecognizer.Tapped += (sender, e) => {				
				if( SearchEntry.Text.Length > 0 )
					SearchEntry.Text = SearchEntry.Text.Remove(SearchEntry.Text.Length - 1);
			};
			deleteButton.GestureRecognizers.Add(deleteButtonTapRecognizer);

			var backButtonTapRecognizer= new TapGestureRecognizer ();
			backButtonTapRecognizer.Tapped += (sender, e) => {				
				mParent.SwitchTab ("BrowseCategories");
			};
			backButton.GestureRecognizers.Add(backButtonTapRecognizer);

			mMidLayout.Children.Add (SearchEntry,
				Constraint.Constant(0),
				Constraint.RelativeToView (mTopLayout, (parent, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (1);
				})
			);

			mMidLayout.Children.Add (mSearchLayout,
				Constraint.Constant(0),
				Constraint.RelativeToView (mTopLayout, (parent, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (1);
				})
			);

			mMidLayout.Children.Add (searchImage,
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize (28);
				}),
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize (10);
				})
			);

			mMidLayout.Children.Add (searchButton,
				Constraint.RelativeToView (searchImage, (parent, sibling) => {
					return sibling.Bounds.Left;
				}),
				Constraint.RelativeToView (searchImage, (parent, sibling) => {
					return sibling.Bounds.Top;
				})
			);

			mMidLayout.Children.Add (SearchLabel,
				Constraint.RelativeToView (searchButton, (parent, sibling) => {
					return sibling.Bounds.Left+ MyDevice.GetScaledSize(118);
				}),
				Constraint.RelativeToView (searchButton, (parent, sibling) => {
					return sibling.Bounds.Top;
				})
			);

			mMidLayout.Children.Add (deleteButton,
				Constraint.RelativeToView (searchImage, (parent, sibling) => {
					return sibling.Bounds.Right - MyDevice.GetScaledSize (67);
				}),
				Constraint.RelativeToView (searchImage, (parent, sibling) => {
					return sibling.Bounds.Top;
				})
			);

			mMidLayout.Children.Add (backButton,
				Constraint.Constant (0),
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Top;
				})
			);
		}

		private void InitializeBottomLayout()
		{
			ProductGrid = new Grid () {
				Padding = 0
			};

			//PopulateGrid ();

			ProductScrollView = new ScrollView () {
				Orientation = ScrollOrientation.Vertical,
				Content = ProductGrid
			};



			mMidLayout.Children.Add (ProductScrollView,
				Constraint.Constant(0),
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Bottom;
				}),
				Constraint.Constant(MyDevice.GetScaledSize(630)),
				Constraint.Constant(MyDevice.ScreenHeight-MyDevice.GetScaledSize(87)-MyDevice.GetScaledSize(73)-MyDevice.GetScaledSize(1)-MyDevice.GetScaledSize(50))
			);
		}

		private void EventHandlers()
		{
			SearchEntry.PropertyChanged += (sender, e) => {
				SearchLabel.Text = SearchEntry.Text;
			};

			SearchEntry.Focused += (sender, e) => {
				SearchEntry.Text = "";
				mMidLayout.Children.Add( InputBlocker,
					Constraint.Constant(0),
					Constraint.Constant(0)
				);
			};

			SearchEntry.Unfocused += (sender, e) => {
				if( SearchEntry.Text == "" )
					SearchEntry.Text = "Search";
				mMidLayout.Children.Remove(InputBlocker);
			};

			SearchEntry.Completed += (sender, e) => {
				if (SearchEntry.Text.Length >= 3) {				
					mParent.LoadSearchPage (SearchEntry.Text);
				} else {				
					SearchEntry.Text = "Must be longer than 2 characters!";
				}
				mMidLayout.Children.Remove(InputBlocker);
			};

			//ProductScrollView.Scrolled += OnScrolled;
		}

		public void  UpdatePriceLabel()
		{
			PriceLabel.Text = Cart.ProductTotalPrice.ToString()+"\nAED";
		}

		public void UpdateProductCountLabel()
		{
			int count = 0;

			foreach (var product in Cart.ProductsInCart) {
				count += product.ProductNumberInCart;	
			}

			ProductCountLabel.Text = count.ToString ();
		}

		private void PopulateSearch(BrowseProductsPage productPage)
		{
			if (productPage != null)
				ProductModel.PopulateSearchProductListWithCategoryID (mSearchString,productPage.mCategoryID);
			else
				ProductModel.PopulateSearchProductList (mSearchString);

			mRowCount = Convert.ToInt32 (Math.Ceiling (ProductModel.mSearchProductIDList.Count / 2.0f));
			PopulateProducts ();
			PopulateGrid ();
		}

		void PopulateProducts()
		{
			mProductCellList.Clear ();
			mProductList.Clear ();
			ProductGrid.Children.Clear ();
			foreach (string productID in ProductModel.mSearchProductIDList ) {
				string ImagePath = ProductModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + ProductModel.mProductImageNameDictionary [productID] + ".jpg";
				string ProductName = ProductModel.mProductNameDictionary [productID];
				decimal price = ProductModel.mProductPriceDictionary [productID];
				string quantity = ProductModel.mProductQuantityDictionary [productID];
				string parentCategory = ProductModel.mProductParentCategoryIDsDictionary [productID];
				mProductList.Add (new Product (productID, ProductName, ImagePath, price, parentCategory, quantity)); 
			}
		}

		private void PopulateGrid()
		{
			SetGrid2Definitions ();
			int counter = 0;
			for (int row = 0; row < mRowCount; row++) 
			{
				for (int col = 0; col < 2; col++) 
				{
					ProductCell productCell = new ProductCell (ProductGrid,mProductList[counter++],this );
					productCell.ProduceStreamsAndImages ();
					productCell.ProduceProductImages ();
					mProductCellList.Add (productCell);
					ProductGrid.Children.Add (productCell.View, col, row);

					if ( counter == ProductModel.mSearchProductIDList.Count)
						break;
				}
			}
		}

		private void SetGrid2Definitions()
		{
			//SubCategoryStackLayout.Spacing = MyDevice.ViewPadding;
			//SubcategoryScrollView.Padding = MyDevice.ViewPadding/2;
			/*for (int i = 0; i < mRowCount; i++) 
			{
				Grid2.RowDefinitions.Add (new RowDefinition ());
			}*/
			ProductGrid.Padding = new Thickness (MyDevice.GetScaledSize(12), 0, 0, 0); 
			ProductGrid.ColumnSpacing = MyDevice.GetScaledSize (0);
			/*ProductGrid.ColumnDefinitions.Add (new ColumnDefinition(){Width = (MyDevice.ScreenWidth-ProductGrid.ColumnSpacing-MyDevice.ViewPadding)/2});
			ProductGrid.ColumnDefinitions.Add (new ColumnDefinition(){Width = (MyDevice.ScreenWidth-ProductGrid.ColumnSpacing-MyDevice.ViewPadding)/2}); */
			ProductGrid.ColumnDefinitions.Add (new ColumnDefinition(){Width = MyDevice.ScreenWidth/2});
			ProductGrid.ColumnDefinitions.Add (new ColumnDefinition(){Width = MyDevice.ScreenWidth/2});
		}

		/*private void SetGrid1Definitions()
		{
			Grid1.RowDefinitions [0].Height = GridLength.Auto;
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
		}

		public void UpdatePriceLabel()
		{
			mParent.mPriceLabel.Text = Cart.ProductTotalPrice.ToString();
		}


		private void PopulateSearch(BrowseProductsPage productPage)
		{
			if (productPage != null)
				ProductModel.PopulateSearchProductListWithCategoryID (mSearchString,productPage.mCategoryID);
			else
				ProductModel.PopulateSearchProductList (mSearchString);
			
			mRowCount = Convert.ToInt32 (Math.Ceiling (ProductModel.mSearchProductIDList.Count / 2.0f));
			PopulateProducts ();
			//PopulateGrid ();
		}

		private void PopulateGrid()
		{
			SetGrid2Definitions ();
			int counter = 0;
			for (int row = 0; row < mRowCount; row++) 
			{
				for (int col = 0; col < 2; col++) 
				{
					ProductCell productCell = new ProductCell (Grid2,mProductList[counter++],this );
					productCell.ProduceStreamsAndImages ();
					productCell.ProduceProductImages ();
					mProductCellList.Add (productCell);
					Grid2.Children.Add (productCell.View, col, row);

					if ( counter == ProductModel.mSearchProductIDList.Count)
						break;
				}
			}
		}

		private void SetGrid2Definitions()
		{
			for (int i = 0; i < mRowCount; i++) 
			{
				Grid2.RowDefinitions.Add (new RowDefinition ());
			}

			Grid2.ColumnDefinitions.Add (new ColumnDefinition(){Width = (MyDevice.ScreenWidth-Grid2.ColumnSpacing)/2});
			Grid2.ColumnDefinitions.Add (new ColumnDefinition(){Width = (MyDevice.ScreenWidth-Grid2.ColumnSpacing)/2}); 
		}

		void PopulateProducts()
		{
			mProductCellList.Clear ();
			mProductList.Clear ();
			Grid2.Children.Clear ();
			foreach (string productID in ProductModel.mSearchProductIDList ) {
				string ImagePath = ProductModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + ProductModel.mProductImageNameDictionary [productID] + ".jpg";
				string ProductName = ProductModel.mProductNameDictionary [productID];
				decimal price = ProductModel.mProductPriceDictionary [productID];
				string quantity = ProductModel.mProductQuantityDictionary [productID];
				string parentCategory = ProductModel.mProductParentCategoryIDsDictionary [productID];
				mProductList.Add (new Product (productID, ProductName, ImagePath, price, parentCategory, quantity)); 
			}
		}*/
	}
}


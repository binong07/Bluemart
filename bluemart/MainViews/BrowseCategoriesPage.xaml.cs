using System;
using System.Collections.Generic;
using bluemart.Common.Objects;
using bluemart.Common.Utilities;
using bluemart.Common.ViewCells;
using Xamarin.Forms;
using bluemart.Models.Remote;
using XLabs.Forms.Controls;
using bluemart.Models.Local;
using System.Threading.Tasks;
using System.Linq;
using FFImageLoading.Forms;
using System.Collections.ObjectModel;


namespace bluemart.MainViews
{
	public partial class BrowseCategoriesPage : ContentPage
	{		
		public List<Category> mCategories;
		RootPage mParent;
		//public List<CategoryCell> mCategoryCellList = new List<CategoryCell>();

		private RelativeLayout InputBlocker;
		private RelativeLayout mTopLayout;
		private RelativeLayout mMenuLayout;
		private RelativeLayout mMidLayout;

		private double scrolly=0;
		//private ScrollView ScrollView1;
		//private StackLayout StackLayout1;
		private ListView listView;
		public ListView cartListView;
		private RelativeLayout mSearchLayout;
		private Entry SearchEntry;
		private Label PriceLabel;
		private Label ProductCountLabel;
		public bool IsMenuOpen = false;
		public bool IsCartOpen = false;
		private double mMenuWidth = 517.0;
		private Label categoriesLabel;
		public  Label userNameLabel;

		UserClass mUserModel = new UserClass();
		AddressClass mAddressModel = new AddressClass();
		private RelativeLayout mCartLayout;
		private double mCartWidth = 552.0;
	//	public StackLayout CartStackLayout;
		public Label subtotalPriceLabel;
		public Label checkoutPriceLabel;

		private RelativeLayout InputBlockerForSwipeMenu;
		private RelativeLayout InputBlockerForSwipeCart;

		public BrowseCategoriesPage (RootPage parent)
		{		
			InitializeComponent ();
			mParent = parent;
			CategoryModel.PopulateCategories ();
			mCategories = CategoryModel.CategoryList;
			NavigationPage.SetHasNavigationBar (this, false);
			InitializeLayout ();

		}

		private void InitializeLayout()
		{	
			mMidLayout = new RelativeLayout ()
			{
				Padding = 0
			};

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
				HeightRequest = MyDevice.ScreenHeight,
				Padding = 0
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

		private void InitializeMenuLayout()
		{
			mMenuLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(mMenuWidth),
				HeightRequest = MyDevice.ScreenHeight,
				BackgroundColor = Color.FromRgb(51,51,51),
				Padding = 0
			};

			var openImage = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(54),
				HeightRequest = MyDevice.GetScaledSize(44),
				Source = "MenuPage_Open.png",
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
				HeightRequest = MyDevice.GetScaledSize(50),
				Padding = 0
			};

			var firstLine = new BoxView (){
				HeightRequest = 1,
				WidthRequest = MyDevice.GetScaledSize(mMenuWidth),
				Color = Color.FromRgb(129,129,129)
			};

			var settingsImage = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(40),
				HeightRequest = MyDevice.GetScaledSize(35),
				Source = "MenuPage_Settings.png",
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
				HeightRequest = MyDevice.GetScaledSize(50),
				Padding = 0
			};

			var favoritesImage = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(40),
				HeightRequest = MyDevice.GetScaledSize(35),
				Source = "MenuPage_Favorites.png",
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
				HeightRequest = MyDevice.GetScaledSize(50),
				Padding = 0
			};

			var trackImage = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(40),
				HeightRequest = MyDevice.GetScaledSize(35),
				Source = "MenuPage_Track.png",
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

			for (int i = 0; i < mCategories.Count; i++) {
				if (!mCategories [i].IsSubCategory) {
					Label label = new Label () {
						WidthRequest = MyDevice.GetScaledSize(442),	
						HeightRequest = MyDevice.GetScaledSize(78),
						TextColor = Color.White,
						HorizontalTextAlignment = TextAlignment.Start,
						VerticalTextAlignment = TextAlignment.Center,
						Text = mCategories [i].Name,
						FontSize = MyDevice.FontSizeMedium
					};

					var tapRecog = new TapGestureRecognizer ();
					tapRecog.Tapped += async (sender, e) => {
						string categoryName = (sender as Label).Text;
						Category category = null;
						foreach(var tempCategory in mCategories)
						{
							if(tempCategory.Name == categoryName)
							{
								category = tempCategory;
							}
						}
						if(category.CategoryID == ReleaseConfig.TOBACCO_ID)
						{					
							var isOk = await mParent.DisplayAlert("Warning","I am over 20 years old and I know smoking is bad for my health.","AGREE","DISAGREE");
							if(isOk)
								MyDevice.rootPage.LoadCategory(category);										
						}else if(category.CategoryID == ReleaseConfig.FRUITS_ID||category.CategoryID == ReleaseConfig.MEAT_ID)
						{					
							await mParent.DisplayAlert("Please Remember","Delivered quantity might differ from the actual ordered quantity by ± 50 grams.","OK");

							MyDevice.rootPage.LoadCategory(category);										
						}
						else
							MyDevice.rootPage.LoadCategory(category);
						/*foreach(var categoryCell in mCategoryCellList)
						{
							if( category != null && categoryCell.mCategory == category )
							{								
								categoryCell.LoadProductsPage(category.CategoryID,mParent);
							}
						}*/

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

			mMenuLayout.Children.Add (favoritesButton,
				Constraint.Constant(0),
				Constraint.RelativeToView (favoritesImage, (parent,sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize(3);
				})
			);

			mMenuLayout.Children.Add (settingsButton,
				Constraint.Constant(0),
				Constraint.RelativeToView (settingsImage, (parent,sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize(3);
				})
			);


		}
						
		private void InitializeHeaderLayout()
		{			
			mTopLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.GetScaledSize(87),
				BackgroundColor = Color.FromRgb(7,94,173),
				Padding = 0
			};
					
			var menuIcon = new Image () {
				WidthRequest = MyDevice.GetScaledSize(36),
				HeightRequest = MyDevice.GetScaledSize(37),
				Source = "CategoriesPage_MenuIcon.png"
			};

			var exploreLabel = new Label (){ 
				Text = "Explore",
				TextColor = Color.White,
				FontSize = MyDevice.FontSizeLarge
			};

			PriceLabel = new Label () {
				TextColor = Color.White,
				FontSize = MyDevice.FontSizeSmall,
				HorizontalTextAlignment = TextAlignment.End,
				WidthRequest = MyDevice.GetScaledSize(130)
			};

			var verticalLine = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(1),
				HeightRequest = MyDevice.GetScaledSize(63),			
				Source = "CategoriesPage_VerticalLine.png",
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false
			};

			var cartImage = new Image () {
				WidthRequest = MyDevice.GetScaledSize(71),
				HeightRequest = MyDevice.GetScaledSize(57),
				Source = "CategoriesPage_BasketIcon.png"
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
				HeightRequest = MyDevice.GetScaledSize(86),
				Padding = 0
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
				Constraint.Constant(MyDevice.GetScaledSize(20)),
				Constraint.Constant(MyDevice.GetScaledSize(27))
				/*Constraint.RelativeToParent (parent => {
					return parent.Bounds.Left +  MyDevice.GetScaledSize(20);
				}),
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Top + MyDevice.GetScaledSize(27);
				})*/
			);

			mMidLayout.Children.Add (exploreLabel,
				Constraint.RelativeToView (menuIcon, (parent, sibling) => {
					return sibling.Bounds.Right + MyDevice.GetScaledSize (22);
				}),
				Constraint.RelativeToView (menuIcon, (parent, sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize (3);
				})
			);

			mMidLayout.Children.Add (cartImage, 
				Constraint.Constant( MyDevice.GetScaledSize(561) ),
				Constraint.Constant( MyDevice.GetScaledSize(16) )
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
					return sibling.Bounds.Left - MyDevice.GetScaledSize (150);
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
		}
		public bool isMenuAnimationWorking = false;
		public void ActivateOrDeactivateMenu()
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

			mMenuLayout.TranslateTo (menuRectangle.X,menuRectangle.Y, MyDevice.AnimationTimer, Easing.Linear).ContinueWith(antecendent => isMenuAnimationWorking=false) ;
			mMidLayout.TranslateTo (midRectangle.X,midRectangle.Y, MyDevice.AnimationTimer, Easing.Linear);
									
			IsMenuOpen = !IsMenuOpen;
		}

		public void ActivateOrDeactivateCart()
		{
			if (isMenuAnimationWorking)
				return;
			else
				isMenuAnimationWorking = true;
			Rectangle cartRectangle;
			Rectangle midRectangle;

			CartCellNew.deleteIsOpenDictionary = new Dictionary<string, bool> ();

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
				//ObservableCollection<Product> obser = new ObservableCollection<Product> (Cart.ProductsInCart);

				cartListView.ItemsSource = Cart.ProductsInCart;

				/*CartStackLayout.Children.Clear ();

				foreach (Product p in Cart.ProductsInCart) {
					var cartCell = new CartCell (p, this);
					//mCartCellList.Add (cartCell);
					CartStackLayout.Children.Add( cartCell.View );
				}*/
			} else {
				cartRectangle = new Rectangle (new Point (0, 0), new Size (mCartLayout.Bounds.Width, mCartLayout.Bounds.Height));
				midRectangle = new Rectangle (new Point (0, 0), new Size (mMidLayout.Bounds.Width, mMidLayout.Bounds.Height));
				mainRelativeLayout.Children.Remove (InputBlockerForSwipeCart);
			}

			mCartLayout.TranslateTo (cartRectangle.X,cartRectangle.Y, MyDevice.AnimationTimer, Easing.Linear).ContinueWith(antecendent => isMenuAnimationWorking=false) ;
			mMidLayout.TranslateTo (midRectangle.X,midRectangle.Y, MyDevice.AnimationTimer, Easing.Linear);

			IsCartOpen = !IsCartOpen;
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

		private void InitializeSearchLayout()
		{
			mSearchLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.GetScaledSize(73),
			BackgroundColor = Color.FromRgb(7,94,173),
				Padding = 0
			};

			SearchEntry = new Entry () {
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.GetScaledSize(73),
				Text = "Search",
				TextColor = Color.White,
				FontSize = MyDevice.FontSizeMedium,
				HorizontalTextAlignment = TextAlignment.Start,
				BackgroundColor = Color.Transparent,
			};
			var searchImage = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(530),
				HeightRequest = MyDevice.GetScaledSize(52),
				Source = "CategoriesPage_SearchBar.png",
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false
			};

			var searchButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(444),
				HeightRequest = MyDevice.GetScaledSize(51),
				Padding = 0
			};
					

			var deleteButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(69),
				HeightRequest = MyDevice.GetScaledSize(51),
				Padding = 0
			};



			var searchEntryTapRecognizer= new TapGestureRecognizer ();
			searchEntryTapRecognizer.Tapped += (sender, e) => {				
				SearchEntry.Focus();
			};
			searchButton.GestureRecognizers.Add(searchEntryTapRecognizer);

			var deleteButtonTapRecognizer= new TapGestureRecognizer ();
			deleteButtonTapRecognizer.Tapped += (sender, e) => {				
				//if( SearchEntry.Text.Length > 0 )
				SearchEntry.Text = "";
			};
			deleteButton.GestureRecognizers.Add(deleteButtonTapRecognizer);



			mMidLayout.Children.Add (mSearchLayout,
				Constraint.Constant(0),
				Constraint.RelativeToView (mTopLayout, (parent, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (1);
				})
			);

			mMidLayout.Children.Add (searchImage,
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize (76);
				}),
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize (10);
				})
			);

			mMidLayout.Children.Add (SearchEntry,
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize (136);
				}),
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize (5);
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

			mMidLayout.Children.Add (deleteButton,
				Constraint.RelativeToView (searchImage, (parent, sibling) => {
					return sibling.Bounds.Right - MyDevice.GetScaledSize (67);
				}),
				Constraint.RelativeToView (searchImage, (parent, sibling) => {
					return sibling.Bounds.Top;
				})
			);
		}

		private void InitializeBottomLayout()
		{			
			listView = new ListView(){RowHeight = (int)MyDevice.GetScaledSize(210)};
			List<Category> tempCategories = new List<Category> ();
			for (int i = 0; i < mCategories.Count; i++) {
				if (!mCategories [i].IsSubCategory) {
					tempCategories.Add (mCategories [i]);
				}
			}
			listView.ItemTemplate = new DataTemplate (typeof(CategoryCellNew));
			listView.ItemsSource = tempCategories;
			mMidLayout.Children.Add (listView,
				Constraint.Constant(0),
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Bottom;
				}),
				Constraint.Constant(MyDevice.GetScaledSize(630)),
				Constraint.Constant(MyDevice.ScreenHeight-MyDevice.GetScaledSize(87)-MyDevice.GetScaledSize(73)-MyDevice.GetScaledSize(1)-MyDevice.GetScaledSize(51))
			);
			/*
			StackLayout1 = new StackLayout {
				Orientation = StackOrientation.Vertical,
				Padding = 0,
				Spacing = 0
			};

			for (int i = 0; i < mCategories.Count; i++) {
				if (!mCategories [i].IsSubCategory) {
					CategoryCell categoryCell = new CategoryCell (StackLayout1,
						mCategories [i],
						mParent);
					mCategoryCellList.Add (categoryCell);
					StackLayout1.Children.Add (categoryCell.View);	
				}
			}

			ScrollView1 = new ScrollView {
				Orientation = ScrollOrientation.Vertical,
				Content = StackLayout1
			};
			ScrollView1.Scrolled+= (object sender, ScrolledEventArgs e) => {scrolly = ScrollView1.ScrollY;};
			mMidLayout.Children.Add (ScrollView1,
				Constraint.Constant(0),
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Bottom;
				}),
				Constraint.Constant(MyDevice.GetScaledSize(630)),
				Constraint.Constant(MyDevice.ScreenHeight-MyDevice.GetScaledSize(87)-MyDevice.GetScaledSize(73)-MyDevice.GetScaledSize(1)-MyDevice.GetScaledSize(51))
			);
*/
			/*mMidLayout.Children.Add (InputBlockerForSwipeMenus,
				Constraint.Constant (0),
				Constraint.Constant (0)
			);*/
		}

		private void InitializeCartLayout()
		{
			mCartLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(mCartWidth),
				HeightRequest = MyDevice.ScreenHeight,
				BackgroundColor = Color.FromRgb(51,51,51),
				Padding = 0
			};


			userNameLabel = new Label () {				
				
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
				Source = "CartPage_ProfilePic.png"
			};

			var firstLine = new BoxView (){
				HeightRequest = 1,
				WidthRequest = MyDevice.GetScaledSize(mCartWidth),
				Color = Color.FromRgb(129,129,129)
			};

			/*CartStackLayout = new StackLayout {
				Orientation = StackOrientation.Vertical,
				Padding = 0,
				Spacing = 0
			};	*/

			cartListView = new ListView()
			{
				RowHeight = (int)MyDevice.GetScaledSize(150)
			};
			cartListView.ItemTemplate = new DataTemplate (typeof(CartCellNew));

			/*
			var cartScrollView = new ScrollView {
				Orientation = ScrollOrientation.Vertical,
				Content = CartStackLayout
			};*/

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
				var hour = Time.GetTime();

				if( hour < 8 || hour > 23 ) 
				{
					string region = mUserModel.GetUser().ActiveRegion;
					int shopNumber = RegionHelper.DecideShopNumber (region);
					string shopName = RegionHelper.DecideShopName (shopNumber);
					await DisplayAlert("Sorry","Bluemart "+ shopName +" is closed between 11pm and 8am ","OK");
				}
				else if( Cart.ProductTotalPrice == 0 )
					await DisplayAlert("Sorry","Please add products to your basket","OK");
				else if( mAddressModel.GetActiveAddress(mUserModel.GetUser().ActiveRegion) == null )
				{					
					await DisplayAlert("Sorry","Please Enter Your Address On Settings Page","OK");
					mParent.LoadSettingsPage();
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

			mCartLayout.Children.Add (cartListView,
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

		private void EventHandlers()
		{			

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
		}

		public void RefreshViews()
		{			
			UserClass user = mUserModel.GetUser ();
			AddressClass activeAdress = mAddressModel.GetActiveAddress (user.ActiveRegion);
			string userName = "";
			if (activeAdress != null)
				userName = activeAdress.Name;
			userNameLabel.Text = userName.Split (' ') [0];
		}
		public void SetScrollPos()
		{
			MyDevice.currentPage = this;
			System.Diagnostics.Debug.WriteLine (scrolly);

			//ScrollView1.ScrollToAsync (0,scrolly,false);
		}
	}
}


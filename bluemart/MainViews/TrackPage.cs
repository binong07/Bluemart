using System;
using System.Collections.Generic;
using bluemart.Common.Objects;
using bluemart.Common.Utilities;
using bluemart.Common.ViewCells;
using Xamarin.Forms;
using bluemart.Models.Remote;
using bluemart.Models.Local;
using FFImageLoading.Forms;
using System.Threading.Tasks;

namespace bluemart.MainViews
{
	public partial class TrackPage : ContentPage
	{				
		public RootPage mParent;
		public TrackCell mActiveTrackCell;
		private Label TrackLabel;
		private Label HistoryLabel;
		private Label mActiveLabel;

		private ScrollView ScrollView1;
		private StackLayout StackLayout1;

		private RelativeLayout mTopLayout;
		private RelativeLayout mMenuLayout;
		private RelativeLayout mMidLayout;
		private RelativeLayout mSwitchLayout;
		private RelativeLayout mBottomLayout;
		private Label categoriesLabel;
		private Image menuIcon;
		private bool IsMenuOpen = false;
		private double mMenuWidth = 517.0;

		private List<StatusClass> mOrderStatusList;
		private List<HistoryClass> mOrderHistoryList;

		private RelativeLayout InputBlockerForSwipeMenu;
		private bool IsRefreshing = false;

		public TrackPage(RootPage parent)
		{
			InitializeComponent ();
			mParent = parent;
			NavigationPage.SetHasNavigationBar (this, false);
			InitializeLayout ();
		}

		public void PopulateTrackPage()
		{			
			mOrderStatusList = OrderModel.GetOrdersForTracking ();
			mOrderHistoryList = OrderModel.GetOrdersForHistory ();
			InitializeBottomLayout ();
		}			

		private void Refresh()
		{				
			mOrderStatusList = OrderModel.GetOrdersForTracking ();
			mOrderHistoryList = OrderModel.GetOrdersForHistory ();
			double scrollViewHeight = 0;
			if (mActiveLabel == TrackLabel) {
				Device.BeginInvokeOnMainThread (() => {
					HistoryLabel.BackgroundColor = Color.Transparent;
					TrackLabel.BackgroundColor = Color.FromRgb (76, 76, 76);

					StackLayout1.Children.Clear ();
				});
				int statusTransitionCount = 0;
				foreach (var status in mOrderStatusList) {
					StackLayout1.Children.Add( new TrackCell(status,this ).View );
					if (status.OrderStatus == OrderModel.OrderStatus.IN_TRANSIT)
						statusTransitionCount++;
				}

				double normalHeight = 180;
				double inTransitionHeight = 250;

				double scrollViewNormalHeight = MyDevice.GetScaledSize (normalHeight) * (StackLayout1.Children.Count - statusTransitionCount);
				double scrollViewInTransitionHeight = MyDevice.GetScaledSize (inTransitionHeight) * (statusTransitionCount);
				scrollViewHeight = scrollViewNormalHeight + scrollViewInTransitionHeight  -  StackLayout1.Spacing*(StackLayout1.Children.Count - 1);
				double screenLimit = MyDevice.GetScaledSize (800);
				if (scrollViewHeight > screenLimit) {
					double cellCount = Math.Floor (MyDevice.GetScaledSize (916) / MyDevice.GetScaledSize (180));
					scrollViewHeight = MyDevice.ScreenHeight - MyDevice.GetScaledSize (181) - StackLayout1.Spacing * (cellCount - 1);
					Device.BeginInvokeOnMainThread(() => {
						mMidLayout.Children.Add (ScrollView1,
							Constraint.RelativeToView (mBottomLayout, (parent, sibling) => {
								return sibling.Bounds.Left;
							}),
							Constraint.RelativeToView (mBottomLayout, (parent, sibling) => {
								return sibling.Bounds.Top;
							}),
							Constraint.Constant(MyDevice.GetScaledSize(600)),
							Constraint.Constant(scrollViewHeight));
					});
				} else {
					Device.BeginInvokeOnMainThread(() => {
						mMidLayout.Children.Add (ScrollView1,
							Constraint.RelativeToView (mBottomLayout, (parent, sibling) => {
								return sibling.Bounds.Left;
							}),
							Constraint.RelativeToView (mBottomLayout, (parent, sibling) => {
								return sibling.Bounds.Top;
							}),
							Constraint.Constant(MyDevice.GetScaledSize(600))/*,
				Constraint.Constant(scrollViewHeight)*/);
					});

				}

			} else if (mActiveLabel == HistoryLabel) {
				Device.BeginInvokeOnMainThread (() => {
					TrackLabel.BackgroundColor = Color.Transparent;
					HistoryLabel.BackgroundColor = Color.FromRgb (76, 76, 76);
					StackLayout1.Children.Clear ();

					foreach (var history in mOrderHistoryList) {
						StackLayout1.Children.Add( new HistoryCell(history,this ).View );
					}
				});


				scrollViewHeight = MyDevice.GetScaledSize (138)*(StackLayout1.Children.Count) +  StackLayout1.Spacing*(StackLayout1.Children.Count - 1);


				if (scrollViewHeight > MyDevice.GetScaledSize (916)) {
					double cellCount = Math.Floor(MyDevice.GetScaledSize (916) / MyDevice.GetScaledSize (138));
					scrollViewHeight = MyDevice.ScreenHeight - MyDevice.GetScaledSize (181) - StackLayout1.Spacing * (cellCount - 1);
				}
				Device.BeginInvokeOnMainThread (() => {
					if (mMidLayout.Children.Contains (ScrollView1))
						mMidLayout.Children.Remove (ScrollView1);

					mMidLayout.Children.Add (ScrollView1,
						Constraint.RelativeToView (mBottomLayout, (parent, sibling) => {
							return sibling.Bounds.Left;
						}),
						Constraint.RelativeToView (mBottomLayout, (parent, sibling) => {
							return sibling.Bounds.Top;
						}),
						Constraint.Constant (MyDevice.GetScaledSize (600)),
						Constraint.Constant (scrollViewHeight)
					);
				});
			}		
		}

		private void ChangeActive(Label label)
		{
			double scrollViewHeight = 0;

			if (label != mActiveLabel) {
				if (label == TrackLabel) {
					Device.BeginInvokeOnMainThread (() => {
						HistoryLabel.BackgroundColor = Color.Transparent;
						TrackLabel.BackgroundColor = Color.FromRgb (76, 76, 76);
						StackLayout1.Children.Clear ();
					});
					int statusTransitionCount = 0;
					foreach (var status in mOrderStatusList) {
						StackLayout1.Children.Add( new TrackCell(status,this ).View );
						if (status.OrderStatus == OrderModel.OrderStatus.IN_TRANSIT)
							statusTransitionCount++;
					}

					double normalHeight = 180;
					double inTransitionHeight = 250;

					double scrollViewNormalHeight = MyDevice.GetScaledSize (normalHeight) * (StackLayout1.Children.Count - statusTransitionCount);
					double scrollViewInTransitionHeight = MyDevice.GetScaledSize (inTransitionHeight) * (statusTransitionCount);
					scrollViewHeight = scrollViewNormalHeight + scrollViewInTransitionHeight  -  StackLayout1.Spacing*(StackLayout1.Children.Count - 1);
					double screenLimit = MyDevice.GetScaledSize (800);
					if (scrollViewHeight > screenLimit) {
						double cellCount = Math.Floor (MyDevice.GetScaledSize (916) / MyDevice.GetScaledSize (180));
						scrollViewHeight = MyDevice.ScreenHeight - MyDevice.GetScaledSize (181) - StackLayout1.Spacing * (cellCount - 1);
						Device.BeginInvokeOnMainThread(() => {
							mMidLayout.Children.Add (ScrollView1,
								Constraint.RelativeToView (mBottomLayout, (parent, sibling) => {
									return sibling.Bounds.Left;
								}),
								Constraint.RelativeToView (mBottomLayout, (parent, sibling) => {
									return sibling.Bounds.Top;
								}),
								Constraint.Constant(MyDevice.GetScaledSize(600)),
								Constraint.Constant(scrollViewHeight));
						});
					} else {
						Device.BeginInvokeOnMainThread(() => {
							mMidLayout.Children.Add (ScrollView1,
								Constraint.RelativeToView (mBottomLayout, (parent, sibling) => {
									return sibling.Bounds.Left;
								}),
								Constraint.RelativeToView (mBottomLayout, (parent, sibling) => {
									return sibling.Bounds.Top;
								}),
								Constraint.Constant(MyDevice.GetScaledSize(600))/*,
					Constraint.Constant(scrollViewHeight)*/);
						});

					}

				} else if (label == HistoryLabel) {
					Device.BeginInvokeOnMainThread (() => {
						TrackLabel.BackgroundColor = Color.Transparent;
						HistoryLabel.BackgroundColor = Color.FromRgb (76, 76, 76);

						StackLayout1.Children.Clear ();
						foreach (var history in mOrderHistoryList) {
							StackLayout1.Children.Add (new HistoryCell (history, this).View);
						}
					});
					scrollViewHeight = MyDevice.GetScaledSize (138)*(StackLayout1.Children.Count) +  StackLayout1.Spacing*(StackLayout1.Children.Count - 1);


					if (scrollViewHeight > MyDevice.GetScaledSize (916)) {
						double cellCount = Math.Floor(MyDevice.GetScaledSize (916) / MyDevice.GetScaledSize (138));
						scrollViewHeight = MyDevice.ScreenHeight - MyDevice.GetScaledSize (181) - StackLayout1.Spacing * (cellCount - 1);
					}
					Device.BeginInvokeOnMainThread (() => {
						if (mMidLayout.Children.Contains (ScrollView1))
							mMidLayout.Children.Remove (ScrollView1);
					
						mMidLayout.Children.Add (ScrollView1,
							Constraint.RelativeToView (mBottomLayout, (parent, sibling) => {
								return sibling.Bounds.Left;
							}),
							Constraint.RelativeToView (mBottomLayout, (parent, sibling) => {
								return sibling.Bounds.Top;
							}),
							Constraint.Constant (MyDevice.GetScaledSize (600)),
							Constraint.Constant (scrollViewHeight)
						);
					});
				}
			}	



			mActiveLabel = label;
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
			InitializeSwitchLayout ();

			/*InitializeAddressLayout ();
			InitializeReceiptLayout ();*/
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

		private void InitializeHeaderLayout ()
		{
			mTopLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.GetScaledSize(87),
				BackgroundColor = Color.White//Color.FromRgb(38,153,200)
			};

			menuIcon = new Image () {
				WidthRequest = MyDevice.GetScaledSize(36),
				HeightRequest = MyDevice.GetScaledSize(37),
				Source = "ReceiptPage_MenuIcon.png"
			};

			var logo = new Image () {
				WidthRequest = MyDevice.GetScaledSize(217),
				HeightRequest = MyDevice.GetScaledSize(39),
				Source = "ReceiptPage_Logo.png"
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

			var refreshImage = new Image () {
				WidthRequest = MyDevice.GetScaledSize(50),
				HeightRequest = MyDevice.GetScaledSize(50),
				Aspect = Aspect.Fill,
				Source = "refresh.png"
			};

			var refreshButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(90),
				HeightRequest = MyDevice.GetScaledSize(90),
				Padding = 0,
				//BackgroundColor = Color.Black
			};
			var refreshTapRecognizer= new TapGestureRecognizer ();

			refreshTapRecognizer.Tapped +=async (sender, e) => {
				if(IsRefreshing)
					return;
				IsRefreshing = true;
				await Task.Factory.StartNew (() => Refresh()
					, TaskCreationOptions.None
				);
				IsRefreshing = false;
			};
			refreshButton.GestureRecognizers.Add(refreshTapRecognizer);


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


			mMidLayout.Children.Add (refreshButton,
				Constraint.Constant(MyDevice.GetScaledSize(550)),
				Constraint.Constant (0)
			);
			mMidLayout.Children.Add (refreshImage, 
				Constraint.Constant( MyDevice.GetScaledSize(570) ),
				Constraint.Constant( MyDevice.GetScaledSize(20) )
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

		private void InitializeSwitchLayout ()
		{
			mSwitchLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.GetScaledSize(74),
				BackgroundColor = Color.Black
			};

			TrackLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (320),
				HeightRequest = MyDevice.GetScaledSize (74),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
				Text = "TRACK",
				FontSize = MyDevice.FontSizeMedium,
				BackgroundColor = Color.FromRgb (76, 76, 76)
			};

			HistoryLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (320),
				HeightRequest = MyDevice.GetScaledSize (74),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
				Text = "HISTORY",
				FontSize = MyDevice.FontSizeMedium
			};

			var trackMaskImage = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(73),
				HeightRequest = MyDevice.GetScaledSize(73),
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false,
				Source = "TrackPage_TrackMask"
			};

			var trackTapRecognizer = new TapGestureRecognizer ();
			trackTapRecognizer.Tapped +=async (sender, e) => {
				if(IsRefreshing)
					return;
				IsRefreshing = true;
				await Task.Factory.StartNew (() => ChangeActive(TrackLabel)
					, TaskCreationOptions.None
				);
				IsRefreshing = false;
			};
			TrackLabel.GestureRecognizers.Add (trackTapRecognizer);

			var historyTapRecognizer = new TapGestureRecognizer ();
			historyTapRecognizer.Tapped +=async (sender, e) => {
				if(IsRefreshing)
					return;
				IsRefreshing = true;
				await Task.Factory.StartNew (() => ChangeActive(HistoryLabel)
					, TaskCreationOptions.None
				);
				IsRefreshing = false;
			};
			HistoryLabel.GestureRecognizers.Add (historyTapRecognizer);

			mMidLayout.Children.Add (mSwitchLayout,
				Constraint.Constant (0),
				Constraint.Constant (MyDevice.GetScaledSize(87))
			);

			mSwitchLayout.Children.Add (TrackLabel,
				Constraint.Constant (0),
				Constraint.Constant (0)
			);

			mSwitchLayout.Children.Add (HistoryLabel,
				Constraint.RelativeToView (TrackLabel, (p, sibling) => {
					return sibling.Bounds.Right;
				}),
				Constraint.Constant (0)
			);

			mSwitchLayout.Children.Add (trackMaskImage,
				Constraint.RelativeToView (TrackLabel, (p, sibling) => {
					return sibling.Bounds.Right - MyDevice.GetScaledSize(37);
				}),
				Constraint.Constant (0)
			);

			mActiveLabel = TrackLabel;
		}

		private void InitializeBottomLayout()
		{	
			mBottomLayout = new RelativeLayout () {
				BackgroundColor = Color.FromRgb(236,240,241),
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.ScreenHeight - MyDevice.GetScaledSize(182) 
			};
			StackLayout1 = new StackLayout {
				Orientation = StackOrientation.Vertical,
				Spacing = MyDevice.GetScaledSize(20),

			};

			StackLayout1.Children.Clear ();
			int statusTransitionCount = 0;
			foreach (var status in mOrderStatusList) {
				StackLayout1.Children.Add( new TrackCell(status,this ).View );
				if (status.OrderStatus == OrderModel.OrderStatus.IN_TRANSIT)
					statusTransitionCount++;
			}

			ScrollView1 = new ScrollView {
				Orientation = ScrollOrientation.Vertical,
				Content = StackLayout1
			};
			Device.BeginInvokeOnMainThread(() => {
				mMidLayout.Children.Add (mBottomLayout,
					Constraint.RelativeToView (mSwitchLayout, (p, sibling) => {
						return mSwitchLayout.Bounds.Left + MyDevice.GetScaledSize(20);		
					}),
					Constraint.RelativeToView (mSwitchLayout, (p, sibling) => {
						return mSwitchLayout.Bounds.Bottom + MyDevice.GetScaledSize(20);		
					})
				);
			});



			double normalHeight = 180;
			double inTransitionHeight = 250;

			double scrollViewNormalHeight = MyDevice.GetScaledSize (normalHeight) * (StackLayout1.Children.Count - statusTransitionCount);
			double scrollViewInTransitionHeight = MyDevice.GetScaledSize (inTransitionHeight) * (statusTransitionCount);
			double scrollViewHeight = scrollViewNormalHeight + scrollViewInTransitionHeight  -  StackLayout1.Spacing*(StackLayout1.Children.Count - 1);
			double screenLimit = MyDevice.GetScaledSize (800);
			if (scrollViewHeight > screenLimit) {
				double cellCount = Math.Floor (MyDevice.GetScaledSize (916) / MyDevice.GetScaledSize (180));
				scrollViewHeight = MyDevice.ScreenHeight - MyDevice.GetScaledSize (181) - StackLayout1.Spacing * (cellCount - 1);
				Device.BeginInvokeOnMainThread(() => {
					mMidLayout.Children.Add (ScrollView1,
						Constraint.RelativeToView (mBottomLayout, (parent, sibling) => {
							return sibling.Bounds.Left;
						}),
						Constraint.RelativeToView (mBottomLayout, (parent, sibling) => {
							return sibling.Bounds.Top;
						}),
						Constraint.Constant(MyDevice.GetScaledSize(600)),
						Constraint.Constant(scrollViewHeight));
				});
			} else {
				Device.BeginInvokeOnMainThread(() => {
					mMidLayout.Children.Add (ScrollView1,
						Constraint.RelativeToView (mBottomLayout, (parent, sibling) => {
							return sibling.Bounds.Left;
						}),
						Constraint.RelativeToView (mBottomLayout, (parent, sibling) => {
							return sibling.Bounds.Top;
						}),
						Constraint.Constant(MyDevice.GetScaledSize(600))/*,
					Constraint.Constant(scrollViewHeight)*/);
				});
				
			}


		}




			/*mTrackLabel = new Label () {
				HorizontalOptions = LayoutOptions.Center,
				TextColor = Color.White,
				Text = "TRACK",
				FontSize = MyDevice.FontSizeMedium
			};

			mHistoryLabel = new Label () {
				HorizontalOptions = LayoutOptions.Center,
				TextColor = Color.White,
				Text = "HISTORY",
				FontSize = MyDevice.FontSizeMedium
			};

			
			MainStackLayout.Spacing = MyDevice.ViewPadding;
			SetGrid1Definitions ();/
		}

		public void PopulateListView()
		{						
			MainStackLayout.Children.Clear ();

			MainStackLayout.Children.Add (mTrackLabel);
			var orderStatusList = OrderModel.GetOrdersForTracking ();
			foreach (var status in orderStatusList) {
				MainStackLayout.Children.Add( new TrackCell(status,this ).View );
			}

			MainStackLayout.Children.Add (mHistoryLabel);

			var orderHistoryList = OrderModel.GetOrdersForHistory ();
			foreach (var history in orderHistoryList) {
				MainStackLayout.Children.Add( new HistoryCell(history,this ).View );
			}
		}

		private void SetGrid1Definitions()
		{
			Grid1.RowDefinitions [0].Height = GridLength.Auto;
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
			Grid1.BackgroundColor = MyDevice.BackgroundColor;
		}*/
			
	}
}


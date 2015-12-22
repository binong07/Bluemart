using System;
using System.Collections.Generic;
using bluemart.Common.Objects;
using bluemart.Common.Utilities;
using bluemart.Common.ViewCells;
using Xamarin.Forms;
using bluemart.Models.Remote;
using XLabs.Forms.Controls;

namespace bluemart.MainViews
{
	public partial class BrowseCategoriesPage : ContentPage
	{		
		List<Category> mCategories;
		RootPage mParent;
		private List<CategoryCell> mCategoryCellList = new List<CategoryCell>();

		private RelativeLayout InputBlocker;
		private RelativeLayout mTopLayout;
		private ScrollView ScrollView1;
		private StackLayout StackLayout1;
		private RelativeLayout mSearchLayout;
		private ExtendedEntry SearchEntry;
		private Label SearchLabel;

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
			mainRelativeLayout.BackgroundColor = Color.FromRgb (236, 240, 241);

			InputBlocker = new RelativeLayout () {
				WidthRequest = MyDevice.ScreenWidth,
				HeightRequest = MyDevice.ScreenHeight
			};

			var inputBlockerTapRecogniser = new TapGestureRecognizer ();
			inputBlockerTapRecogniser.Tapped += (sender, e) => {				
				SearchEntry.Unfocus();
			};
			InputBlocker.GestureRecognizers.Add(inputBlockerTapRecogniser);


			InitializeHeaderLayout ();
			InitializeSearchLayout ();
			InitializeBottomLayout ();
			EventHandlers ();		
		}
						
		private void InitializeHeaderLayout()
		{			
			mTopLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.GetScaledSize(87),
				BackgroundColor = Color.FromRgb(246,182,58)
			};

			var menuIcon = new Image () {
				WidthRequest = MyDevice.GetScaledSize(36),
				HeightRequest = MyDevice.GetScaledSize(37),
				Source = "CategoriesPage_MenuIcon"
			};

			var exploreLabel = new Label (){ 
				Text = "Explore",
				TextColor = Color.White,
				FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label))
			};

			var priceLabel = new Label () {
				Text = "0\nAED",	
				TextColor = Color.White,
				FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
				HorizontalTextAlignment = TextAlignment.Center
			};

			var verticalLine = new Image () {
				WidthRequest = MyDevice.GetScaledSize(1),
				HeightRequest = MyDevice.GetScaledSize(63),
				Aspect = Aspect.Fill,
				Source = "CategoriesPage_VerticalLine"
			};

			var cartImage = new Image () {
				WidthRequest = MyDevice.GetScaledSize(71),
				HeightRequest = MyDevice.GetScaledSize(57),
				Aspect = Aspect.Fill,
				Source = "CategoriesPage_BasketIcon"
			};

			var productCount = new Label () {
				Text = "15",	
				TextColor = Color.White,
				FontSize = Device.GetNamedSize(NamedSize.Micro,typeof(Label)),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				WidthRequest = MyDevice.GetScaledSize(37),
				HeightRequest = MyDevice.GetScaledSize(27)
			};

			mainRelativeLayout.Children.Add (mTopLayout,
				Constraint.Constant (0),
				Constraint.Constant (0)
			);

			mainRelativeLayout.Children.Add (menuIcon, 
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Left +  MyDevice.GetScaledSize(20);
				}),
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Top + MyDevice.GetScaledSize(27);
				})
			);

			mainRelativeLayout.Children.Add (exploreLabel,
				Constraint.RelativeToView (menuIcon, (parent, sibling) => {
					return sibling.Bounds.Right + MyDevice.GetScaledSize (22);
				}),
				Constraint.RelativeToView (menuIcon, (parent, sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize (3);
				})
			);

			mainRelativeLayout.Children.Add (cartImage, 
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Right -  MyDevice.GetScaledSize(79);
				}),
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Top + MyDevice.GetScaledSize(16);
				})
			);

			mainRelativeLayout.Children.Add (verticalLine,
				Constraint.RelativeToView (cartImage, (parent, sibling) => {
					return sibling.Bounds.Left - MyDevice.GetScaledSize (14);
				}),
				Constraint.RelativeToView (cartImage, (parent, sibling) => {
					return sibling.Bounds.Top - MyDevice.GetScaledSize (5);
				})
			);

			mainRelativeLayout.Children.Add (priceLabel,
				Constraint.RelativeToView (verticalLine, (parent, sibling) => {
					return sibling.Bounds.Left - MyDevice.GetScaledSize (75);
				}),
				Constraint.RelativeToView (cartImage, (parent, sibling) => {
					return sibling.Bounds.Top;
				})
			);

			mainRelativeLayout.Children.Add (productCount,
				Constraint.RelativeToView (cartImage, (parent, sibling) => {
					return sibling.Bounds.Right - MyDevice.GetScaledSize (37);
				}),
				Constraint.RelativeToView (cartImage, (parent, sibling) => {
					return sibling.Bounds.Bottom - MyDevice.GetScaledSize (27);
				})
			);
		}

		private void InitializeSearchLayout()
		{
			mSearchLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.GetScaledSize(73),
				BackgroundColor = Color.FromRgb(246,182,58)
			};

			SearchEntry = new ExtendedEntry () {
				WidthRequest = MyDevice.GetScaledSize(640),
				HeightRequest = MyDevice.GetScaledSize(73),
				Text = "Search",
				MaxLength = 15
			};

			var searchImage = new Image () {
				WidthRequest = MyDevice.GetScaledSize(530),
				HeightRequest = MyDevice.GetScaledSize(52),
				Source = "CategoriesPage_SearchBar"	
			};

			var searchButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(444),
				HeightRequest = MyDevice.GetScaledSize(51)
			};

			SearchLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize(444),
				HeightRequest = MyDevice.GetScaledSize(51),
				TextColor = Color.White,
				FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label)),
				Text = "Search",
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center
			};

			var deleteButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(69),
				HeightRequest = MyDevice.GetScaledSize(51)
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

			mainRelativeLayout.Children.Add (SearchEntry,
				Constraint.Constant(0),
				Constraint.RelativeToView (mTopLayout, (parent, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (1);
				})
			);

			mainRelativeLayout.Children.Add (mSearchLayout,
				Constraint.Constant(0),
				Constraint.RelativeToView (mTopLayout, (parent, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (1);
				})
			);

			mainRelativeLayout.Children.Add (searchImage,
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize (76);
				}),
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize (10);
				})
			);

			mainRelativeLayout.Children.Add (searchButton,
				Constraint.RelativeToView (searchImage, (parent, sibling) => {
					return sibling.Bounds.Left;
				}),
				Constraint.RelativeToView (searchImage, (parent, sibling) => {
					return sibling.Bounds.Top;
				})
			);

			mainRelativeLayout.Children.Add (SearchLabel,
				Constraint.RelativeToView (searchButton, (parent, sibling) => {
					return sibling.Bounds.Left+ MyDevice.GetScaledSize(69);
				}),
				Constraint.RelativeToView (searchButton, (parent, sibling) => {
					return sibling.Bounds.Top;
				})
			);

			mainRelativeLayout.Children.Add (deleteButton,
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

			mainRelativeLayout.Children.Add (ScrollView1,
				Constraint.Constant(0),
				Constraint.RelativeToView (mSearchLayout, (parent, sibling) => {
					return sibling.Bounds.Bottom;
				}),
				Constraint.Constant(MyDevice.GetScaledSize(630)),
				Constraint.Constant(MyDevice.ScreenHeight-MyDevice.GetScaledSize(87)-MyDevice.GetScaledSize(73)-MyDevice.GetScaledSize(1)-MyDevice.GetScaledSize(51))
			);

		}

		private void EventHandlers()
		{
			SearchEntry.PropertyChanged += (sender, e) => {
				SearchLabel.Text = SearchEntry.Text;
			};

			SearchEntry.Focused += (sender, e) => {
				SearchEntry.Text = "";
				mainRelativeLayout.Children.Add( InputBlocker,
					Constraint.Constant(0),
					Constraint.Constant(0)
				);
			};

			SearchEntry.Unfocused += (sender, e) => {
				if( SearchEntry.Text == "" )
					SearchEntry.Text = "Search";
				mainRelativeLayout.Children.Remove(InputBlocker);
			};

			SearchEntry.Completed += (sender, e) => {
				if (SearchEntry.Text.Length >= 3) {				
					mParent.LoadSearchPage (SearchEntry.Text);
				} else {				
					SearchEntry.Text = "Must be longer than 2 characters!";
				}
				mainRelativeLayout.Children.Remove(InputBlocker);
			};
		}


		public void RefreshSearchText()
		{
			//mParent.mRootHeader.mSearchEntry.Text = "Search Products";
		}
	
	}
}


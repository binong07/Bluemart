using System;
using System.Collections.Generic;
using bluemart.Common.Objects;
using bluemart.Common.Utilities;
using bluemart.MainViews;
using Xamarin.Forms;
using System.Threading.Tasks;
using Parse;
using PCLStorage;
using System.Net.Http;
using XLabs.Forms.Controls;
using bluemart.Models.Remote;
using bluemart.Models.Local;
using bluemart.Common.ViewCells;
using System.Linq;

namespace bluemart.MainViews
{
	public partial class MainPage : ContentPage
	{				
		UserClass mUserModel = new UserClass ();
		StackLayout mPopupLayout;
		ListView mPopupListView = new ListView ();	
		List<RegionClass> mRegions = new List<RegionClass> ();
		Grid mConfirmationGrid;
		Button mOKButton;
		Button mCancelButton;
		RootPage mRootPage = new RootPage ();

		private RelativeLayout MainRelativeLayout;
		private bool bIsPopuplayoutInitialized = false;

		public MainPage ()
		{			
			NavigationPage.SetHasNavigationBar (this, false);

			if( mUserModel.GetActiveRegionFromUser() != "" )
				Navigation.PushAsync( mRootPage );
			InitRelativeLayout ();
			InitializeComponent ();
			Content = MainRelativeLayout;
		}

		private void InitRelativeLayout()
		{		
			MainRelativeLayout = new RelativeLayout () {
				Padding = 0
			};
			#region InitializeViews
			var BackgroundImage = new Image {
				Source = "MainPage_BG",
				WidthRequest = MyDevice.GetScaledSize(640),
				Aspect = Aspect.Fill
			};

			var linkButton = new Image {
				Source = "MainPage_Footer",	
				WidthRequest = MyDevice.GetScaledSize(471),
				HeightRequest = MyDevice.GetScaledSize(54)
			};

			var ChooseLocationButton = new RelativeLayout () {
				Padding = 0
			};

			var ChooseMapButton = new RelativeLayout () {
				Padding = 0
			};
			#endregion

			#region TapRecognizers
			var chooseYourLocationTapRecognizer= new TapGestureRecognizer ();
			chooseYourLocationTapRecognizer.Tapped += (sender, e) => {				
				PopulatePopup();
			};
			ChooseLocationButton.GestureRecognizers.Add(chooseYourLocationTapRecognizer);

			var chooseFromMapTapRecognizer= new TapGestureRecognizer ();
			chooseFromMapTapRecognizer.Tapped += (sender, e) => {
				Navigation.PushAsync( new MapView(mRootPage,mUserModel));
			};
			ChooseMapButton.GestureRecognizers.Add (chooseFromMapTapRecognizer);

			var linkTapRecognizer= new TapGestureRecognizer ();
			linkTapRecognizer.Tapped += (sender, e) => {				
				Device.OpenUri(new Uri("http://google.com"));
			};
			linkButton.GestureRecognizers.Add (linkTapRecognizer);
			#endregion

			#region AddViews
			MainRelativeLayout.Children.Add (BackgroundImage, 
				Constraint.Constant (0),
				Constraint.Constant (0)
			);

			MainRelativeLayout.Children.Add (linkButton,
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Left +  MyDevice.GetScaledSize(83);
				}),
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Bottom - MyDevice.GetScaledSize(70);
				})
			);

			MainRelativeLayout.Children.Add (ChooseLocationButton,
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Left;
				}),
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Top + MyDevice.GetScaledSize(278);
				}),
				Constraint.Constant(MyDevice.GetScaledSize(307)),
				Constraint.Constant(MyDevice.GetScaledSize(239))					
			);

			MainRelativeLayout.Children.Add (ChooseMapButton,
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Left + + MyDevice.GetScaledSize(314);
				}),
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Top + MyDevice.GetScaledSize(278);
				}),
				Constraint.Constant(MyDevice.GetScaledSize(326)),
				Constraint.Constant(MyDevice.GetScaledSize(239))					
			);
			#endregion
		}

		protected override bool OnBackButtonPressed ()
		{
			if (Navigation.NavigationStack.Last<Page> () is RootPage) {	

				//Check if product page is active
				//if( mRootPage.mTopNavigationBar.IsVisible == true ){	
				if (mRootPage.mCurrentPage != "BrowseCategories")
					mRootPage.SwitchTab ("BrowseCategories");
				else
					//Application.Current.MainPage = new NavigationPage (new MainPage ());
					Navigation.PopAsync ();
				//}

				//return true;	
			}

			if (bIsPopuplayoutInitialized && mPopupLayout.IsVisible) {
				DismissPopup();

			}
			return true;
			//return base.OnBackButtonPressed ();
		}

		private void PopulatePopup()
		{
			//MainRelativeLayout.BackgroundColor = MyDevice.BlueColor;
			MainRelativeLayout.BackgroundColor = Color.FromRgba ( MyDevice.BlueColor.R, MyDevice.BlueColor.G, MyDevice.BlueColor.B,0.5f);
			mRegions.Clear ();
			mPopupListView.WidthRequest = MyDevice.GetScaledSize(360);
			mPopupListView.SeparatorVisibility = SeparatorVisibility.None;
			mPopupListView.SeparatorColor = Color.Transparent;
			var cell = new DataTemplate (typeof(RegionCell));

			foreach (var region in RegionHelper.locationList) {
				mRegions.Add (new RegionClass (region));
			}

			mPopupListView.ItemTemplate = cell;
			mPopupListView.ItemsSource = mRegions;

			PopulateConfirmationGrid ();

			mPopupLayout = new StackLayout {
				WidthRequest = MyDevice.GetScaledSize (360),
				BackgroundColor = Color.White,
				Orientation = StackOrientation.Vertical,
				Children =
				{
					mPopupListView,
					mConfirmationGrid
				}
			};
			
			if(  mUserModel.GetActiveRegionFromUser () != "" )
			{
				string region = mUserModel.GetActiveRegionFromUser ();
				foreach (var item in mPopupListView.ItemsSource) {
					if ((item as RegionClass).Region == region)
						mPopupListView.SelectedItem = item;
				}
			}

			MainRelativeLayout.Children.Add (mPopupLayout,
				Constraint.Constant (MyDevice.GetScaledSize(140)),
				Constraint.Constant (MyDevice.GetScaledSize(100))
			);
			bIsPopuplayoutInitialized = true;	
		}

		private void PopulateConfirmationGrid()
		{
			mConfirmationGrid = new Grid()
			{
				Padding = new Thickness(0,0,0,MyDevice.ViewPadding/2),
				ColumnSpacing = MyDevice.ViewPadding,
				RowSpacing = 0,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				RowDefinitions = 
				{
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = MyDevice.GetScaledSize(180) - MyDevice.ViewPadding},
					new ColumnDefinition { Width = MyDevice.GetScaledSize(180) - MyDevice.ViewPadding},
				}
				};

			mOKButton = new Button () {
				Text = "OK",
				BackgroundColor = Color.White,
				TextColor = MyDevice.RedColor,
				BorderColor = MyDevice.BlueColor,
				BorderWidth = 2
			};

			mOKButton.Clicked += (sender, e) => {
				if(mPopupListView.SelectedItem != null )
				{
					DismissPopup();
					string region = (mPopupListView.SelectedItem as RegionClass).Region;
					mUserModel.AddActiveRegionToUser (region);

					CategoryModel.CategoryLocation = mPopupListView.SelectedItem.ToString();
					mRootPage.mSettingsPage.PopulateListView();
					mRootPage.ReloadStreams();
					Navigation.PushAsync( mRootPage );
				}
			};

			mCancelButton = new Button () { 
				Text = "CANCEL",
				BackgroundColor = Color.White,
				TextColor = MyDevice.RedColor,
				BorderColor = MyDevice.BlueColor,
				BorderWidth = 2
			};

			mCancelButton.Clicked += (sender, e) => {
				DismissPopup();
			};


			mConfirmationGrid.Children.Add (mCancelButton, 0, 0);
			mConfirmationGrid.Children.Add (mOKButton, 1, 0);
		}

		private void DismissPopup()
		{
			mPopupLayout.IsVisible = false;
		}
	}
}
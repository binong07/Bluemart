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

namespace bluemart.MainViews
{
	public partial class MainPage : ContentPage
	{				
		UserClass mUserModel;
		PopupLayout mPopupLayout = new PopupLayout();
		ListView mPopupListView = new ListView ();	
		List<LocationCell> mLocationCellList = new List<LocationCell> ();
		Grid mConfirmationGrid;
		Button mOKButton;
		Button mCancelButton;
		RootPage mRootPage = new RootPage ();
		//StackLayout mPopupStackLayout = new StackLayout();

		private const string _location1Text="Location1Text";
		private const string _location2Text="Location2Text";
		private const string _location3Text="Location3Text";

		private readonly List<string> mLocationList = new List<string>(){"Discovery Gardens","Dubai Marina","Emirates Hills","Greens",
																"Internet City","JLT","Knowledge Village","Lakes","Meadows","Media City",							
																"Palm Jumeirah","Springs","Tecom"};

	
		public MainPage ()
		{			
			NavigationPage.SetHasNavigationBar (this, false);
			InitializeComponent ();
			InitalizeMemberVariables ();
			SetGrid1ButtonsSize ();

			LocationButton.TextColor = MyDevice.RedColor;
			LocationButton.BorderColor = MyDevice.BlueColor;

			MapButton.TextColor = MyDevice.RedColor;
			MapButton.BorderColor = MyDevice.BlueColor;

		}

		protected override bool OnBackButtonPressed ()
		{
			if (mPopupLayout.IsPopupActive) {
				DismissPopup();
				return true;
			}

			if (Device.OS == TargetPlatform.Android) {
				
			}

			return base.OnBackButtonPressed ();
		}

		private void PopulatePopup()
		{
			StackLayout1.BackgroundColor = Color.Gray;

			mPopupLayout.WidthRequest = LocationButton.Width;

			mPopupListView.SeparatorVisibility = SeparatorVisibility.None;
			mPopupListView.SeparatorColor = Color.Transparent;

			mPopupListView.ItemsSource = mLocationList;
			mPopupListView.HorizontalOptions = LayoutOptions.Center;

			mPopupLayout.Content = StackLayout1;
			Content = mPopupLayout;

			PopulateConfirmationGrid ();

			var popup = new StackLayout {
				WidthRequest = LocationButton.Width,
				BackgroundColor = Color.White,
				Orientation = StackOrientation.Vertical,
				Children =
				{
					mPopupListView,
					mConfirmationGrid
				}
			};
			if(  mUserModel.GetLocationFromUser () != "" )
				mPopupListView.SelectedItem = mUserModel.GetLocationFromUser ();
			
			mPopupLayout.ShowPopup (popup,Constraint.Constant(LocationButton.Bounds.Left),Constraint.Constant(MyDevice.ScreenHeight/6));
		}

		private void PopulateConfirmationGrid()
		{
			mConfirmationGrid = new Grid()
			{
				ColumnSpacing = 0,
				RowSpacing = 0,
				RowDefinitions = 
				{
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = LocationButton.Width/2},
					new ColumnDefinition { Width = LocationButton.Width/2},
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
					mUserModel.AddLocationToUser (mPopupListView.SelectedItem.ToString());
					CategoryModel.CategoryLocation = mPopupListView.SelectedItem.ToString();
					Navigation.PushAsync( mRootPage );
					//Navigation.PushAsync (new BrowseCategoriesPage (),true);	
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
			StackLayout1.BackgroundColor = Color.White;
			mPopupLayout.DismissPopup();
		}

		private void InitalizeMemberVariables()
		{
			mUserModel = new UserClass ();
		}

		private void SetGrid1ButtonsSize()
		{
			StackLayout1.WidthRequest = MyDevice.ScreenWidth;
			StackLayout1.HeightRequest = MyDevice.ScreenHeight;
			StackLayout1.Padding = new Thickness(0,0,0,0);
			LocationButton.WidthRequest = MyDevice.ScreenWidth * 2 / 3;
			MapButton.WidthRequest = MyDevice.ScreenWidth * 2 / 3;
			orLabel.FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label));
		}	

		public void OnLocationButtonClicked(Object sender,EventArgs e )
		{
			PopulatePopup();
		}

		public void OnMapButtonClicked(Object sender,EventArgs e )
		{
			//todo: add map view
			Navigation.PushAsync( new MapView(mRootPage));
		}
	}
}


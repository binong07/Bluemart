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
		UserClass mUserModel;
		PopupLayout mPopupLayout = new PopupLayout();
		ListView mPopupListView = new ListView ();	
		List<RegionClass> mRegions = new List<RegionClass> ();
		Grid mConfirmationGrid;
		Button mOKButton;
		Button mCancelButton;
		RootPage mRootPage = new RootPage ();
	
		public MainPage ()
		{			
			NavigationPage.SetHasNavigationBar (this, false);
			InitalizeMemberVariables ();
			if( mUserModel.GetActiveRegionFromUser() != "" )
				Navigation.PushAsync( mRootPage );
			InitializeComponent ();

			SetGrid1ButtonsSize ();

			LocationButton.TextColor = MyDevice.RedColor;
			LocationButton.BorderColor = MyDevice.BlueColor;

			MapButton.TextColor = MyDevice.RedColor;
			MapButton.BorderColor = MyDevice.BlueColor;


		}

		protected override bool OnBackButtonPressed ()
		{
			if (Navigation.NavigationStack.Last<Page> () is RootPage) {				
				//Check if product page is active
				if( mRootPage.mTopNavigationBar.IsVisible == true ){					
					mRootPage.SwitchTab (mRootPage.mCurrentPageParent);
				}

				//return true;	
			}

			if (mPopupLayout.IsPopupActive) {
				DismissPopup();

			}
			return true;
			//return base.OnBackButtonPressed ();
		}

		private void PopulatePopup()
		{
			//StackLayout1.BackgroundColor = MyDevice.BlueColor;
			StackLayout1.BackgroundColor = Color.FromRgba ( MyDevice.BlueColor.R, MyDevice.BlueColor.G, MyDevice.BlueColor.B,0.5f);
			mRegions.Clear ();

			mPopupLayout.WidthRequest = LocationButton.Width;

			mPopupListView.SeparatorVisibility = SeparatorVisibility.None;
			mPopupListView.SeparatorColor = Color.Transparent;


			var cell = new DataTemplate (typeof(RegionCell));

			foreach (var region in RegionHelper.locationList) {
				mRegions.Add (new RegionClass (region));
			}

			mPopupListView.ItemTemplate = cell;
			mPopupListView.ItemsSource = mRegions;
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
			if(  mUserModel.GetActiveRegionFromUser () != "" )
			{
				string region = mUserModel.GetActiveRegionFromUser ();
				foreach (var item in mPopupListView.ItemsSource) {
					if ((item as RegionClass).Region == region)
						mPopupListView.SelectedItem = item;
				}
			}
			
			mPopupLayout.ShowPopup (popup,Constraint.Constant(LocationButton.Bounds.Left),Constraint.Constant(MyDevice.ScreenHeight/6));
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
					new ColumnDefinition { Width = LocationButton.Width/2 - MyDevice.ViewPadding},
					new ColumnDefinition { Width = LocationButton.Width/2 - MyDevice.ViewPadding},
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

					/*AddressClass address = new AddressClass();
					var addressList = address.GetAddressList(region);
					if( address != null )
						address.AddAddress();
					else
					{
						address = new AddressClass();
						address.Region = region;
						address.Address = "";
						address.AddressDescription = "";
						address.AddAddress();
					}*/
					CategoryModel.CategoryLocation = mPopupListView.SelectedItem.ToString();
					mRootPage.mSettingsPage.PopulateListView();
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
			Navigation.PushAsync( new MapView(mRootPage,mUserModel));
		}
	}
}


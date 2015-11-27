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
		PopupLayout mPopupLayout = new PopupLayout();
		ListView mPopupListView = new ListView ();	
		List<RegionClass> mRegions = new List<RegionClass> ();
		Grid mConfirmationGrid;
		Button mOKButton;
		Button mCancelButton;
		RootPage mRootPage = new RootPage ();

		private Image mLogoImage;

		public MainPage ()
		{			
			NavigationPage.SetHasNavigationBar (this, false);

			if( mUserModel.GetActiveRegionFromUser() != "" )
				Navigation.PushAsync( mRootPage );
			
			InitializeComponent ();

			InitRelativeLayout ();
		}

		private void InitRelativeLayout()
		{
			MainRelativeLayout.BackgroundColor = MyDevice.BackgroundColor;

			#region LogoImage
			mLogoImage = new Image () {
				Source = "logo",	
				Aspect = Aspect.AspectFit,
				VerticalOptions = LayoutOptions.Start
			};

			MainRelativeLayout.Children.Add(mLogoImage, 
				Constraint.Constant( MyDevice.MenuPadding*2 ),
				Constraint.Constant( MyDevice.ScreenHeight/12 ),
				Constraint.Constant( MyDevice.ScreenWidth - MyDevice.MenuPadding*4)
			);
			#endregion

			#region ChooseYourLocation Button Definition
			var chooseYourLocationGrid = new Grid () {
				BackgroundColor = Color.White,
				RowSpacing = 0,
				RowDefinitions = {
					new RowDefinition ()
					{
						Height = Device.GetNamedSize(NamedSize.Medium,typeof(Label)) * 4
					},
					new RowDefinition ()
					{
						Height = Device.GetNamedSize(NamedSize.Medium,typeof(Label))*2
					}
				},
				ColumnDefinitions ={					
					new ColumnDefinition (){
						Width = MyDevice.ScreenWidth - MyDevice.MenuPadding*8
					}
				}
			};

			var chooseYourLocationImage = new Image () {
				Source = "ChooseLocation",
				Aspect = Aspect.AspectFit,		
				WidthRequest = ( MyDevice.ScreenWidth - MyDevice.MenuPadding*6) / 4,
				VerticalOptions = LayoutOptions.Center,				
				HorizontalOptions = LayoutOptions.Center
			};

			var chooseYourLocationLabel = new Label () {
				Text = "Choose Your Location",
				FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label)),
				FontAttributes = FontAttributes.Bold,
				TextColor = MyDevice.RedColor,
				BackgroundColor = Color.Transparent,
				HorizontalTextAlignment = TextAlignment.Center
			};

			chooseYourLocationGrid.Children.Add (chooseYourLocationImage, 0, 0);
			chooseYourLocationGrid.Children.Add (chooseYourLocationLabel, 0, 1);



			var chooseYourLocationFrame = new Frame { 				
				Padding = MyDevice.ScreenWidth*0.0111f,
				OutlineColor = Color.White,
				BackgroundColor = Color.White,
				VerticalOptions = LayoutOptions.Start,
				Content = chooseYourLocationGrid
			};

			MainRelativeLayout.Children.Add(chooseYourLocationFrame, 
				Constraint.RelativeToView(mLogoImage, (parent,sibling) => {
					return sibling.Bounds.Left + MyDevice.MenuPadding*2;
				}),
				Constraint.RelativeToView(mLogoImage, (parent,sibling) => {
					return sibling.Bounds.Bottom + MyDevice.ScreenHeight/24;
				})
			);

			var chooseYourLocationTapRecognizer= new TapGestureRecognizer ();
			chooseYourLocationTapRecognizer.Tapped += async (sender, e) => {

				chooseYourLocationImage.Opacity = 0.5f;
				await Task.Delay (MyDevice.DelayTime);
				PopulatePopup();
				chooseYourLocationImage.Opacity = 1f;
			};
			chooseYourLocationGrid.GestureRecognizers.Add (chooseYourLocationTapRecognizer);
			#endregion

			#region OrLabel
			var orLabel = new Label () {
				Text = "or",
				TextColor = Color.Gray,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label)),
				FontAttributes = FontAttributes.Bold,
				BackgroundColor = Color.Transparent
			};

			MainRelativeLayout.Children.Add(orLabel, 
				Constraint.RelativeToView(chooseYourLocationFrame, (parent,sibling) => {
					return sibling.Bounds.Center.X - MyDevice.ViewPadding;
				}),
				Constraint.RelativeToView(chooseYourLocationFrame, (parent,sibling) => {
					return sibling.Bounds.Bottom + MyDevice.ScreenHeight/48;
				})
			);
			#endregion

			#region ChooseYourLocation Button Definition
			var chooseFromMapGrid = new Grid () {
				BackgroundColor = Color.White,
				RowSpacing = 0,
				RowDefinitions = {
					new RowDefinition ()
					{
						Height = Device.GetNamedSize(NamedSize.Medium,typeof(Label)) * 4
					},
					new RowDefinition ()
					{
						Height = Device.GetNamedSize(NamedSize.Medium,typeof(Label))*2
					}
				},
				ColumnDefinitions ={					
					new ColumnDefinition (){
					Width = MyDevice.ScreenWidth - MyDevice.MenuPadding*8
					}
				}
			};

			var chooseFromMapImage = new Image () {
				Source = "ChooseMap",
				Aspect = Aspect.AspectFit,		
				WidthRequest = ( MyDevice.ScreenWidth - MyDevice.MenuPadding*6) / 4,
				VerticalOptions = LayoutOptions.Center,				
				HorizontalOptions = LayoutOptions.Center
			};

			var chooseFromMapLabel = new Label () {
				Text = "Choose From Map",
				FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label)),
				FontAttributes = FontAttributes.Bold,
				TextColor = MyDevice.RedColor,
				BackgroundColor = Color.Transparent,
				HorizontalTextAlignment = TextAlignment.Center
			};

			chooseFromMapGrid.Children.Add (chooseFromMapImage, 0, 0);
			chooseFromMapGrid.Children.Add (chooseFromMapLabel, 0, 1);



			var chooseFromMapFrame = new Frame { 				
				Padding = MyDevice.ScreenWidth*0.0111f,
				OutlineColor = Color.White,
				BackgroundColor = Color.White,
				VerticalOptions = LayoutOptions.Start,
				Content = chooseFromMapGrid
			};

			MainRelativeLayout.Children.Add(chooseFromMapFrame, 
				Constraint.RelativeToView(chooseYourLocationFrame, (parent,sibling) => {
					return sibling.Bounds.Left;
				}),
				Constraint.RelativeToView(orLabel, (parent,sibling) => {
					return sibling.Bounds.Bottom + MyDevice.ScreenHeight/48;
				})
			);

			var chooseFromMapTapRecognizer= new TapGestureRecognizer ();
			chooseFromMapTapRecognizer.Tapped += async (sender, e) => {

				chooseFromMapImage.Opacity = 0.5f;
				await Task.Delay (MyDevice.DelayTime);
				Navigation.PushAsync( new MapView(mRootPage,mUserModel));
				chooseFromMapImage.Opacity = 1f;
			};
			chooseFromMapGrid.GestureRecognizers.Add (chooseFromMapTapRecognizer);
			#endregion

			#region MessageLabel
			var messageLabel1 = new Label () {
				Text = "Whatever you need,",
				TextColor = MyDevice.BlueColor,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label)),
				BackgroundColor = Color.Transparent
			};

			MainRelativeLayout.Children.Add(messageLabel1, 
				Constraint.RelativeToView(chooseFromMapFrame, (parent,sibling) => {
					return sibling.Bounds.Left;
				}),
				Constraint.RelativeToView(chooseFromMapFrame, (parent,sibling) => {
					return sibling.Bounds.Bottom + MyDevice.ScreenHeight/24;
				}),
				Constraint.RelativeToView(chooseFromMapFrame, (parent,sibling) => {
					return sibling.Width;
				})
			);

			var messageLabel2 = new Label () {
				Text = "we deliver within 1 hour!",
				TextColor = MyDevice.BlueColor,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label)),
				BackgroundColor = Color.Transparent
			};

			MainRelativeLayout.Children.Add(messageLabel2, 
				Constraint.RelativeToView(messageLabel1, (parent,sibling) => {
					return sibling.Bounds.Left - MyDevice.ViewPadding;
				}),
				Constraint.RelativeToView(messageLabel1, (parent,sibling) => {
					return sibling.Bounds.Bottom;
				}),
				Constraint.RelativeToView(messageLabel1, (parent,sibling) => {
					return sibling.Width + MyDevice.ViewPadding*2;
				})
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
					Navigation.PopAsync ();
				//}

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
			MainRelativeLayout.BackgroundColor = MyDevice.BlueColor;
			MainRelativeLayout.BackgroundColor = Color.FromRgba ( MyDevice.BlueColor.R, MyDevice.BlueColor.G, MyDevice.BlueColor.B,0.5f);
			mRegions.Clear ();

			mPopupLayout.WidthRequest = mLogoImage.Width;

			mPopupListView.SeparatorVisibility = SeparatorVisibility.None;
			mPopupListView.SeparatorColor = Color.Transparent;


			var cell = new DataTemplate (typeof(RegionCell));

			foreach (var region in RegionHelper.locationList) {
				mRegions.Add (new RegionClass (region));
			}

			mPopupListView.ItemTemplate = cell;
			mPopupListView.ItemsSource = mRegions;
			mPopupListView.HorizontalOptions = LayoutOptions.Center;

			mPopupLayout.Content = MainRelativeLayout;
			Content = mPopupLayout;

			PopulateConfirmationGrid ();

			var popup = new StackLayout {
				WidthRequest = mLogoImage.Width,
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
			
			mPopupLayout.ShowPopup (popup,Constraint.Constant(mLogoImage.Bounds.Left),Constraint.Constant(MyDevice.ScreenHeight/6));
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
					new ColumnDefinition { Width = mLogoImage.Width/2 - MyDevice.ViewPadding},
					new ColumnDefinition { Width = mLogoImage.Width/2 - MyDevice.ViewPadding},
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
			MainRelativeLayout.BackgroundColor = MyDevice.BackgroundColor;
			mPopupLayout.DismissPopup();
		}

		private void InitalizeMemberVariables()
		{
			
		}

		/*public void OnLocationButtonClicked(Object sender,EventArgs e )
		{
			PopulatePopup();
		}*/

		public void OnMapButtonClicked(Object sender,EventArgs e )
		{
			//todo: add map view
			Navigation.PushAsync( new MapView(mRootPage,mUserModel));
		}
	}
}


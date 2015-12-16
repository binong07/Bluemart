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

		private Image mLogoImage;
		private RelativeLayout MainRelativeLayout;

		//Text Layout Related
		private int mButtonCount = 3;
		private List<RelativeLayout> mButtonList = new List<RelativeLayout> ();
		private List<string> mMessageList = new List<string>(){
			"Whatever you need,\nwe deliver within 1 hour!",
			"Whatever you need,\nwe deliver within 2 hour!",
			"Whatever you need,\nwe deliver within 3 hour!"
		};
		private int mActiveButtonIndex = 0;
		private List<Image> mSelectedImageList = new List<Image>();
		private Label mInformationLabel;
		private bool bIsPopuplayoutInitialized = false;
		//private RelativeLayout mPopup = new RelativeLayout();

		public MainPage ()
		{			
			NavigationPage.SetHasNavigationBar (this, false);

			/*if( mUserModel.GetActiveRegionFromUser() != "" )
				Navigation.PushAsync( mRootPage );*/
			InitRelativeLayout ();
			InitializeComponent ();
			Content = MainRelativeLayout;
		}

		private async void InitRelativeLayout()
		{		
			MainRelativeLayout = new RelativeLayout () {
				Padding = 0,
				BackgroundColor = Color.Black//Color.FromRgba ( MyDevice.BlueColor.R, MyDevice.BlueColor.G, MyDevice.BlueColor.B,0.5f)
			};


			#region BackgroundImage

			Image backgroundImage = new Image(){
				Aspect = Aspect.AspectFill
			};
			backgroundImage.Source = ImageSource.FromFile("Screen1_BG_");
			MainRelativeLayout.Children.Add( backgroundImage,				
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.Constant(MyDevice.GetScaledSize(1080)),
				Constraint.Constant(MyDevice.GetScaledSize(1920))
			);
			#endregion

			//await Task.Delay (400);

			#region LogoImage
			mLogoImage = new Image () {
				Source = "Screen1_BMLogo",
				Aspect = Aspect.AspectFill,
				VerticalOptions = LayoutOptions.Start
			};

			MainRelativeLayout.Children.Add(mLogoImage, 
				Constraint.Constant( MyDevice.GetScaledSize(120) ),
				Constraint.Constant( MyDevice.GetScaledSize(130) ),
				Constraint.Constant( MyDevice.GetScaledSize(840) ),
				Constraint.Constant( MyDevice.GetScaledSize(160) )
			);
			#endregion

			#region Gri1

			Image Gri1Image = new Image(){
				Source = "Screen1_Gri1",
				Aspect = Aspect.Fill
			};

			MainRelativeLayout.Children.Add(Gri1Image, 
				Constraint.Constant( MyDevice.GetScaledSize(0) ),
				Constraint.Constant( MyDevice.ScreenHeight/2-MyDevice.GetScaledSize(460)),
				Constraint.Constant( MyDevice.GetScaledSize(1080) ),
				Constraint.Constant( MyDevice.GetScaledSize(420) )
			);

			mInformationLabel = new Label()
			{
				FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label))*1.1f,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.White
			};

			MainRelativeLayout.Children.Add(mInformationLabel, 
				Constraint.RelativeToView(Gri1Image, (parent,sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize(25);
				}),
				Constraint.RelativeToView(Gri1Image, (parent,sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize(25);
				}),
				Constraint.Constant( MyDevice.GetScaledSize(1030) ),
				Constraint.Constant( MyDevice.GetScaledSize(300) )
			);



			for(int i=0;i<mButtonCount;i++)
			{
				var ImageLayout = new RelativeLayout();

				var buttonTapRecognizer= new TapGestureRecognizer ();
				buttonTapRecognizer.Tapped += (sender, e) => {					
					SelectButton(mButtonList.IndexOf(sender as RelativeLayout));
				};
				ImageLayout.GestureRecognizers.Add (buttonTapRecognizer);
				mButtonList.Add( ImageLayout );

				Image buttonImage = new Image(){
					Source = "Screen1_Dot"
				};

				mSelectedImageList.Add( new Image()
					{
						Source = "Screen1_SelectedDot",
						IsVisible = false
					}
				);

				ImageLayout.Children.Add(buttonImage,
					Constraint.Constant( MyDevice.GetScaledSize(20) ),
					Constraint.Constant( MyDevice.GetScaledSize(20) ),
					Constraint.Constant( MyDevice.GetScaledSize(30) ),
					Constraint.Constant( MyDevice.GetScaledSize(30) )
				);

				ImageLayout.Children.Add(mSelectedImageList[i],
					Constraint.Constant( MyDevice.GetScaledSize(20) ),
					Constraint.Constant( MyDevice.GetScaledSize(20) ),
					Constraint.Constant( MyDevice.GetScaledSize(30) ),
					Constraint.Constant( MyDevice.GetScaledSize(30) )
				);


				if( i == 0)
				{
					MainRelativeLayout.Children.Add(ImageLayout, 
						Constraint.RelativeToView(Gri1Image, (parent,sibling) => {
							return sibling.Bounds.Left + MyDevice.GetScaledSize(480-mButtonCount*23);
						}),
						Constraint.RelativeToView(Gri1Image, (parent,sibling) => {
							return sibling.Bounds.Top + MyDevice.GetScaledSize(350);
						}),
						Constraint.Constant( MyDevice.GetScaledSize(80) ),
						Constraint.Constant( MyDevice.GetScaledSize(80) )
					);						
				}
				else
				{
					MainRelativeLayout.Children.Add(ImageLayout, 
						Constraint.RelativeToView(mButtonList[i-1], (parent,sibling) => {
							return sibling.Bounds.Right + MyDevice.GetScaledSize(33);
						}),
						Constraint.RelativeToView(Gri1Image, (parent,sibling) => {
							return sibling.Bounds.Top + MyDevice.GetScaledSize(350);
						}),
						Constraint.Constant( MyDevice.GetScaledSize(80) ),
						Constraint.Constant( MyDevice.GetScaledSize(80) )
					);	
				}

				/*MainRelativeLayout.Children.Add(mSelectedImageList[i], 
					Constraint.RelativeToView(mButtonList[i], (parent,sibling) => {
						return sibling.Bounds.Left;
					}),
					Constraint.RelativeToView(mButtonList[i], (parent,sibling) => {
						return sibling.Bounds.Top;
					}),
					Constraint.Constant( MyDevice.GetScaledSize(30) ),
					Constraint.Constant( MyDevice.GetScaledSize(30) )
				);*/

			}
			SelectButton(0);
			#endregion

			#region Gri2

			Image Gri2Image = new Image(){
				Source = "Screen1_Gri2",
				Aspect = Aspect.Fill
			};

			MainRelativeLayout.Children.Add(Gri2Image, 
				Constraint.Constant( MyDevice.GetScaledSize(0) ),
				Constraint.Constant( MyDevice.ScreenHeight - MyDevice.GetScaledSize(670) ),
				Constraint.Constant( MyDevice.GetScaledSize(1080) ),
				Constraint.Constant( MyDevice.GetScaledSize(550) )
			);

			Image locationButton = new Image()
			{
				Source = "Screen1_Button1",
				Aspect = Aspect.Fill,
				WidthRequest = MyDevice.GetScaledSize(930),
				HeightRequest = MyDevice.GetScaledSize(120)
			};

			MainRelativeLayout.Children.Add(locationButton, 
				Constraint.RelativeToView(Gri2Image, (parent,sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize(75);
				}),
				Constraint.RelativeToView(Gri2Image, (parent,sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize(50);
				}),
				Constraint.Constant( MyDevice.GetScaledSize(930) ),
				Constraint.Constant( MyDevice.GetScaledSize(120) )
			);

			var chooseYourLocationTapRecognizer= new TapGestureRecognizer ();
			chooseYourLocationTapRecognizer.Tapped += async (sender, e) => {

				locationButton.Opacity = 0.5f;
				await Task.Delay (MyDevice.DelayTime);
				PopulatePopup();
				locationButton.Opacity = 1f;
			};
			locationButton.GestureRecognizers.Add(chooseYourLocationTapRecognizer);

			Image mapButton = new Image()
			{
				Source = "Screen1_Button1",
				Aspect = Aspect.Fill,
				WidthRequest = MyDevice.GetScaledSize(930),
				HeightRequest = MyDevice.GetScaledSize(120)
			};

			MainRelativeLayout.Children.Add(mapButton, 
				Constraint.RelativeToView(Gri2Image, (parent,sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize(75);
				}),
				Constraint.RelativeToView(Gri2Image, (parent,sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize(210);
				}),
				Constraint.Constant( MyDevice.GetScaledSize(930) ),
				Constraint.Constant( MyDevice.GetScaledSize(120) )
			);

			var chooseFromMapTapRecognizer= new TapGestureRecognizer ();
			chooseFromMapTapRecognizer.Tapped += async (sender, e) => {

				mapButton.Opacity = 0.5f;
				await Task.Delay (MyDevice.DelayTime);
				await Navigation.PushAsync( new MapView(mRootPage,mUserModel));
				mapButton.Opacity = 1f;
			};
			mapButton.GestureRecognizers.Add (chooseFromMapTapRecognizer);

			Image linkButton = new Image()
			{
				Source = "Screen1_Button1",
				Aspect = Aspect.Fill,
				WidthRequest = MyDevice.GetScaledSize(930),
				HeightRequest = MyDevice.GetScaledSize(120)
			};

			MainRelativeLayout.Children.Add(linkButton, 
				Constraint.RelativeToView(Gri2Image, (parent,sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize(75);
				}), 
				Constraint.RelativeToView(Gri2Image, (parent,sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize(370);
				}),
				Constraint.Constant( MyDevice.GetScaledSize(930) ),
				Constraint.Constant( MyDevice.GetScaledSize(120) )
			);

			var linkTapRecognizer= new TapGestureRecognizer ();
			linkTapRecognizer.Tapped += (sender, e) => {				
				Device.OpenUri(new Uri("http://google.com"));
			};
			linkButton.GestureRecognizers.Add (linkTapRecognizer);

			#endregion

		}

		private void SelectButton(int newIndex)
		{	
			mSelectedImageList [mActiveButtonIndex].IsVisible = false;
			mActiveButtonIndex = newIndex;
			mSelectedImageList [mActiveButtonIndex].IsVisible = true;
			mInformationLabel.Text = mMessageList [mActiveButtonIndex];

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
			mPopupListView.WidthRequest = mLogoImage.Width;
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

			MainRelativeLayout.Children.Add (mPopupLayout,
				Constraint.Constant (mLogoImage.Bounds.Left),
				Constraint.Constant (MyDevice.ScreenHeight/6)
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
					mRootPage.ReloadStreams();
					mRootPage.mBrowseCategoriesPage.RefreshBorderStream();
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
			//MainRelativeLayout.BackgroundColor = MyDevice.BackgroundColor;
			//mPopupLayout.DismissPopup();
			mPopupLayout.IsVisible = false;
		}
	}
}
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
		RelativeLayout mPopupLayout;
		StackLayout mPopupStackLayout;
		ScrollView mPopupScrollView;	
		Label mActiveLabel;
		List<Label> mLabelList = new List<Label> ();
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

			if (bIsPopuplayoutInitialized && mPopupStackLayout.IsVisible) {
				DismissPopup();

			}
			return true;
			//return base.OnBackButtonPressed ();
		}
		public View lastSelectedRegion;

		private void PopulatePopup()
		{
			//MainRelativeLayout.BackgroundColor = MyDevice.BlueColor;
			MainRelativeLayout.BackgroundColor = Color.FromRgba ( MyDevice.GreyColor.R, MyDevice.GreyColor.G, MyDevice.GreyColor.B,0.5f);
			mRegions.Clear ();
			mPopupLayout = new RelativeLayout () {
				BackgroundColor = Color.White,
				WidthRequest = MyDevice.GetScaledSize(369),
				HeightRequest = MyDevice.GetScaledSize(543)
			};

			mPopupStackLayout = new StackLayout () {
				Orientation = StackOrientation.Vertical
			};

			foreach (var region in RegionHelper.locationList) {
				var label = new Label () {
					WidthRequest = MyDevice.GetScaledSize(365),
					HeightRequest = MyDevice.GetScaledSize(50),
					HorizontalTextAlignment = TextAlignment.Center,
					VerticalTextAlignment = TextAlignment.Center,
					TextColor = Color.FromRgb(77,77,77),
					Text = region,
					FontSize = MyDevice.FontSizeSmall
				};

				if (region == mUserModel.GetActiveRegionFromUser ()) {
					mActiveLabel = label;
					ActivateLabel (label);
				}

				var labelTapRecogniser = new TapGestureRecognizer ();
				labelTapRecogniser.Tapped += (sender, e) => {
					ActivateLabel(label);
				};

				label.GestureRecognizers.Add (labelTapRecogniser);

				mPopupStackLayout.Children.Add (label);
			}

			mPopupScrollView = new ScrollView () {
				Orientation = ScrollOrientation.Vertical,
				Content = mPopupStackLayout
			};

			mCancelButton = new ExtendedButton () {
				WidthRequest = MyDevice.GetScaledSize(124),
				HeightRequest = MyDevice.GetScaledSize(60),
				BorderWidth = 1,
				HorizontalContentAlignment = TextAlignment.Center,
				VerticalContentAlignment = TextAlignment.Center,
				TextColor = Color.White,
				BackgroundColor = Color.FromRgb(213,53,53),
				Text = "CANCEL",
				FontSize = MyDevice.FontSizeMicro
			};
			mCancelButton.Clicked += (sender, e) => {
				DismissPopup();
			};

			mOKButton = new ExtendedButton () {
				WidthRequest = MyDevice.GetScaledSize(105),
				HeightRequest = MyDevice.GetScaledSize(60),
				BorderWidth = 1,
				HorizontalContentAlignment = TextAlignment.Center,
				VerticalContentAlignment = TextAlignment.Center,
				TextColor = Color.White,
				BackgroundColor = Color.FromRgb(132,178,98),
				Text = "OK",
				FontSize = MyDevice.FontSizeMicro
			};
			mOKButton.Clicked += (sender, e) => {
				if(mActiveLabel != null )
				{
					DismissPopup();
					string region = mActiveLabel.Text;
					mUserModel.AddActiveRegionToUser (region);
					CategoryModel.CategoryLocation = mActiveLabel.Text;
					mRootPage.ReloadStreams();
					mRootPage.SwitchTab("BrowseCategories");
					Navigation.PushAsync( mRootPage );
				}
			};				

			MainRelativeLayout.Children.Add (mPopupLayout,
				Constraint.Constant (MyDevice.GetScaledSize(136)),
				Constraint.Constant (MyDevice.GetScaledSize(190))
			);

			mPopupLayout.Children.Add(mPopupScrollView,
				Constraint.Constant (MyDevice.GetScaledSize(2)),
				Constraint.Constant (MyDevice.GetScaledSize(60)),
				Constraint.Constant (MyDevice.GetScaledSize(365)),
				Constraint.Constant (MyDevice.GetScaledSize(362))
			);

			mPopupLayout.Children.Add(mCancelButton,
				Constraint.Constant (MyDevice.GetScaledSize(37)),
				Constraint.Constant (MyDevice.GetScaledSize(457))
			);

			mPopupLayout.Children.Add(mOKButton,
				Constraint.RelativeToView (mCancelButton, (p,sibling) => {
					return sibling.Bounds.Right + MyDevice.GetScaledSize(64);
				}),
				Constraint.RelativeToView (mCancelButton, (p,sibling) => {
					return sibling.Bounds.Top;
				})
			);

			bIsPopuplayoutInitialized = true;	
		}

		private void ActivateLabel(Label label)
		{
			if (mActiveLabel != null) {
				mActiveLabel.TextColor = Color.FromRgb(77,77,77);
				mActiveLabel.BackgroundColor = Color.Transparent;
			}
			mActiveLabel = label;
			mActiveLabel.TextColor = Color.White;
			mActiveLabel.BackgroundColor = Color.FromRgb (132, 178, 98);

		}			

		private void DismissPopup()
		{
			MainRelativeLayout.Children.Remove (mPopupLayout);
			//mPopupLayout.IsVisible = false;
		}
	}
}
using System;
using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Local;
using System.Threading.Tasks;
using bluemart.MainViews;

namespace bluemart.Common.ViewCells
{
	public class AddressCell : ViewCell
	{
		public Label mNameLabel;
		public Label mAddressLabel;
		public Label mPhoneLabel;
		public AddressClass mAddressClass;
		private SettingsPage mRootPage;

		public AddressCell (AddressClass address, SettingsPage rootPage)
		{
			mRootPage = rootPage;
			mAddressClass = address;

			var imageSize = MyDevice.ScreenWidth / 12;

			RelativeLayout relLayout = new RelativeLayout () {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.Center,
				BackgroundColor = Color.White,
				Padding = 0,
				WidthRequest = MyDevice.ScreenWidth - MyDevice.ViewPadding*2
			};

			Grid mainGrid = new Grid (){	
				//Padding = new Thickness(1,1,1,1),
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowSpacing = 0,
				RowDefinitions = 
				{ 
					new RowDefinition(){Height = Device.GetNamedSize(NamedSize.Large,typeof(Label))}, 
					new RowDefinition(){Height = Device.GetNamedSize(NamedSize.Large,typeof(Label))}, 
					new RowDefinition(){Height = Device.GetNamedSize(NamedSize.Large,typeof(Label))}, 
				},
				ColumnDefinitions = 
				{
					new ColumnDefinition(){ Width = MyDevice.ScreenWidth - MyDevice.ViewPadding*2 - imageSize }
				},
				BackgroundColor = MyDevice.BlueColor
			};

			mNameLabel = new Label (){ 
				TextColor = MyDevice.BlueColor,
				FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = address.Name,
				BackgroundColor = Color.White
			};

			mAddressLabel = new Label (){ 
				TextColor = MyDevice.BlueColor,
				FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = address.Address,
				BackgroundColor = Color.White
			};

			mPhoneLabel = new Label (){ 
				TextColor = MyDevice.BlueColor,
				FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = address.PhoneNumber,
				BackgroundColor = Color.White
			};

			mainGrid.Children.Add (mNameLabel, 0, 0);
			mainGrid.Children.Add (mAddressLabel, 0, 1);
			mainGrid.Children.Add (mPhoneLabel, 0, 2);

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += async (sender, e) => {

				mainGrid.Opacity = 0.5f;
				await Task.Delay (MyDevice.DelayTime);
				SwitchColor();
				mRootPage.mParent.LoadAddAddress(mAddressClass);
				//mRootPage.mParent.LoadReceiptPage(mHistoryClass);
				mainGrid.Opacity = 1f;
			};
			mainGrid.GestureRecognizers.Add (tapGestureRecognizer);

			var activeAddressImage = new Image () {
				Source = "ActiveAddress"	
			};

			relLayout.Children.Add(activeAddressImage, 
				Constraint.RelativeToView(mainGrid, (parent,sibling) => {
					return sibling.Bounds.Right;
				}),
				Constraint.RelativeToView(mainGrid, (parent,sibling) => {
					return sibling.Bounds.Top + 5;
				})
			);
			relLayout.Children.Add(mainGrid, Constraint.RelativeToParent(parent => {
				return 0;	
			}));

			activeAddressImage.IsVisible = mAddressClass.IsActive;

			this.View = relLayout;
		}

		private void SwitchColor()
		{
			/*if (mRootPage.mActiveHistoryCell != null) {
				mRootPage.mActiveHistoryCell.mTotalPriceLabel.BackgroundColor = Color.White;
				mRootPage.mActiveHistoryCell.mDateLabel.BackgroundColor = Color.White;
				mRootPage.mActiveHistoryCell.mRegionLabel.BackgroundColor = Color.White;
			}

			mRootPage.mActiveHistoryCell = this;

			mNameLabel.BackgroundColor = MyDevice.RedColor;
			mAddressLabel.BackgroundColor = MyDevice.RedColor;
			mPhoneLabel.BackgroundColor = MyDevice.RedColor;*/
		}
	}
}


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
		//private HistoryPage mRootPage;
		//private HistoryClass mHistoryClass;

		public AddressCell (AddressClass address)
		{
			//mRootPage = rootPage;
			//mHistoryClass = history;
			mAddressClass = address;

			Grid mainGrid = new Grid (){	
				//Padding = new Thickness(1,1,1,1),
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowSpacing = 0,
				RowDefinitions = 
				{ 
					new RowDefinition(){Height = GridLength.Auto}, 
					new RowDefinition(){Height = GridLength.Auto}, 
					new RowDefinition(){Height = GridLength.Auto}, 
				},
				ColumnDefinitions = 
				{
					new ColumnDefinition(){ Width = MyDevice.ScreenWidth-MyDevice.ViewPadding*2}
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
				//mRootPage.mParent.LoadReceiptPage(mHistoryClass);
				mainGrid.Opacity = 1f;
			};
			mainGrid.GestureRecognizers.Add (tapGestureRecognizer);

			this.View = mainGrid;
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


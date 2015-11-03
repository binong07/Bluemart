using System;
using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Local;
using System.Threading.Tasks;
using bluemart.MainViews;

namespace bluemart.Common.ViewCells
{
	public class TrackCell : ViewCell
	{
		public Label mTotalPriceLabel;
		public Label mDateLabel;
		public Label mRegionLabel;
		public Label mStatusLabel;
		private TrackPage mRootPage;
		private StatusClass mStatus;

		public TrackCell (StatusClass status, TrackPage rootPage)
		{
			mRootPage = rootPage;
			mStatus = status;

			Grid mainGrid = new Grid (){	
				//Padding = new Thickness(0,0,0,0),
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowSpacing = 0,
				RowDefinitions = 
				{ 
					new RowDefinition(){Height = GridLength.Auto}, 
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

			mTotalPriceLabel = new Label (){ 
				TextColor = MyDevice.BlueColor,
				FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Total Price: " + status.TotalPrice + " DH",
				BackgroundColor = Color.White
			};

			mDateLabel = new Label (){ 
				TextColor = MyDevice.BlueColor,
				FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Date: " + status.Date,
				BackgroundColor = Color.White
			};

			mRegionLabel = new Label (){ 
				TextColor = MyDevice.BlueColor,
				FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Region: " + status.Region,
				BackgroundColor = Color.White
			};

			mStatusLabel = new Label (){ 
				TextColor = MyDevice.BlueColor,
				FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = status.Status,
				BackgroundColor = Color.White
			};


			mainGrid.Children.Add (mTotalPriceLabel, 0, 0);
			mainGrid.Children.Add (mDateLabel, 0, 1);
			mainGrid.Children.Add (mRegionLabel, 0, 2);
			mainGrid.Children.Add (mStatusLabel, 0, 3);

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += async (sender, e) => {

				mainGrid.Opacity = 0.5f;
				await Task.Delay (MyDevice.DelayTime);
				SwitchColor();
				mRootPage.mParent.LoadReceiptPage(mStatus);
				mainGrid.Opacity = 1f;
			};
			mainGrid.GestureRecognizers.Add (tapGestureRecognizer);

			this.View = mainGrid;
		}

		private void SwitchColor()
		{
			if (mRootPage.mActiveTrackCell != null) {
				mRootPage.mActiveTrackCell.mTotalPriceLabel.BackgroundColor = Color.White;
				mRootPage.mActiveTrackCell.mDateLabel.BackgroundColor = Color.White;
				mRootPage.mActiveTrackCell.mRegionLabel.BackgroundColor = Color.White;
				mRootPage.mActiveTrackCell.mStatusLabel.BackgroundColor = Color.White;
			}

			mRootPage.mActiveTrackCell = this;

			mTotalPriceLabel.BackgroundColor = MyDevice.RedColor;
			mDateLabel.BackgroundColor = MyDevice.RedColor;
			mRegionLabel.BackgroundColor = MyDevice.RedColor;
			mStatusLabel.BackgroundColor = MyDevice.RedColor;
		}
	}
}


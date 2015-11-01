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

		public TrackCell (StatusClass status, TrackPage rootPage)
		{
			mRootPage = rootPage;

			Grid mainGrid = new Grid (){	
				Padding = new Thickness(1,1,1,1),
				HorizontalOptions = LayoutOptions.FillAndExpand,
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
					new ColumnDefinition(){ Width = MyDevice.ScreenWidth}
				},
				BackgroundColor = MyDevice.RedColor
			};

			mTotalPriceLabel = new Label (){ 
				TextColor = MyDevice.BlueColor,
				FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = status.TotalPrice,
				BackgroundColor = Color.White
			};

			mDateLabel = new Label (){ 
				TextColor = MyDevice.BlueColor,
				FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = status.Date,
				BackgroundColor = Color.White
			};

			mRegionLabel = new Label (){ 
				TextColor = MyDevice.BlueColor,
				FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = status.Region,
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


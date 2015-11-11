using System;
using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Local;
using System.Threading.Tasks;
using bluemart.MainViews;

namespace bluemart.Common.ViewCells
{
	public class HistoryCell : ViewCell
	{
		public Label mTotalPriceLabel;
		public Label mDateLabel;
		public Label mRegionLabel;
		private TrackPage mRootPage;
		private HistoryClass mHistoryClass;

		public HistoryCell (HistoryClass history, TrackPage rootPage)
		{
			mRootPage = rootPage;
			mHistoryClass = history;

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

			mTotalPriceLabel = new Label (){ 
				TextColor = MyDevice.BlueColor,
				FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Total Price: " + history.TotalPrice + " DH",
				BackgroundColor = Color.White
			};

			mDateLabel = new Label (){ 
				TextColor = MyDevice.BlueColor,
				FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Date: " + history.Date,
				BackgroundColor = Color.White
			};

			mRegionLabel = new Label (){ 
				TextColor = MyDevice.BlueColor,
				FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Region: " + history.Region,
				BackgroundColor = Color.White
			};

			mainGrid.Children.Add (mTotalPriceLabel, 0, 0);
			mainGrid.Children.Add (mDateLabel, 0, 1);
			mainGrid.Children.Add (mRegionLabel, 0, 2);

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += async (sender, e) => {

				mainGrid.Opacity = 0.5f;
				await Task.Delay (MyDevice.DelayTime);
				//SwitchColor();
				mRootPage.mParent.LoadReceiptPage(mHistoryClass);
				mainGrid.Opacity = 1f;
			};
			mainGrid.GestureRecognizers.Add (tapGestureRecognizer);

			this.View = mainGrid;
		}

		/*private void SwitchColor()
		{
			if (mRootPage.mActiveHistoryCell != null) {
				mRootPage.mActiveHistoryCell.mTotalPriceLabel.BackgroundColor = Color.White;
				mRootPage.mActiveHistoryCell.mDateLabel.BackgroundColor = Color.White;
				mRootPage.mActiveHistoryCell.mRegionLabel.BackgroundColor = Color.White;
			}

			mRootPage.mActiveHistoryCell = this;

			mTotalPriceLabel.BackgroundColor = MyDevice.RedColor;
			mDateLabel.BackgroundColor = MyDevice.RedColor;
			mRegionLabel.BackgroundColor = MyDevice.RedColor;
		}*/
	}
}


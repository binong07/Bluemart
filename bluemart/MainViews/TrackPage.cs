using System;
using System.Collections.Generic;
using bluemart.Common.Objects;
using bluemart.Common.Utilities;
using bluemart.Common.ViewCells;
using Xamarin.Forms;
using bluemart.Models.Remote;
using bluemart.Models.Local;

namespace bluemart.MainViews
{
	public partial class TrackPage : ContentPage
	{				
		public RootPage mParent;
		public TrackCell mActiveTrackCell;
		private Label mTrackLabel;
		private Label mHistoryLabel;

		public TrackPage (RootPage parent)
		{						
			InitializeComponent ();

			mTrackLabel = new Label () {
				HorizontalOptions = LayoutOptions.Center,
				TextColor = Color.White,
				Text = "TRACK",
				FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label))
			};

			mHistoryLabel = new Label () {
				HorizontalOptions = LayoutOptions.Center,
				TextColor = Color.White,
				Text = "HISTORY",
				FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label))
			};

			mParent = parent;
			MainStackLayout.Spacing = MyDevice.ViewPadding;
			SetGrid1Definitions ();
		}

		public void PopulateListView()
		{						
			MainStackLayout.Children.Clear ();

			MainStackLayout.Children.Add (mTrackLabel);
			var orderStatusList = OrderModel.GetOrdersForTracking ();
			foreach (var status in orderStatusList) {
				MainStackLayout.Children.Add( new TrackCell(status,this ).View );
			}

			MainStackLayout.Children.Add (mHistoryLabel);

			var orderHistoryList = OrderModel.GetOrdersForHistory ();
			foreach (var history in orderHistoryList) {
				MainStackLayout.Children.Add( new HistoryCell(history,this ).View );
			}
		}

		private void SetGrid1Definitions()
		{
			Grid1.RowDefinitions [0].Height = GridLength.Auto;
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
			Grid1.BackgroundColor = MyDevice.BackgroundColor;
		}
			
	}
}


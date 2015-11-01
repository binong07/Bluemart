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
		RootPage mParent;
		public TrackCell mActiveTrackCell;

		public TrackPage (RootPage parent)
		{						
			InitializeComponent ();
			mParent = parent;
		}

		public void PopulateListView()
		{
			MainStackLayout.Children.Clear ();
			var orderStatusList = OrderModel.GetOrdersForTracking ();
			foreach (var status in orderStatusList) {
				MainStackLayout.Children.Add( new TrackCell(status,this ).View );
			}
		}
			
	}
}


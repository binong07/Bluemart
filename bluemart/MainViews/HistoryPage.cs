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
	public partial class HistoryPage : ContentPage
	{				
		public RootPage mParent;
		public HistoryCell mActiveHistoryCell;

		public HistoryPage (RootPage parent)
		{						
			InitializeComponent ();
			mParent = parent;
		}

		public void PopulateListView()
		{
			MainStackLayout.Children.Clear ();
			var orderHistoryList = OrderModel.GetOrdersForHistory ();
			foreach (var history in orderHistoryList) {
				MainStackLayout.Children.Add( new HistoryCell(history,this ).View );
			}
		}
			
	}
}


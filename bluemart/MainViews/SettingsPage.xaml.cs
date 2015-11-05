using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Local;
using bluemart.Common.Objects;
using bluemart.Common.ViewCells;

namespace bluemart.MainViews
{
	public partial class SettingsPage : ContentPage
	{
		UserClass mUserModel = new UserClass ();
		AddressClass mAddressModel = new AddressClass ();
		RootPage mParent;


		public SettingsPage (RootPage parent)
		{
			InitializeComponent ();
			mParent = parent;
			StackLayout2.Spacing = MyDevice.ViewPadding;
			NavigationPage.SetHasNavigationBar (this, false);
			SetGrid1Definitions ();
			PopulateListView ();
		}

		public void PopulateListView()
		{
			StackLayout2.Children.Clear ();
			string region = mUserModel.ActiveRegion;
			var addressList = mAddressModel.GetAddressList (RegionHelper.DecideShopNumber (region));

			foreach (var address in addressList) {
				StackLayout2.Children.Add (new AddressCell (address).View);	
			}
			/*for (int i = 0; i < 10; i++) {
				StackLayout2.Children.Add (new AddressCell ().View);
			}*/
			/*var orderHistoryList = OrderModel.GetOrdersForHistory ();
			foreach (var history in orderHistoryList) {
				MainStackLayout.Children.Add( new HistoryCell(history,this ).View );
			}*/
		}
			

		private void SetGrid1Definitions()
		{
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;
			Grid1.RowDefinitions [0].Height = GridLength.Auto;
			Grid1.RowDefinitions [1].Height = GridLength.Auto;
			Grid1.BackgroundColor = MyDevice.BlueColor;
			AddressExplanationLabel.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));

		}
			
		private void OnChangeLocationButtonClicked( Object sender, EventArgs e )
		{
			mParent.Navigation.PopAsync ();
		}

		private void OnAddAddressButtonClicked( Object sender, EventArgs e )
		{
			mParent.LoadAddAddress ();
		}
	}
}



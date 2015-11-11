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
		public AddressCell mActiveAddressCell;
		public RootPage mParent;


		public SettingsPage (RootPage parent)
		{
			InitializeComponent ();
			mParent = parent;
			StackLayout2.Spacing = MyDevice.ViewPadding;
			NavigationPage.SetHasNavigationBar (this, false);
			SetGrid1Definitions ();

		}

		public void PopulateListView()
		{
			StackLayout2.Children.Clear ();
			string region = mUserModel.GetUser().ActiveRegion;
			int shopNumber = RegionHelper.DecideShopNumber (region);
			string shopName = RegionHelper.DecideShopName (shopNumber);
			ChangeLocationButton.Text = "You're in " + region + "(Bluemart - " + shopName + " Area). Tap To Change.";
			AddressExplanationLabel.Text = "Your Bluemart - " + shopName + " Adresses :";
			var addressList = mAddressModel.GetAddressList (shopNumber);

			foreach (var address in addressList) {
				if (address != null) {
					if (address.IsActive) {
						mActiveAddressCell = new AddressCell (address, this);
						StackLayout2.Children.Add (mActiveAddressCell.View);	
					}
					else
						StackLayout2.Children.Add (new AddressCell (address, this).View);	
				}
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
			ChangeLocationButton.WidthRequest = MyDevice.ScreenWidth - MyDevice.ViewPadding * 2;
			AddAddressButton.WidthRequest = MyDevice.ScreenWidth - MyDevice.ViewPadding * 2;
		}
			
		private void OnChangeLocationButtonClicked( Object sender, EventArgs e )
		{
			mParent.Navigation.PopAsync ();
		}

		private void OnAddAddressButtonClicked( Object sender, EventArgs e )
		{
			mParent.LoadAddAddress ();
		}

		public void SwitchActiveAddress(AddressCell address)
		{
			mActiveAddressCell.mActiveAddressImage.IsVisible = false;
			mActiveAddressCell = address;
			mActiveAddressCell.mActiveAddressImage.IsVisible = true;
		}
	}
}



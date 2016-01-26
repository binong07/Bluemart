using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;
using bluemart.Models.Remote;
using bluemart.Common;
using bluemart.Models.Local;


namespace bluemart.MainViews
{
	public partial class MapView : ContentPage
	{		
		public Button StartShopingButton;
		public int selectedPinIndex;

		RootPage mParent;
		UserClass mUserModel;

		List<PinData> pinDatas;

		public void OnStartShopingButtonClicked(Object sender,EventArgs e )
		{
			string region="";
			switch (selectedPinIndex) {
			case 0:
				region = "JVC";
				break;
			case 1:
				region = "Tecom";
				break;
			case 2:
				region = "Greens";
				break;
			}
			mUserModel.AddActiveRegionToUser (region);
			CategoryModel.CategoryLocation = "bluemart.Models.Local.RegionClass";
			//mParent.mSettingsPage.PopulateListView();
			Navigation.PushAsync( mParent );
		}
		public void OnPinClicked(int index )
		{
			selectedPinIndex = index;
			StartShopingButton.BorderWidth = 2;
			StartShopingButton.BorderColor = MyDevice.BlueColor;
			StartShopingButton.Text = "Start Shopping at " + pinDatas[index].text;
			StartShopingButton.IsEnabled=true;
		}
		public MapView (RootPage parent, UserClass mUserModel)
		{
			this.mUserModel = mUserModel;
			InitializeComponent ();
			mParent = parent;
			NavigationPage.SetHasNavigationBar (this, false);

			//InitalizeMemberVariables ();
			pinDatas = new List<PinData>();
			/*pinDatas.Add (new PinData(25.082742, 55.147174,"bm_pin","Dubai Marina BlueMart"));
			pinDatas.Add (new PinData(25.099536, 55.178529,"bm_pin","Tecom BlueMart"));
			pinDatas.Add (new PinData(25.094988, 55.172659,"bm_pin","Greens BlueMart"));*/
			pinDatas.Add (new PinData(25.049560, 55.205282,"bm_pin","Blue Mart JVC"));
			var map = new ExtendedMap (new Position (25.20, 55.26), 20, pinDatas,this);

			/*var map = new Map(
				MapSpan.FromCenterAndRadius(
					new Position(25.20,55.26), Distance.FromMiles(20))) {
				IsShowingUser = true,
				HeightRequest = 100,
				WidthRequest = 960,
				VerticalOptions = LayoutOptions.FillAndExpand
			};


			var position = new Position(25.082742, 55.147174);
			var pin = new Pin {
				Type = PinType.Place,
				Position = position,
				Label = "Blue Mart Supermarket",
				Address = "Dubai Marina, Dubai - UAE"
			};
			pin.Clicked += (object sender, EventArgs e) => 
			{
				OnPinClicked(0);
			};
			map.Pins.Add(pin);

			position = new Position(25.099536, 55.178529);
			pin = new Pin {
				Type = PinType.Place,
				Position = position,
				Label = "Blue Mart",
				Address = "Tecom, Dubai - UAE"
			};
			pin.Clicked += (object sender, EventArgs e) => 
			{
				OnPinClicked(1);
			};
			map.Pins.Add(pin);

			position = new Position(25.046035, 55.232205);
			pin = new Pin {
				Type = PinType.Place,
				Position = position,
				Label = "Blue Mart",
				Address = "Motor City, Dubai - UAE"
			};
			pin.Clicked += (object sender, EventArgs e) => 
			{
				OnPinClicked(2);
			};
			map.Pins.Add(pin);
*/
			StartShopingButton = new Button { Text = "Choose nearest bluemart" , BackgroundColor= Color.White, BorderWidth=0, TextColor = MyDevice.RedColor};
			StartShopingButton.Clicked += OnStartShopingButtonClicked;
			StartShopingButton.IsEnabled=false;

			Content = new StackLayout { 
				Spacing = 0,
				Children = {
					map,
					StartShopingButton
				}};

		}
	}
}


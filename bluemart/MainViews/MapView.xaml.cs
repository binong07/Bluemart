using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;
using bluemart.Models.Remote;

namespace bluemart.MainViews
{
	public partial class MapView : ContentPage
	{		
		public Button StartShopingButton;
		public int selectedPinIndex;
		RootPage mParent;
		public void OnStartShopingButtonClicked(Object sender,EventArgs e )
		{
			CategoryModel.CategoryLocation = "asd";
			Navigation.PushAsync (mParent);
		}
		public void OnPinClicked(int index )
		{
			selectedPinIndex = index;
			StartShopingButton.BorderWidth = 2;
			StartShopingButton.BorderColor = MyDevice.BlueColor;
			StartShopingButton.Text = "Start Shoping";
			StartShopingButton.IsEnabled=true;
		}
		public MapView (RootPage parent)
		{
			InitializeComponent ();
			mParent = parent;
			NavigationPage.SetHasNavigationBar (this, false);

			//InitalizeMemberVariables ();

			var map = new Map(
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


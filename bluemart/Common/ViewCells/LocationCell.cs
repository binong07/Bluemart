using System;
using Xamarin.Forms;
using bluemart.Common.Utilities;

namespace bluemart.Common.ViewCells
{
	public class LocationCell : ViewCell
	{
		public LocationCell (string location)
		{

			var absLayout = new AbsoluteLayout (){ 
				BackgroundColor = Color.White,
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.Fill
			};

			var LocationLabel = new Label (){ 
				BackgroundColor = Color.White,
				TextColor = MyDevice.BlueColor,
				Text = location,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			absLayout.Children.Add (LocationLabel);

			View = absLayout;
		}
	}
}


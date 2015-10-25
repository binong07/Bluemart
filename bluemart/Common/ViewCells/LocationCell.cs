using System;
using Xamarin.Forms;
using bluemart.Common.Utilities;

namespace bluemart.Common.ViewCells
{
	public class LocationCell : ViewCell
	{		
		public Label mLabel;
		public LocationCell ()
		{
			mLabel = new Label ();
			mLabel.TextColor = MyDevice.BlueColor;
			mLabel.XAlign = TextAlignment.Center;
			mLabel.YAlign = TextAlignment.Center;
			mLabel.SetBinding (Label.TextProperty, "Location");
			this.View = mLabel;
		}
	}
}


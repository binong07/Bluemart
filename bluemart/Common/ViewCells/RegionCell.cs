using System;
using Xamarin.Forms;
using bluemart.Common.Utilities;

namespace bluemart.Common.ViewCells
{
	public class RegionCell : ViewCell
	{		
		public Label mLabel;
		public RegionCell ()
		{
			mLabel = new Label ();
			mLabel.TextColor = MyDevice.BlueColor;
			mLabel.XAlign = TextAlignment.Center;
			mLabel.YAlign = TextAlignment.Center;
			mLabel.SetBinding (Label.TextProperty, "Region");
			this.View = mLabel;
		}
	}
}


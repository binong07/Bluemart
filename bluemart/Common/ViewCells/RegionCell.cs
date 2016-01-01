using System;
using Xamarin.Forms;
using bluemart.Common.Utilities;

namespace bluemart.Common.ViewCells
{
	public class RegionCell : ViewCell
	{		
		public Label mLabel;
		//public static Label selectedLabel;
		public RegionCell (string regionText)
		{
			mLabel = new Label ();
			mLabel.TextColor = MyDevice.GreyColor;
			mLabel.HorizontalTextAlignment = TextAlignment.Center;
			mLabel.VerticalTextAlignment = TextAlignment.Center;

			this.View = mLabel;
		}
	}
}


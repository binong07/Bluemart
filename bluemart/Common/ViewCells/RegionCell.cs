using System;
using Xamarin.Forms;
using bluemart.Common.Utilities;

namespace bluemart.Common.ViewCells
{
	public class RegionCell : ViewCell
	{		
		public Label mLabel;
		//public static Label selectedLabel;
		public RegionCell ()
		{
			mLabel = new Label ();
			mLabel.TextColor = MyDevice.GreyColor;
			mLabel.XAlign = TextAlignment.Center;
			mLabel.YAlign = TextAlignment.Center;
			mLabel.SetBinding (Label.TextProperty, "Region");
			/*mLabel.GestureRecognizers.Add(new TapGestureRecognizer
				{
					Command = new Command(() =>
						{
							if(selectedLabel!=null)
								selectedLabel.BackgroundColor=Color.White;
							selectedLabel=mLabel;
							mLabel.BackgroundColor = MyDevice.PinkColor;
						})
				});
			

			mLabel.Unfocused += (sender, args) => {
				mLabel.BackgroundColor = Color.White;
			};
			SelectedItemChangedEventArgs*/
			this.View = mLabel;
		}
	}
}


using System;
using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Local;
using System.Threading.Tasks;
using bluemart.MainViews;

namespace bluemart.Common.ViewCells
{
	public class TrackCell : ViewCell
	{
		public Label mTotalPriceLabel;
		public Label mDateLabel;
		public Label mRegionLabel;
		public Label mStatusLabel;
		private TrackPage mRootPage;
		private StatusClass mStatus;

		public TrackCell (StatusClass status, TrackPage rootPage)
		{
			mRootPage = rootPage;
			mStatus = status;

			var mainLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize (600),
				HeightRequest = MyDevice.GetScaledSize (180),
				BackgroundColor = Color.White,
				Padding = 0
			};

			var backgroundImage = new Image () {
				WidthRequest = MyDevice.GetScaledSize (62),
				HeightRequest = MyDevice.GetScaledSize (82),
				Aspect = Aspect.Fill,
				Source = "TrackPage_TrackBackground"
			};

			var totalPriceLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (455),
				HeightRequest = MyDevice.GetScaledSize (26),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Start,
				TextColor = Color.FromRgb (98, 98, 98),
				Text = "Total Price: " + status.TotalPrice + " AED",
				FontSize = MyDevice.FontSizeMicro	
			};

			DateTime statusDate = DateTime.Now;

			DateTime.TryParse(status.Date,out statusDate);				

			var dateLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (455),
				HeightRequest = MyDevice.GetScaledSize (26),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Start,
				TextColor = Color.FromRgb (98, 98, 98),
				Text = "Date: " + statusDate.ToString ("MM/dd/yyyy") + " - Time: " + statusDate.ToString ("hh:mm:ss"),
				FontSize = MyDevice.FontSizeMicro	
			};

			var regionLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (455),
				HeightRequest = MyDevice.GetScaledSize (26),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Start,
				TextColor = Color.FromRgb (98, 98, 98),
				Text = "Region: " + status.Region,
				FontSize = MyDevice.FontSizeMicro	
			};

			var statusLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (455),
				HeightRequest = MyDevice.GetScaledSize (50),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Start,
				TextColor = Color.FromRgb (98, 98, 98),
				Text = status.Status,
				FontSize = MyDevice.FontSizeMicro	
			};

			var line = new BoxView () {
				WidthRequest = MyDevice.GetScaledSize (600),
				HeightRequest = MyDevice.GetScaledSize (1),
				Color = Color.FromRgb (181, 185, 187)
			};

			mainLayout.Children.Add (backgroundImage,
				Constraint.Constant (MyDevice.GetScaledSize (42)),
				Constraint.Constant (MyDevice.GetScaledSize (42))
			);

			mainLayout.Children.Add (totalPriceLabel,
				Constraint.Constant (MyDevice.GetScaledSize (140)),
				Constraint.Constant (MyDevice.GetScaledSize (26))
			);

			mainLayout.Children.Add (dateLabel,
				Constraint.RelativeToView (totalPriceLabel, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (totalPriceLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (2);	
				})
			);

			mainLayout.Children.Add (regionLabel,
				Constraint.RelativeToView (dateLabel, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (dateLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (2);	
				})
			);

			mainLayout.Children.Add (statusLabel,
				Constraint.RelativeToView (regionLabel, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (regionLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (2);	
				})
			);

			mainLayout.Children.Add (line,
				Constraint.Constant (0),
				Constraint.Constant (MyDevice.GetScaledSize (179))
			);

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (sender, e) => {
				mRootPage.mParent.LoadReceiptPage (mStatus);
			};
			mainLayout.GestureRecognizers.Add (tapGestureRecognizer);

			this.View = mainLayout;
		}
	}
}


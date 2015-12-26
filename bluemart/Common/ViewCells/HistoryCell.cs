using System;
using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Local;
using System.Threading.Tasks;
using bluemart.MainViews;

namespace bluemart.Common.ViewCells
{
	public class HistoryCell : ViewCell
	{
		public Label mTotalPriceLabel;
		public Label mDateLabel;
		public Label mRegionLabel;
		private TrackPage mRootPage;
		private HistoryClass mHistoryClass;

		public HistoryCell (HistoryClass history, TrackPage rootPage)
		{
			mRootPage = rootPage;
			mHistoryClass = history;

			var mainLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(600),
				HeightRequest = MyDevice.GetScaledSize(138),
				BackgroundColor = Color.White,
				Padding = 0
			};

			var backgroundImage = new Image () {
				WidthRequest = MyDevice.GetScaledSize(32),
				HeightRequest = MyDevice.GetScaledSize(32),
				Aspect = Aspect.Fill,
				Source = "TrackPage_HistroyBackground"
			};

			var totalPriceLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (455),
				HeightRequest = MyDevice.GetScaledSize(26),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Start,
				TextColor = Color.FromRgb(98,98,98),
				Text = "Total Price: " + history.TotalPrice + " AED",
				FontSize = MyDevice.FontSizeMicro	
			};

			DateTime historyDate = DateTime.Now;

			DateTime.TryParse(history.Date,out historyDate);

			var dateLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (455),
				HeightRequest = MyDevice.GetScaledSize(26),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Start,
				TextColor = Color.FromRgb(98,98,98),
				Text = "Date: " + historyDate.ToString("MM/dd/yyyy") +" - Time: " + historyDate.ToString("hh:mm:ss"),
				FontSize = MyDevice.FontSizeMicro	
			};

			var regionLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (455),
				HeightRequest = MyDevice.GetScaledSize(26),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Start,
				TextColor = Color.FromRgb(98,98,98),
				Text = "Region: " + history.Region,
				FontSize = MyDevice.FontSizeMicro	
			};

			var line = new BoxView () {
				WidthRequest = MyDevice.GetScaledSize(600),
				HeightRequest = MyDevice.GetScaledSize(1),
				Color = Color.FromRgb(181,185,187)
			};

			mainLayout.Children.Add (backgroundImage,
				Constraint.Constant(MyDevice.GetScaledSize(51)),
				Constraint.Constant(MyDevice.GetScaledSize(51))
			);

			mainLayout.Children.Add (totalPriceLabel,
				Constraint.Constant(MyDevice.GetScaledSize(140)),
				Constraint.Constant(MyDevice.GetScaledSize(28))
			);

			mainLayout.Children.Add (dateLabel,
				Constraint.RelativeToView (totalPriceLabel, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (totalPriceLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(2);	
				})
			);

			mainLayout.Children.Add (regionLabel,
				Constraint.RelativeToView (dateLabel, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (dateLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(2);	
				})
			);

			mainLayout.Children.Add (line,
				Constraint.Constant(0),
				Constraint.Constant(MyDevice.GetScaledSize(137))
			);

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped +=  (sender, e) => {
				mRootPage.mParent.LoadReceiptPage(mHistoryClass);
			};
			mainLayout.GestureRecognizers.Add (tapGestureRecognizer);

			this.View = mainLayout;

		}			
	}
}


using System;
using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Local;
using System.Threading.Tasks;
using bluemart.MainViews;
using bluemart.Common.Objects;

namespace bluemart.Common.ViewCells
{
	public class ReceiptCell : ViewCell
	{
		public Label mTotalPriceLabel;
		public Label mDateLabel;
		public Label mRegionLabel;
		private TrackPage mRootPage;
		private HistoryClass mHistoryClass;

		public ReceiptCell (int i, Product product)
		{
			double height = MyDevice.GetScaledSize (34);
			Color bgColor;
			if (i % 2 == 0)
				bgColor = Color.White;
			else
				bgColor = Color.FromRgb (236, 236, 236);
			
			var mainLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(571),
				HeightRequest = MyDevice.GetScaledSize(height),
				BackgroundColor = bgColor,
				Padding = 0
			};


			var quantityLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (82),
				HeightRequest = MyDevice.GetScaledSize(height),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = product.ProductNumberInCart.ToString(),
				FontSize = MyDevice.FontSizeMicro
			};

			var productLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (189),
				HeightRequest = MyDevice.GetScaledSize(height),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = product.Name,
				FontSize = MyDevice.FontSizeMicro
			};

			var descriptionLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (109),
				HeightRequest = MyDevice.GetScaledSize(height),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = product.Quantity,
				FontSize = MyDevice.FontSizeMicro
			};

			var costLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (103),
				HeightRequest = MyDevice.GetScaledSize(height),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = product.Price.ToString(),
				FontSize = MyDevice.FontSizeMicro
			};


			mainLayout.Children.Add (quantityLabel,
				Constraint.Constant(0),
				Constraint.Constant(0)
			);

			mainLayout.Children.Add (productLabel,
				Constraint.RelativeToView (quantityLabel, (p, sibling) => {
					return sibling.Bounds.Right + MyDevice.GetScaledSize(26);	
				}),
				Constraint.RelativeToView (quantityLabel, (p, sibling) => {
					return sibling.Bounds.Top;	
				})
			);

			mainLayout.Children.Add (descriptionLabel,
				Constraint.RelativeToView (productLabel, (p, sibling) => {
					return sibling.Bounds.Right;	
				}),
				Constraint.RelativeToView (productLabel, (p, sibling) => {
					return sibling.Bounds.Top;	
				})
			);

			mainLayout.Children.Add (costLabel,
				Constraint.RelativeToView (descriptionLabel, (p, sibling) => {
					return sibling.Bounds.Right + MyDevice.GetScaledSize(62);	
				}),
				Constraint.RelativeToView (descriptionLabel, (p, sibling) => {
					return sibling.Bounds.Top;	
				})
			);

			this.View = mainLayout;
		}


	}
}



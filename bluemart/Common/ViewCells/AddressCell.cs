using System;
using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Local;
using System.Threading.Tasks;
using bluemart.MainViews;
using FFImageLoading.Forms;

namespace bluemart.Common.ViewCells
{
	public class AddressCell : ViewCell
	{
		public Label mNameLabel;
		public Label mAddressLabel;
		public Label mPhoneLabel;
		public Label mEditLabel;
		public RelativeLayout mActiveAddressImage;
		public AddressClass mAddressClass;
		public AddressClass mAddressModel = new AddressClass();
		private SettingsPage mRootPage;

		public AddressCell (AddressClass address, SettingsPage rootPage)
		{
			mRootPage = rootPage;
			mAddressClass = address;

			var mainLayout = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(600),
				HeightRequest = MyDevice.GetScaledSize(122),
				Padding = 0
			};

			var backgroundImage = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize(600),
				HeightRequest = MyDevice.GetScaledSize(122),
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false,
				Source = "SettingsPage_AddressCellBackground.png"
			};

			var editButton = new Label () {
				WidthRequest = MyDevice.GetScaledSize (48),
				HeightRequest = MyDevice.GetScaledSize(68),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.End,
				TextColor = Color.FromRgb(98,98,98),
				Text = "edit",
				FontSize = MyDevice.FontSizeMicro
			};

			mActiveAddressImage = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(82),
				HeightRequest = MyDevice.GetScaledSize(90),
				BackgroundColor = Color.White
			};

			var nameLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (300),
				HeightRequest = MyDevice.GetScaledSize(28),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = address.Name,
				FontSize = MyDevice.FontSizeSmall
			};

			var addressLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (360),
				HeightRequest = MyDevice.GetScaledSize(28),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = address.Address,
				FontSize = MyDevice.FontSizeSmall
			};

			var phoneLabel = new Label () {
				WidthRequest = MyDevice.GetScaledSize (360),
				HeightRequest = MyDevice.GetScaledSize(28),
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(98,98,98),
				Text = address.PhoneNumber,
				FontSize = MyDevice.FontSizeSmall
			};

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (sender, e) => {


				mAddressModel.MakeActive(mAddressClass);
				mRootPage.SwitchActiveAddress(this);

			};
			mainLayout.GestureRecognizers.Add (tapGestureRecognizer);

			var editTapGestureRecognizer = new TapGestureRecognizer ();
			editTapGestureRecognizer.Tapped += (sender, e) => {
				mRootPage.mParent.LoadAddAddress(mAddressClass);
			};
			editButton.GestureRecognizers.Add (editTapGestureRecognizer);

			mainLayout.Children.Add (backgroundImage, 
				Constraint.Constant (0),
				Constraint.Constant (0)
			);

			mainLayout.Children.Add (editButton,
				Constraint.RelativeToView (backgroundImage, (p, sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize (26);	
				}),
				Constraint.RelativeToView (backgroundImage, (p, sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize (26);	
				})
			);

			mainLayout.Children.Add (mActiveAddressImage,
				Constraint.RelativeToView (backgroundImage, (p, sibling) => {
					return sibling.Bounds.Right - MyDevice.GetScaledSize (105);	
				}),
				Constraint.RelativeToView (backgroundImage, (p, sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize (13);	
				})
			);

			mainLayout.Children.Add (nameLabel,
				Constraint.RelativeToView (backgroundImage, (p, sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize (105);	
				}),
				Constraint.RelativeToView (backgroundImage, (p, sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize (16);	
				})
			);
			mainLayout.Children.Add (addressLabel,
				Constraint.RelativeToView (nameLabel, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (nameLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (2);	
				})
			);
			mainLayout.Children.Add (phoneLabel,
				Constraint.RelativeToView (addressLabel, (p, sibling) => {
					return sibling.Bounds.Left;	
				}),
				Constraint.RelativeToView (addressLabel, (p, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (2);	
				})
			);

			mActiveAddressImage.IsVisible = !mAddressClass.IsActive;


			this.View = mainLayout;
		}
	}
}


using System;
using XLabs.Forms.Controls;
using TwinTechs.Controls;
using Xamarin.Forms;
using bluemart.Common.Objects;
using FFImageLoading.Forms;
using bluemart.Common.Utilities;

namespace bluemart
{
	public class ProductCellNew : FastGridCell
	{
		public CachedImage mRemoveFavoriteImage;
		public CachedImage mAddFavoriteImage;
		public CachedImage mProductImage;
		public CachedImage mBorderImage;
		public CachedImage mProductForegroundImage;

		private Label mProductNumberLabel;
		public Product mProduct;
		private bool bIsFavorite;
		public Page mParent;

		private RelativeLayout mFavoriteButton;
		private RelativeLayout mMinusButton;
		private RelativeLayout mPlusButton;
		private RelativeLayout mainRelativeLayout;
		private Label productNameLabel ;
		private Label productQuantityLabel;
		private Label productPriceLabel;
		public ProductCellNew ()
		{
		}
		protected override void SetupCell (bool isRecycled)
		{
			if (product != null)
			{
				mProductImage.Source = product.ProductImagePath;	
				productNameLabel.Text = product.Name;
				productQuantityLabel.Text = product.Quantity;
				productPriceLabel.Text = "AED " + product.Price.ToString ();
			}
		}

		protected override void InitializeCell ()
		{
			mainRelativeLayout = new RelativeLayout(){
				Padding = 0,
				WidthRequest = MyDevice.GetScaledSize (299),
				HeightRequest = MyDevice.GetScaledSize (377),
				BackgroundColor = Color.White
			};

			mProductImage = new CachedImage ()
			{
				WidthRequest = MyDevice.GetScaledSize(250),
				HeightRequest = MyDevice.GetScaledSize(198),
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false
			};
			productNameLabel = new Label (){ 
				FontSize = MyDevice.FontSizeMicro, 
				TextColor = Color.FromRgb(77,77,77), 
				HorizontalTextAlignment=TextAlignment.Start, 
				VerticalTextAlignment = TextAlignment.Center,
				WidthRequest = MyDevice.GetScaledSize(240),
				HeightRequest = MyDevice.GetScaledSize(60)
			};

			productQuantityLabel = new Label (){ 
				FontSize = MyDevice.FontSizeMicro,
				VerticalTextAlignment = TextAlignment.Start, 
				HorizontalTextAlignment = TextAlignment.Start, 
				TextColor = Color.FromRgb(176,176,176),
				WidthRequest = MyDevice.GetScaledSize(111),
				HeightRequest = MyDevice.GetScaledSize(22),
				FontAttributes = FontAttributes.Italic
			};

			productPriceLabel = new Label (){ 
				FontSize = MyDevice.FontSizeMicro, 
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Start,
				TextColor = Color.FromRgb(213,53,53),
				WidthRequest = MyDevice.GetScaledSize(111),
				HeightRequest = MyDevice.GetScaledSize(22),
				FontAttributes = FontAttributes.Bold
			};

			mFavoriteButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(74),
				HeightRequest = MyDevice.GetScaledSize(65)
			};



			mRemoveFavoriteImage = new CachedImage()
			{
				WidthRequest = MyDevice.GetScaledSize(42),
				HeightRequest = MyDevice.GetScaledSize(35),
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				Source = "CartPage_RemoveFavorites",
				IsVisible = false,
				FadeAnimationEnabled = false
			};

			mAddFavoriteImage = new CachedImage()
			{
				WidthRequest = MyDevice.GetScaledSize(42),
				HeightRequest = MyDevice.GetScaledSize(35),
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				Source = "CartPage_AddFavorites",
				IsVisible = false,
				FadeAnimationEnabled = false		
			};

			mProductForegroundImage = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize (300),
				HeightRequest = MyDevice.GetScaledSize (377),
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				Source = "ProductsPage_ProductForeground",
				IsVisible = false,
			};

			mProductNumberLabel = new Label (){ 
				FontSize = MyDevice.FontSizeMedium,
				VerticalTextAlignment = TextAlignment.Center, 
				HorizontalTextAlignment = TextAlignment.Center, 
				TextColor = Color.FromRgb(117,117,117),
				WidthRequest = MyDevice.GetScaledSize(78),
				HeightRequest = MyDevice.GetScaledSize(55),
				IsVisible = false
			};

			var addButton = new Label () {
				WidthRequest = MyDevice.GetScaledSize(118),
				HeightRequest = MyDevice.GetScaledSize(64),
				BackgroundColor = Color.FromRgb(213,53,53),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				Text = "ADD",
				TextColor = Color.White,
				FontSize = MyDevice.FontSizeSmall
			};

			mMinusButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(64),
				HeightRequest = MyDevice.GetScaledSize(64)
			};

			mPlusButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(64),
				HeightRequest = MyDevice.GetScaledSize(64)
			};

			mainRelativeLayout.Children.Add (mProductImage,
				Constraint.Constant(MyDevice.GetScaledSize(24)),
				Constraint.Constant(MyDevice.GetScaledSize(52))
			);

			mainRelativeLayout.Children.Add (productNameLabel,
				Constraint.RelativeToView(mProductImage, (p,sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize(5);
				}),
				Constraint.RelativeToView(mProductImage, (p,sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(5);
				})
			);

			mainRelativeLayout.Children.Add (productQuantityLabel,
				Constraint.RelativeToView(productNameLabel, (p,sibling) => {
					return sibling.Bounds.Left;
				}),
				Constraint.RelativeToView(productNameLabel, (p,sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(5);
				})
			);

			mainRelativeLayout.Children.Add (productPriceLabel,
				Constraint.RelativeToView(productQuantityLabel, (p,sibling) => {
					return sibling.Bounds.Left;
				}),
				Constraint.RelativeToView(productQuantityLabel, (p,sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize(5);
				})
			);

			mainRelativeLayout.Children.Add (addButton,
				Constraint.RelativeToView(mProductForegroundImage, (p,sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize(159);
				}),
				Constraint.RelativeToView(mProductForegroundImage, (p,sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize(307);
				})
			);

			mainRelativeLayout.Children.Add (mProductForegroundImage,
				Constraint.Constant (MyDevice.GetScaledSize(0)),
				Constraint.Constant (MyDevice.GetScaledSize(0))
			);

			mainRelativeLayout.Children.Add (mProductNumberLabel,
				Constraint.RelativeToView(mProductForegroundImage, (p,sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize(113);
				}),
				Constraint.RelativeToView(mProductForegroundImage, (p,sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize(162);
				})
			);

			mainRelativeLayout.Children.Add (mFavoriteButton,
				Constraint.RelativeToView(mProductForegroundImage, (p,sibling) => {
					return sibling.Bounds.Right - MyDevice.GetScaledSize(74);
				}),
				Constraint.RelativeToView(mProductForegroundImage, (p,sibling) => {
					return sibling.Bounds.Top;
				})
			);

			mainRelativeLayout.Children.Add (mRemoveFavoriteImage,
				Constraint.RelativeToView(mProductForegroundImage, (p,sibling) => {
					return sibling.Bounds.Right - MyDevice.GetScaledSize(58);
				}),
				Constraint.RelativeToView(mProductForegroundImage, (p,sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize(19);
				})
			);

			mainRelativeLayout.Children.Add (mAddFavoriteImage,
				Constraint.RelativeToView(mProductForegroundImage, (p,sibling) => {
					return sibling.Bounds.Right - MyDevice.GetScaledSize(58);
				}),
				Constraint.RelativeToView(mProductForegroundImage, (p,sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize(19);
				})
			);

			//simpleLayout.Children.Add (_image, new Rectangle (0, 0, width, height - 30));
			//simpleLayout.Children.Add (_titleLabel, new Rectangle (0, height - 30, width, 30));
			View = mainRelativeLayout;
		}

		public Product product {
			get { return (Product)BindingContext; }
		}
	}
}


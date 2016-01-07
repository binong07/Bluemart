using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using bluemart.Common.Objects;
using bluemart.MainViews;
using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Local;
using System.IO;
using System.Reflection;
using PCLStorage;
using System.Linq;
using FFImageLoading.Forms;

using XLabs.Forms.Controls;
using TwinTechs.Extensions;

namespace bluemart.Common.ViewCells
{
	public class ProductCell : ViewCell
	{
		public CachedImage mRemoveFavoriteImage;
		public CachedImage mAddFavoriteImage;
		public CachedImage mProductImage;
		public Image mBorderImage;
		public CachedImage mProductForegroundImage;

		private Label mProductNumberLabel;
		public Product mProduct;
		private FavoritesClass mFavoriteModel;
		private bool bIsFavorite;
		public Page mParent;
		private RootPage mRootPage;

		public Stream mProductImageStream;
		public Stream mBorderStream;
		public Stream mRemoveFavoriteStream;
		public Stream mAddFavoriteStream;
		public Stream mProductForegroundStream;

		public bool bIsImageSet=false;
		public ProductCell mPairCell = null;
		private RelativeLayout mFavoriteButton;
		private RelativeLayout mMinusButton;
		private RelativeLayout mPlusButton;
		private RelativeLayout mainRelativeLayout;

		public ProductCell (Grid parentGrid, Product product, Page parent)
		{		

			double width = (MyDevice.ScreenWidth-parentGrid.ColumnSpacing-MyDevice.ViewPadding)/2;
			mParent = parent;
			SetRootPage ();
			//change PriceLabel
			//mQuantity = Convert.ToInt32 (product.Quantity.Split (' ') [0]);
			//mQuantityLabel = product.Quantity.Split (' ') [1];

			if (Cart.ProductsInCart.Count != 0) {
				foreach (Product p in Cart.ProductsInCart) {
					if (p.ProductID == product.ProductID) {
						mProduct = p;
						break;
					}
					else
						mProduct = product;
				}
			} else
				mProduct = product;
			mFavoriteModel = new FavoritesClass ();


			bIsFavorite = mFavoriteModel.IsProductFavorite (product.ProductID);



			mainRelativeLayout = new RelativeLayout(){
				Padding = 0,
				WidthRequest = MyDevice.GetScaledSize (300),
				HeightRequest = MyDevice.GetScaledSize (377),
				BackgroundColor = Color.White
			};

			/*mBorderImage = new Image ()
			{
				WidthRequest = MyDevice.GetScaledSize (300),
				HeightRequest = MyDevice.GetScaledSize (377),
				Aspect = Aspect.Fill
			};*/					

			mProductImage = new CachedImage ()
			{
				WidthRequest = MyDevice.GetScaledSize(250),
				HeightRequest = MyDevice.GetScaledSize(198),
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				Source = mProduct.ProductImageName,
				FadeAnimationEnabled = false
			};

			/*var background = new CachedImage () {
				WidthRequest = MyDevice.GetScaledSize (300),
				HeightRequest = MyDevice.GetScaledSize (377),
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				Source = "ProductsPage_ProductCell"
			};*/


			Label productNameLabel = new Label (){ 
				FontSize = MyDevice.FontSizeMicro, 
				TextColor = Color.FromRgb(77,77,77), 
				HorizontalTextAlignment=TextAlignment.Start, 
				VerticalTextAlignment = TextAlignment.Center,
				Text = product.Name,
				WidthRequest = MyDevice.GetScaledSize(240),
				HeightRequest = MyDevice.GetScaledSize(60)
			};

			Label productQuantityLabel = new Label (){ 
				FontSize = MyDevice.FontSizeMicro,
				VerticalTextAlignment = TextAlignment.Start, 
				HorizontalTextAlignment = TextAlignment.Start, 
				TextColor = Color.FromRgb(176,176,176),
				Text = product.Quantity,
				WidthRequest = MyDevice.GetScaledSize(111),
				HeightRequest = MyDevice.GetScaledSize(22),
				FontAttributes = FontAttributes.Italic
			};

			Label productPriceLabel = new Label (){ 
				FontSize = MyDevice.FontSizeMicro, 
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Start,
				TextColor = Color.FromRgb(213,53,53),
				Text = "AED " + product.Price.ToString(),
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

			var addButtonTapGestureRecognizer = new TapGestureRecognizer ();
			addButtonTapGestureRecognizer.Tapped += (sender, e) => {
				if( mProductNumberLabel.IsVisible )
					return;

				AddProductInCart ();
				ActivateAddMenu();
			};		
			addButton.GestureRecognizers.Add (addButtonTapGestureRecognizer);

			/*mainRelativeLayout.Children.Add (mBorderImage,
				Constraint.Constant (MyDevice.GetScaledSize(0)),
				Constraint.Constant (MyDevice.GetScaledSize(0))
			);*/

			/*mainRelativeLayout.Children.Add (background,
			Constraint.Constant(MyDevice.GetScaledSize(0)),
			Constraint.Constant(MyDevice.GetScaledSize(0))
			);*/

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



			AddTapRecognizers ();
			UpdateNumberLabel ();

			if (mProduct.ProductNumberInCart > 0)
				ActivateAddMenu ();

			if (bIsFavorite)
				ActivateRemoveFavorite ();
			else
				ActivateAddFavorite ();

			this.View = mainRelativeLayout;
		}

		private void ActivateAddMenu()
		{			
			/*mProductForegroundStream = new MemoryStream ();
			mRootPage.mProductCellForeground.Position = 0;
			mRootPage.mProductCellForeground.CopyToAsync(mProductForegroundStream);
			mProductForegroundStream.Position = 0;
			mProductForegroundImage.Source = StreamImageSource.FromStream(() => mProductForegroundStream);*/
			mProductNumberLabel.IsVisible = true;

			mainRelativeLayout.Children.Add (mMinusButton,
				Constraint.RelativeToView(mProductForegroundImage, (p,sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize(28);
				}),
				Constraint.RelativeToView(mProductForegroundImage, (p,sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize(152);
				})
			);

			mainRelativeLayout.Children.Add (mPlusButton,
				Constraint.RelativeToView(mProductForegroundImage, (p,sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize(209);
				}),
				Constraint.RelativeToView(mProductForegroundImage, (p,sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize(160);
				})
			);
		}

		public void DeactivateAddMenu()
		{
			//mProductForegroundImage.Source = null;
			mProductNumberLabel.IsVisible = false;

			mainRelativeLayout.Children.Remove (mPlusButton);
			mainRelativeLayout.Children.Remove (mMinusButton);
		}

		public void ActivateRemoveFavorite()
		{
			mRemoveFavoriteImage.IsVisible = true;
			/*mRemoveFavoriteStream = new MemoryStream ();
			mRootPage.mRemoveFavoritesImage.Position = 0;
			mRootPage.mRemoveFavoritesImage.CopyToAsync(mRemoveFavoriteStream);
			mRemoveFavoriteStream.Position = 0;
			mRemoveFavoriteImage.Source = StreamImageSource.FromStream (() => mRemoveFavoriteStream);*/
		}

		public void DeactivateRemoveFavorite()
		{
			mRemoveFavoriteImage.IsVisible = false;
			//mRemoveFavoriteImage.Source = null;
		}

		public void ActivateAddFavorite()
		{
			mAddFavoriteImage.IsVisible = true;
			/*mAddFavoriteStream = new MemoryStream ();
			mRootPage.mAddFavoritesImage.Position = 0;
			mRootPage.mAddFavoritesImage.CopyToAsync(mAddFavoriteStream);
			mAddFavoriteStream.Position = 0;
			mAddFavoriteImage.Source = StreamImageSource.FromStream (() => mAddFavoriteStream);*/
		}

		public void DeactivateAddFavorite()
		{
			mAddFavoriteImage.IsVisible = false;
			//mAddFavoriteImage.Source = null;
		}

		public void ProduceStreamsAndImages()
		{			
			/*mBorderStream = new MemoryStream();
			mRootPage.mBorderImage.Position = 0;
			mRootPage.mBorderImage.CopyToAsync(mBorderStream);
			mBorderStream.Position = 0;
			
			mBorderImage.Source = StreamImageSource.FromStream(() => mBorderStream);*/

			//mProductForegroundImage.Source = StreamImageSource.FromStream(() => mProductForegroundStream);
		}

		public void ProduceProductImages()
		{			
			System.Diagnostics.Debug.WriteLine (mProduct.ProductImageName);

			/*var file = mRootPage.mFolder.GetFileAsync (mProduct.ProductImagePath).Result;
			Stream stream = new MemoryStream();
			mProductImageStream = new MemoryStream ();
			try
			{
				stream = file.OpenAsync (FileAccess.ReadAndWrite).Result;
			}
			catch
			{				
			}

			stream.Position = 0;
			stream.CopyTo (mProductImageStream);
			stream.Dispose ();
			stream = null;

			mProductImageStream.Position = 0;*/

			//mProductImage.Source = StreamImageSource.FromStream (()=>mProductImageStream);
			mProductImage.Source = mProduct.ProductImageName;
		}

		public void ClearStreamsAndImages()
		{		
			/*if (bIsImageSet) {	
				mProductImageStream.Dispose ();									
			}*/
		}

		private void SetRootPage()
		{
			if( mParent is BrowseProductsPage)
			{	
				mRootPage = (mParent as BrowseProductsPage).mParent;
			}
			else if( mParent is FavoritesPage)
			{
				mRootPage = (mParent as FavoritesPage).mParent;
			}
			else if( mParent is SearchPage )
			{
				mRootPage = (mParent as SearchPage).mParent;
			}
		}

		private void AddTapRecognizers()
		{
			var addButtonTapGestureRecognizer = new TapGestureRecognizer ();
			addButtonTapGestureRecognizer.Tapped += async (sender, e) => {
				if( CheckIfSearchEntryIsFocused() )
					return;

				AddProductInCart();
				await Task.Delay(MyDevice.DelayTime);
			};

			mPlusButton.GestureRecognizers.Add (addButtonTapGestureRecognizer);
			//mInsideGrid2.Children[2].GestureRecognizers.Add (addButtonTapGestureRecognizer);

			var removeButtonTapGestureRecognizer = new TapGestureRecognizer ();
			removeButtonTapGestureRecognizer.Tapped += async (sender, e) => {
				if( CheckIfSearchEntryIsFocused() )
					return;

				RemoveProductFromCart();
				await Task.Delay(MyDevice.DelayTime);		
			};
			mMinusButton.GestureRecognizers.Add (removeButtonTapGestureRecognizer);
			//mInsideGrid2.Children [0].GestureRecognizers.Add (removeButtonTapGestureRecognizer);
			//	mRemoveImage.GestureRecognizers.Add (removeButtonTapGestureRecognizer);

			var favoriteButtonTapGestureRecognizer = new TapGestureRecognizer ();
			favoriteButtonTapGestureRecognizer.Tapped += (sender, e) => {
				if( CheckIfSearchEntryIsFocused() )
					return;
				mFavoriteModel.AddProductID(mProduct.ProductID);

				if( !bIsFavorite )
				{
					bIsFavorite = true;
					mFavoriteModel.AddProductID(mProduct.ProductID);
					ActivateRemoveFavorite();
					DeactivateAddFavorite();
					if(mPairCell != null )
					{
						mPairCell.ActivateRemoveFavorite();
						mPairCell.DeactivateAddFavorite();
					}
				}
				else
				{		
					bIsFavorite = false;
					mFavoriteModel.RemoveProductID(mProduct.ProductID);
					ActivateAddFavorite();
					DeactivateRemoveFavorite();
					if(mPairCell != null )
					{
						mPairCell.DeactivateRemoveFavorite();
						mPairCell.ActivateAddFavorite();
					}


					if( mParent is FavoritesPage )
					{
						FavoritesPage pa = mParent as FavoritesPage;
						//pa.RefreshFavoritesGrid();
					}
				}
			};
			mFavoriteButton.GestureRecognizers.Add (favoriteButtonTapGestureRecognizer);
			//mMainCellGrid.Children.ElementAt (0).GestureRecognizers.Add(favoriteButtonTapGestureRecognizer);
		}

		private void AddProductToFavorites()
		{

		}

		public void UpdateNumberLabel()
		{
			mProductNumberLabel.Text = mProduct.ProductNumberInCart.ToString ();
			//change PriceLabel
			//mProductNumberLabel.Text = mProduct.ProductNumberInCart.ToString () + " " + mQuantityLabel;
		}

		private void RemoveProductFromCart()
		{
			if (mProduct.ProductNumberInCart > 0) {
				mProduct.ProductNumberInCart--;
				//change PriceLabel
				//mProduct.ProductNumberInCart -= mQuantity;
				Cart.ProductTotalPrice -= mProduct.Price;

				if (mParent is BrowseProductsPage) {
					(mParent as BrowseProductsPage).UpdatePriceLabel ();
					(mParent as BrowseProductsPage).UpdateProductCountLabel ();
				} else if (mParent is FavoritesPage) {
					(mParent as FavoritesPage).UpdatePriceLabel ();
					(mParent as FavoritesPage).UpdateProductCountLabel ();
				} else if (mParent is SearchPage) {
					(mParent as SearchPage).UpdatePriceLabel ();
					(mParent as SearchPage).UpdateProductCountLabel ();
				}
			}
			if (mProduct.ProductNumberInCart == 0) {
				DeactivateAddMenu ();
				Cart.ProductsInCart.Remove (mProduct);
			}


			UpdateNumberLabel ();
			if (mPairCell != null)
				mPairCell.UpdateNumberLabel ();
		}

		private bool CheckIfSearchEntryIsFocused()
		{
			bool bFocused = false;



			return bFocused;
		}

		private void AddProductInCart()
		{
			if (!Cart.ProductsInCart.Contains (mProduct)) 
			{
				Cart.ProductsInCart.Add (mProduct);
			}

			mProduct.ProductNumberInCart++;
			//change PriceLabel		
			//mProduct.ProductNumberInCart += mQuantity;

			Cart.ProductTotalPrice += mProduct.Price;

			if (mParent is BrowseProductsPage) {
				(mParent as BrowseProductsPage).UpdatePriceLabel ();
				(mParent as BrowseProductsPage).UpdateProductCountLabel ();
			} else if (mParent is FavoritesPage) {
				(mParent as FavoritesPage).UpdatePriceLabel ();
				(mParent as FavoritesPage).UpdateProductCountLabel ();
			} else if (mParent is SearchPage) {
				(mParent as SearchPage).UpdatePriceLabel ();
				(mParent as SearchPage).UpdateProductCountLabel ();
			}


			UpdateNumberLabel ();
			if (mPairCell != null)
				mPairCell.UpdateNumberLabel ();			
		}
	}


}



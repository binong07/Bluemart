using System;
using XLabs.Forms.Controls;
using TwinTechs.Controls;
using Xamarin.Forms;
using bluemart.Common.Objects;
using FFImageLoading.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Local;
using bluemart.MainViews;
using System.Threading.Tasks;
//using bluemart.Models.Remote;
using System.Collections.Generic;

namespace bluemart
{
	public class ProductCellNew : FastGridCell
	{
		public CachedImage mRemoveFavoriteImage;
		public CachedImage mAddFavoriteImage;
		public Image mProductImage;
		public CachedImage mBorderImage;
		public CachedImage mProductForegroundImage;

		private Label mProductNumberLabel;
		public Product mProduct;
		private FavoritesClass mFavoriteModel;
		private bool bIsFavorite;
		public Page mParent;
		//private RootPage mRootPage;

		public bool bIsImageSet=false;
		public ProductCellNew mPairCell = null;

		private RelativeLayout mFavoriteButton;
		private RelativeLayout mMinusButton;
		private RelativeLayout mPlusButton;
		private RelativeLayout mainRelativeLayout;
		private Label productNameLabel ;
		private Label productQuantityLabel;
		private Label productPriceLabel;
		private Label addButton;

		//private static Queue<CachedImage> iosImageLoadingList;

		public ProductCellNew ()
		{
		}
		protected override void SetupCell (bool isRecycled)
		{
			if (product != null)
			{
				if (Device.OS == TargetPlatform.iOS)
				{
					System.Diagnostics.Debug.WriteLine ("SetupCell" + product.Name);
					/*if (iosImageLoadingList == null)
						iosImageLoadingList = new Queue<CachedImage> ();
					if (iosImageLoadingList.Count > 11) {
						
					}
					iosImageLoadingList.Enqueue (cac);


					if (mProductImage.IsLoading)
						mProductImage.Cancel ();*/

					//mProductImage.Source = null;  
				}

				  

				mParent = MyDevice.currentPage;
				//SetRootPage ();
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
				//System.Diagnostics.Debug.WriteLine (mProduct.ProductImagePath);
				mProductImage.Source = mProduct.ProductImagePath;    

				/*
				PCLStorage.IFolder folder=null;

				if (mParent is BrowseProductsPage) {
					folder = (mParent as BrowseProductsPage).mParent.mFolder;
				} else if (mParent is FavoritesPage) {
					folder = (mParent as FavoritesPage).mParent.mFolder;
				} else if (mParent is SearchPage) {
					folder = (mParent as SearchPage).mParent.mFolder;
				}

				if( folder.CheckExistsAsync(ProductModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + mProduct.ProductImageName).Result != PCLStorage.ExistenceCheckResult.NotFound)
					mProductImage.Source = ProductModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + mProduct.ProductImageName;
				else
					mProductImage.Source = ImageSource.FromResource("bluemart.SavedImages."+mProduct.ProductImageName);
*/


				productNameLabel.Text = mProduct.Name;
				productQuantityLabel.Text = mProduct.Quantity;
				productPriceLabel.Text = "AED " + mProduct.Price.ToString ();
				mFavoriteModel = new FavoritesClass ();
				bIsFavorite = mFavoriteModel.IsProductFavorite (mProduct.ProductID);

				UpdateNumberLabel ();

				if (mProduct.ProductNumberInCart > 0)
					ActivateAddMenu ();
				else
					DeactivateAddMenu ();
				if (bIsFavorite)
					AddFavorite ();
				else
					RemoveFavorite ();



			}
		}
		/*private void SetRootPage()
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
		}*/
		private void ActivateAddMenu()
		{            
			mProductNumberLabel.IsVisible = true;
			mProductForegroundImage.IsVisible = true;
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
			mProductNumberLabel.IsVisible = false;
			mProductForegroundImage.IsVisible = false;
			if(mainRelativeLayout.Children.Contains(mPlusButton))
				mainRelativeLayout.Children.Remove (mPlusButton);
			if(mainRelativeLayout.Children.Contains(mMinusButton))
				mainRelativeLayout.Children.Remove (mMinusButton);
		}

		public void RemoveFavorite()
		{
			mRemoveFavoriteImage.IsVisible = false;
			mAddFavoriteImage.IsVisible = true;
		}

		public void AddFavorite()
		{
			mRemoveFavoriteImage.IsVisible = true;
			mAddFavoriteImage.IsVisible = false;
		}

		protected override void InitializeCell ()
		{
			mainRelativeLayout = new RelativeLayout(){
				Padding = 0,
				WidthRequest = MyDevice.GetScaledSize (299),
				HeightRequest = MyDevice.GetScaledSize (377),
				BackgroundColor = Color.White
			};

			mProductImage = new Image ()
			{
				WidthRequest = MyDevice.GetScaledSize(250),
				HeightRequest = MyDevice.GetScaledSize(198),
				/*CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false,*/

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

			addButton = new Label () {
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

			View = mainRelativeLayout;
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

			var mainRelativeLayoutTapGestureRecognizer = new TapGestureRecognizer ();
			mainRelativeLayoutTapGestureRecognizer.NumberOfTapsRequired = 2;
			mainRelativeLayoutTapGestureRecognizer.Tapped += async (sender, e) => {
				if( CheckIfSearchEntryIsFocused() )
					return;
				AddProductInCart();
				await Task.Delay(MyDevice.DelayTime);
			};
			mainRelativeLayout.GestureRecognizers.Add (mainRelativeLayoutTapGestureRecognizer);

			var removeButtonTapGestureRecognizer = new TapGestureRecognizer ();
			removeButtonTapGestureRecognizer.Tapped += async (sender, e) => {
				if( CheckIfSearchEntryIsFocused() )
					return;

				RemoveProductFromCart();
				await Task.Delay(MyDevice.DelayTime);        
			};
			mMinusButton.GestureRecognizers.Add (removeButtonTapGestureRecognizer);

			var favoriteButtonTapGestureRecognizer = new TapGestureRecognizer ();
			favoriteButtonTapGestureRecognizer.Tapped += (sender, e) => {
				if( CheckIfSearchEntryIsFocused() )
					return;
				mFavoriteModel.AddProductID(mProduct.ProductID);

				if( !bIsFavorite )
				{
					bIsFavorite = true;
					mFavoriteModel.AddProductID(mProduct.ProductID);
					AddFavorite();
					if(mPairCell != null )
					{
						mPairCell.AddFavorite();
					}
				}
				else
				{        
					bIsFavorite = false;
					mFavoriteModel.RemoveProductID(mProduct.ProductID);
					RemoveFavorite();
					if(mPairCell != null )
					{
						mPairCell.RemoveFavorite();
					}
					/*if( mParent is FavoritesPage )
					{
						FavoritesPage pa = mParent as FavoritesPage;
					}*/
				}
			};
			mFavoriteButton.GestureRecognizers.Add (favoriteButtonTapGestureRecognizer);

			var addButtonnTapGestureRecognizer = new TapGestureRecognizer ();
			addButtonnTapGestureRecognizer.Tapped += (sender, e) => {
				if( mProductNumberLabel.IsVisible )
					return;

				AddProductInCart ();
				ActivateAddMenu();
			};		
			addButton.GestureRecognizers.Add (addButtonnTapGestureRecognizer);
		}

		private void AddProductToFavorites()
		{

		}

		public void UpdateNumberLabel()
		{
			mProductNumberLabel.Text = mProduct.ProductNumberInCart.ToString ();
		}

		private void RemoveProductFromCart()
		{
			if (mProduct.ProductNumberInCart > 0) {
				mProduct.ProductNumberInCart--;
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

		public Product product {
			get { return (Product)BindingContext; }
		}
	}
}
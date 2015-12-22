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


namespace bluemart.Common.ViewCells
{
	public class ProductCell : ViewCell
	{
		public Image mFavoriteImage;
		public Image mProductImage;
		public Image mBorderImage;
		public Image mProductForegroundImage;

		private Label mProductNumberLabel;
		public Product mProduct;
		private FavoritesClass mFavoriteModel;
		private bool bIsFavorite;
		public Page mParent;
		private RootPage mRootPage;

		public Stream mProductImageStream;
		public Stream mBorderStream;
		public Stream mFavoriteStream;
		public Stream mProductForegroundStream;

		public bool bIsImageSet=false;
		public ProductCell mPairCell = null;
		private RelativeLayout mFavoriteButton;

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

			var mainRelativeLayout = new RelativeLayout(){
				Padding = 0
			};

			mBorderImage = new Image ()
			{
				WidthRequest = MyDevice.GetScaledSize (300),
				HeightRequest = MyDevice.GetScaledSize (377),
				Aspect = Aspect.Fill
			};					

			mProductImage = new Image ()
			{
				WidthRequest = MyDevice.GetScaledSize(250),
				HeightRequest = MyDevice.GetScaledSize(198),
				Aspect = Aspect.Fill
			};

			Label productNameLabel = new Label (){ 
				FontSize = Device.GetNamedSize(NamedSize.Micro,typeof(Label)), 
				TextColor = Color.FromRgb(77,77,77), 
				HorizontalTextAlignment=TextAlignment.Start, 
				VerticalTextAlignment = TextAlignment.Center,
				Text = product.Name,
				WidthRequest = MyDevice.GetScaledSize(111),
				HeightRequest = MyDevice.GetScaledSize(60)
			};

			Label productQuantityLabel = new Label (){ 
				FontSize = Device.GetNamedSize(NamedSize.Micro,typeof(Label)),
				VerticalTextAlignment = TextAlignment.Start, 
				HorizontalTextAlignment = TextAlignment.Start, 
				TextColor = Color.FromRgb(176,176,176),
				Text = product.Quantity,
				WidthRequest = MyDevice.GetScaledSize(111),
				HeightRequest = MyDevice.GetScaledSize(18),
				FontAttributes = FontAttributes.Italic
			};

			Label productPriceLabel = new Label (){ 
				FontSize = Device.GetNamedSize(NamedSize.Micro ,typeof(Label)), 
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Start,
				TextColor = Color.FromRgb(213,53,53),
				Text = "AED " + product.Price.ToString(),
				WidthRequest = MyDevice.GetScaledSize(111),
				HeightRequest = MyDevice.GetScaledSize(20),
				FontAttributes = FontAttributes.Bold
			};

			mFavoriteButton = new RelativeLayout () {
				WidthRequest = MyDevice.GetScaledSize(74),
				HeightRequest = MyDevice.GetScaledSize(65)
			};

			mFavoriteImage = new Image()
			{
				WidthRequest = MyDevice.GetScaledSize(42),
				HeightRequest = MyDevice.GetScaledSize(35),
				Aspect = Aspect.Fill			
			};

			mProductForegroundImage = new Image () {
				WidthRequest = MyDevice.GetScaledSize (300),
				HeightRequest = MyDevice.GetScaledSize (377),
				Aspect = Aspect.Fill
			};

			mProductNumberLabel = new Label (){ 
				FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label)),
				VerticalTextAlignment = TextAlignment.Center, 
				HorizontalTextAlignment = TextAlignment.Center, 
				TextColor = Color.FromRgb(117,117,117),
				WidthRequest = MyDevice.GetScaledSize(78),
				HeightRequest = MyDevice.GetScaledSize(55)
			};

			mainRelativeLayout.Children.Add (mBorderImage,
				Constraint.Constant (MyDevice.GetScaledSize(0)),
				Constraint.Constant (MyDevice.GetScaledSize(0))
			);

			mainRelativeLayout.Children.Add (mProductImage,
				Constraint.RelativeToView(mBorderImage, (p,sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize(24);
				}),
				Constraint.RelativeToView(mBorderImage, (p,sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize(52);
				})
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

			mainRelativeLayout.Children.Add (mProductForegroundImage,
				Constraint.Constant (MyDevice.GetScaledSize(0)),
				Constraint.Constant (MyDevice.GetScaledSize(0))
			);

			mainRelativeLayout.Children.Add (mProductNumberLabel,
				Constraint.RelativeToView(mBorderImage, (p,sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize(113);
				}),
				Constraint.RelativeToView(mBorderImage, (p,sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize(162);
				})
			);

			mainRelativeLayout.Children.Add (mFavoriteButton,
				Constraint.RelativeToView(mBorderImage, (p,sibling) => {
					return sibling.Bounds.Right - MyDevice.GetScaledSize(74);
				}),
				Constraint.RelativeToView(mBorderImage, (p,sibling) => {
					return sibling.Bounds.Top;
				})
			);

			mainRelativeLayout.Children.Add (mFavoriteImage,
				Constraint.RelativeToView(mBorderImage, (p,sibling) => {
					return sibling.Bounds.Right - MyDevice.GetScaledSize(58);
				}),
				Constraint.RelativeToView(mBorderImage, (p,sibling) => {
					return sibling.Bounds.Top + MyDevice.GetScaledSize(19);
				})
			);

			AddTapRecognizers ();
			UpdateNumberLabel ();
			/*mMainCellGrid = new Grid (){VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Transparent, Padding = 0, RowSpacing = 0, ColumnSpacing =0 };

			mMainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = Device.GetNamedSize(NamedSize.Medium,typeof(Label))*2 });
			mMainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = GridLength.Auto });
			mMainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = Device.GetNamedSize(NamedSize.Small,typeof(Label))*2 });
			mMainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = Device.GetNamedSize(NamedSize.Small,typeof(Label))*2 });
			mMainCellGrid.ColumnDefinitions.Add (new ColumnDefinition (){ Width =  width });

			#region row1
			Grid insideGrid1 = new Grid(){VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand,Padding = 0, RowSpacing = 0, ColumnSpacing = MyDevice.ScreenWidth*0.0083f};
			insideGrid1.Padding = new Thickness (0);
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = MyDevice.ViewPadding - insideGrid1.ColumnSpacing });
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - 2*MyDevice.ViewPadding) / 7 });
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - 2*MyDevice.ViewPadding) *6 / 7 });
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = MyDevice.ViewPadding - insideGrid1.ColumnSpacing });

			mFavoriteImage = new Image();
			mFavoriteImage.Aspect = Aspect.Fill;
			//insideGrid1.Children.Add(mFavoriteImage,1,0);

			Label productNameLabel = new Label (){ FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)), TextColor = Color.Black, HorizontalTextAlignment=TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

			productNameLabel.Text = product.Name;
			insideGrid1.Children.Add(productNameLabel,2,0);
			mMainCellGrid.Children.Add (insideGrid1, 0, 0);
			#endregion


			#region row2
			mProductImage = new Image ();
			mProductImage.HeightRequest = MyDevice.ScreenWidth*0.288f;
			mProductImage.WidthRequest = MyDevice.ScreenWidth*0.4740740741f;

			mMainCellGrid.Children.Add (mProductImage, 0, 1);
			#endregion
							

			#region row3
			Grid insideGrid3 = new Grid(){VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand,Padding = 0, RowSpacing = 0, ColumnSpacing = 0};
			insideGrid3.Padding = new Thickness (0);
			insideGrid3.ColumnDefinitions.Add( new ColumnDefinition() { Width = MyDevice.ViewPadding });
			insideGrid3.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding*2) / 2 });
			insideGrid3.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding*2) / 2 });
			//insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding*2) / 3 });
			insideGrid3.ColumnDefinitions.Add( new ColumnDefinition() { Width = MyDevice.ViewPadding });
			insideGrid3.RowDefinitions.Add( new RowDefinition() { Height = Device.GetNamedSize(NamedSize.Medium ,typeof(Label)) } );

			mInsideGrid1 = insideGrid3;

			Label productPriceLabel = new Label (){ FontSize = Device.GetNamedSize(NamedSize.Small ,typeof(Label)), HorizontalTextAlignment = TextAlignment.Start, TextColor = MyDevice.RedColor };
			productPriceLabel.Text = "AED " + product.Price.ToString();
			insideGrid3.Children.Add(productPriceLabel,1,0);



			Label productQuantityLabel = new Label (){ FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.End, TextColor = MyDevice.RedColor };
			productQuantityLabel.Text = product.Quantity;
			insideGrid3.Children.Add(productQuantityLabel,2,0);

			mMainCellGrid.Children.Add( insideGrid3, 0,2);
			#endregion


			#region row4insidegrid
			Grid insideGrid4 = new Grid(){VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = 0, RowSpacing = 0, ColumnSpacing = 0};
			mInsideGrid2 = insideGrid4;
			insideGrid4.Padding = new Thickness (0);
			insideGrid4.ColumnDefinitions.Add( new ColumnDefinition() { Width = MyDevice.ViewPadding/2 });
			insideGrid4.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding) / 4 });
			insideGrid4.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding) / 2 });//{ Width = ParentWidth - ((ParentHeight - ParentWidth) / 3 * 2)}
			insideGrid4.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding) / 4 });
			insideGrid4.ColumnDefinitions.Add( new ColumnDefinition() { Width = MyDevice.ViewPadding/2 });
			insideGrid4.RowDefinitions.Add( new RowDefinition() { Height = 2*Device.GetNamedSize(NamedSize.Small,typeof(Label)) } );				

			var removeImageLayout = new RelativeLayout(){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Padding = 0
			};

			insideGrid4.Children.Add (removeImageLayout,1,0);

			mProductNumberLabel = new Label(){VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label)), TextColor = MyDevice.RedColor };

			UpdateNumberLabel();
			insideGrid4.Children.Add (mProductNumberLabel,2,0);

			var addImageLayout = new RelativeLayout(){
				HorizontalOptions = LayoutOptions.Fill,
				Padding = 0
			};					

			insideGrid4.Children.Add (addImageLayout,3,0);

			mMainCellGrid.Children.Add( insideGrid4, 0, 3 );
			#endregion

			AddTapRecognizers ();

			//var decimalScreenWidth = new decimal (MyDevice.ScreenWidth);
			var borderWidth = Decimal.Multiply(new decimal(MyDevice.ScreenWidth),new decimal(0.4731481481f));
			var borderHeight = Decimal.Multiply(new decimal(MyDevice.ScreenWidth),new decimal(0.6074074074f));

			mBorderImage = new Image (){};
			mBorderImage.Aspect = Aspect.Fill;
			mBorderImage.WidthRequest = Decimal.ToDouble(borderWidth);
			mBorderImage.HeightRequest = Decimal.ToDouble(borderHeight);

			mainRelativeLayout.Children.Add (mBorderImage, 
				Constraint.RelativeToView (mMainCellGrid, (p, sibling) => {
					return sibling.Bounds.Left;
				}),
				Constraint.RelativeToView (mMainCellGrid, (p, sibling) => {
					return sibling.Bounds.Top;
				}),
				Constraint.RelativeToView (mMainCellGrid, (p, sibling) => {
					return sibling.Width;
				}),
				Constraint.RelativeToView (mMainCellGrid, (p, sibling) => {
					return sibling.Height;
				})
			);

			mainRelativeLayout.Children.Add(mFavoriteImage,
				Constraint.RelativeToView (mMainCellGrid, (p, sibling) => {
					return sibling.Bounds.Left + MyDevice.ScreenWidth*0.0329f;
				}),
				Constraint.RelativeToView (mMainCellGrid, (p, sibling) => {
					return sibling.Bounds.Top + MyDevice.ScreenWidth*0.0317f;
				}),
				Constraint.Constant( MyDevice.ScreenWidth * 0.0522f),
				Constraint.Constant( MyDevice.ScreenWidth * 0.0522f)
			);


			mainRelativeLayout.Children.Add(mMainCellGrid, Constraint.RelativeToParent(p => {
				return 0;	
			}));*/

			this.View = mainRelativeLayout;
		}


		public void ProduceStreamsAndImages()
		{			
			mBorderStream = new MemoryStream();
			mRootPage.mBorderImage.Position = 0;
			mRootPage.mBorderImage.CopyToAsync(mBorderStream);
			mBorderStream.Position = 0;

			mFavoriteStream = new MemoryStream ();
			mRootPage.mRemoveFavoritesImage.Position = 0;
			mRootPage.mRemoveFavoritesImage.CopyToAsync(mFavoriteStream);
			mFavoriteStream.Position = 0;

			mProductForegroundStream = new MemoryStream ();
			mRootPage.mProductCellForeground.Position = 0;
			mRootPage.mProductCellForeground.CopyToAsync(mProductForegroundStream);
			mProductForegroundStream.Position = 0;

			mFavoriteImage.Source = StreamImageSource.FromStream (() => mFavoriteStream);

			if (!bIsFavorite) {
				mFavoriteImage.IsVisible = false;
			}
			
			mBorderImage.Source = StreamImageSource.FromStream(() => mBorderStream);

			mProductForegroundImage.Source = StreamImageSource.FromStream(() => mProductForegroundStream);
		}

		public void ProduceProductImages()
		{			
			System.Diagnostics.Debug.WriteLine (mProduct.ProductImagePath);
			var file = mRootPage.mFolder.GetFileAsync (mProduct.ProductImagePath).Result;
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

			mProductImageStream.Position = 0;

			mProductImage.Source = StreamImageSource.FromStream (()=>mProductImageStream);
		}

		public void ClearStreamsAndImages()
		{		
			if (bIsImageSet) {	
				mProductImageStream.Dispose ();									
			}
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


			//mInsideGrid2.Children[2].GestureRecognizers.Add (addButtonTapGestureRecognizer);

			var removeButtonTapGestureRecognizer = new TapGestureRecognizer ();
			removeButtonTapGestureRecognizer.Tapped += async (sender, e) => {
				if( CheckIfSearchEntryIsFocused() )
					return;

				RemoveProductFromCart();
				await Task.Delay(MyDevice.DelayTime);		
			};
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
					mFavoriteImage.IsVisible =  true;
					if(mPairCell != null )
						mPairCell.mFavoriteImage.IsVisible = true;
				}
				else
				{		
					bIsFavorite = false;
					mFavoriteModel.RemoveProductID(mProduct.ProductID);
					mFavoriteImage.IsVisible =  false;
					if(mPairCell != null )
						mPairCell.mFavoriteImage.IsVisible = false;

					if( mParent is FavoritesPage )
					{
						FavoritesPage pa = mParent as FavoritesPage;
						pa.RefreshFavoritesGrid();
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

				if (mParent is BrowseProductsPage)
					(mParent as BrowseProductsPage).UpdatePriceLabel ();
				else if (mParent is FavoritesPage)
					(mParent as FavoritesPage).UpdatePriceLabel ();
				else if (mParent is SearchPage)
					(mParent as SearchPage).UpdatePriceLabel ();
			}
			if (mProduct.ProductNumberInCart == 0)
				Cart.ProductsInCart.Remove (mProduct);


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

			if (mParent is BrowseProductsPage)
				(mParent as BrowseProductsPage).UpdatePriceLabel ();
			else if (mParent is FavoritesPage)
				(mParent as FavoritesPage).UpdatePriceLabel ();
			else if (mParent is SearchPage)
				(mParent as SearchPage).UpdatePriceLabel ();
			
			UpdateNumberLabel ();
			if (mPairCell != null)
				mPairCell.UpdateNumberLabel ();
		}
	}


}



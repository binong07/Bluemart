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
		private Label mProductNumberLabel;
		public Product mProduct;
		private FavoritesClass mFavoriteModel;
		private bool bIsFavorite;
		public Page mParent;
		private RootPage mRootPage;
		private Grid mInsideGrid2;
		private Grid mInsideGrid1;
		public Stream mProductImageStream;
		public Stream mBorderStream;
		public Stream mFavoriteStream;
		private Grid mMainCellGrid;
		public bool bIsImageSet=false;



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
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Padding = 0
			};

			mMainCellGrid = new Grid (){VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Transparent, Padding = 0, RowSpacing = 0, ColumnSpacing =0 };

			mMainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = Device.GetNamedSize(NamedSize.Medium,typeof(Label))*2 });
			mMainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = GridLength.Auto });
			mMainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = Device.GetNamedSize(NamedSize.Small,typeof(Label))*2 });
			mMainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = Device.GetNamedSize(NamedSize.Small,typeof(Label))*2 });
			mMainCellGrid.ColumnDefinitions.Add (new ColumnDefinition (){ Width =  width });

			#region row1
			Grid insideGrid1 = new Grid(){VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand,Padding = 0, RowSpacing = 0, ColumnSpacing = 0};
			insideGrid1.Padding = new Thickness (0);
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = MyDevice.ViewPadding });
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding*2) / 7 });
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding*2) *6 / 7 });

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

			/*mFrame = new Frame { 				
				Padding = 2,
				OutlineColor = MyDevice.RedColor,
				BackgroundColor = MyDevice.RedColor,
				VerticalOptions = LayoutOptions.Start,
				//HeightRequest = mainCellGrid.Height,
				//WidthRequest = mainCellGrid.Width,
				Content = mMainCellGrid
			};*/


			mBorderImage = new Image (){};
			mBorderImage.Aspect = Aspect.Fill;
			mBorderImage.WidthRequest = MyDevice.ScreenWidth * 0.4731481481f;
			mBorderImage.HeightRequest = MyDevice.ScreenWidth * 0.6074074074f;

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
					return sibling.Bounds.Left + MyDevice.ScreenWidth*0.0275f;
				}),
				Constraint.RelativeToView (mMainCellGrid, (p, sibling) => {
					return sibling.Bounds.Top + MyDevice.ScreenWidth*0.024f;
				}),
				Constraint.Constant( MyDevice.ScreenWidth * 0.06f),
				Constraint.Constant( MyDevice.ScreenWidth * 0.06f)
			);


			mainRelativeLayout.Children.Add(mMainCellGrid, Constraint.RelativeToParent(p => {
				return 0;	
			}));

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

			mFavoriteImage.Source = StreamImageSource.FromStream (() => mFavoriteStream);

			if (!bIsFavorite) {
				mFavoriteImage.IsVisible = false;
			}
			
			mBorderImage.Source = StreamImageSource.FromStream(() => mBorderStream);
		}

		public void ProduceProductImages()
		{			
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
			mBorderStream.Dispose ();
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


			mInsideGrid2.Children[2].GestureRecognizers.Add (addButtonTapGestureRecognizer);

			var removeButtonTapGestureRecognizer = new TapGestureRecognizer ();
			removeButtonTapGestureRecognizer.Tapped += async (sender, e) => {
				if( CheckIfSearchEntryIsFocused() )
					return;

				RemoveProductFromCart();
				await Task.Delay(MyDevice.DelayTime);		
			};
			mInsideGrid2.Children [0].GestureRecognizers.Add (removeButtonTapGestureRecognizer);
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
				}
				else
				{		
					bIsFavorite = false;
					mFavoriteModel.RemoveProductID(mProduct.ProductID);
					mFavoriteImage.IsVisible =  false;

					if( mParent is FavoritesPage )
					{
						FavoritesPage pa = mParent as FavoritesPage;
						pa.RefreshFavoritesGrid();
					}
				}
			};
			mMainCellGrid.Children.ElementAt (0).GestureRecognizers.Add(favoriteButtonTapGestureRecognizer);
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
		}

		private bool CheckIfSearchEntryIsFocused()
		{
			bool bFocused = false;

			if (mParent is BrowseProductsPage)
				bFocused = (mParent as BrowseProductsPage).mParent.mTopNavigationBar.mSearchEntry.IsFocused;		

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
		}
	}


}



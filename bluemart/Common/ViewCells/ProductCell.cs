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

			mMainCellGrid = new Grid (){VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.White, Padding = 0, RowSpacing = 0, ColumnSpacing =0 };

			mMainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = Device.GetNamedSize(NamedSize.Medium,typeof(Label))*2 });
			mMainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = GridLength.Auto });
			mMainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = GridLength.Auto });
			mMainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = Device.GetNamedSize(NamedSize.Small,typeof(Label))*2 });
			mMainCellGrid.ColumnDefinitions.Add (new ColumnDefinition (){ Width =  width });

			Label productNameLabel = new Label (){ FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)), HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center , TextColor = MyDevice.RedColor, HorizontalTextAlignment=TextAlignment.Center };

			productNameLabel.Text = product.Name;
			mMainCellGrid.Children.Add (productNameLabel, 0, 0);
							

			#region row1insidegrid
			Grid insideGrid1 = new Grid(){VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand,Padding = 0, RowSpacing = 0, ColumnSpacing = 0};
			insideGrid1.Padding = new Thickness (0);
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = MyDevice.ViewPadding });
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding*2) / 3 });
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding*2) / 3 });
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding*2) / 3 });
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = MyDevice.ViewPadding });
			insideGrid1.RowDefinitions.Add( new RowDefinition() { Height = Device.GetNamedSize(NamedSize.Medium ,typeof(Label)) } );

			mInsideGrid1 = insideGrid1;

			Label productPriceLabel = new Label (){ FontSize = Device.GetNamedSize(NamedSize.Small ,typeof(Label)), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = MyDevice.RedColor };
			productPriceLabel.Text = "AED " + product.Price.ToString();
			insideGrid1.Children.Add(productPriceLabel,1,0);

			/*mFavoriteImage = new Image();		
			insideGrid1.Children.Add(mFavoriteImage,2,0);*/

			Label productQuantityLabel = new Label (){ FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)), HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start, TextColor = MyDevice.RedColor };
			productQuantityLabel.Text = product.Quantity;
			insideGrid1.Children.Add(productQuantityLabel,3,0);

			mMainCellGrid.Children.Add( insideGrid1, 0,1);
			#endregion


			mProductImage = new Image ();
			mProductImage.HeightRequest = width / 5 * 3;
			mProductImage.WidthRequest = width / 5 * 3;

			mMainCellGrid.Children.Add (mProductImage, 0, 2);

			#region row3insidegrid
			Grid insideGrid2 = new Grid(){VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = 0, RowSpacing = 0, ColumnSpacing = 0};
			mInsideGrid2 = insideGrid2;
			insideGrid2.Padding = new Thickness (0);
			insideGrid2.ColumnDefinitions.Add( new ColumnDefinition() { Width = MyDevice.ViewPadding/2 });
			insideGrid2.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding) / 4 });
			insideGrid2.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding) / 2 });//{ Width = ParentWidth - ((ParentHeight - ParentWidth) / 3 * 2)}
			insideGrid2.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding) / 4 });
			insideGrid2.ColumnDefinitions.Add( new ColumnDefinition() { Width = MyDevice.ViewPadding/2 });
			insideGrid2.RowDefinitions.Add( new RowDefinition() { Height = 2*Device.GetNamedSize(NamedSize.Small,typeof(Label)) } );				

			var removeImageLayout = new RelativeLayout(){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Padding = 0
			};

			insideGrid2.Children.Add (removeImageLayout,1,0);

			mProductNumberLabel = new Label(){VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label)), TextColor = MyDevice.RedColor };

			UpdateNumberLabel();
			insideGrid2.Children.Add (mProductNumberLabel,2,0);

			var addImageLayout = new RelativeLayout(){
				HorizontalOptions = LayoutOptions.Fill,
				Padding = 0
			};					

			insideGrid2.Children.Add (addImageLayout,3,0);

			mMainCellGrid.Children.Add( insideGrid2, 0, 3 );
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



			mainRelativeLayout.Children.Add(mMainCellGrid, Constraint.RelativeToParent(p => {
				return 0;	
			}));

			mBorderImage = new Image (){VerticalOptions = LayoutOptions.CenterAndExpand,HorizontalOptions = LayoutOptions.CenterAndExpand};
			mBorderImage.Aspect = Aspect.Fill;


			mainRelativeLayout.Children.Add (mBorderImage, 
				Constraint.RelativeToView (mMainCellGrid, (p, sibling) => {
					return sibling.Bounds.Left-3;
				}),
				Constraint.RelativeToView (mMainCellGrid, (p, sibling) => {
					return sibling.Bounds.Top-3;
				}),
				Constraint.RelativeToView (mMainCellGrid, (p, sibling) => {
					return width+6;
				}),
				Constraint.RelativeToView (mMainCellGrid, (p, sibling) => {
					return sibling.Height+6;
				})
			);


			this.View = mainRelativeLayout;
		}


		public async void ProduceStreamsAndImages()
		{			
			mBorderStream = new MemoryStream();
			mRootPage.mBorderImage.Position = 0;
			await mRootPage.mBorderImage.CopyToAsync(mBorderStream);
			mBorderStream.Position = 0;

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
			bIsImageSet = true;
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
			favoriteButtonTapGestureRecognizer.Tapped += async (sender, e) => {
				if( CheckIfSearchEntryIsFocused() )
					return;
				mFavoriteModel.AddProductID(mProduct.ProductID);
				bIsFavorite = true;
				/*if( !bIsFavorite )
				{
					mFavoriteImage.Opacity = 0.5f;
					mFavoriteModel.AddProductID(mProduct.ProductID);
					await Task.Delay(MyDevice.DelayTime);
					mFavoriteImage.Opacity = 1f;
					mFavoriteImage.Source = "bookmark_remove";
					bIsFavorite = true;
				}
				else
				{					
					mFavoriteImage.Opacity = 0.5f;
					mFavoriteModel.RemoveProductID(mProduct.ProductID);
					await Task.Delay(MyDevice.DelayTime);
					mFavoriteImage.Opacity = 1f;
					mFavoriteImage.Source = "bookmark_add";
					bIsFavorite = false;

					if( mParent is FavoritesPage )
					{
						FavoritesPage pa = mParent as FavoritesPage;
						pa.RefreshFavoritesGrid();
					}
				}*/
				//else if( mFavoriteImage.Source ==
			};
			//mInsideGrid1.Children [2].GestureRecognizers.Add (favoriteButtonTapGestureRecognizer);
			//mFavoriteImage.GestureRecognizers.Add (favoriteButtonTapGestureRecognizer);
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



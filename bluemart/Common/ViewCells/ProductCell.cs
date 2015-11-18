﻿using System;
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
		public Image mAddImage;
		public Image mRemoveImage;
		public Image mFavoriteImage;
		public Image mProductImage;
		private Label mProductNumberLabel;
		private Product mProduct;
		private FavoritesClass mFavoriteModel;
		private bool bIsFavorite;
		public Page mParent;
		private RootPage mRootPage;
		private Grid mInsideGrid2;
		public Stream mFavoriteStream;
		public Stream mRemoveProductStream;
		public Stream mAddProductStream;
		public Stream mProductImageStream = new MemoryStream();

		//change PriceLabel
		//int mQuantity = 0;
		//string mQuantityLabel;


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

			Grid mainCellGrid = new Grid (){VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.White, Padding = 0, RowSpacing = 0, ColumnSpacing =0 };

			mainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = Device.GetNamedSize(NamedSize.Medium,typeof(Label))*2 });
			mainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = GridLength.Auto });
			mainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = GridLength.Auto });
			mainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = Device.GetNamedSize(NamedSize.Small,typeof(Label))*2 });
			mainCellGrid.ColumnDefinitions.Add (new ColumnDefinition (){ Width =  width });

			Label productNameLabel = new Label (){ FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)), HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center , TextColor = MyDevice.RedColor, HorizontalTextAlignment=TextAlignment.Center };

			productNameLabel.Text = product.Name;
			mainCellGrid.Children.Add (productNameLabel, 0, 0);



			//mainCellGrid.Children.Add(mFavoriteImage,0,1);

			#region row1insidegrid
			Grid insideGrid1 = new Grid(){VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand,Padding = 0, RowSpacing = 0, ColumnSpacing = 0};
			insideGrid1.Padding = new Thickness (0);
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = MyDevice.ViewPadding });
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding*2) / 3 });
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding*2) / 3 });
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = (width - MyDevice.ViewPadding*2) / 3 });
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = MyDevice.ViewPadding });
			insideGrid1.RowDefinitions.Add( new RowDefinition() { Height = Device.GetNamedSize(NamedSize.Medium ,typeof(Label)) } );

			Label productPriceLabel = new Label (){ FontSize = Device.GetNamedSize(NamedSize.Small ,typeof(Label)), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = MyDevice.RedColor };
			productPriceLabel.Text = "AED " + product.Price.ToString();
			insideGrid1.Children.Add(productPriceLabel,1,0);

			mFavoriteImage = new Image();

			mFavoriteStream = new MemoryStream();									
			if (!bIsFavorite) {								
				mRootPage.mAddFavoritesImage.Position = 0;
				mRootPage.mAddFavoritesImage.CopyToAsync(mFavoriteStream);								
			} else {
				mRootPage.mRemoveFavoritesImage.Position = 0;
				mRootPage.mRemoveFavoritesImage.CopyToAsync(mFavoriteStream);	
			}

			mFavoriteStream.Position = 0;
			mFavoriteImage.Source = StreamImageSource.FromStream(() => mFavoriteStream);

			insideGrid1.Children.Add(mFavoriteImage,2,0);

			Label productQuantityLabel = new Label (){ FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)), HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start, TextColor = MyDevice.RedColor };
			productQuantityLabel.Text = product.Quantity;
			insideGrid1.Children.Add(productQuantityLabel,3,0);

			mainCellGrid.Children.Add( insideGrid1, 0,1);
			#endregion


			mProductImage = new Image ();
			//productImage.Aspect = Aspect.AspectFit;
			mProductImage.HeightRequest = width / 5 * 3;
			mProductImage.WidthRequest = width / 5 * 3;
			var file = mRootPage.mFolder.GetFileAsync (product.ProductImagePath).Result;
			Stream stream = file.OpenAsync (FileAccess.ReadAndWrite).Result;

			stream.Position = 0;
			stream.CopyTo (mProductImageStream);
			mProductImageStream.Position = 0;
			stream.Dispose ();
			mProductImage.Source = StreamImageSource.FromStream (()=>mProductImageStream);

			//mProductImage = file.OpenAsync (FileAccess.ReadAndWrite).Result;
			//var assembly = typeof(ProductCell).GetTypeInfo().Assembly;

			/*Stream s = new MemoryStream ();
			mProductImageStream = new MemoryStream ();*/

			//mProductImageStream = assembly.GetManifestResourceStream(product.ProductImagePath);
			//mProductImageStream = File.
			//var folder = mRootFolder.CreateFolderAsync (ParseConstants.IMAGE_FOLDER_NAME, CreationCollisionOption.OpenIfExists).Result;
			/*var folder = mRootFolder.GetFolderAsync(ParseConstants.IMAGE_FOLDER_NAME).Result;
			var file = folder.GetFileAsync (product.ProductImagePath).Result;
			mProductImageStream = file.OpenAsync (FileAccess.ReadAndWrite).Result;
			mProductImageStream.Position = 0;
			mProductImageStream.CopyTo (s);
			s.Position = 0;*/
			//var a = file.OpenAsync ();
			//var file = folder.GetFileAsync(
			//mProductImage.Source = StreamImageSource.FromStream (() => s);
			//mProductImage.Source = ImageSource.FromFile(product.ProductImagePath);
			mainCellGrid.Children.Add (mProductImage, 0, 2);

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




			mRemoveImage = new Image (){VerticalOptions = LayoutOptions.Center,HorizontalOptions = LayoutOptions.Center};
			mRemoveImage.Aspect = Aspect.AspectFit;

			mRemoveProductStream = new MemoryStream();
			mRootPage.mRemoveProductImage.Position = 0;
			mRootPage.mRemoveProductImage.CopyToAsync(mRemoveProductStream);
			mRemoveProductStream.Position = 0;
			mRemoveImage.Source = ImageSource.FromStream(() => mRemoveProductStream);

			var removeImageLayout = new RelativeLayout(){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Padding = 0
			};

			removeImageLayout.Children.Add( mRemoveImage,
				Constraint.RelativeToParent (p => {
					return p.Bounds.Left + MyDevice.ViewPadding/2;
				})
			);

			insideGrid2.Children.Add (removeImageLayout,1,0);

			mProductNumberLabel = new Label(){VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label)), TextColor = MyDevice.RedColor };

			UpdateNumberLabel();
			insideGrid2.Children.Add (mProductNumberLabel,2,0);


			mAddImage = new Image (){VerticalOptions = LayoutOptions.Center,HorizontalOptions = LayoutOptions.Center};
			mAddImage.Aspect = Aspect.AspectFit;

			mAddProductStream = new MemoryStream();
			mRootPage.mAddProductImage.Position = 0;
			mRootPage.mAddProductImage.CopyToAsync(mAddProductStream);
			mAddProductStream.Position = 0;
			mAddImage.Source = ImageSource.FromStream(() => mAddProductStream);

			var addImageLayout = new RelativeLayout(){
				HorizontalOptions = LayoutOptions.Fill,
				Padding = 0
			};

			addImageLayout.Children.Add( mAddImage,
				Constraint.RelativeToParent (p => {					
					return MyDevice.ViewPadding;
				})
			);

			insideGrid2.Children.Add (addImageLayout,3,0);

			mainCellGrid.Children.Add( insideGrid2, 0, 3 );
			#endregion

			AddTapRecognizers ();

			var frame = new Frame { 				
				Padding = 2,
				OutlineColor = MyDevice.RedColor,
				BackgroundColor = MyDevice.RedColor,
				VerticalOptions = LayoutOptions.Start,
				//HeightRequest = mainCellGrid.Height,
				//WidthRequest = mainCellGrid.Width,
				Content = mainCellGrid
			};

			this.View = frame;
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
				
				mAddImage.Opacity = 0.5f;
				AddProductInCart();
				await Task.Delay(MyDevice.DelayTime);
				mAddImage.Opacity = 1f;
			};


			mInsideGrid2.Children[2].GestureRecognizers.Add (addButtonTapGestureRecognizer);

			var removeButtonTapGestureRecognizer = new TapGestureRecognizer ();
			removeButtonTapGestureRecognizer.Tapped += async (sender, e) => {
				if( CheckIfSearchEntryIsFocused() )
					return;

				mRemoveImage.Opacity = 0.5f;
				RemoveProductFromCart();
				await Task.Delay(MyDevice.DelayTime);
				mRemoveImage.Opacity = 1f;
			};
			mInsideGrid2.Children [0].GestureRecognizers.Add (removeButtonTapGestureRecognizer);
		//	mRemoveImage.GestureRecognizers.Add (removeButtonTapGestureRecognizer);

			var favoriteButtonTapGestureRecognizer = new TapGestureRecognizer ();
			favoriteButtonTapGestureRecognizer.Tapped += async (sender, e) => {
				if( CheckIfSearchEntryIsFocused() )
					return;

				if( !bIsFavorite )
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
				}
				//else if( mFavoriteImage.Source ==
			};

			mFavoriteImage.GestureRecognizers.Add (favoriteButtonTapGestureRecognizer);
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
				bFocused = (mParent as BrowseProductsPage).mSearchBar.mSearchEntry.IsFocused;		

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



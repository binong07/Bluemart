using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using bluemart.Common.Objects;
using bluemart.MainViews;
using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Local;


namespace bluemart.Common.ViewCells
{
	public class ProductCell : ViewCell
	{
		private Image mAddImage;
		private Image mRemoveImage;
		private Image mFavoriteImage;
		private Label mProductNumberLabel;
		private Product mProduct;
		private FavoritesClass mFavoriteModel;
		private bool bIsFavorite;
		public Page mParent;

		public ProductCell (Grid parentGrid, Product product, Page parent)
		{		
				
			double width = (MyDevice.ScreenWidth-parentGrid.ColumnSpacing)/2;

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
			mParent = parent;

			bIsFavorite = mFavoriteModel.IsProductFavorite (product.ProductID);

			Grid mainCellGrid = new Grid (){VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.White, Padding = 0, RowSpacing = 0, ColumnSpacing =0 };

			mainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = new GridLength(70,GridUnitType.Absolute) });
			mainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = new GridLength(30,GridUnitType.Absolute) });
			mainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = GridLength.Auto });
			mainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = GridLength.Auto });
			mainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = GridLength.Auto });
			mainCellGrid.ColumnDefinitions.Add (new ColumnDefinition (){ Width =  new GridLength(width,GridUnitType.Absolute) });

			Label productNameLabel = new Label (){ FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label)), HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start , TextColor = MyDevice.RedColor, XAlign=TextAlignment.Center };

			productNameLabel.Text = product.Name;
			mainCellGrid.Children.Add (productNameLabel, 0, 0);

			mFavoriteImage = new Image();
			if (!bIsFavorite) {
				mFavoriteImage.Source = "bookmark_add";
			} else {
				mFavoriteImage.Source = "bookmark_remove";
			}

			mainCellGrid.Children.Add(mFavoriteImage,0,1);

			#region row1insidegrid
			Grid insideGrid1 = new Grid(){VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand,Padding = 0, RowSpacing = 0, ColumnSpacing = 0};
			insideGrid1.Padding = new Thickness (0);
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = new GridLength(width/2,GridUnitType.Absolute) });
			insideGrid1.ColumnDefinitions.Add( new ColumnDefinition() { Width = new GridLength(width/2,GridUnitType.Absolute) });
			insideGrid1.RowDefinitions.Add( new RowDefinition() { Height = GridLength.Auto } );

			Label productPriceLabel = new Label (){ FontSize = Device.GetNamedSize(NamedSize.Small ,typeof(Label)), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = MyDevice.RedColor };
			productPriceLabel.Text = "DH " + product.Price.ToString();
			insideGrid1.Children.Add(productPriceLabel,0,0);

			Label productQuantityLabel = new Label (){ FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)), HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start, TextColor = MyDevice.RedColor };
			productQuantityLabel.Text = product.Quantity;
			insideGrid1.Children.Add(productQuantityLabel,1,0);

			mainCellGrid.Children.Add( insideGrid1, 0,2);
			#endregion


			Image productImage = new Image ();
			productImage.Aspect = Aspect.AspectFill;
			productImage.Source = ImageSource.FromFile(product.ProductImagePath);
			mainCellGrid.Children.Add (productImage, 0, 3);

			#region row3insidegrid
			Grid insideGrid2 = new Grid(){VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = 0, RowSpacing = 0, ColumnSpacing = 0};
			insideGrid2.Padding = new Thickness (0);
			insideGrid2.ColumnDefinitions.Add( new ColumnDefinition() { Width = new GridLength(width / 4,GridUnitType.Absolute) });
			insideGrid2.ColumnDefinitions.Add( new ColumnDefinition() { Width = new GridLength(width * 2  / 4,GridUnitType.Absolute) });//{ Width = ParentWidth - ((ParentHeight - ParentWidth) / 3 * 2)}
			insideGrid2.ColumnDefinitions.Add( new ColumnDefinition() { Width = new GridLength(width / 4,GridUnitType.Absolute) });
			insideGrid2.RowDefinitions.Add( new RowDefinition() { Height = GridLength.Auto } );


			mRemoveImage = new Image (){VerticalOptions = LayoutOptions.Center};
			mRemoveImage.Aspect = Aspect.AspectFill;
			mRemoveImage.Source = "minus";
			insideGrid2.Children.Add (mRemoveImage,0,0);

			mProductNumberLabel = new Label(){VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label)), TextColor = MyDevice.RedColor };

			UpdateNumberLabel();
			insideGrid2.Children.Add (mProductNumberLabel,1,0);


			mAddImage = new Image (){HorizontalOptions = LayoutOptions.Start};
			mAddImage.Aspect = Aspect.AspectFill;
			mAddImage.Source = "plus";
			insideGrid2.Children.Add (mAddImage,2,0);

			mainCellGrid.Children.Add( insideGrid2, 0, 4 );
			#endregion

			AddTapRecognizers ();

			this.View = mainCellGrid;
		}

		private void AddTapRecognizers()
		{
			var addButtonTapGestureRecognizer = new TapGestureRecognizer ();
			addButtonTapGestureRecognizer.Tapped += async (sender, e) => {

				mAddImage.Opacity = 0.5f;
				AddProductInCart();
				await Task.Delay(MyDevice.DelayTime);
				mAddImage.Opacity = 1f;
			};
			mAddImage.GestureRecognizers.Add (addButtonTapGestureRecognizer);

			var removeButtonTapGestureRecognizer = new TapGestureRecognizer ();
			removeButtonTapGestureRecognizer.Tapped += async (sender, e) => {

				mRemoveImage.Opacity = 0.5f;
				RemoveProductFromCart();
				await Task.Delay(MyDevice.DelayTime);
				mRemoveImage.Opacity = 1f;
			};
			mRemoveImage.GestureRecognizers.Add (removeButtonTapGestureRecognizer);

			var favoriteButtonTapGestureRecognizer = new TapGestureRecognizer ();
			favoriteButtonTapGestureRecognizer.Tapped += async (sender, e) => {
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
		}

		private void RemoveProductFromCart()
		{
			if (mProduct.ProductNumberInCart > 0) {
				mProduct.ProductNumberInCart--;
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
		 
		private void AddProductInCart()
		{
			if (!Cart.ProductsInCart.Contains (mProduct)) 
			{
				Cart.ProductsInCart.Add (mProduct);
			}

			mProduct.ProductNumberInCart++;
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



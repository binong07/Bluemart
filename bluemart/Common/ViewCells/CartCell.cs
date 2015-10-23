using System;
using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;
using System.Threading.Tasks;
using bluemart.MainViews;
using bluemart.Models.Local;

namespace bluemart
{
	public class CartCell : ViewCell
	{
		private Image mAddImage;
		private Image mRemoveImage;
		private Image mFavoriteImage;
		private Label mProductNumberLabel;
		private Label mProductPriceLabel;
		private Product mProduct;
		private CartPage mParentPage;
		private bool bIsFavorite;

		private FavoritesClass mFavoriteModel = new FavoritesClass();

		public CartCell (Product product,Page parent)
		{
			mProduct = product; 
			mParentPage = parent as CartPage;
			bIsFavorite = mFavoriteModel.IsProductFavorite (product.ProductID);

			Grid mainCellView = new Grid (){RowSpacing = 0, ColumnSpacing = 0, Padding = 0,BackgroundColor=Color.White};
			mainCellView.HorizontalOptions = LayoutOptions.Start;
			//mainCellView.BackgroundColor = Color.Red;
			mainCellView.RowDefinitions.Add (new RowDefinition (){ Height = GridLength.Auto } );
			mainCellView.ColumnDefinitions.Add (new ColumnDefinition (){ Width = MyDevice.ScreenWidth/10} );
			mainCellView.ColumnDefinitions.Add (new ColumnDefinition (){ Width = MyDevice.ScreenWidth*6/10 });
			mainCellView.ColumnDefinitions.Add (new ColumnDefinition (){ Width = MyDevice.ScreenWidth / 10});
			mainCellView.ColumnDefinitions.Add (new ColumnDefinition (){ Width = MyDevice.ScreenWidth / 10});
			mainCellView.ColumnDefinitions.Add (new ColumnDefinition (){ Width = MyDevice.ScreenWidth / 10});

			Image productImage = new Image (){HorizontalOptions = LayoutOptions.Start};
			productImage.Aspect = Aspect.AspectFill;
			productImage.Source = product.ProductImagePath;
			mainCellView.Children.Add (productImage, 0, 0);

			#region InsideGrid 
			Grid insideGrid = new Grid (){ ColumnSpacing = 0, RowSpacing = 0};
			insideGrid.RowDefinitions.Add (new RowDefinition (){ Height = GridLength.Auto } );
			insideGrid.RowDefinitions.Add (new RowDefinition (){ Height = GridLength.Auto } );
			insideGrid.RowDefinitions.Add (new RowDefinition (){ Height = GridLength.Auto } );
			insideGrid.ColumnDefinitions.Add (new ColumnDefinition (){ Width = MyDevice.ScreenWidth*6/10} );

			Label nameLabel = new Label (){HorizontalOptions = LayoutOptions.Fill, FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),TextColor = Color.Black};
				nameLabel.Text = product.Name;
				insideGrid.Children.Add (nameLabel, 0, 0);

			mProductNumberLabel = new Label (){HorizontalOptions = LayoutOptions.Fill, FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label)),TextColor = Color.Black};

			insideGrid.Children.Add (mProductNumberLabel, 0, 1);

			mProductPriceLabel = new Label (){HorizontalOptions = LayoutOptions.Fill, FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label)),TextColor = Color.Black};
			UpdatePriceLabel();
			insideGrid.Children.Add (mProductPriceLabel, 0, 2);
			#endregion

			mainCellView.Children.Add (insideGrid, 1, 0);

			mFavoriteImage = new Image();
			if (!bIsFavorite) {
				mFavoriteImage.Source = "bookmark_add";
			} else {
				mFavoriteImage.Source = "bookmark_remove";
			}

			mainCellView.Children.Add(mFavoriteImage,2,0);

			#region row3insidegrid
			mAddImage = new Image (){HorizontalOptions = LayoutOptions.Start};
			mAddImage.Source = "plus";
			mainCellView.Children.Add (mAddImage,3,0);

			mRemoveImage = new Image (){VerticalOptions = LayoutOptions.Center};
			mRemoveImage.Source = "minus";
			mainCellView.Children.Add (mRemoveImage,4,0	);
			#endregion

			UpdateNumberLabel();
			AddTapRecognizers ();
			//Calculate Total Price
			Cart.ProductTotalPrice += product.Price * product.ProductNumberInCart;

			this.View = mainCellView;
		}

		private void UpdateNumberLabel()
		{
			mProductNumberLabel.Text = "x" + mProduct.ProductNumberInCart.ToString();
		}

		private void UpdatePriceLabel()
		{
			mProductPriceLabel.Text = "DH " + (mProduct.Price * mProduct.ProductNumberInCart).ToString();
		}

		private void AddTapRecognizers()
		{
			var addButtonTapGestureRecognizer = new TapGestureRecognizer ();
			addButtonTapGestureRecognizer.Tapped += async (sender, e) => {

				mAddImage.Opacity = 0.5f;
				AddProductInCart();
				await Task.Delay(200);
				mAddImage.Opacity = 1f;
			};
			mAddImage.GestureRecognizers.Add (addButtonTapGestureRecognizer);

			var removeButtonTapGestureRecognizer = new TapGestureRecognizer ();
			removeButtonTapGestureRecognizer.Tapped += async (sender, e) => {

				mRemoveImage.Opacity = 0.5f;
				RemoveProductFromCart();
				await Task.Delay(200);
				mRemoveImage.Opacity = 1f;
			};
			mRemoveImage.GestureRecognizers.Add (removeButtonTapGestureRecognizer);

			var favoriteButtonTapGestureRecognizer = new TapGestureRecognizer ();
			favoriteButtonTapGestureRecognizer.Tapped += async (sender, e) => {
				if( !bIsFavorite )
				{
					mFavoriteImage.Opacity = 0.5f;
					mFavoriteModel.AddProductID(mProduct.ProductID);
					await Task.Delay(200);
					mFavoriteImage.Opacity = 1f;
					mFavoriteImage.Source = "bookmark_remove";
					bIsFavorite = true;
				}
				else
				{					
					mFavoriteImage.Opacity = 0.5f;
					mFavoriteModel.RemoveProductID(mProduct.ProductID);
					await Task.Delay(200);
					mFavoriteImage.Opacity = 1f;
					mFavoriteImage.Source = "bookmark_add";
					bIsFavorite = false;
				}
			};

			mFavoriteImage.GestureRecognizers.Add (favoriteButtonTapGestureRecognizer);
		}

		private void RemoveProductFromCart()
		{
			if (mProduct.ProductNumberInCart > 0)
				mProduct.ProductNumberInCart--;
			if (mProduct.ProductNumberInCart == 0) {
				mParentPage.RemoveProductFromCart (this.View);
				Cart.ProductsInCart.Remove (mProduct);
			}
			UpdateNumberLabel ();
			Cart.ProductTotalPrice -= mProduct.Price ;
			mParentPage.UpdateTotalPriceLabel ();
			UpdatePriceLabel();
		}

		private void AddProductInCart()
		{
			if (!Cart.ProductsInCart.Contains (mProduct)) 
			{
				Cart.ProductsInCart.Add (mProduct);
			}

			mProduct.ProductNumberInCart++;	
			UpdateNumberLabel ();
			Cart.ProductTotalPrice += mProduct.Price;
			mParentPage.UpdateTotalPriceLabel ();
			UpdatePriceLabel();
		}
	}
}


using System;
using bluemart.Common.Objects;
using bluemart.MainViews;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using bluemart.Common.Utilities;
using bluemart.Models.Remote;

namespace bluemart.Common.ViewCells
{
	public class CategoryCell : ViewCell
	{	
		Category mCategory;
		List<Category> mCategoryList;
		Dictionary<string, List<Product>> mProductDictionary;

		public CategoryCell (StackLayout parentGrid, Category category, BrowseCategoriesPage parent = null, BrowseSubCategoriesPage subParent = null)
		{
			mCategory = category;
			mCategoryList = new List<Category> ();
			mProductDictionary = new Dictionary<string, List<Product>> ();

			Grid mainCellGrid = new Grid (){VerticalOptions = LayoutOptions.Fill, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Gray, Padding = 0, RowSpacing = 0 };

			mainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = 150});
			mainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = GridLength.Auto});
			mainCellGrid.ColumnDefinitions.Add (new ColumnDefinition (){ Width =  MyDevice.ScreenWidth});

			Image categoryImage = new Image ();
			categoryImage.Aspect = Aspect.AspectFill;
			categoryImage.Source = ImageSource.FromFile(category.CategoryImagePath);

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += async (sender, e) => {

				categoryImage.Opacity = 0.5f;
				await Task.Delay (200);
				LoadProductsPage(category.CategoryID,parent);
				categoryImage.Opacity = 1f;
			};

			categoryImage.GestureRecognizers.Add (tapGestureRecognizer);

			mainCellGrid.Children.Add (categoryImage, 0, 0);

			Label lbl = new Label (){ FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label)), 
				BackgroundColor = Color.White, TextColor = MyDevice.RedColor, XAlign = TextAlignment.Center ,
				YAlign = TextAlignment.Center, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
			lbl.Text = category.Name;

			mainCellGrid.Children.Add (lbl, 0, 1);

			this.View = mainCellGrid;
		}


		void LoadProductsPage(string categoryID,BrowseCategoriesPage parent)
		{			
			PopulateProducts ();
			parent.Navigation.PushAsync (new BrowseProductsPage (mProductDictionary,mCategory));
		}

		private void PopulateProducts()
		{
			mProductDictionary.Clear ();
			PopulateCategories ();

			foreach( Category category in mCategoryList )
			{
				List<Product> product = new List<Product> ();

				if (ProductModel.mProductCategoryIDDictionary.ContainsKey (category.CategoryID)) {
					foreach (string productID in ProductModel.mProductCategoryIDDictionary[category.CategoryID]) {
						string ImagePath = ProductModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + ProductModel.mProductImageNameDictionary [productID] + ".jpg";
						string ProductName = ProductModel.mProductNameDictionary [productID];
						double price = ProductModel.mProductPriceDictionary [productID];
						string quantity = ProductModel.mProductQuantityDictionary [productID];
						product.Add (new Product (productID, ProductName, ImagePath, price, quantity)); 
					}
				}

				mProductDictionary.Add (category.Name, product);
			}
		}

		void PopulateCategories()
		{
			mCategoryList.Clear ();

			if (mCategory.CategoryModel.mSubCategoryDictionary.ContainsKey (mCategory.CategoryID) && 
				mCategory.CategoryModel.mSubCategoryDictionary[mCategory.CategoryID].Count > 0) {
				foreach (string subCategoryID in mCategory.CategoryModel.mSubCategoryDictionary[mCategory.CategoryID]) {
					string ImagePath = mCategory.CategoryModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + mCategory.CategoryModel.mImageNameDictionary [subCategoryID] + ".jpg";
					string CategoryName = mCategory.CategoryModel.mCategoryNameDictionary [subCategoryID];
					List<string> SubCategoryIDList = mCategory.CategoryModel.mSubCategoryDictionary [subCategoryID];

					mCategoryList.Add (new Category (CategoryName, ImagePath, categoryID: subCategoryID));
				}				
			} else {
				mCategoryList.Add (mCategory);	
			}
		}
	}
}



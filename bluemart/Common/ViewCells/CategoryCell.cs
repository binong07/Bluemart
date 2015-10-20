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

		public CategoryCell (StackLayout parentGrid, Category category, BrowseCategoriesPage parent = null, BrowseSubCategoriesPage subParent = null)
		{
			mCategory = category;
			mCategoryList = new List<Category> ();

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
				//IF category doesn't have a sub category
				if( category.SubCategoriesList == null )
				{
					LoadProductsFromSubCategoriesPage(category.CategoryID,subParent);
				}
				else 
				{
					if( category.SubCategoriesList.Count == 0 )
					{
						LoadProductsPage(category.CategoryID,parent);							
					}
					else
						LoadCategoriesPage(parent);
				}
				//category.Name
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
			//this.ParentView.cl
			parent.Navigation.PushAsync (new BrowseProductsPage (PopulateProducts (categoryID),mCategory));
		}

		void LoadProductsFromSubCategoriesPage(string categoryID,BrowseSubCategoriesPage parent)
		{			
			parent.Navigation.PushAsync (new BrowseProductsPage (PopulateProducts (categoryID),mCategory));
		}

		void LoadCategoriesPage( BrowseCategoriesPage parent)
		{
			PopulateCategories ();
			parent.Navigation.PushAsync (new BrowseSubCategoriesPage (mCategoryList,mCategory));
		}

		void PopulateCategories()
		{
			mCategoryList.Clear ();

			if(mCategory.CategoryModel.mSubCategoryDictionary.ContainsKey( mCategory.CategoryID ) )
			{
				foreach (string subCategoryID in mCategory.CategoryModel.mSubCategoryDictionary[mCategory.CategoryID] ){
					string ImagePath = mCategory.CategoryModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + mCategory.CategoryModel.mImageNameDictionary[subCategoryID] + ".jpg";
					string CategoryName = mCategory.CategoryModel.mCategoryNameDictionary [subCategoryID];
					List<string> SubCategoryIDList = mCategory.CategoryModel.mSubCategoryDictionary [subCategoryID];

					mCategoryList.Add( new Category( CategoryName,ImagePath,categoryID:subCategoryID ) );
				}				
			}
		}

		List<Product> PopulateProducts( string CategoryID )
		{
			List<Product> product = new List<Product> ();

			if (ProductModel.mProductCategoryIDDictionary.ContainsKey (CategoryID)) {
				foreach (string productID in ProductModel.mProductCategoryIDDictionary[CategoryID]) {
					string ImagePath = ProductModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + ProductModel.mProductImageNameDictionary [productID] + ".jpg";
					string ProductName = ProductModel.mProductNameDictionary [productID];
					double price = ProductModel.mProductPriceDictionary [productID];
					string quantity = ProductModel.mProductQuantityDictionary [productID];
					product.Add (new Product (productID, ProductName, ImagePath, price, quantity)); 
				}
			}

			return product;
		}


	}
}



using System;
using bluemart.Common.Objects;
using bluemart.MainViews;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using bluemart.Common.Utilities;
using bluemart.Models.Remote;
using System.Linq;
using bluemart.Models.Local;

namespace bluemart.Common.ViewCells
{
	public class CategoryCell : ViewCell
	{	
		Category mCategory;
		List<Category> mCategoryList;
		UserClass mUser;
		Dictionary<string, List<Product>> mProductDictionary;

		public CategoryCell (StackLayout parentGrid, Category category, RootPage parent = null)
		{
			mCategory = category;
			mCategoryList = new List<Category> ();
			mProductDictionary = new Dictionary<string, List<Product>> ();
			mUser = new UserClass ();

			Grid mainCellGrid = new Grid (){VerticalOptions = LayoutOptions.Fill, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Gray, Padding = 0, RowSpacing = 0 };

			mainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = 150});
			mainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = GridLength.Auto});
			mainCellGrid.ColumnDefinitions.Add (new ColumnDefinition (){ Width =  MyDevice.ScreenWidth});


			Image categoryImage = new Image ();
			categoryImage.Aspect = Aspect.AspectFill;
			categoryImage.Source = ImageSource.FromFile(category.CategoryImagePath);



			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += async (sender, e) => {

				if ( parent.mBrowseCategoriesPage.mSearchBar.mSearchEntry.IsFocused )
					return;

				mainCellGrid.Opacity = 0.5f;
				await Task.Delay (MyDevice.DelayTime);
				LoadProductsPage(category.CategoryID,parent);
				mainCellGrid.Opacity = 1f;
			};

			mainCellGrid.GestureRecognizers.Add (tapGestureRecognizer);

			mainCellGrid.Children.Add (categoryImage, 0, 0);

			Label lbl = new Label (){ FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label)), 
				BackgroundColor = Color.White, TextColor = MyDevice.RedColor, XAlign = TextAlignment.Center ,
				YAlign = TextAlignment.Center, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
			lbl.Text = category.Name;

			mainCellGrid.Children.Add (lbl, 0, 1);

			this.View = mainCellGrid;
		}


		void LoadProductsPage(string categoryID,RootPage parent)
		{			
			
			PopulateProducts ();
			parent.LoadProductsPage(mProductDictionary,mCategory);
		}

		private void PopulateProducts()
		{
			mProductDictionary.Clear ();
			PopulateCategories ();

			foreach( Category category in mCategoryList )
			{
				List<Product> product = new List<Product> ();

				string location = mUser.GetActiveRegionFromUser ();
				int store = RegionHelper.DecideShopNumber (location);

				if (ProductModel.mProductCategoryIDDictionary.ContainsKey (category.CategoryID)) {
					foreach (string productID in ProductModel.mProductCategoryIDDictionary[category.CategoryID]) {						
						string storeString = ProductModel.mProductStoresDictionary [productID];

						if (String.IsNullOrEmpty(storeString))
							continue;

						//Get store string list
						var storeList = storeString.Split (',').ToList ();
						//Convert storelist to integer list
						var storeNumberList = storeList.Select (int.Parse).ToList ();

						if (!storeNumberList.Contains (store))
							continue;

						string ImagePath = ProductModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + ProductModel.mProductImageNameDictionary [productID] + ".jpg";
						string ProductName = ProductModel.mProductNameDictionary [productID];
						decimal price = ProductModel.mProductPriceDictionary [productID];
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

			if (CategoryModel.mSubCategoryDictionary.ContainsKey (mCategory.CategoryID) && 
				CategoryModel.mSubCategoryDictionary[mCategory.CategoryID].Count > 0) {
				foreach (string subCategoryID in CategoryModel.mSubCategoryDictionary[mCategory.CategoryID]) {
					string ImagePath = ImageModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + CategoryModel.mImageNameDictionary [subCategoryID] + ".jpg";
					string CategoryName = CategoryModel.mCategoryNameDictionary [subCategoryID];
					List<string> SubCategoryIDList = CategoryModel.mSubCategoryDictionary [subCategoryID];

					mCategoryList.Add (new Category (CategoryName, ImagePath, categoryID: subCategoryID));
				}				
			} else {
				mCategoryList.Add (mCategory);	
			}
		}
	}
}



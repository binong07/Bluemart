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
using System.IO;
using FFImageLoading.Forms;

namespace bluemart.Common.ViewCells
{
	public class CategoryCell : ViewCell
	{	
		public Category mCategory;
		List<Category> mCategoryList;
		UserClass mUser;
		Dictionary<string, List<Product>> mProductDictionary;
		RootPage mParent;

		public CategoryCell (StackLayout parentGrid, Category category, RootPage parent = null)
		{
			mParent = parent;
			var mainRelativeLayout = new RelativeLayout(){				
				Padding = 0
			};
					
			mCategory = category;
			mCategoryList = new List<Category> ();
			mProductDictionary = new Dictionary<string, List<Product>> ();
			mUser = new UserClass ();



			CachedImage categoryImage = new CachedImage ()
			{
				WidthRequest = MyDevice.GetScaledSize (619),
				HeightRequest = MyDevice.GetScaledSize (202),
				Source = category.CategoryImagePath,//Source = category.CategoryImageName,
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false
			};				

				/*
			if( mParent.mFolder.CheckExistsAsync(ProductModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + category.CategoryImageName).Result != PCLStorage.ExistenceCheckResult.NotFound)
				categoryImage.Source = ProductModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + category.CategoryImageName;
			else
				categoryImage.Source = ImageSource.FromResource("bluemart.SavedImages."+category.CategoryImageName);
*/
			/*Label shadowCategoryText = new Label (){
				FontSize = MyDevice.FontSizeMedium, 
				BackgroundColor = Color.Transparent, 
				TextColor = Color.Black,
				FontAttributes = FontAttributes.Bold,
				Text = category.Name
			};

			Label categoryText = new Label (){
				FontSize = MyDevice.FontSizeMedium, 
				BackgroundColor = Color.Transparent, 
				TextColor = Color.White,
				Text = category.Name
			};
*/
			var tapGestureRecognizer = new TapGestureRecognizer ();

			tapGestureRecognizer.Tapped +=async (sender, e) => {
				if(category.CategoryID == ReleaseConfig.TOBACCO_ID)
				{					
					var isOk = await mParent.DisplayAlert("Warning","I am over 20 years old and I know smoking is bad for my health.","OK","CANCEL");
					if(isOk)
						LoadProductsPage(category.CategoryID,parent);										
			}else if(category.CategoryID == ReleaseConfig.FRUITS_ID)
			{					
				await mParent.DisplayAlert("Warning","Gram Warning.","OK");

					LoadProductsPage(category.CategoryID,parent);										
			}
				else
					LoadProductsPage(category.CategoryID,parent);
			};

			categoryImage.GestureRecognizers.Add (tapGestureRecognizer);

			mainRelativeLayout.Children.Add (categoryImage,
				Constraint.Constant (MyDevice.GetScaledSize(11)),
				Constraint.Constant (MyDevice.GetScaledSize(11))
			);
			/*mainRelativeLayout.Children.Add (shadowCategoryText,
				Constraint.RelativeToView (categoryImage, (p, sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize (12);
				}),
				Constraint.RelativeToView (categoryImage, (p, sibling) => {
					return sibling.Bounds.Bottom - MyDevice.GetScaledSize (45);
				})
			);
			mainRelativeLayout.Children.Add (categoryText,
				Constraint.RelativeToView (categoryImage, (p, sibling) => {
					return sibling.Bounds.Left + MyDevice.GetScaledSize (12);
				}),
				Constraint.RelativeToView (categoryImage, (p, sibling) => {
					return sibling.Bounds.Bottom - MyDevice.GetScaledSize (45);
				})
			);*/

			this.View = mainRelativeLayout;
		}
			


		public void LoadProductsPage(string categoryID,RootPage parent)
		{						
			PopulateProducts ();
			//PopulateProductsTest();
			parent.LoadProductsPage(mProductDictionary,mCategory);
		}

		private void PopulateProducts()
		{
			mProductDictionary.Clear ();
			PopulateCategories ();

			//For Top Selling
			foreach (Category category in mCategoryList) {
				List<Product> product = new List<Product> ();

				string location = mUser.GetActiveRegionFromUser ();
				int store = RegionHelper.DecideShopNumber (location);

				if (ProductModel.mProductCategoryIDDictionary.ContainsKey (category.CategoryID)) {
					foreach (string productID in ProductModel.mProductCategoryIDDictionary[category.CategoryID]) {						
						if (ProductModel.mProductIsTopSellingDictionary [productID]) {
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
							//string ImageName = ProductModel.mProductImageNameDictionary [productID] + ".jpg";
							string ProductName = ProductModel.mProductNameDictionary [productID];
							decimal price = ProductModel.mProductPriceDictionary [productID];
							string quantity = ProductModel.mProductQuantityDictionary [productID];
							string parentCategory = ProductModel.mProductParentCategoryIDsDictionary [productID];
						product.Add (new Product (productID, ProductName, ImagePath, price, parentCategory, quantity));
						//product.Add (new Product (productID, ProductName, ImageName, price, parentCategory, quantity));
						}
						 
					}
				}
				if (!mProductDictionary.ContainsKey ("Top Selling"))
					mProductDictionary.Add ("Top Selling", product);
				else {
					List<Product> tempProduct = mProductDictionary ["Top Selling"];
					tempProduct.Concat (product);
					mProductDictionary.Remove ("Top Selling");
					mProductDictionary.Add ("Top Selling", tempProduct);
				}					
			}

			foreach( Category category in mCategoryList )
			{
				List<Product> product = new List<Product> ();

				string location = mUser.GetActiveRegionFromUser ();
				int store = RegionHelper.DecideShopNumber (location);

				if (ProductModel.mProductCategoryIDDictionary.ContainsKey (category.CategoryID)) {
					foreach (string productID in ProductModel.mProductCategoryIDDictionary[category.CategoryID]) {						
						string storeString = ProductModel.mProductStoresDictionary [productID];

						//string parentCategoryID = ProductModel.mProductParentCategoryIDsDictionary [productID];

						/*if (parentCategoryID != "") {
						}*/

						if (String.IsNullOrEmpty(storeString))
							continue;

						//Get store string list
						var storeList = storeString.Split (',').ToList ();
						//Convert storelist to integer list
						var storeNumberList = storeList.Select (int.Parse).ToList ();

						if (!storeNumberList.Contains (store))
							continue;
						
						decimal price = ProductModel.mProductPriceDictionary [productID];
					string ImagePath = ProductModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + ProductModel.mProductImageNameDictionary [productID] + ".jpg";
						//string ImageName = ProductModel.mProductImageNameDictionary [productID] + ".jpg";
						string ProductName = ProductModel.mProductNameDictionary [productID];						
						string quantity = ProductModel.mProductQuantityDictionary [productID];
						string parentCategory = ProductModel.mProductParentCategoryIDsDictionary [productID];
					product.Add (new Product (productID, ProductName, ImagePath, price, parentCategory, quantity)); 
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
					
				//string ImageName = CategoryModel.mImageNameDictionary[subCategoryID] + ".jpg";
					string CategoryName = CategoryModel.mCategoryNameDictionary [subCategoryID];
					List<string> SubCategoryIDList = CategoryModel.mSubCategoryDictionary [subCategoryID];
				mCategoryList.Add (new Category (CategoryName, ImagePath, categoryID: subCategoryID));

					//mCategoryList.Add (new Category (CategoryName, ImageName, categoryID: subCategoryID));
				}				
			} else {
				mCategoryList.Add (mCategory);	
			}
		}

		private void PopulateProductsTest()
		{
			mProductDictionary.Clear ();
			PopulateCategories ();

			List<Product> product = new List<Product> ();
			for (int i = 0; i < 1000; i++) {				
				string ImagePath = "/data/data/com.app1001.bluemart/files/SavedImages/EnergyDrink_red-bull-sugarfree-250ml.jpg";
				string ProductName = "test";
				decimal price = new decimal(i);
				string quantity = "abc";
				string parentCategory = "";
				product.Add (new Product ("asd", ProductName, ImagePath, price, parentCategory, quantity));
			}
			mProductDictionary.Add ("asd", product);
			/*foreach( Category category in mCategoryList )
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
			}*/
		}
	}
}



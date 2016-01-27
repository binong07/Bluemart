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
using TwinTechs.Controls;

namespace bluemart.Common.ViewCells
{
	public class CategoryCellNew : FastCell
	{	
		List<Category> mCategoryList;
		UserClass mUser;
		Dictionary<string, List<Product>> mProductDictionary;
		RootPage mParent;
		CachedImage categoryImage;
		public Category category {
			get { return (Category)BindingContext; }
		}
		public CategoryCellNew ()
		{
		}
		protected override void SetupCell (bool isRecycled)
		{
			if (category != null) {
				mParent = MyDevice.currentPage;
				var mainRelativeLayout = new RelativeLayout(){				
					Padding = 0
				};
				mCategoryList = new List<Category> ();
				mProductDictionary = new Dictionary<string, List<Product>> ();
				categoryImage.Source = category.CategoryImagePath;
			}
		}
		protected override void InitializeCell ()
		{
			mUser = new UserClass ();
			var mainRelativeLayout = new RelativeLayout(){				
				Padding = 0
			};
			categoryImage = new CachedImage ()
			{
				WidthRequest = MyDevice.GetScaledSize (619),
				HeightRequest = MyDevice.GetScaledSize (202),
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false,
			};				
			categoryImage.Success += (object sender, CachedImageEvents.SuccessEventArgs e) => {System.Diagnostics.Debug.WriteLine("aq");};

			var tapGestureRecognizer = new TapGestureRecognizer ();

			tapGestureRecognizer.Tapped +=async (sender, e) => {
				if(category.CategoryID == ReleaseConfig.TOBACCO_ID)
				{					
					var isOk = await mParent.DisplayAlert("Warning","I am over 20 years old and I know smoking is bad for my health.","AGREE","DISAGREE");
					if(isOk)
						LoadProductsPage(category.CategoryID,mParent);										
				}else if(category.CategoryID == ReleaseConfig.FRUITS_ID||category.CategoryID == ReleaseConfig.MEAT_ID)
				{					
					await mParent.DisplayAlert("Please Remember","Delivered quantity might differ from the actual ordered quantity by ± 50 grams.","OK");

					LoadProductsPage(category.CategoryID,mParent);										
				}
				else
					LoadProductsPage(category.CategoryID,mParent);
			};

			categoryImage.GestureRecognizers.Add (tapGestureRecognizer);

			mainRelativeLayout.Children.Add (categoryImage,
				Constraint.Constant (MyDevice.GetScaledSize(11)),
				Constraint.Constant (MyDevice.GetScaledSize(11))
			);

			this.View = mainRelativeLayout;
		}

	public void LoadProductsPage(string categoryID,RootPage parent)
	{						
		PopulateProducts ();
		//PopulateProductsTest();
		parent.LoadProductsPage(mProductDictionary,Category);
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
						bool IsInStock = ProductModel.mProductIsInStockDictionary [productID];
						product.Add (new Product (productID, ProductName, ImagePath, price, parentCategory, quantity,IsInStock));
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
					bool IsInStock = ProductModel.mProductIsInStockDictionary [productID];
					product.Add (new Product (productID, ProductName, ImagePath, price, parentCategory, quantity,IsInStock)); 
				}
			}

			mProductDictionary.Add (category.Name, product);
		}
	}

	void PopulateCategories()
	{
		mCategoryList.Clear ();

		if (CategoryModel.mSubCategoryDictionary.ContainsKey (Category.CategoryID) && 
			CategoryModel.mSubCategoryDictionary[Category.CategoryID].Count > 0) {
			foreach (string subCategoryID in CategoryModel.mSubCategoryDictionary[Category.CategoryID]) {
				string ImagePath = ImageModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + CategoryModel.mImageNameDictionary [subCategoryID] + ".jpg";

				string CategoryName = CategoryModel.mCategoryNameDictionary [subCategoryID];
				List<string> SubCategoryIDList = CategoryModel.mSubCategoryDictionary [subCategoryID];
				mCategoryList.Add (new Category (CategoryName, ImagePath, categoryID: subCategoryID));

			}				
		} else {
			mCategoryList.Add (Category);	
		}
	}
}
}



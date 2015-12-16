using System;
using PCLStorage;
using System.Collections.Generic;
using bluemart.Common.Utilities;
using Parse;
using System.Net.Http;
using System.Linq;
using bluemart.Models.Local;
using Xamarin.Forms;

namespace bluemart.Models.Remote
{
	public static class ProductModel
	{
		public static IFolder mRootFolder;
		public static string mRootFolderPath;
		public static List<string> mProductIDList;
		public static List<string> mSearchProductIDList;
		public static Dictionary<string,string> mProductNameDictionary;
		public static Dictionary<string,string> mProductImageIDDictionary;
		public static Dictionary<string,string> mProductImageNameDictionary;
		public static Dictionary<string,decimal> mProductPriceDictionary;
		public static Dictionary<string,bool> mProductIsTopSellingDictionary;
		public static Dictionary<string,string> mProductQuantityDictionary;
		public static Dictionary<string,string> mProductStoresDictionary;
		public static Dictionary<string,string> mProductParentCategoryIDsDictionary;
		public static Dictionary<string,HashSet<string>> mProductCategoryIDDictionary;
		private static FavoritesClass mFavoritesModel = new FavoritesClass();
		private static ProductClass mProductClass = new ProductClass();
		private static UserClass mUserClass = new UserClass();

		static ProductModel ()
		{
			InitializeMemberVariables ();
		}

		private static void InitializeMemberVariables()
		{			
			
			mRootFolder = FileSystem.Current.LocalStorage;
			mRootFolderPath = mRootFolder.Path;
			mProductIDList = new List<string> ();
			mSearchProductIDList = new List<string> ();
			mProductNameDictionary = new Dictionary<string, string> ();
			mProductImageIDDictionary = new Dictionary<string, string> ();
			mProductImageNameDictionary = new Dictionary<string, string> ();
			mProductPriceDictionary = new Dictionary<string, decimal> ();
			mProductQuantityDictionary = new Dictionary<string, string> ();
			mProductStoresDictionary = new Dictionary<string, string> ();
			mProductParentCategoryIDsDictionary = new Dictionary<string, string> ();
			mProductCategoryIDDictionary = new Dictionary<string,HashSet<string>> ();
			mProductIsTopSellingDictionary = new Dictionary<string, bool> ();
		}

		private static void ClearContainers()
		{
			mProductIDList.Clear ();
			mSearchProductIDList.Clear ();
			mProductNameDictionary.Clear ();
			mProductImageIDDictionary.Clear ();
			mProductImageNameDictionary.Clear ();
			mProductPriceDictionary.Clear ();
			mProductQuantityDictionary.Clear ();
			mProductStoresDictionary.Clear ();
			mProductParentCategoryIDsDictionary.Clear();
			mProductCategoryIDDictionary.Clear ();
			mProductIsTopSellingDictionary.Clear ();
		}

		public static async void GetProductAttributesFromRemoteAndSaveToLocal(DateTime? localUpdate, DateTime? remoteUpdate,LoadingPage loadingPage)
		{		
			

			var productQuery = ParseObject.GetQuery (ParseConstants.PRODUCTS_CLASS_NAME).
				WhereGreaterThan(ParseConstants.UPDATEDATE_NAME,localUpdate).
				WhereLessThanOrEqualTo(ParseConstants.UPDATEDATE_NAME,remoteUpdate);

			var productCount = productQuery.CountAsync().Result;

			int queryLimit = 1000;
			int j = 0;
			for (int i = 0; i < productCount; i += queryLimit) {
				var productObjects = productQuery.Limit(queryLimit).Skip(i).FindAsync ().Result;

				List<ProductClass> tempList = new List<ProductClass> ();
				foreach (var productObject in productObjects) {
					ProductClass tempProduct = new ProductClass ();
					tempProduct.objectId = productObject.ObjectId;
					tempProduct.Name = productObject.Get<string> (ParseConstants.PRODUCT_ATTRIBUTE_NAME);
					tempProduct.ImageName = productObject.Get<string> (ParseConstants.PRODUCT_ATTRIBUTE_IMAGENAME);
					tempProduct.ImageID = productObject.Get<string> (ParseConstants.PRODUCT_ATTRIBUTE_IMAGEID);
					tempProduct.CategoryId = productObject.Get<string> (ParseConstants.PRODUCT_ATTRIBUTE_CATEGORYID);
					tempProduct.Price = new Decimal(productObject.Get<double> (ParseConstants.PRODUCT_ATTRIBUTE_PRICE));
					tempProduct.Quantity = productObject.Get<string> (ParseConstants.PRODUCT_ATTRIBUTE_QUANTITY);
					tempProduct.IsTopSelling = productObject.Get<bool> (ParseConstants.PRODUCT_ATTRIBUTE_ISTOPSELLING);
					if (tempList.Count > 152) {
					}
					/*if (productObject.Get<object> (ParseConstants.PRODUCT_ATTRIBUTE_PARENTCATEGORY) == null)
						tempProd*uct.ParentCategory = "";
					else*/
						tempProduct.ParentCategory = productObject.Get<string> (ParseConstants.PRODUCT_ATTRIBUTE_PARENTCATEGORY);
					var storeList = productObject.Get<IEnumerable<object>> (ParseConstants.PRODUCT_ATTRIBUTE_STORES).Cast<Int64>().ToList ();

					foreach (Int64 store in storeList ) {
						tempProduct.Stores += store.ToString ();

						if (store != storeList.Last ())
							tempProduct.Stores += ",";					
					}

					tempList.Add (tempProduct);

					double scrollPos = Decimal.ToDouble (Decimal.Add(Decimal.Multiply (Decimal.Multiply (Decimal.Divide ((Decimal.Divide (1, productCount)), 10), 1), j++),new decimal(0.9f)));
					await loadingPage.ProgressBar1.ProgressTo (scrollPos, 1, Easing.Linear);
				}

				mProductClass.AddProduct (tempList);	
				
			}
			loadingPage.mFirstTokenSource.Cancel ();
		}

		public static void FetchProducts(LoadingPage loadingPage)
		{

			if (MyDevice.GetNetworkStatus() != "NotReachable") {
				DateTime? localUpdate = mUserClass.GetProductsUpdatedDateFromUser ();
				var query = ParseObject.GetQuery (ParseConstants.PRODUCTS_CLASS_NAME).OrderByDescending (ParseConstants.UPDATEDATE_NAME).Limit (1);
				var parseObject = query.FirstAsync ().Result;
				
				DateTime? remoteUpdate = parseObject.UpdatedAt;
				//update category class
				if (remoteUpdate > localUpdate) {
					//pull from remote and add to database
					GetProductAttributesFromRemoteAndSaveToLocal( localUpdate,remoteUpdate,loadingPage);
					mUserClass.AddProductsUpdateDateToUser (remoteUpdate);
				}
				else
					loadingPage.mFirstTokenSource.Cancel ();
			}
			else
				loadingPage.mFirstTokenSource.Cancel ();

			PopulateProductDictionaries ();

		}

		public static void PopulateProductDictionaries()
		{			
			ClearContainers ();

			foreach (ProductClass product in mProductClass.GetProducts()) {	
				if (product.Price <= 0)
					continue;
				
				mProductIDList.Add (product.objectId);
				mProductNameDictionary.Add(product.objectId,product.Name);
				mProductImageIDDictionary.Add(product.objectId,product.ImageID);
				mProductImageNameDictionary.Add(product.objectId,product.ImageName);
				mProductPriceDictionary.Add(product.objectId,product.Price);
				mProductQuantityDictionary.Add(product.objectId,product.Quantity);
				mProductParentCategoryIDsDictionary.Add (product.objectId, product.ParentCategory);
				mProductStoresDictionary.Add (product.objectId, product.Stores);
				mProductIsTopSellingDictionary.Add (product.objectId, product.IsTopSelling);

				if (mProductCategoryIDDictionary.ContainsKey( product.CategoryId)) {
					mProductCategoryIDDictionary [product.CategoryId].Add (product.objectId);
				} else {					
					mProductCategoryIDDictionary.Add( product.CategoryId, new HashSet<string>() );
					mProductCategoryIDDictionary [product.CategoryId].Add (product.objectId);
				}
			}	
		}	

		public static void PopulateSearchProductList(string searchString)
		{
			mSearchProductIDList.Clear ();
			if (mProductNameDictionary.Count > 0) {
				foreach (var pair in mProductNameDictionary) {
					if( ContainsString (pair.Value, searchString, StringComparison.OrdinalIgnoreCase) )
						mSearchProductIDList.Add (pair.Key);
				}
			}
		}

		public static void PopulateSearchProductListWithCategoryID(string searchString,string categoryID)
		{
			mSearchProductIDList.Clear ();
			//Check if has subcategories or not
			if (CategoryModel.mSubCategoryDictionary[categoryID].Count == 0 ) {
				if (mProductCategoryIDDictionary [categoryID] != null && mProductCategoryIDDictionary [categoryID].Count > 0) {
					var hashKey = mProductCategoryIDDictionary [categoryID];
					foreach (var productId in hashKey) {
						var productName = mProductNameDictionary [productId];
						if (ContainsString (productName, searchString, StringComparison.OrdinalIgnoreCase))
							mSearchProductIDList.Add (productId);
					}
				}
			} else {
				var subCategoryList = CategoryModel.mSubCategoryDictionary [categoryID];
				foreach (var subCategoryID in subCategoryList) {
					if (!mProductCategoryIDDictionary.ContainsKey (subCategoryID))
						continue;
					
					var hashKey = mProductCategoryIDDictionary [subCategoryID];
					foreach (var productId in hashKey) {
						var productName = mProductNameDictionary [productId];
						if (ContainsString (productName, searchString, StringComparison.OrdinalIgnoreCase))
							mSearchProductIDList.Add (productId);
					}
				}
			}
		}

		public static bool ContainsString(this string source, string toCheck, StringComparison comp)
		{
			return source.IndexOf(toCheck, comp) >= 0;
		}
	}
}


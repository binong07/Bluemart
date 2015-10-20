using System;
using PCLStorage;
using System.Collections.Generic;
using bluemart.Common.Utilities;
using Parse;
using System.Net.Http;
using System.Linq;
using bluemart.Models.Local;

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
		public static Dictionary<string,double> mProductPriceDictionary;
		public static Dictionary<string,string> mProductQuantityDictionary;
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
			mProductPriceDictionary = new Dictionary<string, double> ();
			mProductQuantityDictionary = new Dictionary<string, string> ();
			mProductCategoryIDDictionary = new Dictionary<string,HashSet<string>> ();
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
			mProductCategoryIDDictionary.Clear ();
		}

		public static void GetProductAttributesFromRemoteAndSaveToLocal(DateTime? localUpdate, DateTime? remoteUpdate)
		{		
			var productQuery = ParseObject.GetQuery (ParseConstants.PRODUCTS_CLASS_NAME).
				WhereGreaterThan(ParseConstants.UPDATEDATE_NAME,localUpdate).
				WhereLessThanOrEqualTo(ParseConstants.UPDATEDATE_NAME,remoteUpdate);

			var productObjects = productQuery.FindAsync ().Result;

			List<ProductClass> tempList = new List<ProductClass> ();
			foreach (var productObject in productObjects) {
				ProductClass tempProduct = new ProductClass ();
				tempProduct.objectId = productObject.ObjectId;
				tempProduct.Name = productObject.Get<string> (ParseConstants.PRODUCT_ATTRIBUTE_NAME);
				tempProduct.ImageName = productObject.Get<string> (ParseConstants.PRODUCT_ATTRIBUTE_IMAGENAME);
				tempProduct.ImageID = productObject.Get<string> (ParseConstants.PRODUCT_ATTRIBUTE_IMAGEID);
				tempProduct.CategoryId = productObject.Get<string> (ParseConstants.PRODUCT_ATTRIBUTE_CATEGORYID);
				tempProduct.Price = productObject.Get<double> (ParseConstants.PRODUCT_ATTRIBUTE_PRICE);
				tempProduct.Quantity = productObject.Get<string> (ParseConstants.PRODUCT_ATTRIBUTE_QUANTITY);

				tempList.Add (tempProduct);
			}

			mProductClass.AddProduct (tempList);	
		}

		public static void FetchProducts()
		{
			if (MyDevice.NetworkStatus != "NotReachable") {
				DateTime? localUpdate = mUserClass.GetProductsUpdatedDateFromUser ();
				var query = ParseObject.GetQuery (ParseConstants.PRODUCTS_CLASS_NAME).OrderByDescending (ParseConstants.UPDATEDATE_NAME).Limit (1);
				var parseObject = query.FirstAsync ().Result;
				DateTime? remoteUpdate = parseObject.UpdatedAt;
				//update category class
				if (remoteUpdate > localUpdate) {
					//pull from remote and add to database
					GetProductAttributesFromRemoteAndSaveToLocal( localUpdate,remoteUpdate);
					mUserClass.AddProductsUpdateDateToUser (remoteUpdate);
				}
			}

			PopulateProductDictionaries ();
		}

		public static void PopulateProductDictionaries()
		{			
			ClearContainers ();

			foreach (ProductClass product in mProductClass.GetProducts()) {					
				mProductIDList.Add (product.objectId);
				mProductNameDictionary.Add(product.objectId,product.Name);
				mProductImageIDDictionary.Add(product.objectId,product.ImageID);
				mProductImageNameDictionary.Add(product.objectId,product.ImageName);
				mProductPriceDictionary.Add(product.objectId,product.Price);
				mProductQuantityDictionary.Add(product.objectId,product.Quantity);	

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

		public static bool ContainsString(this string source, string toCheck, StringComparison comp)
		{
			return source.IndexOf(toCheck, comp) >= 0;
		}
	}
}


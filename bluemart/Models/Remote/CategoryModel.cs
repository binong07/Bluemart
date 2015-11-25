using System;
using bluemart.Common.Utilities;
using PCLStorage;
using System.Threading.Tasks;
using System.Collections.Generic;
using Parse;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using bluemart.Models.Local;
using System.Linq;
using bluemart.Common.Objects;

namespace bluemart.Models.Remote
{
	public static class CategoryModel
	{		
		public static string CategoryLocation;
		public static List<Category> CategoryList;
		public static List<string> mCategoryIDList;
		public static Dictionary<string,string> mCategoryNameDictionary;
		public static Dictionary<string,string> mImageNameDictionary;
		public static Dictionary<string,string> mImageIDDictionary;
		public static Dictionary<string,bool> mIsSubCategoryDictionary;
		public static Dictionary<string,List<string>> mSubCategoryDictionary;
		private static UserClass mUserModel = new UserClass();
		private static CategoryClass mCategoryClass = new CategoryClass();

		static CategoryModel ()
		{
			InitializeMemberVariables ();				
		}
		
		private static void InitializeMemberVariables()
		{			
			CategoryList = new List<Category> ();
			mCategoryIDList = new List<string> ();
			mCategoryNameDictionary = new Dictionary<string,string> ();
			mImageNameDictionary = new Dictionary<string,string> ();
			mImageIDDictionary = new Dictionary<string,string> ();
			mIsSubCategoryDictionary = new Dictionary<string,bool> ();
			mSubCategoryDictionary = new Dictionary<string,List<string>> ();
		}

		/*public static bool CheckIfImageFileExists(string categoryID)
		{
			bool fileExists = false;

			string imageName = mImageNameDictionary [categoryID];				
			if (mRootFolder.CheckExistsAsync (mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + imageName + ".jpg").Result.ToString () != "FileExists") {
				fileExists = false;
			} else
				fileExists = true;

			return fileExists;		
		}*/

		public static int GetCategoryAttributesFromRemoteAndSaveToLocal(DateTime? localUpdate, DateTime? remoteUpdate)
		{							

			var categoryQuery = ParseObject.GetQuery (ParseConstants.CATEGORIES_CLASS_NAME).
				WhereGreaterThan(ParseConstants.UPDATEDATE_NAME,localUpdate).
				WhereLessThanOrEqualTo(ParseConstants.UPDATEDATE_NAME,remoteUpdate).
				OrderBy(ParseConstants.CATEGORY_ATTRIBUTE_NAME);

			var categoryCount = categoryQuery.CountAsync ().Result;

			int queryLimit = 1000;

			for (int i = 0; i < categoryCount; i += queryLimit) {
				var categoryObjects = categoryQuery.Limit(queryLimit).Skip(i).FindAsync ().Result;

				List<CategoryClass> tempList = new List<CategoryClass> ();
				foreach (var categoryObject in categoryObjects) {
					CategoryClass tempCategory = new CategoryClass ();
					tempCategory.objectId = categoryObject.ObjectId;
					tempCategory.Name = categoryObject.Get<string> (ParseConstants.CATEGORY_ATTRIBUTE_NAME);
					tempCategory.ImageName = categoryObject.Get<string> (ParseConstants.CATEGORY_ATTRIBUTE_IMAGENAME);
					tempCategory.ImageID = categoryObject.Get<string> (ParseConstants.CATEGORY_ATTRIBUTE_IMAGEID);
					tempCategory.isSubCategory = categoryObject.Get<bool> (ParseConstants.CATEGORY_ATTRIBUTE_ISSUBCATEGORYNAME);
					var subcategoryList = categoryObject.Get<IEnumerable<object>> (ParseConstants.CATEGORY_ATTRIBUTE_SUB).Cast<string> ().ToList ();
					foreach (string sub in subcategoryList ) {
						tempCategory.Sub += sub;
						if (sub != subcategoryList.Last ())
							tempCategory.Sub += ",";
					}


					tempList.Add (tempCategory);
				}

				mCategoryClass.AddCategory (tempList);

			}

			return categoryCount;
		}

		private static void PopulateCategoryDictionaries()
		{
			//populate categories from database
			mCategoryIDList.Clear ();
			mCategoryNameDictionary.Clear ();
			mImageNameDictionary.Clear ();
			mImageIDDictionary.Clear ();
			mIsSubCategoryDictionary.Clear ();
			mSubCategoryDictionary.Clear ();


			foreach (CategoryClass categoryObj in mCategoryClass.GetCategories()) {
				mCategoryIDList.Add (categoryObj.objectId);
				mCategoryNameDictionary.Add(categoryObj.objectId,categoryObj.Name);
				mImageNameDictionary.Add (categoryObj.objectId,categoryObj.ImageName);
				mImageIDDictionary.Add (categoryObj.objectId,categoryObj.ImageID);
				mIsSubCategoryDictionary.Add (categoryObj.objectId, categoryObj.isSubCategory);
				List<string> subCategoryList = new List<string>();
				if ( categoryObj.Sub != null )
					subCategoryList = categoryObj.Sub.Split(',').ToList<string>();
				
				mSubCategoryDictionary.Add (categoryObj.objectId,subCategoryList);
			}
		}

		public static int FetchCategories()
		{
			int categoryNumber = 0;

			if (MyDevice.GetNetworkStatus() != "NotReachable") {
				DateTime? localUpdate = mUserModel.GetCategoriesUpdatedDateFromUser ();
				var query = ParseObject.GetQuery (ParseConstants.CATEGORIES_CLASS_NAME).OrderByDescending (ParseConstants.UPDATEDATE_NAME).Limit (1);
				var parseObject = query.FirstAsync ().Result;
				DateTime? remoteUpdate = parseObject.UpdatedAt;
				//update category class
				if (remoteUpdate > localUpdate) {
					//pull from remote and add to database
					categoryNumber = GetCategoryAttributesFromRemoteAndSaveToLocal(localUpdate,remoteUpdate);
					mUserModel.AddCategoriesUpdateDateToUser (remoteUpdate);
				}
			}

			PopulateCategoryDictionaries ();

			return categoryNumber;
		}
		
		/*public static void GetImagesAndSaveToLocal()
		{
			if (MyDevice.NetworkStatus != "NotReachable") {
				DateTime? localUpdate = mUserModel.GetImageUpdatedDateFromUser ();
				var query = ParseObject.GetQuery (ParseConstants.IMAGES_CLASS_NAME).OrderByDescending (ParseConstants.UPDATEDATE_NAME).Limit (1);
				var parseObject = query.FirstAsync ().Result;
				DateTime? remoteUpdate = parseObject.UpdatedAt;

				if (remoteUpdate > localUpdate) {
					var imageQuery = ParseObject.GetQuery (ParseConstants.IMAGES_CLASS_NAME).
						WhereGreaterThan (ParseConstants.UPDATEDATE_NAME, localUpdate).
						WhereLessThanOrEqualTo (ParseConstants.UPDATEDATE_NAME, remoteUpdate);

					var imageObjects = imageQuery.FindAsync ().Result;

					foreach (ParseObject imageObject in imageObjects) {
						ParseFile img = imageObject.Get<ParseFile> (ParseConstants.IMAGE_ATTRIBUTE_IMAGEFILE);
						string imageName = imageObject.Get<string> (ParseConstants.IMAGE_ATTRIBUTE_IMAGENAME);

						byte[] data = new HttpClient ().GetByteArrayAsync (img.Url).Result;	

						var folder = mRootFolder.CreateFolderAsync (ParseConstants.IMAGE_FOLDER_NAME, CreationCollisionOption.OpenIfExists).Result;

						var file = folder.CreateFileAsync (imageName + ".jpg", CreationCollisionOption.ReplaceExisting).Result;

						using (System.IO.Stream stream = file.OpenAsync (FileAccess.ReadAndWrite).Result) {
							stream.Write (data, 0, data.Length);
						}
					}
					mUserModel.AddImagesUpdateDateToUser (remoteUpdate);
				}

			} 
		}*/

		public static void PopulateCategories()
		{			
			CategoryList.Clear ();

			foreach (string categoryID in CategoryModel.mCategoryIDList) {
				string ImagePath = ImageModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + CategoryModel.mImageNameDictionary[categoryID] + ".jpg";
				string CategoryName = CategoryModel.mCategoryNameDictionary [categoryID];
				bool isSubCategory = CategoryModel.mIsSubCategoryDictionary [categoryID];
				List<string> SubCategoryIDList = CategoryModel.mSubCategoryDictionary [categoryID];

				CategoryList.Add( new Category( CategoryName,ImagePath,isSubCategory,categoryID,SubCategoryIDList) );
			}
		}
	}
}


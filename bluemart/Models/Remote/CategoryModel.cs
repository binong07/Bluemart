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

namespace bluemart.Models.Remote
{
	public class CategoryModel
	{
		public IFolder mRootFolder;
		public string mRootFolderPath;
		public List<string> mCategoryIDList;
		public Dictionary<string,string> mCategoryNameDictionary;
		public Dictionary<string,string> mImageNameDictionary;
		public Dictionary<string,string> mImageIDDictionary;
		public Dictionary<string,bool> mIsSubCategoryDictionary;
		public Dictionary<string,List<string>> mSubCategoryDictionary;
		private UserClass mUserModel = new UserClass();
		private CategoryClass mCategoryClass = new CategoryClass();

		public CategoryModel ()
		{
			InitializeMemberVariables ();				
		}
		
		private void InitializeMemberVariables()
		{
			mRootFolder = FileSystem.Current.LocalStorage;
			mRootFolderPath = mRootFolder.Path;
			mCategoryIDList = new List<string> ();
			mCategoryNameDictionary = new Dictionary<string,string> ();
			mImageNameDictionary = new Dictionary<string,string> ();
			mImageIDDictionary = new Dictionary<string,string> ();
			mIsSubCategoryDictionary = new Dictionary<string,bool> ();
			mSubCategoryDictionary = new Dictionary<string,List<string>> ();
		}
		
		/*public ExistenceCheckResult CheckIfImageFolderExists()
		{
			return mRootFolder.CheckExistsAsync (mRootFolderPath + "/" +ParseConstants.IMAGE_FOLDER_NAME).Result;
		}*/

		public bool CheckIfImageFileExists(string categoryID)
		{
			bool fileExists = false;

			string imageName = mImageNameDictionary [categoryID];				
			if (mRootFolder.CheckExistsAsync (mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + imageName + ".jpg").Result.ToString () != "FileExists") {
				fileExists = false;
			} else
				fileExists = true;

			return fileExists;		
		}

		public void GetCategoryAttributesFromRemoteAndSaveToLocal(DateTime? localUpdate, DateTime? remoteUpdate)
		{		
			var categoryQuery = ParseObject.GetQuery (ParseConstants.CATEGORIES_CLASS_NAME).
				WhereGreaterThan(ParseConstants.UPDATEDATE_NAME,localUpdate).
				WhereLessThanOrEqualTo(ParseConstants.UPDATEDATE_NAME,remoteUpdate);

			var categoryObjects = categoryQuery.FindAsync ().Result;

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

		private void PopulateCategoryDictionaries()
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

		public void FetchCategories()
		{
			if (MyDevice.NetworkStatus != "NotReachable") {
				DateTime? localUpdate = mUserModel.GetCategoriesUpdatedDateFromUser ();
				var query = ParseObject.GetQuery (ParseConstants.CATEGORIES_CLASS_NAME).OrderByDescending (ParseConstants.UPDATEDATE_NAME).Limit (1);
				var parseObject = query.FirstAsync ().Result;
				DateTime? remoteUpdate = parseObject.UpdatedAt;
				//update category class
				if (remoteUpdate > localUpdate) {
					//pull from remote and add to database
					GetCategoryAttributesFromRemoteAndSaveToLocal(localUpdate,remoteUpdate);
					mUserModel.AddCategoriesUpdateDateToUser (remoteUpdate);
				}
			}

			PopulateCategoryDictionaries ();
		}
		
		public void GetImagesAndSaveToLocal()
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
		}
	}
}


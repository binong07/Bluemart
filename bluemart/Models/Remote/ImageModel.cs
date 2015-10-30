using System;
using PCLStorage;
using System.Linq;
using Parse;
using bluemart.Common.Utilities;
using bluemart.Models.Local;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Net.Http;

namespace bluemart.Models.Remote
{
	public static class ImageModel
	{
		public static IFolder mRootFolder =  FileSystem.Current.LocalStorage;
		public static string mRootFolderPath = mRootFolder.Path;
		private static UserClass mUserModel = new UserClass();
		private static List<string> mImageNameList = new List<string> ();

		/*
		 * Gets image from resource to stream
		 * Copys it to local storage via stream
		*/
		public static void MoveImagesToLocal()
		{
			var assembly = typeof(ImageModel).GetTypeInfo().Assembly;
			var projectName = assembly.GetName ().Name;
			var imageNames = assembly.GetManifestResourceNames ().Where(name => name.Contains(ParseConstants.IMAGE_FOLDER_NAME));
			//Generate resource path to be able to get length
			var directoryPath = String.Concat (projectName, ".", ParseConstants.IMAGE_FOLDER_NAME, ".");

			foreach (string fullImageName in imageNames) {	
				
				using (Stream s = assembly.GetManifestResourceStream(fullImageName) )
				{
					//Start from resource path length
					//to be able to get image name from full name
					var imageName = fullImageName.Substring (directoryPath.Length);

					if (s == null) {
						System.Diagnostics.Debug.WriteLine ("imagename:" + imageName);
						continue;
					}

					var folder = mRootFolder.CreateFolderAsync (ParseConstants.IMAGE_FOLDER_NAME, CreationCollisionOption.OpenIfExists).Result;

					var file = folder.CreateFileAsync (imageName, CreationCollisionOption.ReplaceExisting).Result;

					using (System.IO.Stream stream = file.OpenAsync (FileAccess.ReadAndWrite).Result) {
						s.CopyTo (stream);
					}		
				}
			}
		}

		public static void GetImagesFromRemote()
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
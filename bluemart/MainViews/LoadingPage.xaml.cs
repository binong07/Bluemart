using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Threading.Tasks;
using bluemart.MainViews;
using bluemart.Common.Utilities;
using XLabs.Platform.Services;
using XLabs.Ioc;
using XLabs.Platform.Device;
using bluemart.Models.Remote;
using bluemart.Models.Local;

namespace bluemart
{
	public partial class LoadingPage : ContentPage
	{		
		UserClass mUserModel = new UserClass();

		public LoadingPage ()
		{
			NavigationPage.SetHasNavigationBar (this, false);
			InitializeComponent ();
		}

		protected override async void OnAppearing()
		{		
			await Task.Run (() => MakeNecessaryInitialization ());
			Application.Current.MainPage = new NavigationPage (new MainPage ());
		}			

		private void MakeNecessaryInitialization()
		{		
			mUserModel.CreateUserTable ();

			//Move From EmbeddedResource to SavedImages
			//If the folder doesn't exist
			if( ImageModel.mRootFolder.CheckExistsAsync (ParseConstants.IMAGE_FOLDER_NAME).Result.ToString() != "FolderExists")
				ImageModel.MoveImagesToLocal ();
			ImageModel.GetImagesFromRemote ();			
			CategoryModel.FetchCategories ();
			ProductModel.FetchProducts ();
		}
	}
}


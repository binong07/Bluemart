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
		CategoryModel mCategoryModel = new CategoryModel();
		UserClass mUserModel = new UserClass();

		public LoadingPage ()
		{
			NavigationPage.SetHasNavigationBar (this, false);
			InitializeComponent ();
		}

		protected override async void OnAppearing()
		{		
			await Task.Run (() => MakeNecessaryInitialization ());
			Application.Current.MainPage = new NavigationPage (new MainPage (mCategoryModel));
		}			

		private void MakeNecessaryInitialization()
		{	
			MyDevice.NetworkStatus = Resolver.Resolve<IDevice> ().Network.InternetConnectionStatus ().ToString();
			mUserModel.CreateUserTable ();
			mCategoryModel.FetchCategories ();
			ProductModel.FetchProducts ();
			mCategoryModel.GetImagesAndSaveToLocal ();
		}
	}
}


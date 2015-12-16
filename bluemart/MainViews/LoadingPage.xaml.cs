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
using System.Threading;

namespace bluemart
{
	public partial class LoadingPage : ContentPage
	{		
		UserClass mUserModel = new UserClass();
		public ProgressBar ProgressBar1;
		public CancellationTokenSource mFirstTokenSource = new CancellationTokenSource();
		Image bgImage;
		public LoadingPage ()
		{
			NavigationPage.SetHasNavigationBar (this, false);
			InitializeComponent ();
			ProgressBar1 = new ProgressBar () {
				Progress = 0
			};

			bgImage = new Image {
				Source = "Screen1_BG",
				HeightRequest = MyDevice.GetScaledSize(1920),
				WidthRequest = MyDevice.GetScaledSize(1080),
				Aspect = Aspect.Fill
			};
			/*
			var frame = new Frame { 				
				Padding = 1,
				OutlineColor = Color.Transparent,
				BackgroundColor = Color.Transparent,
				Content = ProgressBar1
			};
*/
			ProgressBar1.WidthRequest = MyDevice.ScreenWidth / 2;

			relLayout1.Children.Add (bgImage, 
				Constraint.Constant (0),
				Constraint.Constant (0)
			);

			relLayout1.Children.Add (ProgressBar1, 
				Constraint.Constant (MyDevice.ScreenWidth/2-MyDevice.ScreenWidth/4),
				Constraint.Constant (MyDevice.ScreenHeight/2)
			);
				
			//SlideImage ();
			Load();
		}

		private async void SlideImage()
		{
			while (true) {
				await bgImage.TranslateTo (MyDevice.ScreenHeight*1.5f / 2 * -1, 0, 24000, Easing.Linear);
				await bgImage.TranslateTo (0, 0, 24000, Easing.Linear);
			}

		}


		private async void Load()
		{
			//await bgImage.TranslateTo (MyDevice.ScreenWidth * -1, 0, 6000, Easing.Linear);
			//var newPos = new Rectangle (10, 0, 40, 40);
			//await bgImage.LayoutTo ( newPos, 6000, Easing.Linear);
			mUserModel.CreateUserTable ();
			await Task.Delay (100);
			if (ImageModel.mRootFolder.CheckExistsAsync (ParseConstants.IMAGE_FOLDER_NAME).Result.ToString () != "FolderExists") {
				ImageModel.MoveImagesToLocal (this);
				try {
					await Task.Delay (600000000, mFirstTokenSource.Token);
				} catch {
					mFirstTokenSource = new CancellationTokenSource ();
				}
			}
			await ProgressBar1.ProgressTo (0.2f, 250, Easing.Linear);
			ImageModel.GetImagesFromRemote (this);
			try{
				await Task.Delay (600000000,mFirstTokenSource.Token);
			}
			catch {
				mFirstTokenSource = new CancellationTokenSource ();
			}

			await ProgressBar1.ProgressTo (0.8f, 250, Easing.Linear);

			CategoryModel.FetchCategories (this);

			try{
				await Task.Delay (600000000,mFirstTokenSource.Token);
			}
			catch {
				CategoryModel.PopulateCategoryDictionaries ();
				mFirstTokenSource = new CancellationTokenSource ();
			}

			await ProgressBar1.ProgressTo (0.9f, 250, Easing.Linear);

			ProductModel.FetchProducts (this);

			try{
				await Task.Delay (600000000,mFirstTokenSource.Token);
			}
			catch {
				ProductModel.PopulateProductDictionaries ();
				mFirstTokenSource = new CancellationTokenSource ();
			}

			await ProgressBar1.ProgressTo (1f, 250, Easing.Linear);
			Application.Current.MainPage = new NavigationPage (new MainPage ());
		}			
	}
}


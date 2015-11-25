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
		public LoadingPage ()
		{
			NavigationPage.SetHasNavigationBar (this, false);
			InitializeComponent ();
			ProgressBar1 = new ProgressBar () {
				Progress = 0
			};

			var frame = new Frame { 				
				Padding = 1,
				OutlineColor = Color.Gray,
				BackgroundColor = Color.Gray,
				Content = ProgressBar1
			};

			ProgressBar1.WidthRequest = MyDevice.ScreenWidth / 2;

			relLayout1.Children.Add (frame, 
				Constraint.Constant (MyDevice.ScreenWidth/2-MyDevice.ScreenWidth/4),
				Constraint.Constant (MyDevice.ScreenHeight/2)
			);

			Load();
		}


		private async void Load()
		{
			mUserModel.CreateUserTable ();
			await Task.Delay (100);
			ImageModel.MoveImagesToLocal (this);
			try{
				await Task.Delay (60000,mFirstTokenSource.Token);
			}
			catch {
				mFirstTokenSource = new CancellationTokenSource ();
			}

			ImageModel.GetImagesFromRemote (this);
			try{
				await Task.Delay (600000000,mFirstTokenSource.Token);
			}
			catch {
				mFirstTokenSource = new CancellationTokenSource ();
			}

			//await ProgressBar1.ProgressTo (.6, 5000, Easing.Linear);
			//ImageModel.MoveImagesToLocal(this);

			//await ImageModel.MoveImagesToLocal(this);
			//Task a = ImageModel.MoveImagesToLocal(this);
			//Task.WaitAny (a);
			//if (ImageModel.mRootFolder.CheckExistsAsync (ParseConstants.IMAGE_FOLDER_NAME).Result.ToString () != "FolderExists") {
				/*int recordCount = Task.Run(ImageModel.MoveImagesToLocal (this));
				for (int i = 0; i < recordCount; i++) {				
					double scrollPos = Decimal.ToDouble (Decimal.Multiply (Decimal.Multiply (Decimal.Divide ((Decimal.Divide (1, recordCount)), 10), 2), i));
					await ProgressBar1.ProgressTo (scrollPos, 1, Easing.Linear);
				}*/
			//}
			//await ProgressBar1.ProgressTo (.2, 1, Easing.Linear);

			/*int remoteImageNumber = ImageModel.GetImagesFromRemote ();	
			for (int i = 0; i < remoteImageNumber; i++) {				
				double scrollPos = Decimal.ToDouble (Decimal.Add(Decimal.Multiply (Decimal.Multiply (Decimal.Divide ((Decimal.Divide (1, remoteImageNumber)), 10), 3), i),new decimal(0.2f)));
				await ProgressBar1.ProgressTo (scrollPos, 1, Easing.Linear);
			}

			await ProgressBar1.ProgressTo (.5, 1, Easing.Linear);

			int categoryNumber = CategoryModel.FetchCategories ();

			for (int i = 0; i < categoryNumber; i++) {				
				double scrollPos = Decimal.ToDouble (Decimal.Add(Decimal.Multiply (Decimal.Multiply (Decimal.Divide ((Decimal.Divide (1, categoryNumber)), 10), 2), i),new decimal(0.5f)));
				await ProgressBar1.ProgressTo (scrollPos, 1, Easing.Linear);
			}

			await ProgressBar1.ProgressTo (.8, 1, Easing.Linear);

			int productNumber = ProductModel.FetchProducts ();
			for (int i = 0; i < productNumber; i++) {				
				double scrollPos = Decimal.ToDouble (Decimal.Add(Decimal.Multiply (Decimal.Multiply (Decimal.Divide ((Decimal.Divide (1, productNumber)), 10), 3), i),new decimal(0.8f)));
				await ProgressBar1.ProgressTo (scrollPos, 1, Easing.Linear);
			}

			await ProgressBar1.ProgressTo (1, 1, Easing.Linear);

			Application.Current.MainPage = new NavigationPage (new MainPage ());*/
		}			
	}
}


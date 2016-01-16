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
using FFImageLoading.Forms;

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
				Source = "Loading_BG",
				WidthRequest = MyDevice.GetScaledSize(640),
				/*CacheDuration = TimeSpan.FromDays(30),
				//DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false,*/
				Aspect = Aspect.Fill
			};

			var footerImage = new CachedImage () {
				Source = "Loading_Footer",
				WidthRequest = MyDevice.GetScaledSize(421),
				HeightRequest = MyDevice.GetScaledSize(94),
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 10,
				RetryDelay = 250,
				TransparencyEnabled = false,
				FadeAnimationEnabled = false
			};

			var versionText = new Label () {
				Text = "Ver 1.0",
				FontSize = MyDevice.FontSizeMicro,
				HorizontalTextAlignment = TextAlignment.Center,
				WidthRequest = MyDevice.GetScaledSize(80),
				HeightRequest = MyDevice.GetScaledSize(30),
				TextColor = Color.White,
				FontAttributes = FontAttributes.Bold
			};

			ProgressBar1.WidthRequest = MyDevice.GetScaledSize (415);
			ProgressBar1.HeightRequest = MyDevice.GetScaledSize (90);

			relLayout1.Children.Add (bgImage, 
				Constraint.Constant (0),
				Constraint.Constant (0)
			);

			relLayout1.Children.Add (footerImage,
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Left +  MyDevice.GetScaledSize(114);
				}),
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Bottom - MyDevice.GetScaledSize(170);
				})
			);

			relLayout1.Children.Add (versionText,
				Constraint.RelativeToView (footerImage, (parent, sibling) => {
					return sibling.Bounds.Right - MyDevice.GetScaledSize (80);
				}),
				Constraint.RelativeToView (footerImage, (parent, sibling) => {
					return sibling.Bounds.Bottom + MyDevice.GetScaledSize (10);
				})
			);

			relLayout1.Children.Add (ProgressBar1, 
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Left +  MyDevice.GetScaledSize(117);
				}),
				Constraint.RelativeToParent (parent => {
					return parent.Bounds.Bottom - MyDevice.GetScaledSize(130);
				})
			);
				
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

			DateTime start = DateTime.Now;

			if (ImageModel.mRootFolder.CheckExistsAsync (ParseConstants.IMAGE_FOLDER_NAME).Result.ToString () != "FolderExists") {
				ImageModel.MoveImagesToLocal (this);
				try {
					await Task.Delay (600000000, mFirstTokenSource.Token);
				} catch {
					mFirstTokenSource = new CancellationTokenSource ();
				}
			}
			await ProgressBar1.ProgressTo (0.2f, 250, Easing.Linear);

			TimeSpan delta = start - DateTime.Now;
			System.Diagnostics.Debug.WriteLine ("Move Local takes:" + delta.TotalMilliseconds.ToString());

			start = DateTime.Now;

			ImageModel.GetImagesFromRemote (this);
			try{
				await Task.Delay (600000000,mFirstTokenSource.Token);
			}
			catch {
				mFirstTokenSource = new CancellationTokenSource ();
			}

			await ProgressBar1.ProgressTo (0.8f, 250, Easing.Linear);

			delta = start - DateTime.Now;
			System.Diagnostics.Debug.WriteLine ("Get Images From Remote takes:" + delta.TotalMilliseconds.ToString());

			start = DateTime.Now;

			CategoryModel.FetchCategories (this);

			try{
				await Task.Delay (600000000,mFirstTokenSource.Token);
			}
			catch {
				CategoryModel.PopulateCategoryDictionaries ();
				mFirstTokenSource = new CancellationTokenSource ();
			}

			await ProgressBar1.ProgressTo (0.9f, 250, Easing.Linear);

			delta = start - DateTime.Now;
			System.Diagnostics.Debug.WriteLine ("Get Categories takes:" + delta.TotalMilliseconds.ToString());

			start = DateTime.Now;

			await Task.Factory.StartNew (() => ProductModel.FetchProducts (this)
				, TaskCreationOptions.LongRunning
			);

			//ProductModel.FetchProducts (this);

			//mFirstTokenSource = new CancellationTokenSource();
			await Task.Factory.StartNew (() => ProductModel.PopulateProductDictionaries ()
				, TaskCreationOptions.LongRunning
			);//, mFirstTokenSource.Token); 

			/*try{
				await Task.Delay (600000000,mFirstTokenSource.Token);
			}
			catch {
				ProductModel.PopulateProductDictionaries ();
				mFirstTokenSource = new CancellationTokenSource ();
			}*/

			await ProgressBar1.ProgressTo (1f, 250, Easing.Linear);

			delta = start - DateTime.Now;
			System.Diagnostics.Debug.WriteLine ("Get Products takes:" + delta.TotalMilliseconds.ToString());

			Application.Current.MainPage = new NavigationPage (new MainPage ());
		}			
	}
}


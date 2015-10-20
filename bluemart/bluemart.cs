using System;
using bluemart.MainViews;
using Xamarin.Forms;

namespace bluemart
{
	public class App : Application
	{	
		public App ()
		{
			MainPage = new NavigationPage( new LoadingPage() );
		}


		 
		protected override void OnStart ()
		{

		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}


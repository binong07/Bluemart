using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using XLabs.Forms;
using XLabs.Platform.Device;
using XLabs.Ioc;
using XLabs.Platform.Services;
using Parse;
using Android.Util;
using FFImageLoading.Forms.Droid;
using FFImageLoading;


namespace bluemart.Droid
{
	[Activity (Label = "Bluemart", Icon = "@drawable/icon1", MainLauncher = true, ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : XFormsApplicationDroid
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			FontsOverride.setDefaultFont(this, "DEFAULT", "fonts/Arial-Rounded-MT-Bold.ttf");

			global::Xamarin.Forms.Forms.Init (this, bundle);
				Xamarin.FormsMaps.Init(this, bundle);

			MR.Gestures.Android.Settings.LicenseKey = "6XZT-V54J-73E4-4VK8-RFAJ-ZBS8-Q8UB-Y3W7-BSGP-FETM-EYQ9-QEPP-WRTA";
			CachedImageRenderer.Init ();

			//Necessary IOC Code To Get Device Properties
			var container = new SimpleContainer ();
			container.Register<IDevice> (t => AndroidDevice.CurrentDevice);
			container.Register<IDisplay> (t => t.Resolve<IDevice> ().Display);
			if( !Resolver.IsSet )
				Resolver.SetResolver (container.GetResolver ());

			// Initialize the parse bluemart client
			try
			{
				ParseClient.Initialize("EUDL8rKwCc1JcL8tw5KsW1QB9ePSGx2dSBTobbE5","PNOG7XhRV8tuB907fQ0S0b5ShaIzYN0wVPZ3AyoN");
			}
			catch(ParseException e) {
				Log.Warn("BlueMart",e.Message.ToString());
			}

			LoadApplication (new App ());


		}
	}
}


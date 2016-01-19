using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using FFImageLoading.Forms.Touch;
using XLabs.Ioc;
using XLabs.Platform.Device;
using Parse;

namespace bluemart.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			Xamarin.FormsMaps.Init ();
			// Code for starting up the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
			#endif

			MR.Gestures.iOS.Settings.LicenseKey = "6XZT-V54J-73E4-4VK8-RFAJ-ZBS8-Q8UB-Y3W7-BSGP-FETM-EYQ9-QEPP-WRTA";
			CachedImageRenderer.Init ();

			//ImageService.Initialize(
			//Necessary IOC Code To Get Device Properties
			var container = new SimpleContainer ();
			container.Register<IDevice> (t => AppleDevice.CurrentDevice);
			container.Register<IDisplay> (t => t.Resolve<IDevice> ().Display);
			if( !Resolver.IsSet )
				Resolver.SetResolver (container.GetResolver ());

			// Initialize the parse bluemart client
			try
			{
				ParseClient.Initialize("EUDL8rKwCc1JcL8tw5KsW1QB9ePSGx2dSBTobbE5","PNOG7XhRV8tuB907fQ0S0b5ShaIzYN0wVPZ3AyoN");
			}
			catch(ParseException e) {
				//Log.Warn("BlueMart",e.Message.ToString());
			}


			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}


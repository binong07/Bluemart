using System;
using XLabs.Platform.Device;
using XLabs.Ioc;
using XLabs.Platform.Services;
using Xamarin.Forms;
using SQLite;
using bluemart.MainViews;

namespace bluemart.Common.Utilities
{
	public static class MyDevice
	{
		private static double DeviceHeight = Resolver.Resolve<IDevice> ().Display.Height;
		private static double DeviceWidth = Resolver.Resolve<IDevice> ().Display.Width;
		private static double DeviceHeightInInches = Resolver.Resolve<IDevice> ().Display.HeightRequestInInches(1);
		private static double DeviceWidthInInches = Resolver.Resolve<IDevice> ().Display.WidthRequestInInches(1);
		private static double ScreenWidthInches = Resolver.Resolve<IDevice> ().Display.ScreenWidthInches ();
		private static double ScreenHeightInches = Resolver.Resolve<IDevice> ().Display.ScreenHeightInches ();
		public static double ScreenWidth = Resolver.Resolve<IDevice> ().Display.WidthRequestInInches (1) * Resolver.Resolve<IDevice> ().Display.ScreenWidthInches ();
		public static double ScreenHeight = Resolver.Resolve<IDevice> ().Display.HeightRequestInInches (1) * Resolver.Resolve<IDevice> ().Display.ScreenHeightInches ();

		/*#if __ANDROID__
		public static double ScreenHeight = Resolver.Resolve<IDevice> ().Display.HeightRequestInInches (1) * Resolver.Resolve<IDevice> ().Display.ScreenHeightInches ();
		#else
		public static double ScreenHeight = Resolver.Resolve<IDevice> ().Display.HeightRequestInInches (1) * Resolver.Resolve<IDevice> ().Display.ScreenHeightInches ()-5;
		#endif*/
		public static Color BlueColor =	Color.FromRgb (12,92,169);
		public static Color RedColor = Color.FromRgb (204,27,39);
		public static Color BackgroundColor = Color.FromRgb (230, 230, 230);
		public static Color SelectedColor = Color.FromRgb (162, 183, 182);
		public static Color GreyColor = Color.FromRgb (154, 156, 162);
		public static Color PinkColor = Color.FromRgb (233, 90, 75);
		public static int DelayTime = 1;
		public static string DeviceID = Resolver.Resolve<IDevice> ().Id;
		public static double MenuPadding = MyDevice.ScreenWidth / 25;
		public static double ViewPadding = MyDevice.ScreenWidth / 50;
		public static double FontSizeMicro = ScreenWidth/36.0;
		public static double FontSizeSmall = ScreenWidth/25.7;
		public static double FontSizeMedium = ScreenWidth/20.0;
		public static double FontSizeLarge = ScreenWidth/16.3;
		public static double SwipeDistance = ScreenWidth / 2;
		public static uint AnimationTimer = 300;
		public static Page currentPage;
		public static RootPage rootPage;
		public static string GetNetworkStatus()
		{
			return Resolver.Resolve<IDevice> ().Network.InternetConnectionStatus ().ToString();
		}

		public static double GetScaledSize(double x){
			return ScreenWidth * (x/640.0);
		}

	}
}


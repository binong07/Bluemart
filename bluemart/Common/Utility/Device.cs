using System;
using XLabs.Platform.Device;
using XLabs.Ioc;
using XLabs.Platform.Services;
using Xamarin.Forms;

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
		public static Color BlueColor =	Color.FromRgb (12,92,169);
		public static Color RedColor = Color.FromRgb (204,27,39);
		public static int DelayTime = 10;
		public static string DeviceID = Resolver.Resolve<IDevice> ().Id;

		public static string GetNetworkStatus()
		{
			return Resolver.Resolve<IDevice> ().Network.InternetConnectionStatus ().ToString();
		}
	}
}


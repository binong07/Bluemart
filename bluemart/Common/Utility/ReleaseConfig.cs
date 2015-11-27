using System;
using XLabs.Platform.Device;
using XLabs.Ioc;
using XLabs.Platform.Services;
using Xamarin.Forms;

namespace bluemart.Common.Utilities
{
	public static class ReleaseConfig
	{
		public static DateTime LAST_UPDATEDATE;
		//public static string FONT_PATH = "Arial-Rounded-MT-Bold";
		public static string FONT_PATH = "monospace";

		static ReleaseConfig()
		{
			DateTime.TryParse ("2015-11-27T10:56:57.003Z", out LAST_UPDATEDATE);
		}
	}
}


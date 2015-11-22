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

		static ReleaseConfig()
		{
			DateTime.TryParse ("2015-11-14T18:56:57.003Z", out LAST_UPDATEDATE);
		}
	}
}


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
			DateTime.TryParse ("2015-10-29T07:07:39.035Z", out LAST_UPDATEDATE);
		}
	}
}


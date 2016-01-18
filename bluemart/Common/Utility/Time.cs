using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bluemart.Common.Utilities
{
	public static class Time
	{
		private static string URL = "http://api.timezonedb.com/?zone=Asia%2FDubai&format=json&key=3KQP2BBMXURP";

		public static int GetTime()
		{
			var client = new HttpClient ();
			var response = client.GetStringAsync (URL).Result;
			var jObj = JObject.Parse (response);
			var date = ConvertTimestampToDateTime (Convert.ToDouble (jObj ["timestamp"].ToString ()));
			return date.Hour;
		}

		private static DateTime ConvertTimestampToDateTime(double timeStamp)
		{
			System.DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds( timeStamp );
			return dtDateTime;
		}
	}
}


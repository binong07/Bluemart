using System;

namespace bluemart.Common
{
	public class PinData
	{
		public double lat;
		public double lng;
		public string icon;
		public string text;
		public PinData (double lat,double lng,string icon,string text)
		{
			this.lat = lat;
			this.lng = lng;
			this.icon = icon;
			this.text = text;
		}
	}
}


using System;
using System.Collections.Generic;

namespace bluemart.Common.Utilities
{
	public static class RegionHelper
	{
		//Location Keys
		private const string DISCOVERY_GARDENS = "Discovery Gardens";
		private const string DUBAI_MARINA = "Dubai Marina";
		private const string EMIRATES_HILLS = "Emirates Hills";
		private const string GREENS = "Greens";
		private const string INTERNET_CITY = "Internet City";
		private const string JLT = "JLT";
		private const string KNOWLEDGE_VILLAGE = "Knowledge Village";
		private const string LAKES = "Lakes";
		private const string MEADOWS = "Meadows";
		private const string MEDIA_CITY = "Media City";
		private const string PALM_JUMEIRAH = "Palm Jumeirah";
		private const string SPRINGS = "Springs";
		private const string TECOM = "Tecom";

		public static readonly List<string> locationList = new List<string>(){DISCOVERY_GARDENS,DUBAI_MARINA,EMIRATES_HILLS,GREENS,
			INTERNET_CITY,JLT,KNOWLEDGE_VILLAGE,LAKES,MEADOWS,MEDIA_CITY,PALM_JUMEIRAH,SPRINGS,TECOM};
		/*
		 * DubaiMarina(0) => jlt,dubai marina, discovery gardens
		 * tecom(1) => tecom, media city, internet city, knowledge village, palm
		 * bm greens(2) => greens, springs, meadows, lakes, emirates hills
		 */

		public static int DecideShopNumber( string location )
		{
			int shopNumber = 0;

			switch (location) {
			case JLT:
			case DUBAI_MARINA:
			case DISCOVERY_GARDENS:
				shopNumber = 0;
				break;
			case TECOM:
			case MEDIA_CITY:
			case INTERNET_CITY:
			case KNOWLEDGE_VILLAGE:
			case PALM_JUMEIRAH:
				shopNumber = 1;
				break;
			case GREENS:
			case SPRINGS:
			case MEADOWS:
			case LAKES:
			case EMIRATES_HILLS:
				shopNumber = 2;
				break;
			default:
				shopNumber = 0;
				break;
			}

			return shopNumber;
		}
	}
}


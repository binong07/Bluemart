using System;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using System.Collections.Generic;
using Xamarin.Forms;
using bluemart.MainViews;
namespace bluemart.Common
{
	public class ExtendedMap : Map
	{
		public List<PinData> pinDatas;
		public Position center;
		public bool isOverlayNeeded = false;
		public Position sw;
		public Position ne;
		public string overlayURL;
		MapView view;
		public ExtendedMap(Position c,double d,List<PinData> pDatas,MapView view)
		//Default start position
			: base(MapSpan.FromCenterAndRadius(c, Distance.FromKilometers(d)))
		{
			this.view = view;
			pinDatas = new List<PinData> ();
			for (int i = 0; i < pDatas.Count; i++)
				pinDatas.Add (pDatas[i]);
			center = c;
		}

		public void pin_Clicked( int i)
		{
			view.OnPinClicked (i);
			//Debug.WriteLine("Pin clicked : " + i.ToString());
		}
	}
}


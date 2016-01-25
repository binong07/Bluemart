using System.ComponentModel;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Views;
using Android.Widget;
using bluemart;
using bluemart.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps.Android;
using Application = Android.App.Application;
using Button = Android.Widget.Button;
using View = Android.Views.View;
using Android.Graphics;
using Android.OS;
using System.Net;
using bluemart.Common;
using bluemart.Common.Utilities;
[assembly: ExportRenderer(typeof(ExtendedMap), typeof(ExtendedMapRenderer))]

namespace bluemart.Droid
{
	public class ExtendedMapRenderer : MapRenderer//, GoogleMap.IInfoWindowAdapter
	{
		ExtendedMap extendedMap;
		MapView mapView;
		GoogleMap map;

		private Bitmap GetImageBitmapFromUrl(string url)
		{
			Bitmap imageBitmap = null;
			BitmapFactory.Options options = new BitmapFactory.Options();
			options.InSampleSize = 2;
			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
				{
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length,options);
				}
			}
			return imageBitmap;
		}

		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.View> e)
		{
			base.OnElementChanged(e);




			extendedMap = (ExtendedMap)Element;
			mapView = Control as MapView;
			map = mapView.Map;

			map.MarkerClick+= HandleMarkerClick;

			// Pin tıklanınca sağalta açılan menüyü engellemek için
			map.UiSettings.MapToolbarEnabled = true;
			map.UiSettings.MyLocationButtonEnabled = true;

			if (extendedMap.isOverlayNeeded) {
				LatLng southwest = new LatLng (extendedMap.sw.Latitude, extendedMap.sw.Longitude);
				LatLng northeast = new LatLng (extendedMap.ne.Latitude, extendedMap.ne.Longitude);

				LatLngBounds bounds = new LatLngBounds (southwest, northeast);

				string url = extendedMap.overlayURL;//"http://www.mgm.gov.tr/mobile/mblhrt/data/radar/MAX--_6100_P00.png";

				Bitmap objBitmap = GetImageBitmapFromUrl (url);

				BitmapDescriptor objBitmapDescriptor = BitmapDescriptorFactory.FromBitmap (objBitmap);
				GroundOverlayOptions objGroundOverlayOptions = new GroundOverlayOptions ().PositionFromBounds (bounds)/*.Position (objMapPosition, 100000)*/.InvokeImage (objBitmapDescriptor);

				map.AddGroundOverlay (objGroundOverlayOptions);

				//For freeing memory
				objBitmap.Recycle ();
			}

			for (int i = 0; i < extendedMap.pinDatas.Count; i++) {
				var markerWithIcon = new MarkerOptions ();
				markerWithIcon.SetPosition (new LatLng (extendedMap.pinDatas[i].lat, extendedMap.pinDatas[i].lng));
				markerWithIcon.SetTitle (i.ToString());
				/*markerWithIcon.SetTitle ("aa");
				markerWithIcon.SetSnippet ("bb");*/
				int resID = Resources.GetIdentifier (extendedMap.pinDatas [i].icon, "drawable" , "com.app1001.bluemart");
				//System.Diagnostics.Debug.WriteLine (resID);
				markerWithIcon.SetIcon(BitmapDescriptorFactory.FromResource(resID));
				map.AddMarker (markerWithIcon);
			}

			//Add Pins


			//map.SetInfoWindowAdapter(this);
			map.UiSettings.RotateGesturesEnabled = false;
		}
		private Circle circle;
		private Polygon polygon;
		private void ClearMapOverlays()
		{
			if(circle!=null)
				circle.Remove ();
			if(polygon!=null)
				polygon.Remove ();
		}

		private void HandleMarkerClick (object sender, GoogleMap.MarkerClickEventArgs e)
		{
			ClearMapOverlays ();
			Marker marker = e.Marker;
			int index = int.Parse (marker.Title);
			extendedMap.pin_Clicked (index );


			Android.Graphics.Color clrb = new Android.Graphics.Color (12,92,169,130);
			Android.Graphics.Color clrr = new Android.Graphics.Color (204,27,39,130);
			PolygonOptions polygonOptions = new PolygonOptions();
			switch (index)
			{
			case 0:
				CircleOptions circleOptions = new CircleOptions ();
				circleOptions.InvokeCenter (new LatLng (extendedMap.pinDatas [index].lat, extendedMap.pinDatas [index].lng));
				circleOptions.InvokeRadius (2000);
					
				circleOptions.InvokeFillColor (clrb.ToArgb ());
				circleOptions.InvokeStrokeColor (clrr.ToArgb ());

				circle = map.AddCircle (circleOptions);

				break;
			case 1:
				polygonOptions.Add(new LatLng(extendedMap.pinDatas[index].lat-0.01, extendedMap.pinDatas[index].lng+0.01));
				polygonOptions.Add(new LatLng(extendedMap.pinDatas[index].lat+0.01, extendedMap.pinDatas[index].lng+0.01));
				polygonOptions.Add(new LatLng(extendedMap.pinDatas[index].lat+0.01, extendedMap.pinDatas[index].lng-0.01));
				polygonOptions.Add(new LatLng(extendedMap.pinDatas[index].lat-0.01, extendedMap.pinDatas[index].lng-0.01));
				polygonOptions.InvokeFillColor (clrb.ToArgb());
				polygonOptions.InvokeStrokeColor (clrr.ToArgb());
				polygon = map.AddPolygon(polygonOptions);
				break;
			case 2:
				polygonOptions.Add(new LatLng(extendedMap.pinDatas[index].lat-0.01, extendedMap.pinDatas[index].lng+0.01));
				polygonOptions.Add(new LatLng(extendedMap.pinDatas[index].lat+0.01, extendedMap.pinDatas[index].lng+0.01));
				polygonOptions.Add(new LatLng(extendedMap.pinDatas[index].lat+0.005, extendedMap.pinDatas[index].lng));
				polygonOptions.Add(new LatLng(extendedMap.pinDatas[index].lat+0.02, extendedMap.pinDatas[index].lng-0.02));
				polygonOptions.Add(new LatLng(extendedMap.pinDatas[index].lat-0.01, extendedMap.pinDatas[index].lng-0.01));
				polygonOptions.InvokeFillColor (clrb.ToArgb());
				polygonOptions.InvokeStrokeColor (clrr.ToArgb());
				polygon = map.AddPolygon(polygonOptions);
				break;
				default:
				CircleOptions circleOptions2 = new CircleOptions ();
				circleOptions2.InvokeCenter (new LatLng (extendedMap.pinDatas [index].lat, extendedMap.pinDatas [index].lng));
				circleOptions2.InvokeRadius (2000);

				circleOptions2.InvokeFillColor (clrb.ToArgb ());
				circleOptions2.InvokeStrokeColor (clrr.ToArgb ());

				circle = map.AddCircle (circleOptions2);

				break;
			}

		}
		/*protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine ("OnElementPropertyChanged");
			base.OnElementPropertyChanged(sender, e);
			//var map = ((MapView) Control).Map;




			//System.Diagnostics.Debug.WriteLine ("2");

		}*/
		/*public View GetInfoContents(Marker marker)
		{*/
			//extendedMap.pin_Clicked2 ();
			/*LayoutInflater inflater = Application.Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
			//System.Diagnostics.Debug.WriteLine ("Pin Clicked AA");
			if (inflater != null)
			{
				View v = inflater.Inflate(Resource.Layout.MapInfoWindow, null);

				TextView infoTitle = v.FindViewById(Resource.Id.infoWindowTitle) as TextView;
				TextView infoSubtitle = v.FindViewById(Resource.Id.infoWindowSubtitle) as TextView;
				Button button = v.FindViewById(Resource.Id.infoWindowButton) as Button;

				if (infoTitle != null) infoTitle.Text = marker.Title;
				if (infoSubtitle != null) infoSubtitle.Text = marker.Snippet;

				return v;
			}*/
			/*return null;
		}

		public View GetInfoWindow(Marker marker)
		{
			return null;
		}*/
	}
}
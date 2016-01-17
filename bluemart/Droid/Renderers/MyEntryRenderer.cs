using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using CustomRenderer.Android;
using Android.Graphics.Drawables;

[assembly: ExportRenderer (typeof(Entry), typeof(MyEntryRenderer))]
namespace CustomRenderer.Android
{
	class MyEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{			
			base.OnElementChanged (e);
			if (Control != null) {
				Control.Background.SetAlpha (0);
			}
		}
	}
}
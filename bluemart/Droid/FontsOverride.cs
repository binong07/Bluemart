using System;
using Android.Content;
using Android.Graphics;
using Java.Lang.Reflect;
using Android.App;
using System.Reflection;
using Java.Lang;
using Android.Runtime;

namespace bluemart.Droid
{
	public static class FontsOverride {

		public static void setDefaultFont(Context context, string staticTypefaceFieldName, string fontAssetName) {
			Typeface regular = Typeface.CreateFromAsset (Application.Context.Assets, fontAssetName);
			replaceFont(staticTypefaceFieldName, regular);
		}

		static void replaceFont(string staticTypefaceFieldName,Typeface newTypeface) {
			try {
				IntPtr cls = JNIEnv.FindClass("android/graphics/Typeface"); 
				IntPtr fld = JNIEnv.GetStaticFieldID(cls, staticTypefaceFieldName, "Landroid/graphics/Typeface;"); 
				JNIEnv.SetStaticField(cls, fld, newTypeface.Handle);
			} catch {
			}
		}
	}
}


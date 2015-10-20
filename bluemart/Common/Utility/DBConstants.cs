using System;
using System.IO;
using PCLStorage;

namespace bluemart.Common.Utilities
{
	public static class DBConstants
	{
		private const string DB_NAME = "bluemart.db3";
		private static IFolder DB_FOLDER = FileSystem.Current.LocalStorage;
		private static string DB_ROOTFOLDERNAME = DB_FOLDER.Path;
		public static string DB_PATH = DB_ROOTFOLDERNAME + "/" + DB_NAME;
	}
}


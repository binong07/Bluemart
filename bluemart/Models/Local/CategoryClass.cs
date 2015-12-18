using System;
using SQLite;
using bluemart.Common.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace bluemart.Models.Local
{
	[Table("Categories")]
	public class CategoryClass
	{
		[PrimaryKey]
		public string objectId { get; set; }
		public string Sub { get; set; }
		public bool isSubCategory{ get; set; }
		public string ImageID { get; set; }
		public string ImageName { get; set; }
		public string Name { get; set; }
		public int Priority { get; set; }

		private bool TableExists<T> (SQLiteConnection connection,string tableName)
		{    
			const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
			var cmd = connection.CreateCommand (cmdText, tableName);
			return cmd.ExecuteScalar<string> () != null;
		}
		//string objectId, string [] Sub, bool isSubCategory, string ImageID, string Name
		public void AddCategory( List<CategoryClass> categoryList  )
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<CategoryClass> (db,"Categories")) {
				db.CreateTable<CategoryClass>();
			}

			foreach( CategoryClass category in categoryList)
				db.InsertOrReplace (category);

			db.Close ();
		}

		public List<CategoryClass> GetCategories()
		{
			List<CategoryClass> categoryList = new List<CategoryClass> ();

			var db = new SQLiteConnection (DBConstants.DB_PATH);
		

			if (!TableExists<CategoryClass> (db,"Categories")) {
				return categoryList;			
			}

			var query = from Categories in  db.Table<CategoryClass> ()
				orderby Categories.Priority,Categories.Name
				select Categories;

			foreach (var category in query) {
				categoryList.Add (category);
			}

			db.Close ();

			return categoryList;
		}
	}
}


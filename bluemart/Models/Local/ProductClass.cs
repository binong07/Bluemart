using System;
using SQLite;
using bluemart.Common.Utilities;
using System.Collections.Generic;

namespace bluemart.Models.Local
{
	[Table("Products")]
	public class ProductClass
	{
		[PrimaryKey]
		public string objectId { get; set; }
		public string CategoryId { get; set; }
		public string ImageID { get; set; }
		public string ImageName { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public string Quantity { get; set; }

		private bool TableExists<T> (SQLiteConnection connection,string tableName)
		{    
			const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
			var cmd = connection.CreateCommand (cmdText, tableName);
			return cmd.ExecuteScalar<string> () != null;
		}
		//string objectId, string [] Sub, bool isSubProduct, string ImageID, string Name
		public void AddProduct( List<ProductClass> productList  )
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<ProductClass> (db,"Products")) {
				db.CreateTable<ProductClass>();
			}

			foreach( ProductClass product in productList)
				db.InsertOrReplace (product);

			db.Close ();
		}

		public List<ProductClass> GetProducts()
		{
			List<ProductClass> productList = new List<ProductClass> ();

			var db = new SQLiteConnection (DBConstants.DB_PATH);
		

			if (!TableExists<ProductClass> (db,"Products")) {
				return productList;			
			}

			var productTable = db.Table<ProductClass> ();

			foreach (var product in productTable) {
				productList.Add (product);
			}

			db.Close ();

			return productList;
		}
	}
}


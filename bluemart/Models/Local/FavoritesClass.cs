using System;
using SQLite;
using bluemart.Common.Utilities;
using System.Collections.Generic;

namespace bluemart.Models.Local
{
	[Table("Favorites")]
	public class FavoritesClass
	{
		[PrimaryKey]
		public string productID { get; set; }

		private bool TableExists<T> (SQLiteConnection connection,string tableName)
		{    
			const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
			var cmd = connection.CreateCommand (cmdText, tableName);
			return cmd.ExecuteScalar<string> () != null;
		}

		public void AddProductID(string productID)
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<FavoritesClass> (db,"Favorites")) {
				db.CreateTable<FavoritesClass>();
			}

			var newFavorite = new FavoritesClass ();
			newFavorite.productID = productID;
			db.InsertOrReplace (newFavorite);  		

			db.Close ();
		}

		public void RemoveProductID(string productID)
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<FavoritesClass> (db,"Favorites")) {
				return;
			} 

			db.Table<FavoritesClass>();

			var favoriteToBeRemoved = new FavoritesClass ();
			favoriteToBeRemoved.productID = productID;
			db.Delete (favoriteToBeRemoved);  		

			db.Close ();
		}


		public List<string> GetProductIDs()
		{
			List<string> productIDList = new List<string> ();

			var db = new SQLiteConnection (DBConstants.DB_PATH);


			if (!TableExists<FavoritesClass> (db,"Favorites")) {
				return productIDList;			
			}

			var favoritesTable = db.Table<FavoritesClass> ();

			foreach (var favorite in favoritesTable) {
				productIDList.Add (favorite.productID);
			}

			db.Close ();

			return productIDList;
		}

		public bool IsProductFavorite( string productID )
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<FavoritesClass> (db,"Favorites")) {
				return false;
			}

			var queryResult = db.Query<FavoritesClass> ("SELECT * FROM Favorites WHERE productID = ?",productID );
			db.Close ();

			if (queryResult.Count == 0)
				return false;
			else
				return true;			
		}

		//For Development Purposes
		public void ResetTable()
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);
			db.DeleteAll<FavoritesClass> ();
			db.Query<FavoritesClass> ("UPDATE sqlite_sequence SET seq=0 WHERE name='Favorites';");
			db.Close ();
		}
	}
}


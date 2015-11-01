using System;
using bluemart.Common.Utilities;
using SQLite;
using System.Collections.Generic;


namespace bluemart.Models.Local
{
	[Table("User")]
	public class UserClass
	{
		[PrimaryKey, AutoIncrement, Column("_id")]
		public int Id { get; set; }
		public string ActiveRegion { get; set; }
		public string Name { get; set; }
		public string PhoneNumber{ get; set; }
		public DateTime? ImagesUpdateDate { get; set; }
		public DateTime? CategoriesUpdateDate { get; set; }
		public DateTime? ProductsUpdateDate { get; set; }
		//public string Location{ get; set; }

		private bool TableExists<T> (SQLiteConnection connection,string tableName)
		{    
			const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
			var cmd = connection.CreateCommand (cmdText, tableName);
			return cmd.ExecuteScalar<string> () != null;
		}

		public void CreateUserTable()
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<UserClass> (db, "User")) {
				db.CreateTable<UserClass> ();	
				var newUser = new UserClass ();
				newUser.ActiveRegion = "";
				newUser.Name = "";
				newUser.PhoneNumber = "";
				newUser.Id = 1;
				newUser.ImagesUpdateDate = ReleaseConfig.LAST_UPDATEDATE;
				newUser.CategoriesUpdateDate = new DateTime? (DateTime.MinValue);
				newUser.ProductsUpdateDate = new DateTime? (DateTime.MinValue);
				//newUser.Location = "";
				db.InsertOrReplace (newUser); 
			}

			db.Close ();
		}

		#region UserLocation
		public void AddActiveRegionToUser( string activeRegion )
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<UserClass> (db, "User")) {
				return;	
			}

			List<UserClass> userList =db.Query<UserClass> (" select * from User ");
			UserClass user = userList [0];

			user.ActiveRegion = activeRegion;
			db.InsertOrReplace (user);

			db.Close ();
		}

		public string GetActiveRegionFromUser()
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			List<UserClass> userList =db.Query<UserClass> ("select ActiveRegion from User WHERE _id = 1");
			UserClass user = userList [0];

			db.Close ();
			return user.ActiveRegion;
		}
		#endregion

		#region ImagesUpdatesDate
		public void AddImagesUpdateDateToUser( DateTime? updatedAt)
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<UserClass> (db, "User")) {
				return;	
			}

			List<UserClass> userList =db.Query<UserClass> (" select * from User ");
			UserClass user = userList [0];

			user.ImagesUpdateDate = updatedAt;
			db.InsertOrReplace (user);

			db.Close ();
		}
		
		public DateTime? GetImageUpdatedDateFromUser()
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			List<UserClass> userList =db.Query<UserClass> (" select ImagesUpdateDate from User WHERE _id = 1");
			UserClass user = userList [0];

			db.Close ();
			return user.ImagesUpdateDate;
		}
		#endregion

		#region CategoriesUpdateDate
		public void AddCategoriesUpdateDateToUser( DateTime? updatedAt)
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<UserClass> (db, "User")) {
				return;	
			}

			List<UserClass> userList =db.Query<UserClass> (" select * from User ");
			UserClass user = userList [0];

			user.CategoriesUpdateDate = updatedAt;
			db.InsertOrReplace (user);

			db.Close ();
		}

		public DateTime? GetCategoriesUpdatedDateFromUser()
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			List<UserClass> userList =db.Query<UserClass> (" select CategoriesUpdateDate from User WHERE _id = 1");
			UserClass user = userList [0];

			db.Close ();
			return user.CategoriesUpdateDate;
		}
		#endregion

		#region ProductsUpdateDate
		public void AddProductsUpdateDateToUser( DateTime? updatedAt)
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<UserClass> (db, "User")) {
				return;	
			}

			List<UserClass> userList =db.Query<UserClass> (" select * from User ");
			UserClass user = userList [0];

			user.ProductsUpdateDate = updatedAt;
			db.InsertOrReplace (user);

			db.Close ();
		}

		public DateTime? GetProductsUpdatedDateFromUser()
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			List<UserClass> userList =db.Query<UserClass> (" select ProductsUpdateDate from User WHERE _id = 1");
			UserClass user = userList [0];

			db.Close ();
			return user.ProductsUpdateDate;
		}
		#endregion

		public void AddUserInfo(string activeRegion,string name, string phoneNumber)
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<UserClass> (db, "User")) {
				db.CreateTable<UserClass>();	
			}				

			List<UserClass> userList =db.Query<UserClass> (" select * from User ");
			UserClass user = userList [0];

			user.ActiveRegion = activeRegion;
			user.Name = name;
			user.PhoneNumber = phoneNumber;
			db.InsertOrReplace (user);
				
			db.Close ();
		}

		public UserClass GetUser()
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			List<UserClass> userList =db.Query<UserClass> (" select * from User WHERE _id = 1");
			UserClass user = userList [0];

			db.Close ();

			return user;
		}

		//For Development Purposes
		public void ResetTable()
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);
			db.DeleteAll<UserClass> ();
			db.Query<UserClass> ("UPDATE sqlite_sequence SET seq=0 WHERE name='User';");
			db.Close ();
		}
	}

}


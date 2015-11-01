using System;
using SQLite;
using System.Linq;
using bluemart.Common.Utilities;

namespace bluemart.Models.Local
{
	[Table("AddressTable")]
	public class AddressClass
	{
		[PrimaryKey]
		public string Region {get; set;}
		public string Address {get; set;}
		public string AddressDescription {get; set;}

		private bool TableExists<T> (SQLiteConnection connection,string tableName)
		{    
			const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
			var cmd = connection.CreateCommand (cmdText, tableName);
			return cmd.ExecuteScalar<string> () != null;
		}

		public void AddAddress()
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<AddressClass> (db,"AddressTable")) {
				db.CreateTable<AddressClass>();
			}
				
			var a = db.InsertOrReplace (this);

			db.Close ();
		}

		public AddressClass GetAddress( string region )
		{
			AddressClass address = null;

			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<AddressClass> (db,"AddressTable")) {
				return address;			
			}

			var query = from AddressTable in db.Table<AddressClass> ()
					where AddressTable.Region == region select AddressTable;

			foreach (var tempAdd in query)
				address = tempAdd;
			/*if (query.e > 0)
				address = query.Cast<AddressClass> ().ElementAt (0);
			else
				address = null;*/

 			db.Close ();

			return address;
		}
	}
}


using System;
using SQLite;
using System.Linq;
using bluemart.Common.Utilities;
using System.Collections.Generic;

namespace bluemart.Models.Local
{
	[Table("AddressTable")]
	public class AddressClass
	{		
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string Name { get; set; }
		public string PhoneNumber{ get; set; }
		public int ShopNumber {get; set;}
		public string Address {get; set;}
		public string AddressDescription {get; set;}
		public bool IsActive { get; set; }

		private bool TableExists<T> (SQLiteConnection connection,string tableName)
		{    
			const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
			var cmd = connection.CreateCommand (cmdText, tableName);
			return cmd.ExecuteScalar<string> () != null;
		}

		public void AddAddress(AddressClass address)
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<AddressClass> (db,"AddressTable")) {
				db.CreateTable<AddressClass>();
			}
				
			var a = db.InsertOrReplace (address);

			db.Close ();
		}

		public List<AddressClass> GetAddressList( int shopNumber )
		{
			List<AddressClass> addressList = null;

			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<AddressClass> (db,"AddressTable")) {
				return addressList;			
			}

			var query = from AddressTable in db.Table<AddressClass> ()
					where AddressTable.ShopNumber == shopNumber select AddressTable;

			foreach (var tempAdd in query)
				addressList.Add(tempAdd);			

 			db.Close ();

			return addressList;
		}

		public void MakeActive(AddressClass address)
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<AddressClass> (db,"AddressTable")) {
				return;			
			}

			var query = from AddressTable in db.Table<AddressClass> ()
					where AddressTable.IsActive == true && 
						  AddressTable.ShopNumber == address.ShopNumber 
				select AddressTable;

			if ( query.Count<AddressClass>() > 0 )
			{
				AddressClass tempAddress = query.First<AddressClass> ();
				tempAddress.IsActive = false;
				db.InsertOrReplace (tempAddress);
			}

			address.IsActive = true;
			db.InsertOrReplace (address);

			/*foreach (var tempAdd in query)
				addressList.Add(tempAdd);	*/		

			db.Close ();
		}

		public AddressClass GetAddress(int id)
		{
			AddressClass address = null;

			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<AddressClass> (db,"AddressTable")) {
				return address;			
			}

			var query = from AddressTable in db.Table<AddressClass> ()
					where AddressTable.Id == id select AddressTable;

			foreach (var tempAdd in query)
				address = tempAdd;			
			
			db.Close ();

			return address;
		}

		public AddressClass GetActiveAddress(string region)
		{
			AddressClass address = null;

			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<AddressClass> (db,"AddressTable")) {
				return address;			
			}

			int shopNumber = RegionHelper.DecideShopNumber (region);

			var query = from AddressTable in db.Table<AddressClass> ()
					where AddressTable.ShopNumber == shopNumber &&
						  AddressTable.IsActive == true	select AddressTable;
			
			if ( query.Count<AddressClass>() > 0 )
			{
				address = query.First<AddressClass> ();
			}

			return address;
		}
	}
}


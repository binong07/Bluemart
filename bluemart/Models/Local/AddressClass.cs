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
		public string AddressLine3 { get; set; }
		public bool IsActive { get; set; }

		private bool TableExists<T> (SQLiteConnection connection,string tableName)
		{    
			const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
			var cmd = connection.CreateCommand (cmdText, tableName);
			return cmd.ExecuteScalar<string> () != null;
		}

		public void AddAddress(AddressClass address)
		{
			//if(MyDevice.db==nul)
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<AddressClass> (db,"AddressTable")) {
				db.CreateTable<AddressClass>();
			}
				
			db.Insert (address);

			db.Close ();
		}

		public void UpdateAddress( AddressClass address )
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<AddressClass> (db,"AddressTable")) {
				db.CreateTable<AddressClass>();
			}

			AddressClass tempAddress = (from a in db.Table<AddressClass> ()
			                            where a.Id == address.Id
			                            select a).SingleOrDefault ();
			tempAddress = address;

			db.Update (tempAddress);


			db.Close ();
		}

		public List<AddressClass> GetAddressList( int shopNumber )
		{
			List<AddressClass> addressList = new List<AddressClass>();

			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<AddressClass> (db,"AddressTable")) {
				db.Close ();
				return addressList;			
			}
			//Get active address
			AddressClass activeAddress = (from a in db.Table<AddressClass> ()
				where a.IsActive == true && a.ShopNumber == shopNumber	
											select a).SingleOrDefault ();

			addressList.Add (activeAddress);

			var passiveAddresses = from AddressTable in db.Table<AddressClass> ()
					where AddressTable.ShopNumber == shopNumber && AddressTable.IsActive == false
					select AddressTable;

			foreach (var tempAdd in passiveAddresses)
				addressList.Add(tempAdd);			

 			db.Close ();

			return addressList;
		}

		public void MakeActive(AddressClass address)
		{
			var db = new SQLiteConnection (DBConstants.DB_PATH);

			if (!TableExists<AddressClass> (db,"AddressTable")) {
				db.Close ();
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
				db.Close ();
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
				db.Close ();
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
			db.Close ();
			return address;
		}
	}
}


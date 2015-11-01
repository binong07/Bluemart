using System;
using System.Collections.Generic;

namespace bluemart.Models.Local
{
	public class HistoryClass
	{
		public HistoryClass (List<string> productOrderList,string address, string addressDesc,string name, string surname, string phone, string totalPrice, string date, string region)
		{	
			ProductOrderList = productOrderList;
			TotalPrice = totalPrice;
			Date = date;
			Region = region;
			Address = address;
			AddressDescription = addressDesc;
			Name = name;
			Surname = surname;
			Phone = phone;
		}
			
		public List<string> ProductOrderList;
		public string TotalPrice {get;set;}
		public string Date {get;set;}
		public string Region {get;set;}
		public string Address {get;set;}
		public string AddressDescription {get;set;}
		public string Name {get;set;}
		public string Surname {get;set;}
		public string Phone {get;set;}
	}
}


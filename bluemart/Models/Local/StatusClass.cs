using System;
using bluemart.Models.Remote;
using System.Collections.Generic;

namespace bluemart.Models.Local
{
	public class StatusClass
	{
		public StatusClass (List<string> productOrderList,string address, string addressDesc,string name, string surname, string phone, string totalPrice, string date, string region, OrderModel.OrderStatus orderStatus)
		{
			ProductOrderList = productOrderList;
			TotalPrice = totalPrice;
			Date = date;
			Region = region;
			Status = "Status: ";
			Address = address;
			AddressDescription = addressDesc;
			Name = name;
			Surname = surname;
			Phone = phone;

			switch (orderStatus) {
			case OrderModel.OrderStatus.WAITING_CONFIRMATION:
				Status += "Waiting for confirmation";
				break;
			case OrderModel.OrderStatus.CONFIRMED:
				Status += "Your order has been received and is being prepared";
				break;
			case OrderModel.OrderStatus.IN_TRANSIT:
				Status += "Your order has been prepared and it has been dispatched";
				break;
			default:
				break;
			}

		}

		public List<string> ProductOrderList;
		public string TotalPrice {get;set;}
		public string Date {get;set;}
		public string Region {get;set;}
		public string Status { get; set; }
		public string Address {get;set;}
		public string AddressDescription {get;set;}
		public string Name {get;set;}
		public string Surname {get;set;}
		public string Phone {get;set;}
	}
}


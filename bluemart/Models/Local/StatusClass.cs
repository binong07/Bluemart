using System;
using bluemart.Models.Remote;
using System.Collections.Generic;

namespace bluemart.Models.Local
{
	public class StatusClass
	{
		public StatusClass (List<string> productOrderList,string address, string addressDesc,string name, string surname, string phone, string totalPrice, string date, string region, OrderModel.OrderStatus orderStatus,string deliveryStaffName,string deliveryStaffPhone)
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
			DeliveryStaffName = deliveryStaffName;
			DeliveryStaffPhone = deliveryStaffPhone;
			OrderStatus = orderStatus;

			switch (orderStatus) {
			case OrderModel.OrderStatus.WAITING_CONFIRMATION:
				Status += "Waiting for confirmation.";
				break;
			case OrderModel.OrderStatus.CONFIRMED:
				Status += "Your order has been confirmed and being prepared.";
				break;
			case OrderModel.OrderStatus.IN_TRANSIT:
				Status += "Your order has been prepared and it has been dispatched.";
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
		public string DeliveryStaffName { get; set; }
		public string DeliveryStaffPhone { get; set; }
		public OrderModel.OrderStatus OrderStatus { get; set; }
	}
}


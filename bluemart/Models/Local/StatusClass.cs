using System;
using bluemart.Models.Remote;

namespace bluemart.Models.Local
{
	public class StatusClass
	{
		public StatusClass (string totalPrice, string date, string region, OrderModel.OrderStatus orderStatus)
		{
			TotalPrice = "Total Price: " + totalPrice + " DH";
			Date = "Date: " + date;
			Region = "Region: " + region;
			Status = "Status: ";

			switch (orderStatus) {
			case OrderModel.OrderStatus.WAITING_CONFIRMATION:
				Status += "Waiting for confirmation.";
				break;
			case OrderModel.OrderStatus.CONFIRMED:
				Status += "Your order has been confirmed and being prepared.";
				break;
			case OrderModel.OrderStatus.IN_TRANSIT:
				Status += "Your order has been prepared and is in transition.";
				break;
			default:
				break;
			}

		}

		public string TotalPrice {get;set;}
		public string Date {get;set;}
		public string Region {get;set;}
		public string Status { get; set; }
	}
}


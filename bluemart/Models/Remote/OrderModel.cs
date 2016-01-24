using System;
using bluemart.Common.Utilities;
using bluemart.Models.Local;
using bluemart.Common.Objects;
using System.Collections.Generic;
using Parse;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace bluemart.Models.Remote
{
	public static class OrderModel
	{		
		public enum OrderStatus { WAITING_CONFIRMATION, CONFIRMED, IN_TRANSIT, COMPLETED };
		private static AddressClass mAddressModel = new AddressClass();
		private struct OrderObject
		{
			public string Quantity;
			public string Product;
			public string Description;
			public string Price;
		};
		//public static int ActiveOrderStatus = OrderStatus.WAITING_CONFIRMATION;

		public static async Task<bool> SendOrderToRemote(UserClass user)
		{	
			AddressClass address = mAddressModel.GetActiveAddress (user.ActiveRegion);
			var fullname = address.Name.Split (' ');
			List<OrderObject> OrderList = new List<OrderObject> ();

			foreach (var product in Cart.ProductsInCart) {				
				string quantity = product.ProductNumberInCart.ToString();
				string productName = product.Name;
				string description = product.Quantity;
				string cost = (product.ProductNumberInCart * product.Price).ToString ();
				OrderObject orderObject;
				orderObject.Quantity = quantity;
				orderObject.Product = productName;
				orderObject.Description = description;
				orderObject.Price = cost;
				OrderList.Add (orderObject);
				//OrderList.Add("Quantity:" + quantity + "," + "Product:" + productName + "," + "Description:" + description + "," + "Price:" + cost);
			}	


			ParseObject order= new ParseObject(ParseConstants.ORDERS_CLASS_NAME);

			order [ParseConstants.ORDERS_ATTRIBUTE_ADDRESS] = address.Address;
			order [ParseConstants.ORDERS_ATTRIBUTE_ADDRESSDESC] = address.AddressDescription;
			order [ParseConstants.ORDERS_ATTRIBUTE_ADDRESSLINE3] = address.AddressLine3;
			order [ParseConstants.ORDERS_ATTRIBUTE_REGION] = user.ActiveRegion;
			order [ParseConstants.ORDERS_ATTRIBUTE_USERNAME] = fullname[0];
			order [ParseConstants.ORDERS_ATTRIBUTE_SURNAME] = fullname[1];
			order [ParseConstants.ORDERS_ATTRIBUTE_PHONE] = address.PhoneNumber;
			order [ParseConstants.ORDERS_ATTRIBUTE_STORE] = RegionHelper.DecideShopNumber (user.ActiveRegion);
			order [ParseConstants.ORDERS_ATTRIBUTE_STATUS] = (int)OrderStatus.WAITING_CONFIRMATION;
			order [ParseConstants.ORDERS_ATTRIBUTE_USERID] =  MyDevice.DeviceID;
			order [ParseConstants.ORDERS_ATTRIBUTE_ORDERARRAY] = Newtonsoft.Json.JsonConvert.SerializeObject (OrderList);

			bool bIsSucceeded = false;
			var tokenSource = new CancellationTokenSource ();
			var ct = tokenSource.Token;
			var task = order.SaveAsync (ct);
			bIsSucceeded = task.Wait (5000);
			tokenSource.Cancel ();
				

			return bIsSucceeded;
		}


		public static List<StatusClass> GetOrdersForTracking()
		{
			var statusClassList = new List<StatusClass> ();

			if (MyDevice.GetNetworkStatus () != "NotReachable") {
				var orderQuery = ParseObject.GetQuery (ParseConstants.ORDERS_CLASS_NAME).
					WhereEqualTo (ParseConstants.ORDERS_ATTRIBUTE_USERID, MyDevice.DeviceID).
					WhereLessThan (ParseConstants.ORDERS_ATTRIBUTE_STATUS, 3).
					OrderByDescending("updatedAt");

				var orderObjects = orderQuery.FindAsync ().Result;

				foreach (var order in orderObjects) {				
					List<string> productOrderList = new List<string>();

					var orderString = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_ORDERARRAY);
					var orderObjectList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderObject>>(orderString);
					foreach (var orderobject in orderObjectList) {
						string orderObjectString = "Quantity:" + orderobject.Quantity + ";" + 
							"Product:" + orderobject.Product + ";" + "Description:" + 
							orderobject.Description + ";" + "Price:" + orderobject.Price;
						productOrderList.Add (orderObjectString);
					}

					var region = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_REGION);
					int status = order.Get<int> (ParseConstants.ORDERS_ATTRIBUTE_STATUS);
					var deliveryStaffName = "";
					var deliveryStaffPhone = "";
					if (status == (int)OrderStatus.IN_TRANSIT) {
						var deliveryStaff = GetDeliveryStaff (order.Get<string> (ParseConstants.ORDERS_DELIVERY_STAFF_ID));
						if (deliveryStaff != null) {
							deliveryStaffName = deliveryStaff.Get<string>(ParseConstants.DELIVERYSTAFF_NAME) + " " + deliveryStaff.Get<string>(ParseConstants.DELIVERYSTAFF_SURNAME);
							deliveryStaffPhone = deliveryStaff.Get<string> (ParseConstants.DELIVERYSTAFF_PHONE);
						}
					}
					var date = order.CreatedAt.ToString ();
					var totalPrice = CalculateTotalPrice (productOrderList).ToString ();
					var address = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_ADDRESS);
					var addressDesc = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_ADDRESSDESC);
					var addressLine3 = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_ADDRESSLINE3);
					var name = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_USERNAME);
					var surname = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_SURNAME);
					var phone = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_PHONE);
					statusClassList.Add (new StatusClass (productOrderList,address,addressDesc,addressLine3,name,surname,phone,totalPrice, date, region, (OrderStatus)status,deliveryStaffName,deliveryStaffPhone));
				}
			}
				
			return statusClassList;
		}

		private static ParseObject GetDeliveryStaff(string DeliveryStaffName)
		{
			ParseObject deliveryStaffQuery = null;
			if (MyDevice.GetNetworkStatus () != "NotReachable") {
				deliveryStaffQuery = ParseObject.GetQuery (ParseConstants.DELIVERYSTAFF_CLASS_NAME).
					WhereEqualTo (ParseConstants.DELIVERYSTAFF_NAME, DeliveryStaffName).FirstAsync().Result;
			}
			return deliveryStaffQuery;
		}

		public static List<HistoryClass> GetOrdersForHistory()
		{
			var historyClassList = new List<HistoryClass> ();

			if (MyDevice.GetNetworkStatus () != "NotReachable") {
				var orderQuery = ParseObject.GetQuery (ParseConstants.ORDERS_CLASS_NAME).
					WhereEqualTo (ParseConstants.ORDERS_ATTRIBUTE_USERID, MyDevice.DeviceID).
					WhereEqualTo (ParseConstants.ORDERS_ATTRIBUTE_STATUS, 3).
					OrderByDescending("updatedAt");

				var orderObjects = orderQuery.FindAsync ().Result;

				foreach (var order in orderObjects) {				
					List<string> productOrderList = new List<string>();
					var orderString = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_ORDERARRAY);
					var orderObjectList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderObject>>(orderString);
					foreach (var orderobject in orderObjectList) {
						string orderObjectString = "Quantity:" + orderobject.Quantity + ";" + 
							"Product:" + orderobject.Product + ";" + "Description:" + 
							orderobject.Description + ";" + "Price:" + orderobject.Price;
						productOrderList.Add (orderObjectString);
					}
					var region = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_REGION);
					var date = order.CreatedAt.ToString ();
					var totalPrice = CalculateTotalPrice (productOrderList).ToString ();
					var address = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_ADDRESS);
					var addressDesc = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_ADDRESSDESC);
					var addressLine3 = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_ADDRESSLINE3);
					var name = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_USERNAME);
					var surname = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_SURNAME);
					var phone = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_PHONE);

					historyClassList.Add (new HistoryClass (productOrderList,address,addressDesc,addressLine3,name,surname,phone,totalPrice, date, region));
				}
			}

			return historyClassList;
		}



		private static double CalculateTotalPrice( List<string> productOrderList )
		{
			double totalPrice = 0.0f;

			foreach (string order in productOrderList) {
				var orderProperty = order.Split (';') [3];
				var orderPrice = orderProperty.Split (':') [1];
				totalPrice += Double.Parse(orderPrice);
			}

			return totalPrice;
		}
	}
}


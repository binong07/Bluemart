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

		//public static int ActiveOrderStatus = OrderStatus.WAITING_CONFIRMATION;

		public static async Task<bool> SendOrderToRemote(UserClass user)
		{			
			var fullname = user.Name.Split (' ');
			List<string> OrderList = new List<string> ();

			foreach (var product in Cart.ProductsInCart) {				
				string quantity = product.ProductNumberInCart.ToString();
				string productName = product.Name;
				string description = product.Quantity;
				string cost = (product.ProductNumberInCart * product.Price).ToString ();

				OrderList.Add("Quantity:" + quantity + "," + "Product Name:" + productName + "," + "Description:" + description + "," + "Cost:" + cost);
			}		


			ParseObject order= new ParseObject(ParseConstants.ORDERS_CLASS_NAME);

			order [ParseConstants.ORDERS_ATTRIBUTE_ADDRESS] = user.Address;
			order [ParseConstants.ORDERS_ATTRIBUTE_ADDRESSDESC] = user.AddressDescription;
			order [ParseConstants.ORDERS_ATTRIBUTE_REGION] = user.Region;
			order [ParseConstants.ORDERS_ATTRIBUTE_USERNAME] = fullname[0];
			order [ParseConstants.ORDERS_ATTRIBUTE_SURNAME] = fullname[1];
			order [ParseConstants.ORDERS_ATTRIBUTE_PHONE] = user.PhoneNumber;
			order [ParseConstants.ORDERS_ATTRIBUTE_STORE] = LocationHelper.DecideShopNumber (user.Location);
			order [ParseConstants.ORDERS_ATTRIBUTE_STATUS] = (int)OrderStatus.WAITING_CONFIRMATION;
			order [ParseConstants.ORDERS_ATTRIBUTE_USERID] =  MyDevice.DeviceID;
			order [ParseConstants.ORDERS_ATTRIBUTE_ORDERARRAY] = OrderList.ToArray ();

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
				Select (ParseConstants.ORDERS_ATTRIBUTE_ORDERARRAY).
				Select (ParseConstants.ORDERS_ATTRIBUTE_REGION).
				Select (ParseConstants.ORDERS_ATTRIBUTE_STATUS);

				var orderObjects = orderQuery.FindAsync ().Result;

				foreach (var order in orderObjects) {				
					var productOrderList = order.Get<IEnumerable<object>> (ParseConstants.ORDERS_ATTRIBUTE_ORDERARRAY).Cast<string> ().ToList ();
					var region = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_REGION);
					int status = order.Get<int> (ParseConstants.ORDERS_ATTRIBUTE_STATUS);
					var date = order.CreatedAt.ToString ();
					var totalPrice = CalculateTotalPrice (productOrderList).ToString ();
					statusClassList.Add (new StatusClass (totalPrice, date, region, (OrderStatus)status));
				}
			}
				
			return statusClassList;
		}

		public static List<HistoryClass> GetOrdersForHistory()
		{
			var historyClassList = new List<HistoryClass> ();

			if (MyDevice.GetNetworkStatus () != "NotReachable") {
				var orderQuery = ParseObject.GetQuery (ParseConstants.ORDERS_CLASS_NAME).
					WhereEqualTo (ParseConstants.ORDERS_ATTRIBUTE_USERID, MyDevice.DeviceID).
					WhereEqualTo (ParseConstants.ORDERS_ATTRIBUTE_STATUS, 3);

				var orderObjects = orderQuery.FindAsync ().Result;

				foreach (var order in orderObjects) {				
					var productOrderList = order.Get<IEnumerable<object>> (ParseConstants.ORDERS_ATTRIBUTE_ORDERARRAY).Cast<string> ().ToList ();
					var region = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_REGION);
					var date = order.CreatedAt.ToString ();
					var totalPrice = CalculateTotalPrice (productOrderList).ToString ();
					var address = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_ADDRESS);
					var addressDesc = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_ADDRESSDESC);
					var name = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_USERNAME);
					var surname = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_SURNAME);
					var phone = order.Get<string> (ParseConstants.ORDERS_ATTRIBUTE_PHONE);

					historyClassList.Add (new HistoryClass (productOrderList,address,addressDesc,name,surname,phone,totalPrice, date, region));
				}
			}

			return historyClassList;
		}



		private static double CalculateTotalPrice( List<string> productOrderList )
		{
			double totalPrice = 0.0f;

			foreach (string order in productOrderList) {
				var orderProperty = order.Split (',') [3];
				var orderPrice = orderProperty.Split (':') [1];
				totalPrice += Double.Parse(orderPrice);
			}

			return totalPrice;
		}
	}
}


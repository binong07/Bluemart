using System;
using System.Collections.ObjectModel;

namespace bluemart.Common.Objects
{
	public static class Cart
	{
		public static ObservableCollection<Product> ProductsInCart = new ObservableCollection<Product> ();	
		public static decimal ProductTotalPrice = new decimal (0.0f);
		//public static Dictionary<Product,int> ProductDictionary = new Dictionary<Product, int>();
	}
}


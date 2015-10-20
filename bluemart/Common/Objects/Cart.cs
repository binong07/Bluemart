using System;
using System.Collections.Generic;

namespace bluemart.Common.Objects
{
	public static class Cart
	{
		public static List<Product> ProductsInCart = new List<Product> ();	
		public static double ProductTotalPrice = 0.0f;
		//public static Dictionary<Product,int> ProductDictionary = new Dictionary<Product, int>();
	}
}


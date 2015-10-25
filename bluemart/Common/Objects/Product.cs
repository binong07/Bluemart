using System;

namespace bluemart.Common.Objects
{
	public class Product
	{
		public Product(string productID, string name, string productImagePath, decimal price, string quantity)
		{
			this.ProductID = productID;
			this.Name = name;
			this.ProductImagePath = productImagePath;
			this.Price = price;
			this.Quantity = quantity;
			this.ProductNumberInCart = 0;
		}

		public string ProductID { private set; get; }
		public string Name { private set; get; }
		public string ProductImagePath { private set; get; }
		public decimal Price { private set; get; }
		public string Quantity { private set; get; }
		public int ProductNumberInCart { set; get; }
	}
}


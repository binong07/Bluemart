using System;
using PCLStorage;

namespace bluemart.Common.Utilities
{
	public static class ParseConstants
	{
		//Class Names
		public const string CATEGORIES_CLASS_NAME = "Categories";
		public const string IMAGES_CLASS_NAME = "Images";
		public const string PRODUCTS_CLASS_NAME = "Products";
		public const string ORDERS_CLASS_NAME = "Orders";
		public const string DELIVERYSTAFF_CLASS_NAME = "DeliveryStaff";
		//Common Column Names
		public const string UPDATEDATE_NAME = "updatedAt";
		//Folder Names
		public const string IMAGE_FOLDER_NAME = "SavedImages";
		//Category Attribute Names
		public const string CATEGORY_ATTRIBUTE_NAME = "Name";
		public const string CATEGORY_ATTRIBUTE_IMAGEID = "ImageID";
		public const string CATEGORY_ATTRIBUTE_IMAGENAME = "ImageName";
		public const string CATEGORY_ATTRIBUTE_ISSUBCATEGORYNAME = "isSubCategory";
		public const string CATEGORY_ATTRIBUTE_SUB = "Sub";
		public const string CATEGORY_ATTRIBUTE_PRIORITY = "Priority";
		//Image Attribute Names
		public const string IMAGE_ATTRIBUTE_IMAGEFILE = "Image"; 
		public const string IMAGE_ATTRIBUTE_IMAGENAME = "Name";
		//Product Attribute Names
		public const string PRODUCT_ATTRIBUTE_NAME = "Name";
		public const string PRODUCT_ATTRIBUTE_CATEGORYID = "Category";
		public const string PRODUCT_ATTRIBUTE_IMAGEID = "ImageID";
		public const string PRODUCT_ATTRIBUTE_IMAGENAME = "ImageName";
		public const string PRODUCT_ATTRIBUTE_PRICE = "Price";
		public const string PRODUCT_ATTRIBUTE_QUANTITY = "Quantity";
		public const string PRODUCT_ATTRIBUTE_PARENTCATEGORY = "ParentCategory";
		public const string PRODUCT_ATTRIBUTE_STORES = "Stores";
		public const string PRODUCT_ATTRIBUTE_ISTOPSELLING = "IsTopSelling";
		public const string PRODUCT_ATTRIBUTE_PRIORITY = "Priority";
		public const string PRODUCT_ATTRIBUTE_ISINSTOCK = "IsInStock";
		//Orders Attribute Names
		public const string ORDERS_ATTRIBUTE_ADDRESS = "Address";
		public const string ORDERS_ATTRIBUTE_ADDRESSDESC = "AddressDescription";
		public const string ORDERS_ATTRIBUTE_ADDRESSLINE3 = "AddressLine3";
		public const string ORDERS_ATTRIBUTE_USERNAME = "Name";
		public const string ORDERS_ATTRIBUTE_ORDERARRAY = "OrderTest";
		public const string ORDERS_ATTRIBUTE_PHONE = "Phone";
		public const string ORDERS_ATTRIBUTE_REGION = "Region";
		public const string ORDERS_ATTRIBUTE_STATUS = "Status";
		public const string ORDERS_ATTRIBUTE_STORE = "Store";
		public const string ORDERS_ATTRIBUTE_SURNAME = "SurName";
		public const string ORDERS_ATTRIBUTE_USERID = "userID";
		public const string ORDERS_DELIVERY_STAFF_ID = "DeliveryStaffID";
		//Delivery staff Attribute Names
		public const string DELIVERYSTAFF_PHONE = "TelNo";
		public const string DELIVERYSTAFF_NAME = "Name";
		public const string DELIVERYSTAFF_SURNAME = "SurName";
	}
}


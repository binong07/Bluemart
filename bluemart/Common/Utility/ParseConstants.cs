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
		public const string PRODUCT_ATTRIBUTE_STORES = "Stores";
	}
}


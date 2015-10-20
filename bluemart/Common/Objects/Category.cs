using System;

using Xamarin.Forms;
using System.Collections.Generic;
using bluemart.Models.Remote;

namespace bluemart.Common.Objects
{
	public class Category
	{
		public Category(string name, string categoryImagePath,bool isSubCategory = true, string categoryID = null, CategoryModel categoryModel = null, List<string> subCategories = null  )
		{
			this.Name = name;
			this.CategoryImagePath = categoryImagePath;
			this.SubCategoriesList = subCategories;
			this.CategoryModel = categoryModel;
			this.CategoryID = categoryID;
			this.IsSubCategory = isSubCategory;
		}

		public bool IsSubCategory { private set; get;}

		public string Name { private set; get; }

		public string CategoryImagePath { private set; get; }

		public List<string> SubCategoriesList { private set; get; }

		public CategoryModel CategoryModel { private set; get; }

		public string CategoryID { private set; get; }
	};
}



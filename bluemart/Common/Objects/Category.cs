using System;

using Xamarin.Forms;
using System.Collections.Generic;
using bluemart.Models.Remote;

namespace bluemart.Common.Objects
{
	public class Category
	{
		public Category(string name, string categoryImageName,bool isSubCategory = true, string categoryID = null, List<string> subCategories = null  )
		{
			this.Name = name;
			this.CategoryImageName = categoryImageName;
			this.SubCategoriesList = subCategories;
			this.CategoryID = categoryID;
			this.IsSubCategory = isSubCategory;
		}

		public bool IsSubCategory { private set; get;}

		public string Name { private set; get; }

		public string CategoryImageName { private set; get; }

		public List<string> SubCategoriesList { private set; get; }

		public string CategoryID { private set; get; }
	};
}



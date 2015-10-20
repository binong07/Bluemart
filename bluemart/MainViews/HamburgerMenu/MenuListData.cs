using System;
using System.Collections.Generic;
using bluemart.MainViews.HamburgerMenu;
using bluemart.MainViews;

namespace bluemart.MainViews.HamburgerMenu
{
	public class MenuListData : List<MenuItem>
	{
		public MenuListData ()
		{
			this.Add (new MenuItem () { 
				Title = "Browse Categories", 
				TargetType = typeof(BrowseCategoriesPage)
			});

			this.Add (new MenuItem () { 
				Title = "Favorites", 
				TargetType = typeof(FavoritesPage)
			});

			this.Add (new MenuItem () { 
				Title = "Settings", 
				TargetType = typeof(SettingsPage)
			});					
		}
	}
}

